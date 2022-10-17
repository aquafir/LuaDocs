--Syntax ---@param <name[?]> <type[|type...]> [description]

---@param paramName ParamType Description
---@param optionalParam? string An optional parameter
---@return ReturnType # Description of return
local function doSomething(paramName, optionalParam) end -- Do something documentation

---@alias ParamType 
---| "A value"

doSomething("A value", "Optional")


