using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeProject
{
    public class UndocumentedClass
    {
        public string Field;
        public string Property { get; set; }
        public FakeClass Method(string Param, params int[] VariadicParams) { return null; }
    }
}
