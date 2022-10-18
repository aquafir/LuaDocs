---@meta
---Separate declaration of class
ClassName = {}
---@type string
ClassName.staticString = nil
---@return ClassName 
function ClassName.new() end
---@param str string 
---@return number
function ClassName.toNumber(str) end

---Actual class documentation goes above
---@class STClassName Name of the class
---@field instanceString string String instance
---@field instanceFunction fun():STClassName Make a copy
---@field withParams fun(str: string, num: number): any
--No variadic support
-----@field withVar fun(str: ...string ): nil
---@field withOptional fun(str: string?,): nil
---@field withVoid fun(): nil
---@field withSelf fun(self: STClassName): nil

---@type STClassName
local _STClassName = {}
function _STClassName:InstanceFunction() end

---Static class needs different naming convention?
STClassName = {}
--@type function
---@source https://ubstaging.surge.sh/ub-scripts/api/UBCommon.Enums.AllegianceHouseAction.html
---@see https://ubstaging.surge.sh/ub-scripts/api/UBCommon.Enums.AllegianceHouseAction.html
---@param staticArg string Static function argument
---@return boolean # Returns true if static function does the needful
STClassName.staticMethod = function (staticArg) end

---@return STClassName
STClassName.new = function() end

---@type string String documentation
STClassName.staticString = "<Default/Const Value>"
