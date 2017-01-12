using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PersonSearch.Tests.Controllers
{
    [TestClass]
    public class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
        }
    }
}
