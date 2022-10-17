using FakeProject;
using HarmonyLib;
using MonoMod.Utils;
using NuDoq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;

namespace LuaDocs
{
    internal class LuaVisitor : Visitor
    {
        private Dictionary<string, List<XmlEntry>> _docs = new();
        private MemberIdMap idMap;
        private readonly Regex memberFilter = new Regex(@"TestConsoleApp\.Fake\.|UBCommon\.Enums\.|UBCommon\.HandlerEventArgs\.", RegexOptions.Compiled);
        private TextWriter output;


        public LuaVisitor(TextWriter output, Dictionary<string, List<XmlEntry>> documentation)
        { 
         this.output = output;
            _docs = documentation;
            idMap = new MemberIdMap();
            idMap.Add(typeof(FakeClass).Assembly);
        }


        private bool IsSkipped(Member member)
        {
            if (member is null) return true;

            //Filter by name
            if (!memberFilter.IsMatch(member.Id))
                return true;

            //Filter by visibility
            var typeInfo = member.Info.ReflectedType.BaseType;
            if (typeInfo.IsNotPublic)
                return true;

            //if (member.Id == "T:TestConsoleApp.Fake.FakeClass.FakeInnerClass")
            //    Debugger.Break();

            switch (member.Kind)
            {
                case MemberKinds.Unknown:
                    break;
                case MemberKinds.Type:
                    Debugger.Break();
                    break;
                case MemberKinds.Field:
                    break;
                case MemberKinds.Property:
                    break;
                case MemberKinds.Method:
                    break;
                case MemberKinds.Event:
                    break;
                case MemberKinds.ExtensionMethod:
                    break;
                case MemberKinds.Enum:
                    break;
                case MemberKinds.Interface:
                    break;
                case MemberKinds.Class:
                    break;
                case MemberKinds.Struct:
                    break;
            }

            switch (member.Info.MemberType)
            {
                case MemberTypes.Constructor:
                    return typeInfo.IsAbstract;
                case MemberTypes.Event:
                    return true;
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Method:
                    return true;
                case MemberTypes.Property:
                    return true;
                case MemberTypes.TypeInfo:
                    Debugger.Break();
                    break;
                case MemberTypes.Custom:
                    return true;
                case MemberTypes.NestedType:
                    return true;
                case MemberTypes.All:
                    return true;
            }


            return false;
        }

        //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags
        public override void VisitMember(Member member)
        {
            if (member is null) return;

            #region Test?
            if (member.Kind.HasAnyFlag(MemberKinds.Class | MemberKinds.Struct))
            {
                //Handle non-enum types
                HandleType(member);
            }
            else if (member.Kind.HasFlag(MemberKinds.Enum))
            {
                //Handle enums
                HandleEnum(member);
            }
            else if (member.Kind.HasAnyFlag(MemberKinds.Property | MemberKinds.Field | MemberKinds.Method | MemberKinds.Event))
            {
                //Handle members
                HandleMember(member);
            }
            else
            {
                //Unhandled elements?
                Console.WriteLine($"Unhandled member: {member.Id}");
            }
            return;
            #endregion

            //Check things you don't want
            //bool skip = IsSkipped(member);

            base.VisitMember(member);
        }

        private void HandleEnum(Member member)
        {
            var info = member.Info;

            //Relevant documentation
            _docs.TryGetValue(member.Id, out var docs);

            output.WriteLine($"----Enum----{member.Id}");

            //XML docs?
            foreach(var el in member.Elements) { }

            output.WriteLine($"---@enum {info.Name}");
            output.WriteLine($"local {info.Name} = {{");

            var id = idMap.FindId(member.Info);
            var m = idMap.FindMember(member.Id);
            //var t = m.GetUnderlyingType();
            var tt = m.GetRealDeclaringType();
            var ttt = m.GetType();
            //This sucks.  Might be a dealbreaker for NuDoqs beyond extracting XML
            var type = AccessTools.TypeByName(info.Name);

            var values = System.Enum.GetValues(type);

            foreach (var v in values)
            {
                //Todo: formatting for different types of enums?
                output.WriteLine($"\t{v} = {v:D},");
            }
            output.WriteLine("}");
        }

        public override void VisitEnum(NuDoq.Enum type)
        {

            base.VisitEnum(type);
        }

        private void HandleMember(Member member)
        {
        }

        private void HandleType(Member member)
        {
            var fields = member.Elements;


        }



        #region Unused -- Only want documentation and signature info will be pulled from from Member?
        //public override void VisitParam(Param param)
        //{
        //    if (lastMember is not null)
        //        output.WriteLine($"---@param {(param.Name == "" ? "<missing>" : param.Name)} {param.ToText()}");
        //    base.VisitParam(param);
        //}
        //public override void VisitReturns(Returns returns)
        //{
        //    if (lastMember is not null)
        //        output.WriteLine($"---@return {returns.ToText()}");
        //    base.VisitReturns(returns);
        //}
        //public override void VisitSummary(Summary summary)
        //{
        //    if (lastMember is not null)
        //        output.WriteLine($"---Summary");
        //    base.VisitSummary(summary);
        //}
        //public override void VisitSee(See see)
        //{
        //    if (lastMember is not null)
        //        output.WriteLine("---See");
        //    base.VisitSee(see);
        //}
        
        //public override void VisitMethod(Method method) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Method {method.Kind} {method.Id} {method.ToText()}");
        //    base.VisitMethod(method);
        //}
        //public override void VisitAssembly(AssemblyMembers assembly) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Assembly");
        //    base.VisitAssembly(assembly);
        //}
        //public override void VisitC(C code) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---C");
        //    base.VisitC(code);
        //}
        //public override void VisitClass(Class type) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Class");
        //    base.VisitClass(type);
        //}
        //public override void VisitCode(Code code) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Code");
        //    base.VisitCode(code);
        //}
        //public override void VisitDescription(Description description) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Description");
        //    base.VisitDescription(description);
        //}
        //public override void VisitDocument(DocumentMembers document) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Document");
        //    base.VisitDocument(document);
        //}
        //public override void VisitEnum(NuDoq.Enum type) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Enum {type.ToText()}");
        //    base.VisitEnum(type);
        //}
        //public override void VisitEvent(Event @event) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Event {@event.ToText()}");
        //    base.VisitEvent(@event);
        //}
        //public override void VisitExample(Example example) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Example");
        //    base.VisitExample(example);
        //}
        //public override void VisitException(NuDoq.Exception exception) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Exception");
        //    base.VisitException(exception);
        //}
        //public override void VisitExtensionMethod(ExtensionMethod method) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---ExtensionMethod");
        //    base.VisitExtensionMethod(method);
        //}
        //public override void VisitField(Field field) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Field");
        //    base.VisitField(field);
        //}
        //public override void VisitInterface(Interface type) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Interface");
        //    base.VisitInterface(type);
        //}
        //public override void VisitItem(Item item) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Item");
        //    base.VisitItem(item);
        //}
        //public override void VisitList(List list) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---List");
        //    base.VisitList(list);
        //}
        //public override void VisitListHeader(ListHeader header) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---ListHeader");
        //    base.VisitListHeader(header);
        //}
        //public override void VisitPara(Para para) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Para");
        //    base.VisitPara(para);
        //}
        //public override void VisitParamRef(ParamRef paramRef) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---ParamRef");
        //    base.VisitParamRef(paramRef);
        //}
        //public override void VisitProperty(Property property) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Property {property.ToText()}");
        //    base.VisitProperty(property);
        //}
        //public override void VisitRemarks(Remarks remarks) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Remarks");
        //    base.VisitRemarks(remarks);
        //}
        //public override void VisitSeeAlso(SeeAlso seeAlso) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---SeeAlso");
        //    base.VisitSeeAlso(seeAlso);
        //}
        //public override void VisitStruct(Struct type) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine($"---Struct");
        //    base.VisitStruct(type);
        //}
        //public override void VisitTerm(Term term) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Term");
        //    base.VisitTerm(term);
        //}
        //public override void VisitText(Text text) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Text");
        //    base.VisitText(text);
        //}
        //public override void VisitType(TypeDeclaration type) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Type");
        //    base.VisitType(type);
        //}
        //public override void VisitTypeParam(TypeParam typeParam) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---TypeParam");
        //    base.VisitTypeParam(typeParam);
        //}
        //public override void VisitTypeParamRef(TypeParamRef typeParamRef) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---TypeParamRef");
        //    base.VisitTypeParamRef(typeParamRef);
        //}
        //public override void VisitUnknownElement(UnknownElement element) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---UnknownElement");
        //    base.VisitUnknownElement(element);
        //}
        //public override void VisitUnknownMember(UnknownMember member) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---UnknownMember");
        //    base.VisitUnknownMember(member);
        //}
        //public override void VisitValue(Value value) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Value");
        //    base.VisitValue(value);
        //}
        //protected override void VisitContainer(Container container) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Container");
        //    base.VisitContainer(container);
        //}
        //protected override void VisitElement(Element element) {
        //    Debugger.Break();
        //    if (lastMember is not null)
        //        output.WriteLine("---Element");
        //    base.VisitElement(element);
        //}

        #endregion
        private string NormalizeLink(string cref) => cref.Replace(":", "-").Replace("(", "-").Replace(")", "");

    }
}
