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
        #region Events / Delegates
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/
        /// <summary>
        /// Event check
        /// </summary>
        
        
        // Existing event
        public event OnChangeEventHandler OnEventFire;

        /// <summary>
        /// Delegate for event
        /// </summary>
        //public delegate void EventHandler(object sender, Event args)

        public void WireEvent()
        {
            OnEventFire += FakeClass_OnEventFire;
            OnEventFire += (sender, e) => { };            
        }

        private void FakeClass_OnEventFire(object sender, SqlNotificationEventArgs e)
        {
            throw new NotImplementedException();
        }

        //Multi-cast
        #endregion

        #region Members (Fields, Properties, Constants/Read-only, Methods, Constructors)  //-Events,
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

        #region Constants / Read-only
        //https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constants
        /// <summary>
        /// Constant field
        /// </summary>
        const string CONST_FIELD = "Constant field";

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
        public FakeClass(string defaultString = "DefaultString") { }

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

        #region Types (Class, Interface, Struct, Enum, Delegate) --Tuples, Records, Nullables
        //https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/types

        #region Enums / Flags
        /// <summary>
        /// Enum check
        /// </summary>
        public enum FakeEnum
        {
            A,
            B,
        }

        public enum EnumByte : byte
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Flags]
        public enum EnumFlag
        {
            Red = 0x1,
            Green = 0x2,
            Blue = 0x4,
            Yellow = 0x8,
        }
        #endregion


        #endregion

        #region Other
        //Static            https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members
        //Attributes        https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members
        //Access modifiers  https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers
        //ref, out
        //Record
        //async             https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
        //arrays            https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/single-dimensional-arrays
        //generics          https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/generic-type-parameters
        //partialne

        #region Attributes

        #endregion
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
