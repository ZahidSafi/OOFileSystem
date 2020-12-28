using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOFileSystem;
using System;
using static OOFileSystem.FileSystem;

namespace OOFileSystemUnitTest
{
    [TestClass]
    public class OOFileSystemTests
    {
        [TestMethod]
        public void TestCreateWithOneEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestCreateDrive()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Drive", "D:", "");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestCreateWithMultipleEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Folder", "folder2", "C:");
            fileSystem.Create("Folder", "folder3", "C:");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestCreateWithMultipleEntityWithNestedEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Folder", "folder2", "C:");
            fileSystem.Create("Folder", "folder3", "C:");
            fileSystem.Create("Folder", "A", "C:\\folder");
            fileSystem.Create("Folder", "B", "C:\\folder2");
            fileSystem.Create("Folder", "C", "C:\\folder3");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDeleteWithOneEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Delete("C:\\folder");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDeleteWithMultipleEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Folder", "folder2", "C:");
            fileSystem.Create("Folder", "folder3", "C:");
            fileSystem.Delete("C:\\folder");
            fileSystem.Delete("C:\\folder2");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestDeleteWithNestedEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Folder", "folder2", "C:\\folder");
            fileSystem.Create("Folder", "folder3", "C:\\folder\\folder2");
            fileSystem.Delete("C:\\folder");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestMoveWithTwoEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Folder", "folder2", "C:");
            fileSystem.Move("C:\\folder", "C:\\folder2");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestMoveWithMultipleEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Folder", "A", "C:\\folder");
            fileSystem.Create("Folder", "B", "C:");
            fileSystem.Create("Folder", "C", "C:\\folder\\A");
            fileSystem.Create("Folder", "D", "C:\\folder\\A");
            fileSystem.Create("Folder", "E", "C:\\B");
            fileSystem.Create("Folder", "F", "C:\\B");
            fileSystem.Move("C:\\folder\\A", "C:\\B");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestWriteToFileWithOneEntity()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Text", "txt", "C:");
            fileSystem.WriteToFile("C:\\txt", "Hello World!");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestWriteToFileWithNestedEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Text", "txt", "C:");
            fileSystem.WriteToFile("C:\\txt", "Hello World!");
            fileSystem.Create("Folder", "folder", "C:");
            fileSystem.Create("Text", "txt2", "C:\\folder");
            fileSystem.WriteToFile("C:\\folder\\txt2", "This is a test!");
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void TestWriteToFileWithZipFilesEntities()
        {
            FileSystem fileSystem = new FileSystem();
            fileSystem.Create("Text", "txt", "C:");
            fileSystem.WriteToFile("C:\\txt", "Hello World!");
            fileSystem.Create("Zip", "folder", "C:");
            fileSystem.Create("Text", "txt2", "C:\\folder");
            fileSystem.Create("Text", "txt3", "C:\\folder");
            fileSystem.WriteToFile("C:\\folder\\txt2", "This is a test!");
            fileSystem.WriteToFile("C:\\folder\\txt3", "Another test!");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestCreateFileWithInvalidFilePath()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "D:\\f2");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Path not found");
            }

        }
        [TestMethod]
        public void TestCreateFileWithMissingDriverInFilePath()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "f2");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Illegal File System Operation");
            }

        }
        [TestMethod]
        public void TestCreateFileWithDriveAsChild()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "C:\\C:");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Illegal File System Operation");
            }

        }
        [TestMethod]
        public void TestCreateFileWithFileThatExist()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "C:");
                fileSystem.Create("Folder", "folder", "C:");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Path already exists");
            }

        }
        [TestMethod]
        public void TestCreateEntityWithinTextFile()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Text", "txt", "C:");
                fileSystem.Create("Folder", "folder", "C:\\txt");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Illegal File System Operation");
            }

        }
        [TestMethod]
        public void TestCreateFileWithInvalidFolder()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Text", "folder", "C:");
                fileSystem.Create("Folder", "folder3", "C:\\folder\\folder2");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Path not found");
            }

        }
        [TestMethod]
        public void TestMoveFileWithPathExist()
        {

            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "C:");
                fileSystem.Create("Folder", "A", "C:\\folder");
                fileSystem.Create("Folder", "B", "C:");
                fileSystem.Create("Folder", "A", "C:\\B");
                fileSystem.Move("C:\\B\\A", "C:\\folder");
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Path already exists");
            }

        }
        [TestMethod]
        public void TestWriteToFileThatIsNotText()
        {
            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "C:");
                fileSystem.WriteToFile("C:\\folder", "Hello World!");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Not a text file");
            }

        }
        [TestMethod]
        public void TestWriteToFilePathNotFound()
        {
            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.WriteToFile("C:\\folder2", "Hello World!");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Path not found");
            }

        }
        [TestMethod]
        public void TestCreateDriveWithinAFolder()
        {
            try
            {
                FileSystem fileSystem = new FileSystem();
                fileSystem.Create("Folder", "folder", "C:");
                fileSystem.Create("Drive", "D:", "C:\\folder");
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Illegal File System Operation");
            }

        }
    }
}
