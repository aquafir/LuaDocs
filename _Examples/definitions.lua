---@meta
---Fake class to cover cases
---@class FakeClass
---@field _customBackingField string Private field
---@field PublicField string Public field
---@field READONLY_FIELD string Read-only instance field
---@field _nestedClassInstance NestedClass 
---@field PublicProperty string Public property
---@field GetOnlyProperty string Get-only property
---@field AutoProperty string Property with auto-generated backing field
---@type FakeClass
local _FakeClass = { }
FakeClass = { }

---Void method with blank parameter documentation
---@param c string Description of parameter c
function _FakeClass:TestVoid(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
function _FakeClass:TestMethod(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
function _FakeClass:TestMethodEnum(e) end

---@param a string 
---@param b string 
---@return string # 
function _FakeClass:TestAbstract(a,b) end

function _FakeClass:InterfaceMethod() end

---Static void method with blank parameter documentation
---@param c string Description of parameter c
function _FakeClass:STTestVoid(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
function _FakeClass:STTestMethod(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
function _FakeClass:STTestMethodEnum(e) end

---@return string # 
function _FakeClass:Virtual() end

---@return string # 
function _FakeClass:ToString() end

---@param obj userdata 
---@return boolean # 
function _FakeClass:Equals(obj) end

---@return number # 
function _FakeClass:GetHashCode() end

---@return userdata # 
function _FakeClass:GetType() end

---Empty constructor
---@return FakeClass # 
FakeClass.new = function() end

---Constructor with parameters
---@param a FakeClass A.  B left empty
---@param b string 
---@return FakeClass # 
FakeClass.new = function(a,b) end

---Overload constructor with params
---@param ... number 
---@return FakeClass # 
FakeClass.new = function(...) end

---Constructor with a default parameter
---@param number number 
---@param defaultString? string 
---@return FakeClass # 
FakeClass.new = function(number,defaultString) end

---@type string Private static field
FakeClass._STcustomBackingField = nil
---@type string Public static field
FakeClass.STPublicField = nil
---@type string Static readonly field
FakeClass.STREADONLY_FIELD = nil
---@type string Constant field
FakeClass.STCONST_FIELD = "Constant field"
---@type string public static property
FakeClass.STPublicProperty = nil
---@type string Get-only static property
FakeClass.STGetOnlyProperty = nil
---@type string Static property with auto-generated backing field
FakeClass.STAutoProperty = nil
---Void method with blank parameter documentation
---@param c string Description of parameter c
FakeClass.TestVoid = function(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
FakeClass.TestMethod = function(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
FakeClass.TestMethodEnum = function(e) end

---@param a string 
---@param b string 
---@return string # 
FakeClass.TestAbstract = function(a,b) end

FakeClass.InterfaceMethod = function() end

---Static void method with blank parameter documentation
---@param c string Description of parameter c
FakeClass.STTestVoid = function(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
FakeClass.STTestMethod = function(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
FakeClass.STTestMethodEnum = function(e) end

---@return string # 
FakeClass.Virtual = function() end

---@return string # 
FakeClass.ToString = function() end

---@param obj userdata 
---@return boolean # 
FakeClass.Equals = function(obj) end

---@return number # 
FakeClass.GetHashCode = function() end

---@return userdata # 
FakeClass.GetType = function() end

----Enum----T:FakeProject.NonNestedEnum
---Enum outside of class
---@enum NonNestedEnum
NonNestedEnum = { -- Enum outside of class
	A = 0, -- A is first
	B = 1, -- B is second
	C = 2, -- C is third
}
---
---@class FakeEvent
---@field Text string Text of event
---@type FakeEvent
local _FakeEvent = { }
FakeEvent = { }

function _FakeEvent:AddEvent() end

---@return string # 
function _FakeEvent:ToString() end

---@param obj userdata 
---@return boolean # 
function _FakeEvent:Equals(obj) end

---@return number # 
function _FakeEvent:GetHashCode() end

---@return userdata # 
function _FakeEvent:GetType() end

---Constructor of fake event
---@param text string Text passed along on raised event
---@return FakeEvent # 
FakeEvent.new = function(text) end

FakeEvent.AddEvent = function() end

---@return string # 
FakeEvent.ToString = function() end

---@param obj userdata 
---@return boolean # 
FakeEvent.Equals = function(obj) end

---@return number # 
FakeEvent.GetHashCode = function() end

---@return userdata # 
FakeEvent.GetType = function() end

---Structure for coordinates
---@class Coords
---@field X number X coord
---@field Y number 
---@type Coords
local _Coords = { }
Coords = { }

---ToString override for coordinates
---@return string # Formatted coordinates
function _Coords:ToString() end

---@param obj userdata 
---@return boolean # 
function _Coords:Equals(obj) end

---@return number # 
function _Coords:GetHashCode() end

---@return userdata # 
function _Coords:GetType() end

---Constructor for coordinate struct
---@param x number X coord
---@param y number Y coord
---@return Coords # 
Coords.new = function(x,y) end

---ToString override for coordinates
---@return string # Formatted coordinates
Coords.ToString = function() end

---@param obj userdata 
---@return boolean # 
Coords.Equals = function(obj) end

---@return number # 
Coords.GetHashCode = function() end

---@return userdata # 
Coords.GetType = function() end

---
---@class IAbstract
---@type IAbstract
local _IAbstract = { }
IAbstract = { }

---@return string # 
function _IAbstract:Virtual() end

---@param a string 
---@param b string 
---@return string # 
function _IAbstract:TestAbstract(a,b) end

---@return string # 
function _IAbstract:ToString() end

---@param obj userdata 
---@return boolean # 
function _IAbstract:Equals(obj) end

---@return number # 
function _IAbstract:GetHashCode() end

---@return userdata # 
function _IAbstract:GetType() end

---@return string # 
IAbstract.Virtual = function() end

---@param a string 
---@param b string 
---@return string # 
IAbstract.TestAbstract = function(a,b) end

---@return string # 
IAbstract.ToString = function() end

---@param obj userdata 
---@return boolean # 
IAbstract.Equals = function(obj) end

---@return number # 
IAbstract.GetHashCode = function() end

---@return userdata # 
IAbstract.GetType = function() end

---
---@class IInterface
---@type IInterface
local _IInterface = { }
IInterface = { }

function _IInterface:InterfaceMethod() end

IInterface.InterfaceMethod = function() end

---
---@class UndocumentedClass
---@field Field string 
---@field Property string 
---@type UndocumentedClass
local _UndocumentedClass = { }
UndocumentedClass = { }

---@param Param string 
---@param ... number 
---@return FakeClass # 
function _UndocumentedClass:Method(Param,...) end

---@return string # 
function _UndocumentedClass:ToString() end

---@param obj userdata 
---@return boolean # 
function _UndocumentedClass:Equals(obj) end

---@return number # 
function _UndocumentedClass:GetHashCode() end

---@return userdata # 
function _UndocumentedClass:GetType() end

---@return UndocumentedClass # 
UndocumentedClass.new = function() end

---@param Param string 
---@param ... number 
---@return FakeClass # 
UndocumentedClass.Method = function(Param,...) end

---@return string # 
UndocumentedClass.ToString = function() end

---@param obj userdata 
---@return boolean # 
UndocumentedClass.Equals = function(obj) end

---@return number # 
UndocumentedClass.GetHashCode = function() end

---@return userdata # 
UndocumentedClass.GetType = function() end

---Nested class
---@class NestedClass
---@field _fakeClass FakeClass Nested class private field
---@type NestedClass
local _NestedClass = { }
NestedClass = { }

---@return string # 
function _NestedClass:ToString() end

---@param obj userdata 
---@return boolean # 
function _NestedClass:Equals(obj) end

---@return number # 
function _NestedClass:GetHashCode() end

---@return userdata # 
function _NestedClass:GetType() end

---Nested class constructor
---@param s string S
---@param fc? FakeClass Accepts fake class
---@return NestedClass # 
NestedClass.new = function(s,fc) end

---@return string # 
NestedClass.ToString = function() end

---@param obj userdata 
---@return boolean # 
NestedClass.Equals = function(obj) end

---@return number # 
NestedClass.GetHashCode = function() end

---@return userdata # 
NestedClass.GetType = function() end

----Enum----T:FakeProject.FakeClass.FakeEnum
---Fake enum desc
---@enum FakeEnum
FakeEnum = { -- Fake enum desc
	A = 0, -- First value of nested enum
	B = 1, -- Second value of nested enum
}
----Enum----T:FakeProject.FakeClass.EnumByte
---Fake nested byte enum desc
---@enum EnumByte
EnumByte = { -- Fake nested byte enum desc
	C = 0, -- First byte value
	D = 1, -- Second byte value
}
----Enum----T:FakeProject.FakeClass.EnumFlag
---Enum with flags attribute
---@enum EnumFlag
EnumFlag = { -- Enum with flags attribute
	Red = 1, -- Red bit
	Green = 2, -- Green bit
	Blue = 4, -- Blue bit
}
---Published of fake events
---@class FakeEventPublisher
---@type FakeEventPublisher
local _FakeEventPublisher = { }
FakeEventPublisher = { }

---@return string # 
function _FakeEventPublisher:ToString() end

---@param obj userdata 
---@return boolean # 
function _FakeEventPublisher:Equals(obj) end

---@return number # 
function _FakeEventPublisher:GetHashCode() end

---@return userdata # 
function _FakeEventPublisher:GetType() end

---@param value FakeEventHandler 
function _FakeEventPublisher:add_FakeEvent(value) end

---@param value FakeEventHandler 
function _FakeEventPublisher:remove_FakeEvent(value) end

---@return FakeEventPublisher # 
FakeEventPublisher.new = function() end

---@return string # 
FakeEventPublisher.ToString = function() end

---@param obj userdata 
---@return boolean # 
FakeEventPublisher.Equals = function(obj) end

---@return number # 
FakeEventPublisher.GetHashCode = function() end

---@return userdata # 
FakeEventPublisher.GetType = function() end

---Some fake event handler
---@class FakeEventHandler
---@field _target userdata 
---@field _methodBase MethodBase 
---@field _methodPtr IntPtr 
---@field _methodPtrAux IntPtr 
---@field Method function 
---@field Target userdata 
---@type FakeEventHandler
local _FakeEventHandler = { }
FakeEventHandler = { }

---@param sender userdata 
---@param e FakeEvent 
function _FakeEventHandler:Invoke(sender,e) end

---@param sender userdata 
---@param e FakeEvent 
---@param callback AsyncCallback 
---@param object userdata 
---@return IAsyncResult # 
function _FakeEventHandler:BeginInvoke(sender,e,callback,object) end

---@param result IAsyncResult 
function _FakeEventHandler:EndInvoke(result) end

---@param info SerializationInfo 
---@param context StreamingContext 
function _FakeEventHandler:GetObjectData(info,context) end

---@param obj userdata 
---@return boolean # 
function _FakeEventHandler:Equals(obj) end

---@return function[] # 
function _FakeEventHandler:GetInvocationList() end

---@return number # 
function _FakeEventHandler:GetHashCode() end

---@param ... userdata 
---@return userdata # 
function _FakeEventHandler:DynamicInvoke(...) end

---@return userdata # 
function _FakeEventHandler:Clone() end

---@return string # 
function _FakeEventHandler:ToString() end

---@return userdata # 
function _FakeEventHandler:GetType() end

---@param object userdata 
---@param method IntPtr 
---@return FakeEventHandler # 
FakeEventHandler.new = function(object,method) end

---@param sender userdata 
---@param e FakeEvent 
FakeEventHandler.Invoke = function(sender,e) end

---@param sender userdata 
---@param e FakeEvent 
---@param callback AsyncCallback 
---@param object userdata 
---@return IAsyncResult # 
FakeEventHandler.BeginInvoke = function(sender,e,callback,object) end

---@param result IAsyncResult 
FakeEventHandler.EndInvoke = function(result) end

---@param info SerializationInfo 
---@param context StreamingContext 
FakeEventHandler.GetObjectData = function(info,context) end

---@param obj userdata 
---@return boolean # 
FakeEventHandler.Equals = function(obj) end

---@return function[] # 
FakeEventHandler.GetInvocationList = function() end

---@return number # 
FakeEventHandler.GetHashCode = function() end

---@param ... userdata 
---@return userdata # 
FakeEventHandler.DynamicInvoke = function(...) end

---@return userdata # 
FakeEventHandler.Clone = function() end

---@return string # 
FakeEventHandler.ToString = function() end

---@return userdata # 
FakeEventHandler.GetType = function() end

