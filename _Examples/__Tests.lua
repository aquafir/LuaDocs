--@module "Constructor"
---@module "definitions"
---@module "Static"

--Adding constructor to static object named after type
local staticTest = SomeClass:new()
local field = staticTest.someField

--Adding constructor as class field function
--Still requires a global "static" object with the name of the static type it represents
local funcTest = SomeClass.new("string for diff signature")
local funcField = funcTest.new("test")

local defEnumTest = FakeEnum.A
-- local defConstTest = FakeClass:new("param") -- Other style
local defConstTest = FakeClass.new(FakeClass.new(), "test")
local defConstTest2 = FakeClass.new(1, "default")
local defTestVariadic = FakeClass.new(1, 2, 3, 4) 
local defFieldTest = defConstTest.GetOnlyProperty
-- local defStaticTest = FakeClass.READONLY_FIELD   -- Not working

local test = FakeClass.new(FakeClass.new(), "this")
