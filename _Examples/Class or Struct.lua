---@meta
---@see Unsure how to have metadata for class/struct.  @see might be an option
---@class ClassName
---@field memberName memberType Description  ...for each visible? 
---@field new fun(str: string): ClassName # Constructor using a string
---@field SayName fun(): string # Method that returns a string
--@field isEnabled function Methods / constructors can be added like this but that messes up function Intellisense?
local ClassName = {}
---@param name string Name provided for method
---@return bool # Returns true if name was accepted
function ClassName:ClassMethod(name) end

-----Classes can have periods/full qualified names for disambiguation?
---@class Full.Class
---@field memberName submemberType Sub desc

---@type ClassName
classInstance = {}
classInstance.ClassMethod()
-- ClassName.someField = 

classInstance.ClassMethod()
