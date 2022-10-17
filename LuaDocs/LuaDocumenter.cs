using FakeProject;
using HarmonyLib;
using Mono.Cecil;
using NuDoq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LuaDocs
{
    public static class LuaDocumenter
    {
        static Dictionary<string, List<XmlEntry>> documentation = new();
        static MemberIdMap map = new();
        static IEnumerable<Type> types;
        static StringBuilder output = new();


        public static void Document(Dictionary<string, List<XmlEntry>> documentation, MemberIdMap map, Assembly assembly)
        {
            LuaDocumenter.documentation = documentation;
            LuaDocumenter.map = map;

            //Get non-MS Types from Assembly to document
            types = assembly.GetTypes().Where(x => !(x.Namespace.StartsWith("Microsoft.") || x.Namespace.StartsWith("System.")));

            CreateDocs();

            if (!Directory.Exists("definitions"))
                Directory.CreateDirectory("definitions");
            File.WriteAllText(@"definitions\_definitions.lua", output.ToString());
        }

        static void CreateDocs()
        {
            //Header
            output.AppendLine(@"---@meta");

            foreach (var type in types)
            {
                //Todo: think about what to exclude
                if (type.Name.Contains("<>"))
                    continue;

                var info = type as MemberInfo;
                var id = map.FindId(info);

                //Get xml documentation elements
                documentation.TryGetValue(id, out var docs);

                if (type.IsEnum)
                {
                    HandleEnum(type, info, id, docs);
                }
                else
                {
                    HandleInstanceType(type, info, id, docs);
                    HandleStaticType(type, info, id, docs);
                }
            }
        }

        #region Instance
        private static void HandleInstanceType(Type type, MemberInfo info, string id, List<XmlEntry> docs)
        {
            //Headers for sections?
            output.AppendLine($"-------------Instance Class/Struct {id}-----------");

            output.AppendLine($"---@class {type.LuaType()}");

            //Unsure how to document xml documentation for class
            //foreach(var element in docs) { }

            //Currently just distinguishing between static and instance?
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            //Type's @class must be followed by field/property annotations
            HandleInstanceFields(type, flags);

            HandleInstanceProperties(type, flags);

            HandleInstanceMethods(type);

            HandleInstanceEvents(type);

            //Constructors show up as instance types but the syntax desired is static?
            HandleInstanceConstructors(type);
        }

        private static void HandleInstanceFields(Type type, BindingFlags flags)
        {
            output.AppendLine("-------------------------Fields------------------------------");
            var fields = type.GetFields(flags)
                //Filter out auto-generated stuff
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var field in fields)
            {
                var fId = map.FindId(field);
                documentation.TryGetValue(fId, out var fDocs);
                var fDesc = fDocs is null ? "" : fDocs.FirstOrDefault().Representation;

                output.AppendLine($"---@field {field.Name} {field.LuaType()} {fDesc}");
            }
        }

        private static void HandleInstanceProperties(Type type, BindingFlags flags)
        {
            output.AppendLine("-----------------------Properties----------------------------");
            var properties = type.GetProperties(flags)
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var property in properties)
            {
                var pId = map.FindId(property);
                documentation.TryGetValue(pId, out var pDocs);
                var pDesc = pDocs is null ? "" : pDocs.FirstOrDefault().Representation;

                output.AppendLine($"---@field {property.Name} {property.LuaType()} {pDesc}");
            }

            output.AppendLine($"local {type.LuaType()} = {{ }}");
            output.AppendLine();
        }

        private static void HandleInstanceEvents(Type type)
        {
            output.AppendLine("------------------------Events------------------------------");
            var events = type.GetEvents()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var ev in events)
            {
                HandleInstanceMethod(type, ev.GetAddMethod());
                HandleInstanceMethod(type, ev.GetRemoveMethod());
                HandleInstanceMethod(type, ev.GetRaiseMethod());
            }
        }

        private static void HandleInstanceMethods(Type type)
        {
            output.AppendLine("------------------------Methods------------------------------");
            var methods = type.GetMethods()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var method in methods)
            {
                HandleInstanceMethod(type, method);
                //var parameters = method.GetParameters();

                //var mId = map.FindId(method);
                //documentation.TryGetValue(mId, out var mDocs);
                //var mDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;
                ////var pDescs = mDocs is null ? new List<XmlEntry>() : mDocs.Where(x => x.XmlType == XmlType.Param);

                //foreach (var p in parameters)
                //{
                //    //Todo: Rethink XmlEntry
                //    var paramDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                //    //---@param <name[?]> <type[|type...]> [description]
                //    output.AppendLine($"---@param {p.Name}{(p.IsOptional ? "?" : "")} {p.LuaType()}{(p.IsParams() ? "..." : "")} {paramDesc}");
                //}

                //if (method.ReturnType != typeof(void))
                //{
                //    var rDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                //    //---@return <type> [<name> [comment] | [name] #<comment>]
                //    output.AppendLine($"---@return {method.ReturnType.LuaType()} # {rDesc}");
                //}

                ////Todo: think about async support targeting 3.5
                ////if (method.IsAsync())
                ////    output.AppendLine("---@async");

                //var paramNames = parameters.LuaParamNames();
                ////function ClassName:ClassMethod(name) end
                //output.AppendLine($"function {type.LuaType()}:{method.Name}({parameters.LuaParamNames()}) end");
                //output.AppendLine();
            }
        }


        private static void HandleInstanceConstructors(Type type)
        {
            output.AppendLine("---------------------Constructors----------------------------");
            //trevis: Side note, I also changed constructor syntax to MyClass.new() instead of MyClass.__new()…
            var constructors = type.GetConstructors();
            foreach (var method in constructors)
            {
                if (type is null || method is null)
                {
                    Console.WriteLine("Static method null");
                    return;
                }

                var parameters = method.GetParameters();

                var mId = map.FindId(method);
                documentation.TryGetValue(mId, out var mDocs);
                var mDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;

                foreach (var p in parameters)
                {
                    var paramDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                    //---@param <name[?]> <type[|type...]> [description]
                    output.AppendLine($"---@param {p.Name}{(p.IsOptional ? "?" : "")} {p.LuaType()}{(p.IsParams() ? "..." : "")} {paramDesc}");
                }

                var rDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                //---@return <type> [<name> [comment] | [name] #<comment>]
                output.AppendLine($"---@return {type.LuaType()} # {rDesc}");

                var paramNames = parameters.LuaParamNames();
                //TypeName.FunctionName = function(params) end
                output.AppendLine($"{type.LuaType()}.new = function({parameters.LuaParamNames()}) end");
                output.AppendLine();

                //var parameters = constructor.GetParameters();

                //var pId = map.FindId(constructor);
                //documentation.TryGetValue(pId, out var pDocs);
                //var pDesc = pDocs is null ? "" : pDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;
                ////var pDescs = mDocs is null ? new List<XmlEntry>() : mDocs.Where(x => x.XmlType == XmlType.Param);

                //foreach (var p in parameters)
                //{
                //    var paramDesc = pDocs is null ? "" : pDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                //    //---@param <name[?]> <type[|type...]> [description]
                //    output.AppendLine($"---@param {p.Name}{(p.IsOptional ? "?" : "")} {p.LuaType()}{(p.IsParams() ? "..." : "")} {paramDesc}");
                //}

                //var rDesc = pDocs is null ? "" : pDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                ////---@return <type> [<name> [comment] | [name] #<comment>]
                //output.AppendLine($"---@return {type.LuaType()} # {rDesc}");

                //var paramNames = parameters.LuaParamNames();
                ////function ClassName:new(params) end
                //output.AppendLine($"function {type.LuaType()}:new({parameters.LuaParamNames()}) end");
                //output.AppendLine();
            }
        }


        /// <summary>
        /// Adds documentation for an instance method of a provided type.  Used by events, constructors, methods
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        private static void HandleInstanceMethod(Type type, MethodInfo method)
        {
            if (type is null || method is null)
            {
                Console.WriteLine("Instance method null");
                return;
            }

            var parameters = method.GetParameters();

            var mId = map.FindId(method);
            documentation.TryGetValue(mId, out var mDocs);
            var mDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;

            foreach (var p in parameters)
            {
                var paramDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                //---@param <name[?]> <type[|type...]> [description]
                output.AppendLine($"---@param {p.Name}{(p.IsOptional ? "?" : "")} {p.LuaType()}{(p.IsParams() ? "..." : "")} {paramDesc}");
            }

            if (method.ReturnType != typeof(void))
            {
                var rDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                //---@return <type> [<name> [comment] | [name] #<comment>]
                output.AppendLine($"---@return {method.ReturnType.LuaType()} # {rDesc}");
            }

            var paramNames = parameters.LuaParamNames();
            //function ClassName:ClassMethod(name) end
            output.AppendLine($"function {type.LuaType()}:{method.Name}({parameters.LuaParamNames()}) end");
            output.AppendLine();
        }
        #endregion

        #region Static
        private static void HandleStaticType(Type type, MemberInfo info, string id, List<XmlEntry> docs)
        {
            //Headers for sections?
            output.AppendLine($"-------------Static Class/Struct {id}-----------");

            //Create static instance to add static members to
            output.AppendLine($"---@type {type.LuaType()}");
            output.AppendLine($"local {type.LuaType()} = {{}}");

            //Unsure how to document xml documentation for class
            //foreach(var element in docs) { }

            //Currently just distinguishing between static and instance?
            var flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            HandleStaticFields(type, flags);

            HandleStaticProperties(type, flags);

            HandleStaticMethods(type);
        }

        private static void HandleStaticFields(Type type, BindingFlags flags)
        {
            output.AppendLine("-------------------------Fields------------------------------");
            var fields = type.GetFields(flags)
                //Filter out auto-generated stuff
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var field in fields)
            {
                var fId = map.FindId(field);
                documentation.TryGetValue(fId, out var fDocs);
                var fDesc = fDocs is null ? "" : fDocs.FirstOrDefault().Representation;

                output.AppendLine($"---@type {field.LuaType()} {fDesc}");
                output.AppendLine($"{type.LuaType()}.{field.Name} = {field.LuaDefault()}");
            }
        }

        private static void HandleStaticProperties(Type type, BindingFlags flags)
        {
            output.AppendLine("-----------------------Properties----------------------------");
            var properties = type.GetProperties(flags)
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var property in properties)
            {
                var pId = map.FindId(property);
                documentation.TryGetValue(pId, out var pDocs);
                var pDesc = pDocs is null ? "" : pDocs.FirstOrDefault().Representation;

                output.AppendLine($"---@type {property.LuaType()} {pDesc}");
                output.AppendLine($"{type.LuaType()}.{property.Name} = {property.LuaDefault()}");
            }
        }

        private static void HandleStaticEvents(Type type)
        {
            output.AppendLine("------------------------Events------------------------------");
            var events = type.GetEvents()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var ev in events)
            {
                HandleStaticMethod(type, ev.GetAddMethod());
                HandleStaticMethod(type, ev.GetRemoveMethod());
                HandleStaticMethod(type, ev.GetRaiseMethod());
            }
        }

        private static void HandleStaticMethods(Type type)
        {
            output.AppendLine("------------------------Methods------------------------------");
            var methods = type.GetMethods()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var method in methods)
            {
                HandleStaticMethod(type, method);
                //var parameters = method.GetParameters();

                //var mId = map.FindId(method);
                //documentation.TryGetValue(mId, out var mDocs);
                //var mDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;

                //foreach (var p in parameters)
                //{
                //    var paramDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                //    //---@param <name[?]> <type[|type...]> [description]
                //    output.AppendLine($"---@param {p.Name}{(p.IsOptional ? "?" : "")} {p.LuaType()}{(p.IsParams() ? "..." : "")} {paramDesc}");
                //}

                //if (method.ReturnType != typeof(void))
                //{
                //    var rDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                //    //---@return <type> [<name> [comment] | [name] #<comment>]
                //    output.AppendLine($"---@return {method.ReturnType.LuaType()} # {rDesc}");
                //}

                //var paramNames = parameters.LuaParamNames();
                ////TypeName.FunctionName = function(params) end
                //output.AppendLine($"{type.LuaType()}.{method.Name} = function ({parameters.LuaParamNames()}) end");
                //output.AppendLine();
            }
        }

        /// <summary>
        /// Adds documentation for a static method of a provided type.  Used by events, methods
        /// </summary>
        private static void HandleStaticMethod(Type type, MethodInfo method)
        {
            if (type is null || method is null)
            {
                Console.WriteLine("Static method null");
                return;
            }

            var parameters = method.GetParameters();

            var mId = map.FindId(method);
            documentation.TryGetValue(mId, out var mDocs);
            var mDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;

            foreach (var p in parameters)
            {
                var paramDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                //---@param <name[?]> <type[|type...]> [description]
                output.AppendLine($"---@param {p.Name}{(p.IsOptional ? "?" : "")} {p.LuaType()}{(p.IsParams() ? "..." : "")} {paramDesc}");
            }

            if (method.ReturnType != typeof(void))
            {
                var rDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                //---@return <type> [<name> [comment] | [name] #<comment>]
                output.AppendLine($"---@return {method.ReturnType.LuaType()} # {rDesc}");
            }

            var paramNames = parameters.LuaParamNames();
            //TypeName.FunctionName = function(params) end
            output.AppendLine($"{type.LuaType()}.{method.Name} = function({parameters.LuaParamNames()}) end");
            output.AppendLine();
        }
        #endregion

        private static void HandleEnum(Type type, MemberInfo info, string id, List<XmlEntry> docs)
        {
            //Header?
            output.AppendLine($"----Enum----{id}");

            //Todo: unsure how to document enum <summary>, maybe @see
            documentation.TryGetValue(id, out var enumDocs);
            var enumSummary = enumDocs is null ? "" : enumDocs.FirstOrDefault().Representation;

            //Alias approach
            //output.AppendLine($"---@alias {info.Name}");

            //Enum approach
            output.AppendLine($"---@enum {info.Name}");
            output.AppendLine($"local {info.Name} = {{ -- {enumSummary}");

            //Go through values
            foreach (var value in System.Enum.GetValues(type))
            {
                //Get summaries
                var valueId = $"F{id.Substring(1)}.{value}"; //map.FindId(value.GetType()); //This just gives enum ID

                documentation.TryGetValue(valueId, out var valDocs);
                var valueSummary = valDocs is null ? "" : valDocs.FirstOrDefault().Representation;

                //Alias
                //var alias = $"---| `{info.Name}.{value} # {valueSummary}";
                //output.AppendLine($"---| `{info.Name}.{value} # {valueSummary}");

                //Enum
                //var e = $"\t{value} = {value:D}, -- {valueSummary}";
                output.AppendLine($"\t{value} = {value:D}, -- {valueSummary}");
            }
            //Enum
            output.AppendLine("}");
        }
    }
}
