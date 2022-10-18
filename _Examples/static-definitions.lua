FakeClass = {}
---Empty constructor
---@return FakeClass # 
function FakeClass:new() end

---Constructor with parameters
---@param a FakeClass A.  B left empty
---@param b string 
---@return FakeClass # 
function FakeClass:new(a,b) end

---Overload constructor with params
---@param ... number 
---@return FakeClass # 
function FakeClass:new(...) end

---Constructor with a default parameter
---@param number number 
---@param defaultString? string 
---@return FakeClass # 
function FakeClass:new(number,defaultString) end

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
function FakeClass:TestVoid(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
function FakeClass:TestMethod(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
function FakeClass:TestMethodEnum(e) end

---@param a string 
---@param b string 
---@return string # 
function FakeClass:TestAbstract(a,b) end

function FakeClass:InterfaceMethod() end

---Static void method with blank parameter documentation
---@param c string Description of parameter c
function FakeClass:STTestVoid(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
function FakeClass:STTestMethod(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
function FakeClass:STTestMethodEnum(e) end

---@return string # 
function FakeClass:Virtual() end

---@return string # 
function FakeClass:ToString() end

---@param obj userdata 
---@return boolean # 
function FakeClass:Equals(obj) end

---@return number # 
function FakeClass:GetHashCode() end

---@return userdata # 
function FakeClass:GetType() end


FakeEvent = {}
---Constructor of fake event
---@param text string Text passed along on raised event
---@return FakeEvent # 
function FakeEvent:new(text) end

function FakeEvent:AddEvent() end

---@return string # 
function FakeEvent:ToString() end

---@param obj userdata 
---@return boolean # 
function FakeEvent:Equals(obj) end

---@return number # 
function FakeEvent:GetHashCode() end

---@return userdata # 
function FakeEvent:GetType() end


Coords = {}
---Constructor for coordinate struct
---@param x number X coord
---@param y number Y coord
---@return Coords # 
function Coords:new(x,y) end

---ToString override for coordinates
---@return string # Formatted coordinates
function Coords:ToString() end

---@param obj userdata 
---@return boolean # 
function Coords:Equals(obj) end

---@return number # 
function Coords:GetHashCode() end

---@return userdata # 
function Coords:GetType() end


IAbstract = {}
---@return string # 
function IAbstract:Virtual() end

---@param a string 
---@param b string 
---@return string # 
function IAbstract:TestAbstract(a,b) end

---@return string # 
function IAbstract:ToString() end

---@param obj userdata 
---@return boolean # 
function IAbstract:Equals(obj) end

---@return number # 
function IAbstract:GetHashCode() end

---@return userdata # 
function IAbstract:GetType() end


IInterface = {}
function IInterface:InterfaceMethod() end


UndocumentedClass = {}
---@return UndocumentedClass # 
function UndocumentedClass:new() end

---@param Param string 
---@param ... number 
---@return FakeClass # 
function UndocumentedClass:Method(Param,...) end

---@return string # 
function UndocumentedClass:ToString() end

---@param obj userdata 
---@return boolean # 
function UndocumentedClass:Equals(obj) end

---@return number # 
function UndocumentedClass:GetHashCode() end

---@return userdata # 
function UndocumentedClass:GetType() end


NestedClass = {}
---Nested class constructor
---@param s string S
---@param fc? FakeClass Accepts fake class
---@return NestedClass # 
function NestedClass:new(s,fc) end

---@return string # 
function NestedClass:ToString() end

---@param obj userdata 
---@return boolean # 
function NestedClass:Equals(obj) end

---@return number # 
function NestedClass:GetHashCode() end

---@return userdata # 
function NestedClass:GetType() end


FakeEventPublisher = {}
---@return FakeEventPublisher # 
function FakeEventPublisher:new() end

---@return string # 
function FakeEventPublisher:ToString() end

---@param obj userdata 
---@return boolean # 
function FakeEventPublisher:Equals(obj) end

---@return number # 
function FakeEventPublisher:GetHashCode() end

---@return userdata # 
function FakeEventPublisher:GetType() end


FakeEventHandler = {}
---@param object userdata 
---@param method IntPtr 
---@return FakeEventHandler # 
function FakeEventHandler:new(object,method) end

---@param sender userdata 
---@param e FakeEvent 
function FakeEventHandler:Invoke(sender,e) end

---@param sender userdata 
---@param e FakeEvent 
---@param callback AsyncCallback 
---@param object userdata 
---@return IAsyncResult # 
function FakeEventHandler:BeginInvoke(sender,e,callback,object) end

---@param result IAsyncResult 
function FakeEventHandler:EndInvoke(result) end

---@param info SerializationInfo 
---@param context StreamingContext 
function FakeEventHandler:GetObjectData(info,context) end

---@param obj userdata 
---@return boolean # 
function FakeEventHandler:Equals(obj) end

---@return function[] # 
function FakeEventHandler:GetInvocationList() end

---@return number # 
function FakeEventHandler:GetHashCode() end

---@param ... userdata 
---@return userdata # 
function FakeEventHandler:DynamicInvoke(...) end

---@return userdata # 
function FakeEventHandler:Clone() end

---@return string # 
function FakeEventHandler:ToString() end

---@return userdata # 
function FakeEventHandler:GetType() end

