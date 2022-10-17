---@meta
---@type ClassName
local ClassName = {}
---@param staticArg string Static function argument
---@return bool # Returns true if static function does the needful
ClassName.StaticMethod = function (staticArg) end

---@type string String documentation
ClassName.someString = "<Default/Const Value>"

---@param stringArg string Static parameter documentation
ClassName.someFunction = function(stringArg) end

ClassName.someFunction(ClassName.someString)

