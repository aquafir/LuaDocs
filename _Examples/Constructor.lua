---@class SomeClass
local SomeClass = {}
function SomeClass:SomeClassMethod() end

---@generic T
---@param class `T` # the type is captured using `T`
---@return T       # generic type is returned
local function new(class) end

local obj = new("SomeClass")
obj.SomeClassMethod()



