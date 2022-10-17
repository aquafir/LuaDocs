---@meta
--Alias
---@alias Side # Sides of an object
---| `Side.Left` # The left side of the device
---| 2 # The top side of the device
---| '"bottom"' # The bottom side of the device

---@param side Side
local function useEnum(side) end


--Enums can also be used
---@enum EnumName
local EnumName = {  -- Enum description?
	Foo = 0, -- Description of Foo?
	Bar = 1, -- Description of Bar?
}

---@param enum EnumName
local function useEnum2(enum) end

useEnum(Side.Left)
useEnum2(EnumName.Foo)

