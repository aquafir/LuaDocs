using FakeProject;
using NuDoq;
using System.Reflection;

namespace LuaDocs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var outputDir = new ScriptManagerOptions().ScriptDirectory;
            //var outputPath = Path.Combine(outputDir, "__definitions.lua");
            //if (!Directory.Exists(outputDir))
            //{
            //    Directory.CreateDirectory(outputDir);
            //}

            var path = @"FakeProject.xml";
            var target = DocReader.Read(typeof(FakeClass).Assembly, path);

            //Extract docs
            var xmlVisitor = new XmlVisitor();
            target.Accept(xmlVisitor);
            var documentation = xmlVisitor.Documentation;

            LuaDocumenter.Document(documentation, target.IdMap, Assembly.GetAssembly(typeof(FakeClass)));
        }
    }
}
