---@meta
---@class FakeClass
---@field _customBackingField string Private field
---@field PublicField string Public field
---@field READONLY_FIELD string 
---@field _nestedClassInstance NestedClass 
---@field PublicProperty string Public property
---@field GetOnlyProperty string Get-only property
---@field AutoProperty string Property with auto-generated backing field
local FakeClass = { }

function FakeClass:WireEvent() end

---@param c string Description of parameter c
function FakeClass:TestVoid(c) end

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

---@return FakeClass # 
function FakeClass:new() end

---@param a FakeClass A.  B left empty
---@param b string 
---@return FakeClass # 
function FakeClass:new(a,b) end

---@param intParam number[]... 
---@return FakeClass # 
function FakeClass:new(intParam) end

---@param defaultString? string 
---@return FakeClass # 
function FakeClass:new(defaultString) end



FakeClass.new()