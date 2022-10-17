---@meta
---@class STClassName
---@field someString string
---@field someFunction fun()

---@type STClassName
SClassName = {}
--@type function
---@param staticArg string Static function argument
---@return boolean # Returns true if static function does the needful
SClassName.staticMethod = function (staticArg) end

--@type string String documentation
SClassName.someString = "<Default/Const Value>"

--@type function
--@param stringArg string Static parameter documentation
SClassName.someFunction = function(stringArg) end

--STClassName.someFunction(STClassName.someString)

---Description of class?
---@param staticClass STClassName
SClassName.foo = function(staticClass) end

