---@meta
---@class GenClass
local GenClass = {}
function GenClass:SomeClassMethod() end

---@generic T
---@param class `T` # the type is captured using `T`
---@return T       # generic type is returned
local function new(class) end

local obj = new("GenClass")
obj:SomeClassMethod()


---@class SomeClass
---@field someField string Field description
---@field new fun(str: string): SomeClass # Default constructor

---@type SomeClass
SomeClass = {}

---@return SomeClass
SomeClass.new = function() end

