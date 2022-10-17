using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HarmonyLib;
using MonoMod.Utils;
using NuDoq;

namespace LuaDocs
{
    public class XmlVisitor : Visitor
    {
        public Dictionary<string, List<XmlEntry>> Documentation = new ();

        public override void VisitMember(Member member)
        {
            //For each member (which NuDoq considers types to be, see MemberKinds) create an entry using its ID in a documentation store
            if (!Documentation.ContainsKey(member.Id))
                Documentation.Add(member.Id, new List<XmlEntry>());

            //For each element contained in that member create an XmlEntry with the type and desired representation
            foreach(var e in member.Elements)
            {                
                XmlEntry entry = e switch
                {
                    _ when e is NuDoq.Summary => new XmlEntry(XmlType.Summary, e, e.ToText()),
                    Param p when e is NuDoq.Param => new XmlEntry(XmlType.Param, e, e.ToText(), p.Name),
                    Returns r when e is NuDoq.Returns => new XmlEntry(XmlType.Returns, e, e.ToText()),

                    //Handle element types that don't have a default representation the way you want.  Some like "See" have attributes like Cref inaccessible as an Element
                    See s when e is NuDoq.See => new XmlEntry(XmlType.See, e, s.Cref),

                    //A lot of XML elements may be unsupported/unused
                    _ when e is NuDoq.Code => new XmlEntry(XmlType.Code, e, e.ToString()),
                    _ when e is NuDoq.Description => new XmlEntry(XmlType.Description, e, e.ToString()),
                    _ when e is NuDoq.Example => new XmlEntry(XmlType.Example, e, e.ToString()),
                    _ when e is NuDoq.Exception => new XmlEntry(XmlType.Exception, e, e.ToString()),
                    _ when e is NuDoq.Item => new XmlEntry(XmlType.Item, e, e.ToString()),
                    _ when e is NuDoq.List => new XmlEntry(XmlType.List, e, e.ToString()),
                    _ when e is NuDoq.ListHeader => new XmlEntry(XmlType.ListHeader, e, e.ToString()),
                    _ when e is NuDoq.Para => new XmlEntry(XmlType.Para, e, e.ToString()),
                    _ when e is NuDoq.ParamRef => new XmlEntry(XmlType.ParamRef, e, e.ToString()),
                    _ when e is NuDoq.Remarks => new XmlEntry(XmlType.Remarks, e, e.ToString()),
                    _ when e is NuDoq.SeeAlso => new XmlEntry(XmlType.SeeAlso, e, e.ToString()),
                    _ when e is NuDoq.Term => new XmlEntry(XmlType.Term, e, e.ToString()),
                    _ when e is NuDoq.Text => new XmlEntry(XmlType.Text, e, e.ToString()),
                    _ when e is NuDoq.TypeParam => new XmlEntry(XmlType.TypeParam, e, e.ToString()),
                    _ when e is NuDoq.TypeParamRef => new XmlEntry(XmlType.TypeParamRef, e, e.ToString()),
                    _ when e is NuDoq.UnknownElement => new XmlEntry(XmlType.UnknownElement, e, e.ToString()),
                    _ when e is NuDoq.Value => new XmlEntry(XmlType.Value, e, e.ToString()),
                };

                Documentation[member.Id].Add(entry);
            }

            base.VisitMember(member);
        }
    }

    public struct XmlEntry
    {
        public XmlType XmlType;
        public Element Element;
        public string Representation;

        /// <summary>
        /// Optional name corresponding to an element
        /// </summary>
        public string Name;

        public XmlEntry(XmlType xmlType, Element element, string representation, string name = "")
        {
            XmlType = xmlType;
            Element = element;
            Representation = representation;
            Name = name;
        }
    }

    public enum XmlType
    {
        None,
        C,
        Code,
        Description,
        Example,
        Exception,
        Item,
        List,
        ListHeader,
        Para,
        Param,
        ParamRef,
        Remarks,
        Returns,
        See,
        SeeAlso,
        Summary,
        Term,
        Text,
        TypeParam,
        TypeParamRef,
        UnknownElement,
        Value,
    }
}
