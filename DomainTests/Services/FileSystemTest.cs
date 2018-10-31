using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using Armin.Suitsupply.Domain.Services;


namespace DomainTests.Services
{
    [TestClass]
    public class FileSystemTests
    {
        private FileSystem _fs;

        [TestInitialize]
        public void Init()
        {
            FileSystem.RootDirectory = "Files/";

            _fs = new FileSystem();

            if (Directory.Exists("Files"))
                Directory.Delete("Files", true);
        }

        [TestCleanup]
        public void Destroy()
        {

        }

        [TestMethod]
        public void Create1Test()
        {
            string data = "Test Data";
            string fileName1 = "CreatedFile_1.txt";
            string filePath1 = FileSystem.RootDirectory + FileSystem.FileTypeSpecificDirectory[FileTypes.ProductPhoto] + "/" + fileName1;
            byte[] fileData = Encoding.ASCII.GetBytes(data);

            _fs.Create(fileData, FileTypes.ProductPhoto, fileName1);
            Assert.IsTrue(File.Exists(filePath1));
            Assert.AreEqual(data, File.ReadAllText(filePath1));


        }

        [TestMethod]
        public void CreateByDirectoryTest()
        {
            string data = "Test Data";
            string directory = "DirectoryName";
            string fileName1 = "CreatedFile_1.txt";
            string filePath1 = FileSystem.RootDirectory + FileSystem.FileTypeSpecificDirectory[FileTypes.ProductPhoto] + "/" + directory + "/" + fileName1;
            byte[] fileData = Encoding.ASCII.GetBytes(data);

            _fs.Create(fileData, FileTypes.ProductPhoto, directory, fileName1);
            Assert.IsTrue(File.Exists(filePath1));
            Assert.AreEqual(data, File.ReadAllText(filePath1));

        }

        [TestMethod]
        public void MoveFullPathOnFileSystemTest()
        {
            string sourceFilePath = "Files/FileToMove.txt";
            string destFilePath = "Files/FileMoved.txt";

            Directory.CreateDirectory("Files");
            File.WriteAllText(sourceFilePath, "");

            _fs.Move(sourceFilePath, destFilePath);
            Assert.IsTrue(File.Exists(destFilePath));
        }

        [TestMethod]
        public void MoveLocalToFileSystemTest()
        {
            string sourceFilePath = "Files/FileToMove.txt";
            string destFileName = "FileMoved.txt";
            string destFilePath = FileSystem.RootDirectory + FileSystem.FileTypeSpecificDirectory[FileTypes.ProductPhoto] + "/" + destFileName;

            Directory.CreateDirectory("Files");
            File.WriteAllText(sourceFilePath, "");

            _fs.Move(sourceFilePath, FileTypes.ProductPhoto, destFileName);
            Assert.IsTrue(File.Exists(destFilePath));
        }

        [TestMethod]
        public void MoveFileSystemToLocalTest()
        {
            string sourceFilePathToMove = "Files/FileMoved.txt";
            string destFileName = "FileToMove.txt";
            string destFilePath = FileSystem.RootDirectory + FileSystem.FileTypeSpecificDirectory[FileTypes.ProductPhoto] + "/" + destFileName;

            Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
            File.WriteAllText(destFilePath, "");

            _fs.Move(FileTypes.ProductPhoto, destFileName, sourceFilePathToMove);
            Assert.IsTrue(File.Exists(sourceFilePathToMove));
        }

        [TestMethod]
        public void DeleteFileTest()
        {
            string fileName = "FileToDelete.txt";
            string filePath = FileSystem.RootDirectory + FileSystem.FileTypeSpecificDirectory[FileTypes.ProductPhoto] + "/" + fileName;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, "");

            _fs.Delete(fileName, FileTypes.ProductPhoto);
            Assert.IsFalse(File.Exists(filePath));
        }

        [TestMethod]
        public void ExistFileTest()
        {
            string fileName = "FileToExist.txt";
            string filePath = FileSystem.RootDirectory + FileSystem.FileTypeSpecificDirectory[FileTypes.ProductPhoto] + "/" + fileName;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, "");

            _fs.Exists(FileTypes.ProductPhoto, fileName);
            Assert.IsTrue(File.Exists(filePath));
        }
    }
}
