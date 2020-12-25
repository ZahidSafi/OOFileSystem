using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOFileSystem;
using System;

namespace OOFileSystemUnitTest
{
    [TestClass]
    public class OOFileSystemTests
    {
        [TestMethod]
        public void TestCreateWithOneEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestCreateWithMultipleEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Create("Folders", "folder2", "C:");
            fileSystem.Create("Folders", "folder3", "C:");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestCreateWithMultipleEntityWithNestedEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Create("Folders", "folder2", "C:");
            fileSystem.Create("Folders", "folder3", "C:");
            fileSystem.Create("Folders", "A", "C:\\folder");
            fileSystem.Create("Folders", "B", "C:\\folder2");
            fileSystem.Create("Folders", "C", "C:\\folder3");
            Assert.IsTrue(true);
        }
    }
}
