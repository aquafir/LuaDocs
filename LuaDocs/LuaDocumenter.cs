using NuDoq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace LuaDocs
{
    public static class LuaDocumenter
    {
        static Dictionary<string, List<XmlEntry>> documentation = new();
        static MemberIdMap map = new();
        static IEnumerable<Type> types;
        static StringBuilder output = new();
        static Dictionary<Type, string> staticAnnotations = new();
        static Dictionary<Type, string> instanceAnnotations = new();

        public static void Document(Dictionary<string, List<XmlEntry>> documentation, MemberIdMap map, Assembly assembly)
        {
            LuaDocumenter.documentation = documentation;
            LuaDocumenter.map = map;

            //Get non-MS Types from Assembly to document
            types = assembly.GetTypes().Where(x => !(x.Namespace.StartsWith("Microsoft.") || x.Namespace.StartsWith("System.")));

            CreateDocs();

            //Write to _Examples definitions dir
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\_Examples");
            //File.WriteAllText(Path.Combine(path, "definitions.lua"), output.ToString());
            File.WriteAllText(Path.Combine(path, "instance-definitions.lua"), String.Join(Environment.NewLine, instanceAnnotations.Values.ToArray()));
            File.WriteAllText(Path.Combine(path, "static-definitions.lua"), String.Join(Environment.NewLine, staticAnnotations.Values.ToArray()));

            //Write to build dir
            //if (!Directory.Exists("definitions"))
            //    Directory.CreateDirectory("definitions");
            //File.WriteAllText(@"definitions\_definitions.lua", output.ToString());
        }

        static void CreateDocs()
        {
            //Header
            output.AppendLine(@"---@meta");

            foreach (var type in types)
            {
                //Todo: think about what to exclude
                if (FilterType(type))
                    continue;

                var info = type as MemberInfo;
                var id = map.FindId(info);

                //Get xml documentation elements
                documentation.TryGetValue(id, out var docs);

                if (type.IsEnum)
                {
                    output = new();
                    HandleEnum(type, info, id, docs);
                    instanceAnnotations.Add(type, output.ToString());
                }
                else
                {
                    output = new();
                    HandleInstanceType(type, info, id, docs);
                    instanceAnnotations.Add(type, output.ToString());

                    output = new();
                    HandleStaticType(type, info, id, docs);
                    staticAnnotations.Add(type, output.ToString());
                }
            }
        }

        private static bool FilterType(Type type)
        {
            return type.Name.Contains("<>");
        }

        #region Instance
        private static void HandleInstanceType(Type type, MemberInfo info, string id, List<XmlEntry> docs)
        {
            //Headers for sections?
            //output.AppendLine($"-------------Instance Class/Struct {id}-----------");

            //Currently just distinguishing between static and instance?
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            //Document class;
            var summary = docs is null ? "" : docs.FirstOrDefault().Representation;
            output.AppendLine($"---{summary}");

            //Type's @class must be followed by field/property annotations
            output.AppendLine($"---@class {type.LuaType()}");

            HandleInstanceFields(type, flags);
            HandleInstanceProperties(type, flags);

            //Another way to handle instance methods but it has drawbacks.  Can't document params/returns, no variadics, some Intellisense uses field icons, etc.
            //HandleInstanceMethodsAsField(type);
            //HandleInstanceEventsAsField(type);

            //Create local instance for methods
            output.AppendLine($"local {type.LuaType()} = {{}}");       //Instance
            output.AppendLine();

            HandleInstanceMethods(type);
            HandleInstanceEvents(type);
        }

        #region Instance Methods as Fields
        private static void HandleInstanceMethodsAsField(Type type)
        {
            var methods = type.GetMethods()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var method in methods)
            {
                HandleInstanceMethodField(type, method);
            }
        }

        private static void HandleInstanceEventsAsField(Type type)
        {
            var events = type.GetEvents()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var ev in events)
            {
                HandleInstanceMethodField(type, ev.GetAddMethod());
                HandleInstanceMethodField(type, ev.GetRemoveMethod());
                HandleInstanceMethodField(type, ev.GetRaiseMethod());
            }
        }

        /// <summary>
        /// Adds documentation for an instance method of a provided type in class as field.
        /// </summary>
        private static void HandleInstanceMethodField(Type type, MethodInfo method)
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

            output.AppendLine($"---@field {method.Name} {method.LuaFunctionSignature()} {mDesc}");
        }
        #endregion


        private static void HandleInstanceFields(Type type, BindingFlags flags)
        {
            //output.AppendLine($"-------------------------Fields------------------------------");
            var fields = type.GetFields(flags)
                //Filter out auto-generated stuff
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var field in fields)
            {
                HandleFieldAnnotation(field);
            }
        }

        private static void HandleInstanceProperties(Type type, BindingFlags flags)
        {
            //output.AppendLine($"-----------------------Properties----------------------------");
            var properties = type.GetProperties(flags)
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var property in properties)
            {
                var pId = map.FindId(property);
                documentation.TryGetValue(pId, out var pDocs);
                var pDesc = pDocs is null ? "" : pDocs.FirstOrDefault().Representation;

                output.AppendLine($"---@field {property.Name} {property.LuaType()} {pDesc}");
            }
        }

        private static void HandleInstanceEvents(Type type)
        {
            //output.AppendLine($"------------------------Events------------------------------");
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
            //output.AppendLine($"------------------------Methods------------------------------");
            var methods = type.GetMethods()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var method in methods)
            {
                HandleInstanceMethod(type, method);
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

            HandleMethod(type, method);
        }
        #endregion

        #region Static
        private static void HandleStaticType(Type type, MemberInfo info, string id, List<XmlEntry> docs)
        {
            //Headers for sections?
            //output.AppendLine($"-------------Static Class/Struct {id}-----------");

            //Currently just distinguishing between static and instance?
            var flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            //Create static instance to add static members to
            //output.AppendLine($"---@type {type.LuaType()}");
            output.AppendLine($"{type.LuaType()} = {{}}");    //Static

            //Constructors show up as instance types but the syntax desired is static?
            HandleConstructors(type);
            HandleStaticFields(type, flags);
            HandleStaticProperties(type, flags);
            HandleStaticMethods(type);
        }

        private static void HandleStaticFields(Type type, BindingFlags flags)
        {
            //output.AppendLine($"-------------------------Fields------------------------------");
            var fields = type.GetFields(flags)
                //Filter out auto-generated stuff
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var field in fields)
            {
                HandleField(type, field);
            }
        }

        private static void HandleStaticProperties(Type type, BindingFlags flags)
        {
            //output.AppendLine($"-----------------------Properties----------------------------");
            var properties = type.GetProperties(flags)
                .Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var property in properties)
            {
                HandleField(type, property);
            }
        }

        private static void HandleStaticEvents(Type type)
        {
            //output.AppendLine($"------------------------Events------------------------------");
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
            //output.AppendLine($"------------------------Methods------------------------------");
            var methods = type.GetMethods()
                .Where(x => !x.IsSpecialName); //IsHideBySig?
                                               //.Where(f => f.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length == 0).ToArray();
            foreach (var method in methods)
            {
                HandleStaticMethod(type, method);
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

            HandleMethod(type, method);
        }

        private static void HandleConstructors(Type type)
        {
            //output.AppendLine($"---------------------Constructors----------------------------");
            //trevis: Side note, I also changed constructor syntax to MyClass.new() instead of MyClass.__new()…
            var constructors = type.GetConstructors();
            foreach (var method in constructors)
            {
                if (type is null || method is null)
                {
                    Console.WriteLine("Static method null");
                    return;
                }

                //ConstructorInfo and MethodInfo both inherit from MethodBase
                HandleMethod(type, method, method.GetParameters(), type, "new");
            }
        }

        #endregion

        #region Shared
        /// <summary>
        /// Adds ---@field annotation for a field or property
        /// </summary>
        private static void HandleFieldAnnotation(MemberInfo member)
        {
            var mId = map.FindId(member);
            documentation.TryGetValue(mId, out var mDocs);
            var fDesc = mDocs is null ? "" : mDocs.FirstOrDefault().Representation;

            output.AppendLine($"---@field {member.Name} {member.LuaType()} {fDesc}");
        }

        /// <summary>
        /// Adds type annotation, field, and default value for a field or property
        /// </summary>
        private static void HandleField(Type type, MemberInfo field)
        {
            var mId = map.FindId(field);
            documentation.TryGetValue(mId, out var mDocs);
            var mDesc = mDocs is null ? "" : mDocs.FirstOrDefault().Representation;

            output.AppendLine($"---@type {field.LuaType()} {mDesc}");
            output.AppendLine($"{type.LuaType()}.{field.Name} = {field.LuaDefault()}");
        }

        /// <summary>
        /// Adds method declaration and signature annotation
        /// </summary>
        private static void HandleMethod(Type type, MethodInfo method, string nameOverride = null)
        {
            var parameters = method.GetParameters();
            var returnType = method.ReturnType;

            HandleMethod(type, method, parameters, returnType, nameOverride);
        }

        private static void HandleMethod(Type type, MethodBase method, ParameterInfo[] parameters, Type returnType, string nameOverride = null)
        {
            var mId = map.FindId(method);
            documentation.TryGetValue(mId, out var mDocs);
            if (mDocs is not null)
            {
                var mDesc = mDocs.Where(x => x.XmlType == XmlType.Summary).FirstOrDefault().Representation;
                output.AppendLine($"---{mDesc}");
            }

            foreach (var p in parameters)
            {
                var paramDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Param && x.Name == p.Name).FirstOrDefault().Representation;

                output.AppendLine(p.LuaParamAnnotation(paramDesc));
            }

            if (returnType != typeof(void))
            {
                var rDesc = mDocs is null ? "" : mDocs.Where(x => x.XmlType == XmlType.Returns).FirstOrDefault().Representation;

                //---@return <type> [<name> [comment] | [name] #<comment>]
                output.AppendLine($"---@return {returnType.LuaType()} # {rDesc}");
            }

            var paramNames = parameters.LuaParamNames();
            var methodName = String.IsNullOrEmpty(nameOverride) ? method.Name : nameOverride;
            //function ClassName:ClassMethod(name) end
            output.AppendLine($"function {type.LuaType()}:{methodName}({parameters.LuaParamNames()}) end");
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
            output.AppendLine($"---{enumSummary}");

            //Alias approach
            //output.AppendLine($"---@alias {info.Name}");

            //Enum approach
            output.AppendLine($"---@enum {info.Name}");
            //output.AppendLine($"local {info.Name} = {{ -- {enumSummary}");
            output.AppendLine($"{info.Name} = {{ -- {enumSummary}");

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
