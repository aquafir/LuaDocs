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
local FakeClass = { }
_FakeClass = { }

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

---Empty constructor
---@return FakeClass # 
_FakeClass.new = function() end

---Constructor with parameters
---@param a FakeClass A.  B left empty
---@param b string 
---@return FakeClass # 
_FakeClass.new = function(a,b) end

---Overload constructor with params
---@param ... number 
---@return FakeClass # 
_FakeClass.new = function(...) end

---Constructor with a default parameter
---@param number number 
---@param defaultString? string 
---@return FakeClass # 
_FakeClass.new = function(number,defaultString) end

---@type string Private static field
_FakeClass._STcustomBackingField = nil
---@type string Public static field
_FakeClass.STPublicField = nil
---@type string Static readonly field
_FakeClass.STREADONLY_FIELD = nil
---@type string Constant field
_FakeClass.STCONST_FIELD = "Constant field"
---@type string public static property
_FakeClass.STPublicProperty = nil
---@type string Get-only static property
_FakeClass.STGetOnlyProperty = nil
---@type string Static property with auto-generated backing field
_FakeClass.STAutoProperty = nil
---Void method with blank parameter documentation
---@param c string Description of parameter c
_FakeClass.TestVoid = function(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
_FakeClass.TestMethod = function(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
_FakeClass.TestMethodEnum = function(e) end

---@param a string 
---@param b string 
---@return string # 
_FakeClass.TestAbstract = function(a,b) end

_FakeClass.InterfaceMethod = function() end

---Static void method with blank parameter documentation
---@param c string Description of parameter c
_FakeClass.STTestVoid = function(c) end

---Adds two strings
---@param a string The first string
---@param b? string Second string
---@return string # Concatenation of two strings
_FakeClass.STTestMethod = function(a,b) end

---@param e FakeEnum 
---@return FakeEnum # 
_FakeClass.STTestMethodEnum = function(e) end

---@return string # 
_FakeClass.Virtual = function() end

---@return string # 
_FakeClass.ToString = function() end

---@param obj userdata 
---@return boolean # 
_FakeClass.Equals = function(obj) end

---@return number # 
_FakeClass.GetHashCode = function() end

---@return userdata # 
_FakeClass.GetType = function() end

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
local FakeEvent = { }
_FakeEvent = { }

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

---Constructor of fake event
---@param text string Text passed along on raised event
---@return FakeEvent # 
_FakeEvent.new = function(text) end

_FakeEvent.AddEvent = function() end

---@return string # 
_FakeEvent.ToString = function() end

---@param obj userdata 
---@return boolean # 
_FakeEvent.Equals = function(obj) end

---@return number # 
_FakeEvent.GetHashCode = function() end

---@return userdata # 
_FakeEvent.GetType = function() end

---Structure for coordinates
---@class Coords
---@field X number X coord
---@field Y number 
local Coords = { }
_Coords = { }

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

---Constructor for coordinate struct
---@param x number X coord
---@param y number Y coord
---@return Coords # 
_Coords.new = function(x,y) end

---ToString override for coordinates
---@return string # Formatted coordinates
_Coords.ToString = function() end

---@param obj userdata 
---@return boolean # 
_Coords.Equals = function(obj) end

---@return number # 
_Coords.GetHashCode = function() end

---@return userdata # 
_Coords.GetType = function() end

---
---@class IAbstract
local IAbstract = { }
_IAbstract = { }

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

---@return string # 
_IAbstract.Virtual = function() end

---@param a string 
---@param b string 
---@return string # 
_IAbstract.TestAbstract = function(a,b) end

---@return string # 
_IAbstract.ToString = function() end

---@param obj userdata 
---@return boolean # 
_IAbstract.Equals = function(obj) end

---@return number # 
_IAbstract.GetHashCode = function() end

---@return userdata # 
_IAbstract.GetType = function() end

---
---@class IInterface
local IInterface = { }
_IInterface = { }

function IInterface:InterfaceMethod() end

_IInterface.InterfaceMethod = function() end

---
---@class UndocumentedClass
---@field Field string 
---@field Property string 
local UndocumentedClass = { }
_UndocumentedClass = { }

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

---@return UndocumentedClass # 
_UndocumentedClass.new = function() end

---@param Param string 
---@param ... number 
---@return FakeClass # 
_UndocumentedClass.Method = function(Param,...) end

---@return string # 
_UndocumentedClass.ToString = function() end

---@param obj userdata 
---@return boolean # 
_UndocumentedClass.Equals = function(obj) end

---@return number # 
_UndocumentedClass.GetHashCode = function() end

---@return userdata # 
_UndocumentedClass.GetType = function() end

---Nested class
---@class NestedClass
---@field _fakeClass FakeClass Nested class private field
local NestedClass = { }
_NestedClass = { }

---@return string # 
function NestedClass:ToString() end

---@param obj userdata 
---@return boolean # 
function NestedClass:Equals(obj) end

---@return number # 
function NestedClass:GetHashCode() end

---@return userdata # 
function NestedClass:GetType() end

---Nested class constructor
---@param s string S
---@param fc? FakeClass Accepts fake class
---@return NestedClass # 
_NestedClass.new = function(s,fc) end

---@return string # 
_NestedClass.ToString = function() end

---@param obj userdata 
---@return boolean # 
_NestedClass.Equals = function(obj) end

---@return number # 
_NestedClass.GetHashCode = function() end

---@return userdata # 
_NestedClass.GetType = function() end

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
local FakeEventPublisher = { }
_FakeEventPublisher = { }

---@return string # 
function FakeEventPublisher:ToString() end

---@param obj userdata 
---@return boolean # 
function FakeEventPublisher:Equals(obj) end

---@return number # 
function FakeEventPublisher:GetHashCode() end

---@return userdata # 
function FakeEventPublisher:GetType() end

---@param value FakeEventHandler 
function FakeEventPublisher:add_FakeEvent(value) end

---@param value FakeEventHandler 
function FakeEventPublisher:remove_FakeEvent(value) end

---@return FakeEventPublisher # 
_FakeEventPublisher.new = function() end

---@return string # 
_FakeEventPublisher.ToString = function() end

---@param obj userdata 
---@return boolean # 
_FakeEventPublisher.Equals = function(obj) end

---@return number # 
_FakeEventPublisher.GetHashCode = function() end

---@return userdata # 
_FakeEventPublisher.GetType = function() end

---Some fake event handler
---@class FakeEventHandler
---@field _target userdata 
---@field _methodBase MethodBase 
---@field _methodPtr IntPtr 
---@field _methodPtrAux IntPtr 
---@field Method function 
---@field Target userdata 
local FakeEventHandler = { }
_FakeEventHandler = { }

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

---@param object userdata 
---@param method IntPtr 
---@return FakeEventHandler # 
_FakeEventHandler.new = function(object,method) end

---@param sender userdata 
---@param e FakeEvent 
_FakeEventHandler.Invoke = function(sender,e) end

---@param sender userdata 
---@param e FakeEvent 
---@param callback AsyncCallback 
---@param object userdata 
---@return IAsyncResult # 
_FakeEventHandler.BeginInvoke = function(sender,e,callback,object) end

---@param result IAsyncResult 
_FakeEventHandler.EndInvoke = function(result) end

---@param info SerializationInfo 
---@param context StreamingContext 
_FakeEventHandler.GetObjectData = function(info,context) end

---@param obj userdata 
---@return boolean # 
_FakeEventHandler.Equals = function(obj) end

---@return function[] # 
_FakeEventHandler.GetInvocationList = function() end

---@return number # 
_FakeEventHandler.GetHashCode = function() end

---@param ... userdata 
---@return userdata # 
_FakeEventHandler.DynamicInvoke = function(...) end

---@return userdata # 
_FakeEventHandler.Clone = function() end

---@return string # 
_FakeEventHandler.ToString = function() end

---@return userdata # 
_FakeEventHandler.GetType = function() end

