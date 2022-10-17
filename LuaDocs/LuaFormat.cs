using HarmonyLib;
using NuDoq;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace LuaDocs
{
    public static class LuaFormat
    {
        public static string LuaParamNames(this ParameterInfo[] paramsInfo) => String.Join(",", paramsInfo.Select(p => p.Name).ToArray());
        //public static string LuaReturn(this ConstructorInfo ci) => $"---@return {ci.ReflectedType.Name}";
        //public static string LuaParam(this ParameterInfo pi) => $"---@param {pi.ParameterType.Name}{(pi.IsOptional ? "?" : "")} {pi.ParameterType.LuaType()} <<Documentation?>>";


        //Maps CLR type to what will be used in Lua
        private static Dictionary<string, string> luaTypes = new Dictionary<string, string>() {
            {            "System.Void" , ""},
{            "MoonSharp.Interpreter.DynValue" , "*"},
{            "System.SByte" , "number"},
{            "System.Byte" , "number"},
{            "System.Int16" , "number"},
{            "System.UInt16" , "number"},
{            "System.Int32" , "number"},
{            "System.UInt32" , "number"},
{            "System.Int64" , "number"},
{            "System.UInt64" , "number"},
{            "System.Single" , "number"},
{            "System.Decimal" , "number"},
{            "System.Double" , "number"},
{            "System.Boolean" , "boolean"},
{            "System.String" , "string"},
{            "System.Text.StringBuilder" , "string"},
{            "System.Char" , "string"},
            //TODO -- these probably require different mapping
{            "MoonSharp.Interpreter.Table" , "table"},
{            "MoonSharp.Interpreter.CallbackFunction" , "function"},
{            "System.Delegate" , "function"},
{            "System.Object" , "userdata"},
{            "System.Type" , "userdata"},
{            "MoonSharp.Interpreter.Closure" , "function"},
{            "System.Reflection.MethodInfo" , "function"},
{            "System.Collections.IList" , "table"},
{            "System.Collections.IDictionary" , "table"},
{            "System.Collections.IEnumerable" , "iterator"},
{            "System.Collections.IEnumerator" , "iterator"},
            };

       
        public static string LuaReturnType(this ConstructorInfo info) =>
            GetOrAddLuaType(info.GetUnderlyingType().FullName);

        public static string LuaReturnType(this MethodInfo info) =>
            GetOrAddLuaType(info.ReturnType.LuaType());

        public static string LuaType(this EventInfo info) => info.EventHandlerType.LuaType();
        public static string LuaType(this FieldInfo info) => info.FieldType.LuaType();
        public static string LuaType(this ParameterInfo info) => info.ParameterType.LuaType();
        public static string LuaType(this PropertyInfo info) => info.PropertyType.LuaType();

        /// <summary>
        /// Finds the equivalent Type in Lua.  If a non-primitive type is provided it is added to the set of available Types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LuaType(this Type type)
        {
            if (type is null)
                return "nil";

            //---@type Type[]
            if (type.IsArray)
            {
                var elementLuaType = GetOrAddLuaType(type.GetElementType().FullName);
                return GetOrAddLuaType($"{elementLuaType}[]");
            }

            //Todo: figure out full generic support, not just dictionary
            //---@type { [TKey] = TVal }
            if (type.IsGenericType)
            {
                var generics = type.GetGenericArguments();
                if(generics.Length == 2)
                {
                    return $"{{ [{generics[0].LuaType()}] = {generics[1].LuaType()} }}";
                }
            }

            return GetOrAddLuaType(type.FullName);
        }

        private static string GetOrAddLuaType(string name)
        {
            if (luaTypes.TryGetValue(name, out var typeName))
            {
                return typeName;
            }           

            var lastIndex = Math.Max(name.LastIndexOf('.'), name.LastIndexOf('+')) + 1;
            var shortName = name.Substring(lastIndex);
            luaTypes.Add(name, shortName);
            return shortName;

        }

        /// <summary>
        /// Helper to check if a parameter uses params which Lua translates to ...
        /// </summary>
        public static bool IsParams(this ParameterInfo param)
        {
            return param.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0;
        }

        /// <summary>
        /// Helper to check if a method is async
        /// </summary>
        public static bool IsAsync(this MethodInfo m)
        {
            //Todo: Think about supporting @async annotation
            //Might require adding Nuget package for attribute            
            return false;

            //https://stackoverflow.com/questions/13183313/how-to-determine-if-method-is-async-at-runtime
            //var stateMachineAttr = m.GetCustomAttribute<AsyncStateMachineAttribute>();
            //if (stateMachineAttr != null)
            //{
            //    var stateMachineType = stateMachineAttr.StateMachineType;
            //    if (stateMachineType != null)
            //    {
            //        return stateMachineType.GetCustomAttribute<CompilerGeneratedAttribute>() != null;
            //    }
            //}
            //return false;
        }


        #region LuaDefault
        //Needs to be a way to map between default/constant values in C# to their Lua equivalent
        //Todo: definitely rethink this.  Harmony hack
        //https://stackoverflow.com/questions/407337/net-get-default-value-for-a-reflected-propertyinfo
        //Maybe use declaring type with Traverse?
        //return Traverse.Create(type).Field(info.Name).GetValue();
        public static object LuaDefault(this FieldInfo info)
        {
            if (info.IsLiteral)
            {
                var value = info.GetRawConstantValue();
                if (value is string)
                    return $"\"{value}\"";
                return value;
            }

            return "\"TODO\"";
            
        }
        public static object LuaDefault(this PropertyInfo info) {
            //Raw constant value for an enum would be "1" vs. Enum.A (= 1)
            var value = info.GetConstantValue();    
            if (info.PropertyType.IsEnum || value is string)
                return $"\"{value}\"";
            return value;
        }


        //https://stackoverflow.com/questions/407337/net-get-default-value-for-a-reflected-propertyinfo
        /// <summary>
        /// [ <c>public static T GetDefault&lt; T &gt;()</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <typeparam name="T">The Type for which to get the default value</typeparam>
        /// <returns>The default value for Type T</returns>
        /// <remarks>
        /// If a reference Type or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="GetDefault(Type)"/>
        public static T GetDefault<T>()
        {
            return (T)GetDefault(typeof(T));
        }

        /// <summary>
        /// [ <c>public static object GetDefault(Type type)</c> ]
        /// <para></para>
        /// Retrieves the default value for a given Type
        /// </summary>
        /// <param name="type">The Type for which to get the default value</param>
        /// <returns>The default value for <paramref name="type"/></returns>
        /// <remarks>
        /// If a null Type, a reference Type, or a System.Void Type is supplied, this method always returns null.  If a value type 
        /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an 
        /// exception.
        /// </remarks>
        /// <seealso cref="GetDefault&lt;T&gt;"/>
        public static object GetDefault(this Type type)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (System.Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> is not a publicly-visible type, so the default value cannot be retrieved");
        } 
        #endregion
    }
}
