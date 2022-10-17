using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FakeProject
{
    //C# language spec: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/readme
    /// <summary>
    /// Fake class to cover cases
    /// </summary>
    /// <example>
    /// Example tag documentation
    /// </example>
    /// <see cref="string"/>
    /// <seealso cref="short"/>
    public class FakeClass : IAbstract, IInterface
    {
        #region Instance Members (Fields, Properties, Read-only, Methods, Constructors)  //-Events,
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/members
        #region Fields
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields
        /// <summary>
        /// Private field
        /// </summary>
        private string _customBackingField = "Default val";

        /// <summary>
        /// Public field
        /// </summary>
        public string PublicField;
        #endregion

        #region Properties
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
        /// <summary>
        /// Public property
        /// </summary>
        public string PublicProperty { get => _customBackingField; set => _customBackingField = value; }

        /// <summary>
        /// Get-only property
        /// </summary>
        public string GetOnlyProperty { get => _customBackingField; }

        /// <summary>
        /// Property with auto-generated backing field
        /// </summary>
        public string AutoProperty { get; set; }
        #endregion

        #region Read-only
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constants
        /// <summary>
        /// Read-only instance field
        /// </summary>
        readonly string READONLY_FIELD = "Readonly field";
        #endregion

        #region Methods
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods
        /// <summary>
        /// Void method with blank parameter documentation
        /// </summary>
        /// <param name="c">Description of parameter c</param>
        public void TestVoid(string c) { }

        /// <summary>
        /// Adds two strings
        /// </summary>
        /// <param name="a">The first string</param>
        /// <param name="b">Second string</param>
        /// <returns>Concatenation of two strings</returns>
        public string TestMethod(string a, string b = "") => a + b;

        public FakeEnum TestMethodEnum(FakeEnum e) => FakeEnum.A;

        public override string TestAbstract(string a, string b)
        {
            throw new NotImplementedException();
        }

        public void InterfaceMethod()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructors
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public FakeClass() { }
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="a">A.  B left empty</param>
        /// <param name="b"></param>
        public FakeClass(FakeClass a, string b) { }

        /// <summary>
        /// Overload constructor with params
        /// </summary>
        /// <param name="intParam"></param>
        public FakeClass(params int[] intParam) { }

        /// <summary>
        /// Constructor with a default parameter
        /// </summary>
        /// <param name="defaultString"></param>
        public FakeClass(int number, string defaultString = "DefaultString") { }

        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/nested-types
        /// <summary>
        /// Nested class
        /// </summary>
        class NestedClass
        {
            /// <summary>
            /// Nested class private field
            /// </summary>
            private FakeClass _fakeClass;

            /// <summary>
            /// Nested class constructor
            /// </summary>
            /// <param name="s">S</param>
            /// <param name="fc">Accepts fake class</param>
            public NestedClass(string s, FakeClass fc = null)
            {
                _fakeClass = fc;
            }
        }
        private FakeClass.NestedClass _nestedClassInstance = new NestedClass("", null);
        #endregion
        #endregion

        #region Static Members (Fields, Properties, Constants/Read-only, Methods, Constructors)  //-Events,
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/members
        #region Fields
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/fields
        /// <summary>
        /// Private static field
        /// </summary>
        private static string _STcustomBackingField = "Default val";

        /// <summary>
        /// Public static field
        /// </summary>
        public static string STPublicField;
        #endregion

        #region Properties
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
        /// <summary>
        /// public static property
        /// </summary>
        public static string STPublicProperty { get => _STcustomBackingField; set => _STcustomBackingField = value; }

        /// <summary>
        /// Get-only static property
        /// </summary>
        public static string STGetOnlyProperty { get => _STcustomBackingField; }

        /// <summary>
        /// Static property with auto-generated backing field
        /// </summary>
        public static string STAutoProperty { get; set; }
        #endregion

        #region Constants / Read-only
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constants
        /// <summary>
        /// Constant field
        /// </summary>
        public const string STCONST_FIELD = "Constant field";

        /// <summary>
        /// Static readonly field
        /// </summary>
        public readonly static string STREADONLY_FIELD = "Readonly field";
        #endregion

        #region Methods
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods
        /// <summary>
        /// Static void method with blank parameter documentation
        /// </summary>
        /// <param name="c">Description of parameter c</param>
        public static void STTestVoid(string c) { }

        /// <summary>
        /// Adds two strings
        /// </summary>
        /// <param name="a">The first string</param>
        /// <param name="b">Second string</param>
        /// <returns>Concatenation of two strings</returns>
        public static string STTestMethod(string a, string b = "") => a + b;

        public static FakeEnum STTestMethodEnum(FakeEnum e) => FakeEnum.A;
        #endregion
        #endregion

        #region Nested Types (Enum) --Class, Interface, Struct, Delegate, Tuples, Records, Nullables?
        //https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/types
        #region Enums / Flags
        /// <summary>
        /// Fake enum desc
        /// </summary>
        public enum FakeEnum
        {
            /// <summary>
            /// First value of nested enum
            /// </summary>
            A,
            /// <summary>
            /// Second value of nested enum
            /// </summary>
            B,
        }

        /// <summary>
        /// Fake nested byte enum desc
        /// </summary>
        public enum EnumByte : byte
        {
            /// <summary>
            /// First byte value
            /// </summary>
            C = (byte)0,
            /// <summary>
            /// Second byte value
            /// </summary>
            D = (byte)1,
        }

        /// <summary>
        /// Enum with flags attribute
        /// </summary>
        [Flags]
        public enum EnumFlag
        {
            /// <summary>
            /// Red bit
            /// </summary>
            Red = 0x1,
            /// <summary>
            /// Green bit
            /// </summary>
            Green = 0x2,
            /// <summary>
            /// Blue bit
            /// </summary>
            Blue = 0x4,
        }
        #endregion
        #endregion

        #region Other
        //Attributes        https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members
        //Access modifiers  https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers
        //ref, out
        //record
        //async             https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
        //arrays            https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/single-dimensional-arrays
        //generics          https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-type-parameters
        //partials
        #endregion

    }

    /// <summary>
    /// Enum outside of class
    /// </summary>
    public enum NonNestedEnum
    {
        /// <summary>
        /// A is first
        /// </summary>
        A, 
        /// <summary>
        /// B is second
        /// </summary>
        B, 
        /// <summary>
        /// C is third
        /// </summary>
        C
    }
}
