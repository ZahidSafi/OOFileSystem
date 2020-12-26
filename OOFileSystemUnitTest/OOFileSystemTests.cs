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
        [TestMethod]
        public void TestDeleteWithOneEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Delete("C:\\folder");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDeleteWithMultipleEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Create("Folders", "folder2", "C:");
            fileSystem.Create("Folders", "folder3", "C:");
            fileSystem.Delete("C:\\folder");
            fileSystem.Delete("C:\\folder2");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDeleteWithNestedEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Create("Folders", "folder2", "C:\\folder");
            fileSystem.Create("Folders", "folder3", "C:\\folder\\folder2");
            fileSystem.Delete("C:\\folder");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestMoveWithTwoEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Create("Folders", "folder2", "C:");
            fileSystem.Move("C:\\folder", "C:\\folder2");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestMoveWithMultipleEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folders", "folder", "C:");
            fileSystem.Create("Folders", "A", "C:\\folder");
            fileSystem.Create("Folders", "B", "C:");
            fileSystem.Create("Folders", "C", "C:\\folder\\A");
            fileSystem.Create("Folders", "D", "C:\\folder\\A");
            fileSystem.Create("Folders", "E", "C:\\B");
            fileSystem.Create("Folders", "F", "C:\\B");
            fileSystem.Move("C:\\folder\\A", "C:\\B");
            Assert.IsTrue(true);
        }
    }
}
