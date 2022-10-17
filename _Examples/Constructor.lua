---@class SomeClass
local SomeClass = {}
function SomeClass:SomeClassMethod() end

---@generic T
---@param class `T` # the type is captured using `T`
---@return T       # generic type is returned
local function new(class) end

local obj = new("SomeClass")
obj.SomeClassMethod()


---@class SomeClass
---@field someField string Field description

---@type SomeClass
local SomeClass = {}

---@return SomeClass
SomeClass.new = function() end

local constructedObject = SomeClass:new()
local field = constructedObject.someField