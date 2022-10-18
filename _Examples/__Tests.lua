---@module "definitions"
---@module "Static"

--Statics
---@type STClassName
local instance
local copy = instance.instanceFunction()
copy.instanceString = instance.instanceString .. "(copy)"

if(STClassName.staticMethod(STClassName.staticString))
then
    local solution = "Possibly just finding a good prefix for static classes?"
end

--Test of the FakeProject definitions
local defEnumTest = FakeEnum.A
-- local defConstTest = FakeClass:new("param") -- Other style
local defConstTest = FakeClass.new(FakeClass.new(), "test")
local defConstVariadic = FakeClass.new(1, 2, 3, 4) 
local defFieldTest = defConstTest.GetOnlyProperty .. defConstTest.AutoProperty
local defStaticTest = FakeClass.STCONST_FIELD .. FakeClass.STAutoProperty