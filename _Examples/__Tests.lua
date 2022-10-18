--Modules not used in workspace
--@module "definitions"
--@module "Static"

--Statics
---@type STClassName
local instance
local copy = instance.instanceFunction()
copy.instanceString = instance.instanceString .. "(copy)"

if(STClassName.staticMethod(STClassName.staticString))
then
    local solution = "Possibly just finding a good prefix for static classes?"
end

---@type ClassName
local classInstance
classInstance:ClassMethod("test")
local name = classInstance.memberName
-- ClassName.someField = 

--@type STClassName
--local thing = {}
local fieldTest = STClassName.new()
fieldTest:instanceFunction()

--local instance = FakeClass.new()
local opt = fieldTest.withOptional("test")
opt = fieldTest.withOptional()
fieldTest:withSelf()
fieldTest.withParams("test", 1)


--Test of the FakeProject definitions
local defEnumTest = FakeEnum.A
---@type FakeClass
local fake
-- local defConstTest = FakeClass:new("param") -- Other style
-- local defConstTest = FakeClass.new(FakeClass.new(), "test")
-- local defConstVariadic = FakeClass.new(1, 2, 3, 4) 
-- local defFieldTest = defConstTest.GetOnlyProperty .. defConstTest.AutoProperty
-- local defStaticTest = FakeClass.STCONST_FIELD .. FakeClass.STAutoProperty

local defConstTest = _FakeClass.new(1, 2, 3)
local staticFailsByAcessingInstance = _FakeClass.STAutoProperty
defConstTest:TestMethodEnum(FakeEnum.A)
