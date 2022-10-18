---@meta
---@see Unsure how to have metadata for class/struct.  @see might be an option
---@class ClassName
---@field memberName string Description  ...for each visible? 
---@field new fun(str: string): ClassName # Constructor using a string
---@field SayName fun(): string # Method that returns a string
local ClassName = {}
---@param name string Name provided for method
---@return boolean # Returns true if name was accepted
function ClassName:ClassMethod(name) end

-----Classes can have periods/full qualified names for disambiguation?
---@class Full.Class
---@field memberName any Sub desc

