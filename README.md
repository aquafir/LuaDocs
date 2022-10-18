# Intellisense

## Uses

* [NuDoq](https://github.com/devlooped/NuDoq) 
  * Looked at [MSDN](https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/october/csharp-accessing-xml-documentation-via-reflection)  and [SO](https://stackoverflow.com/questions/15602606/programmatically-get-summary-comments-at-runtime) versions but they had issues
* [sumneko](https://github.com/sumneko/lua-language-server)'s Lua [VSCode extension](https://marketplace.visualstudio.com/items?itemName=sumneko.lua))
  * Based on EmmyLua but no longer compatible
* [Harmony](https://harmony.pardeike.net/articles/utilities.html) for assembly traversal (but not really)
* [ReflectionBridge](https://github.com/StefH/ReflectionBridge) for TypeInfo via Type.BaseType





## Approach

Originally looked at this for [no-Intellisense bindings](https://arborinteractive.com/2020/09/18/autogenerating_lua_bindings_for_unity_csharp_moonsharp/index.html), but not ideal.

NuDoqs provides:

* [Member elements](https://camo.githubusercontent.com/ffb0ce6a1330bb9e083ce28e2375117401e1e094dff8bb152b30d02ea18d3235/68747470733a2f2f7261772e6769746875622e636f6d2f6b7a752f4e75446f712f6d61737465722f646f632f4e75446f712e4d656d626572732e706e67), with some things only available from the assembly.
* [Documentation elements](https://camo.githubusercontent.com/e2489e2766e950c2c9768af091d7a9e679de5e1607a0d17607c8c88e24f12536/68747470733a2f2f7261772e6769746875622e636f6d2f6b7a752f4e75446f712f6d61737465722f646f632f4e75446f712e436f6e74656e742e706e67) from XML documentation.



[XML Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments) is needed for Lua annotations, so a first-pass with `XmlVisitor` extracts whatever is needed (e.g., optional name for a param, string representation, type of XML element). 

The `Id` [string](#ID Strings) is used as a key for documentation elements converted to a list of `XmlEntry` for an element.  NuDoq's `MemberIdMap` is used to get a `MemberInfo` from id, or id from info.



NuDoqs does not appear to look at members/types that are lacking XML documentation, even with the assembly provided  It also doesn't provide the type corresponding to an element.  

Reflection with desired [binding flags](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags?view=net-7.0) or [Harmony's utilities](https://harmony.pardeike.net/articles/utilities.html) used in `LuaDocumenter` to traverse an assembly:

* Iterate types in an assembly.
* *Filter out what you want.  Currently just ignore MS/System namespaces*
* Handle enums separate from class/struct/etc.
* Any Type encountered is [translated to an equivalent](#Moonsharp Conversions) Lua type.
  * Types are abbreviate to whatever is after the last `.` in their full name
  * Nested stuff uses whatever is after the last `+`.  *Probably should think more about this*
* Create [@class](#@class) with relevant [@fields](#@fields) for class/struct
  * Create a local instance of the @class [@type](#@type) with a prefix (`_TypeName`) to keep the name separate from the global static type
    * Add constructors for `TypeName:new(params)`
    * Add instance methods for `TypeName:methodName(params)`
    * Add instance event methods for `Type:add`, `Type:remove`
      * (and `Type:handle/raise?`?)
  * Handle statics
    * First option is separating files where static declarations are made.  
      * Probably best option but needs testing.
      * `Static.lua` does this with `ClassName` 
      * Use `:` notation for instances, `.` for static
      * Can't just move the static variable declaration below anything with instance.
    * Second option is some convention for changing the name of the static class
      * Type `SomeTime` accessed like `_SomeType.new()`
    * Third option is adding `fun()` fields.  Probably the worst
      * Lets you only declare the static variable since everything else is coming from the @class
      * Unable to document parameters/returns
      * Don't think it can support variadics (outside of treating them as arrays)
      * Uses the same icon as other fields
    * Add fields/properties with @type annotations
      * Try to set defaults.  ***Definitely need to think more about this***.
        * Convert value to string value that would be used in Lua declaration
    * Add static methods similar to instance methods
* Write out to `_Examples/definitions.lua`



### Todo

* Events
  * Possible [@operator](https://github.com/sumneko/lua-language-server/wiki/Annotations#operator) support

* Think about virtual/interface/abstract, attributes, nested stuff, defaults
* Think about visibility/accessibility
* [@async](#@async) support.  Very easy if not trying to target 3.5
* [@generic](#@generic) support / other C# --> Lua type mapping improvements.
* [@source](#@source) support.  Should be easy to point to trevis' docs
* [@module](#@module) separation / [environment library setup](https://github.com/sumneko/lua-language-server/wiki/Libraries) for performance.
  * Other config / plugin / bundling of annotation possibilities 
  * [@see](#@see) possible usage for things that are missing other documentation support
* Better exclusion of unwanted elements
* Guard against weirdness (e.g., System.Void defaults, duplicate friendly type names)
* Add missing things to test in `FakeProject`
* Clean up code and maybe package it for non-UB use eventually?
  * Logging / testing





## Modules / Filtering

Documented types could be filtered by:

* [MoonSharp Attributes](https://arborinteractive.com/2020/09/18/autogenerating_lua_bindings_for_unity_csharp_moonsharp/index.html) 

```csharp
        if(t.GetCustomAttributes(typeof(MoonSharp.Interpreter.MoonSharpUserDataAttribute), false).Length > 0)
```

* Ones with XML documentation (easy to just use NuDoqs in this case)
* Namespace
* Accessibility



## VS XML Documentation

* `Type`  inherits from `MemberInfo`, which can be used with NuDoq's `MemberIdMap` to get an ID string?

[Recommended Tags](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags)



### [ID Strings](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings)

| Character | Member type  | Notes                                                        |
| --------- | ------------ | ------------------------------------------------------------ |
| N         | namespace    | You can't add documentation comments to a namespace, but you can make cref references to them, where supported. |
| T         | type         | A type is a class, interface, struct, enum, or delegate.     |
| F         | field        |                                                              |
| P         | property     | Includes indexers or other indexed properties.               |
| M         | method       | Includes special methods, such as constructors and operators. |
| E         | event        |                                                              |
| !         | error string | The rest of the string provides information about the error. The C#  compiler generates error information for links that cannot be resolved. |







## Conversion

* [C# Members](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/members)
* [MethodBase](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase?source=recommendations&view=net-6.0) provides info on methods/constructors
* [ParameterInfo](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo?source=recommendations&view=net-6.0)
* [TypeInfo](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.typeinfo?source=recommendations&view=net-6.0) 
  * Represents type declarations for class types, interface types, array  types, value types, enumeration types, type parameters, generic type  definitions, and open or closed constructed generic types.
  * May require a Nuget package for (full?) 3.5 support
* For events, Factorio has a [plugin](https://github.com/JanSharp/FactorioSumnekoLuaPlugin#event-handler-parameter-annotating) that simplifies annotations





### Moonsharp Conversions

[Conversions from C# to Lua types](https://www.moonsharp.org/mapping.html)

| CLR type                               | C# friendly name | Lua type   | Notes                                                        |
| -------------------------------------- | ---------------- | ---------- | ------------------------------------------------------------ |
| void                                   |                  | (no value) | This can be applied to return values of methods only.        |
| null                                   |                  | nil        | Any null will be converted to nil.                           |
| MoonSharp.Interpreter.DynValue         |                  | *          | The DynValue is passed through.                              |
| System.SByte                           | sbyte            | number     |                                                              |
| System.Byte                            | byte             | number     |                                                              |
| System.Int16                           | short            | number     |                                                              |
| System.UInt16                          | ushort           | number     |                                                              |
| System.Int32                           | int              | number     |                                                              |
| System.UInt32                          | uint             | number     |                                                              |
| System.Int64                           | long             | number     | The conversion can lead to a silent precision loss.          |
| System.UInt64                          | ulong            | number     | The conversion can lead to a silent precision loss.          |
| System.Single                          | float            | number     |                                                              |
| System.Decimal                         | decimal          | number     | The conversion can lead to a silent precision loss.          |
| System.Double                          | double           | number     |                                                              |
| System.Boolean                         | bool             | boolean    |                                                              |
| System.String                          | string           | string     |                                                              |
| System.Text.StringBuilder              |                  | string     |                                                              |
| System.Char                            | char             | string     |                                                              |
| MoonSharp.Interpreter.Table            |                  | table      |                                                              |
| MoonSharp.Interpreter.CallbackFunction |                  | function   |                                                              |
| System.Delegate                        |                  | function   |                                                              |
| System.Object                          | object           | userdata   | Only if the type has been registered for userdata.           |
| System.Type                            |                  | userdata   | Only if the type has been registered for userdata, static members access. |
| MoonSharp.Interpreter.Closure          |                  | function   |                                                              |
| System.Reflection.MethodInfo           |                  | function   |                                                              |
| System.Collections.IList               |                  | table      | The resulting table will be indexed 1-based. All values are converted using these rules. |
| System.Collections.IDictionary         |                  | table      | All keys and values are converted using these rules.         |
| System.Collections.IEnumerable         |                  | iterator   | All values are converted using these rules.                  |
| System.Collections.IEnumerator         |                  | iterator   | All values are converted using these rules.                  |









## Lua Environment

* Currently going with a [workspace library](https://github.com/sumneko/lua-language-server/wiki/Libraries) for providing definitions without `require`.  
  * Other options exist and performance can be improved by modularizing.




### [Annotations](https://github.com/sumneko/lua-language-server/wiki/Annotations)

* `---@meta` Marks a file to be used for definitions instead of functional Lua code.

* Basic Types

  * `nil` `any` `boolean` `string` `number` `integer`
  * `function` `table` `thread` `userdata` `lightuserdata`

* Classes/Structs

  * Used with [`@field`](#@field) to define a table structure.  Once defined used for parameters, returns, etc

  * `---@class <name>`

  * ```
    ---@type Car
    local Car = {}
    ```

* Fields

  * Defined field within a table.  Should be after an @class

  * `---@field <name> <type> [description]`

  * ```
    ---@class Person
    ---@field height number The height of this person in cm
    ---@field weight number The weight of this person in kg
    ---@field firstName string The first name of this person
    ---@field lastName string The last name of this person
    ---@field age integer The age of this person
    
    ---@param person Person
    local function hire(person) end
    ```

  

Documenting Types

Properly documenting types with the language server is very important and where a lot of the features and advantages are. Below is a list of  all recognized Lua types (regardless of [version in use](https://github.com/sumneko/lua-language-server/wiki/Settings#runtimeversion)):

- `nil`
- `any`
- `boolean`
- `string`
- `number`
- `integer`
- `function`
- `table`
- `thread`
- `userdata`
- `lightuserdata`

You can also simulate [classes](https://github.com/sumneko/lua-language-server/wiki/Annotations#class) and [fields](https://github.com/sumneko/lua-language-server/wiki/Annotations#field) and even [create your own types](https://github.com/sumneko/lua-language-server/wiki/Annotations#alias).

Below is a list of how you can document more advanced types:

| Type            | Document As                              |
| --------------- | ---------------------------------------- |
| Union Type      | `TYPE_1|TYPE_2`                          |
| Array           | `<VALUE_TYPE>[]`                         |
| Dictionary      | `{ [string]: VALUE_TYPE }`               |
| Key-Value Table | `table<KEY_TYPE, VALUE_TYPE>`            |
| Table Literal   | `{ key1: VALUE_TYPE, key2: VALUE_TYPE }` |
| Function        | `fun(PARAM: TYPE): RETURN_TYPE`          |



### [`@type`](#Annotations)

Mark a variable as being of a certain type. Union types are separated with a pipe character `|`. The `type` provided can be an [`@alias`](https://github.com/sumneko/lua-language-server/wiki/Annotations#alias), [`@enum`](https://github.com/sumneko/lua-language-server/wiki/Annotations#enum), or [`@class`](https://github.com/sumneko/lua-language-server/wiki/Annotations#class) as well.

##### **Syntax**

```
---@type <type>
```



Basic Type Definition

```
---@type boolean
local x
```

Union Type Definition

```
---@type boolean|number
local x
```

Array Type Definition

```
---@type string[]
local names
```

Dictionary Type Definition

```
---@type { [string] = boolean }
local statuses
```

Table Type Definition

```
---@type table<userID, Player>
local players
```

Enum Type Definition

```
---@type "red"|"green"|"blue"|nil
local color
```

Union Type Definition

```
---@type boolean|number
local x
```

Function Type Definition

```
---@type fun(name: string, value: any): boolean
local x
```



### [`@param`](#Annotations)

Define a parameter for a function. This tells the language server  what types are expected and can help enforce types and provide  completion. Putting a question mark (`?`) after the parameter name will mark it as optional, meaning `nil` is an accepted type. The `type` provided can be an [`@alias`](https://github.com/sumneko/lua-language-server/wiki/Annotations#alias), [`@enum`](https://github.com/sumneko/lua-language-server/wiki/Annotations#enum), or [`@class`](https://github.com/sumneko/lua-language-server/wiki/Annotations#class) as well.

##### **Syntax**

```
---@param <name[?]> <type[|type...]> [description]
```



Simple Function Parameter

```
---@param username string The name to set for this user
function setUsername(username) end
```

Parameter Union Type

```
---@param setting string The name of the setting
---@param value string|number|boolean The value of the setting
local function settings.set(setting, value) end
```

Optional Parameter

```
---@param req string Required parameter
---@param optional? Type Description
---@return Role
function Role.new(role, isActive) end
```

Variable Number of Parameters

```
---@param index integer
---@param ... string Tags to add to this entry
local function addTags(index, ...) end
```

Generic Function Parameter

```
---@class Box

---@generic T
---@param objectID integer The ID of the object to set the type of
---@param type `T` The type of object to set
---@return `T` object The object as a Lua object
local function setObjectType(objectID, type) end

--> boxObject: Box
local boxObject = setObjectType(1, "Box")
```

See [@generic](#@generic) for more info.



### [`@return`](#Annotations)

Define a `return` value for a function. This tells the language server what types are expected and can help enforce types and provide completion.

##### **Syntax**

```
---@return <type> [<name> [comment] | [name] #<comment>]
```



Simple Function Return

```
---@return boolean
local function isEnabled() end
```

Named Function Return

```
---@return boolean enabled
local function isEnabled() end
```

Named, Described Function Return

```
---@return boolean enabled If the item is enabled
local function isEnabled() end
```

Described Function Return

```
---@return boolean # If the item is enabled
local function isEnabled() end
```

Optional Function Return

```
---@return boolean|nil error
local function makeRequest() end
```

Variable Function Returns

```
---@return integer count Number of nicknames found
---@return string ...
local function getNicknames() end
```



### [`@class`](#Annotations)

Define a class. Can be used with [`@field`](https://github.com/sumneko/lua-language-server/wiki/Annotations#field) to define a table structure. Once a class is defined, it can be used as a type for [parameters](https://github.com/sumneko/lua-language-server/wiki/Annotations#param), [returns](https://github.com/sumneko/lua-language-server/wiki/Annotations#return), and more.

##### **Syntax**

```
---@class <name>
```



```
---@class Car
local Car = {}
```



### [`@enum`](#Annotations)

Mark a Lua table as an enum, giving it similar functionality to [`@alias`](#@alias).

[View Original Request](https://github.com/sumneko/lua-language-server/issues/1255)

##### **Syntax**

```
---@enum <name>
```



```
---@class Person
---@field height number The height of this person in cm
---@field weight number The weight of this person in kg
---@field firstName string The first name of this person
---@field lastName string The last name of this person
---@field age integer The age of this person

---@param person Person
local function hire(person) end
```





### [`@field`](#Annotations)

Define a field within a table. Should be immediately following a [`@class`](https://github.com/sumneko/lua-language-server/wiki/Annotations#class).

##### **Syntax**

```
---@field <name> <type> [description]
```

**Examples**

<details open="">
<summary>Simple documentation of class</summary>
<div class="highlight highlight-source-lua notranslate position-relative overflow-auto"><pre><span class="pl-c"><span class="pl-c">--</span>-@class Person</span>
<span class="pl-c"><span class="pl-c">--</span>-@field height number The height of this person in cm</span>
<span class="pl-c"><span class="pl-c">--</span>-@field weight number The weight of this person in kg</span>
<span class="pl-c"><span class="pl-c">--</span>-@field firstName string The first name of this person</span>
<span class="pl-c"><span class="pl-c">--</span>-@field lastName string The last name of this person</span>
<span class="pl-c"><span class="pl-c">--</span>-@field age integer The age of this person</span>


<span class="pl-c"><span class="pl-c">--</span>-@param person Person</span>
<span class="pl-k">local</span> <span class="pl-k">function</span> <span class="pl-en">hire</span>(<span class="pl-smi">person</span>) <span class="pl-k">end</span></pre></div>
</details>

![field](https://user-images.githubusercontent.com/61925890/181307814-81f14004-db8a-4f17-af40-03dd27673648.gif)      



### [`@generic`](#Annotations)

Generics allow code to be reused and serve as a sort of "placeholder" for a type. Surrounding the generic in backticks (```) will capture the value and use it for the type.

##### **Syntax**

```
---@generic <name> [:parent_type] [, <name> [:parent_type]]
```



Generic Function

```
---@generic T : integer
---@param p1 T
---@return T, T[]
function Generic(p1) end

-- v1: string
-- v2: string[]
local v1, v2 = Generic("String")

-- v3: integer
-- v4: integer[]
local v3, v4 = Generic(10)
```

Capture with Backticks

```
---@class Vehicle
local Vehicle = {}
function Vehicle:drive() end

---@generic T
---@param class `T` # the type is captured using `T`
---@return T       # generic type is returned
local function new(class) end

-- obj: Vehicle
local obj = new("Vehicle")
```

How the Table Class is Implemented

```
---@class table<K, V>: { [K]: V }
```

Array Class Using Generics

```
---@class Array<T>: { [integer]: T }

---@type Array<string>
local arr = {}

-- Warns that I am assigning a boolean to a string
arr[1] = false

arr[3] = "Correct"
```

Dictionary class using generics

```
---@class Dictionary<T>: { [string]: T }

---@type Dictionary<boolean>
local dict = {}

-- no warning despite assigning a string
dict["foo"] = "bar?"

dict["correct"] = true
```



### [`@alias`](#Annotations)

An alias can be useful when re-using a type. It can also be used to  provide an enum. If you are looking for an enum and already have the  values defined in a Lua table, take a look at [`@enum`](#@enum).

##### **Syntax**

```
---@alias <name> <type>
```

or

```
---@alias <name>
---| '<value>' [# description]
```



Simple Alias

```
---@alias userID integer The ID of a user
```

Enum

```
---@alias modes "r" | "w"
```

Enum with Descriptions

```
---@alias side
---| '"left"' # The left side of the device
---| '"right"' # The right side of the device
---| '"top"' # The top side of the device
---| '"bottom"' # The bottom side of the device
---| '"front"' # The front side of the device
---| '"back"' # The back side of the device

---@param side side
local function checkSide(side) end
```





### [`@source`](#Annotations)

Document the source for a piece of code

**Syntax**

```
---@source <url|uri>
```

**Examples**

<details>
<summary>Link to a Website</summary>


</details>

<details>
<summary>Link to a file</summary>


</details>



### [`@meta`](#Annotations)

Marks a file as "meta", meaning it is used for definitions and not  for its functional Lua code. Used internally by the language server for  defining the built-in Lua libraries. If you are [writing your own library definition files](https://github.com/sumneko/lua-language-server/wiki/Libraries#custom), you will probably want to include this annotation in them. Files with the `@meta` tag in them behave a little different:

- Completion will not display context in a meta file
- Hovering a `require` of a meta file will show `[meta]` instead of its absolute path
- `Find Reference` ignores meta files

**Syntax**

```
---@meta
```



### [`@async`](#Annotations)

Mark a function as being asynchronous. When [`hint.await`](https://github.com/sumneko/lua-language-server/wiki/Settings#hintawait) is `true`, functions marked with `@async` will have an `await` hint displayed next to them. Used by diagnostics from the [`await` group](https://github.com/sumneko/lua-language-server/wiki/Diagnostics#await).

**Syntax**

```
---@async
```

**Examples**

<details>
<summary>Asynchronous Declaration</summary>


</details>

![async](https://user-images.githubusercontent.com/61925890/181307740-22ad1d85-f132-492a-a2c9-c7969cc53637.png)



### [`@module`](#Annotations)

Simulates `require`-ing a file.

**Syntax**

```
---@module '<module_name>'
```

**Examples**

<details>
<summary>"Require" a File</summary>


</details>

<details>
<summary>"Require" a File and Assign to a Variable</summary>


</details>



### [`@overload`](#Annotations)

Define an additional signature for a function. This does not allow  descriptions to be provided for the new signature being defined - if you want descriptions, you are better off writing out an entire `function` with the same name but different [`@param`](https://github.com/sumneko/lua-language-server/wiki/Annotations#param) and [`@return`](https://github.com/sumneko/lua-language-server/wiki/Annotations#return) annotations.

**Syntax**

```
---@overload fun([param: type[, param: type...]]): [return_value[, return_value]]
```

**Examples**

<details>
<summary>Define Function Overload</summary>


</details>

