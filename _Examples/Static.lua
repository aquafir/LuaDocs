---@meta
-- ---Actual class documentation goes above
-- ---@class STClassName Name of the class
-- ---@field instanceString string String instance
-- ---@field instanceFunction fun():STClassName Make a copy

-- ---@type STClassName
-- local STClassName = {}
-- function STClassName:InstanceFunction() end

-- ---Static class needs different naming convention?
-- SClassName = {}
-- --@type function
-- ---@param staticArg string Static function argument
-- ---@return boolean # Returns true if static function does the needful
-- STClassName.staticMethod = function (staticArg) end

-- ---@type string String documentation
-- STClassName.staticString = "<Default/Const Value>"

---Actual class documentation goes above
---@class STClassName Name of the class
---@field instanceString string String instance
---@field instanceFunction fun():STClassName Make a copy

---@type STClassName
local _STClassName = {}
function _STClassName:InstanceFunction() end

---Static class needs different naming convention?
STClassName = {}
--@type function
---@param staticArg string Static function argument
---@return boolean # Returns true if static function does the needful
STClassName.staticMethod = function (staticArg) end

---@type string String documentation
STClassName.staticString = "<Default/Const Value>"
