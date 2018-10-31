using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Armin.Suitsupply.Domain.Helpers;

namespace Armin.Suitsupply.Domain.Services
{
    public class FileSystem
    {
        /// <summary>
        /// File system's root directory, please add \ if there none at the end of string
        /// </summary>
        public static string RootDirectory { get; set; }

        public static Dictionary<FileTypes, string> FileTypeSpecificDirectory { get; set; } =
            new Dictionary<FileTypes, string> { { FileTypes.ProductPhoto, "photos" } };

        public static readonly char PathSep = '/'; //Path.PathSeparator;

        public static string Combine(params string[] tokens)
        {
            List<string> parts = new List<string>();
            foreach (var part in tokens)
            {
                parts.Add(part.Trim('\\', '/'));
            }

            return string.Join(PathSep.ToString(), parts);
        }

        public static string GetCleanPath(string path)
        {
            return Combine(path.Split(new[] { PathSep }, StringSplitOptions.RemoveEmptyEntries));
        }

        private string GetFullFilePathOnFileServer(FileTypes fileType, string fileName, string directory = "")
        {
            string fullPath = RootDirectory;

            if (FileTypeSpecificDirectory.Keys.Contains(fileType))
                fullPath += FileTypeSpecificDirectory[fileType] + "/" + directory + "/" + fileName;

            return fullPath;
        }

        private string GetFullFilePathOnFileServer(string fileName, string directory = "")
        {
            string fullPath = RootDirectory;

            fullPath += (string.IsNullOrEmpty(directory) ? "" : directory + "/") + fileName;

            return fullPath;
        }


        /// <summary>
        /// Create valid file name for further usage
        /// </summary>
        /// <param name="uploadedName">Uploaded file name</param>
        public string CreateDefaultFileName(string uploadedName)
        {
            return CommonHelper.GetRandomString(10, false, false)
                   + Path.GetExtension(uploadedName);
        }

        /// <summary>
        /// Move the source file to new destination, create destination folders if not exist
        /// </summary>
        private void MoveFull(string sourcePath, string destinationPath, bool overwrite = false)
        {
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

            if (Exists(destinationPath))
            {
                if (overwrite)
                {
                    try
                    {
                        File.Delete(destinationPath);
                    }
                    catch (Exception er)
                    {
                        throw new Exception("Error in deleting file" + Environment.NewLine + er.Message);
                    }
                }
                else
                {
                    throw new Exception("Destination file already exists");
                }
            }

            File.Move(sourcePath, destinationPath);
        }

        /// <summary>
        /// Create file in file server, Replace file if exist
        /// </summary>
        /// <param name="fileData">File data to write.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="fileName">Name of the file.</param>
        public void Create(byte[] fileData, FileTypes fileType, string fileName)
        {
            Create(fileData, fileType, "", fileName);
        }

        /// <summary>
        /// Create file in file server with additional directory, Replace file if exist
        /// </summary>
        /// <param name="fileData">File data to write.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="directory">Any directory path before fileName.</param>
        /// <param name="fileName">Name of the file.</param>
        public void Create(byte[] fileData, FileTypes fileType, string directory, string fileName)
        {
            try
            {
                string fullPath = GetFullFilePathOnFileServer(fileType, fileName, directory);
                string dirName = Path.GetDirectoryName(fullPath);

                if (null != dirName && !Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                File.WriteAllBytes(fullPath, fileData);
            }
            catch (Exception er)
            {
                throw new Exception("Error in creating file" + Environment.NewLine + er.Message);
            }
        }

        public async Task<string> Create(Stream stream, FileTypes fileType, string fileName)
        {
            return await Create(stream, fileType, String.Empty, fileName);
        }

        /// <summary>
        /// Create file in file server with additional directory, Replace file if exist
        /// </summary>
        /// <param name="stream">File stream to write.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="directory">Any directory path before fileName.</param>
        /// <param name="fileName">Name of the file.</param>
        public async Task<string> Create(Stream stream, FileTypes fileType, string directory, string fileName)
        {
            try
            {
                string fullPath = GetFullFilePathOnFileServer(fileType, fileName, directory);
                string dirName = Path.GetDirectoryName(fullPath);

                if (null != dirName && !Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);

                using (var fileStream = File.Create(fullPath))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(fileStream);
                }

                return FileSystem.GetCleanPath(FileTypeSpecificDirectory[fileType] + "/" + directory + "/" + fileName);
            }
            catch (Exception er)
            {
                throw new Exception("Error in creating file" + Environment.NewLine + er.Message);
            }
        }

        /// <summary>
        /// Move file with full path in file server
        /// </summary>
        /// <param name="sourcePath">The full path of the file to move.</param>
        /// <param name="destinationPath">The full path of the new file.</param>
        /// <param name="overwrite">Overwrite the file if exist.</param>
        public void Move(string sourcePath, string destinationPath, bool overwrite = false)
        {
            try
            {
                MoveFull(sourcePath, destinationPath, overwrite);
            }
            catch (Exception er)
            {
                throw new Exception("Error in moving file" + Environment.NewLine + er.Message);
            }
        }

        /// <summary>
        /// Move file from local to file system server
        /// </summary>
        /// <param name="sourcePath">The full path of the file to move.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="overwrite">Overwrite the file if exist.</param>
        public void Move(string sourcePath, FileTypes fileType, string fileName, bool overwrite = false)
        {
            try
            {
                string destinationPath = GetFullFilePathOnFileServer(fileType, fileName);

                MoveFull(sourcePath, destinationPath, overwrite);
            }
            catch (Exception er)
            {
                throw new Exception("Error in moving file" + Environment.NewLine + er.Message);
            }
        }

        /// <summary>
        /// Move file from file server to local
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="destnationPath">The full path of the new file.</param>
        /// <param name="overwrite">Overwrite the file if exist.</param>
        public void Move(FileTypes fileType, string fileName, string destnationPath, bool overwrite = false)
        {
            try
            {
                string sourcePath = GetFullFilePathOnFileServer(fileType, fileName);

                MoveFull(sourcePath, destnationPath, overwrite);
            }
            catch (Exception er)
            {
                throw new Exception("Error in moving file" + Environment.NewLine + er.Message);
            }
        }

        /// <summary>
        /// Determines the file exist on the file server.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="fileName">Name of the file.</param>
        public bool Exists(FileTypes fileType, string fileName)
        {
            return File.Exists(GetFullFilePathOnFileServer(fileType, fileName));
        }

        /// <summary>
        /// Determines the file exist on the file server.
        /// </summary>
        /// <param name="filePath">Name of the file.</param>
        public bool Exists(string filePath)
        {
            return File.Exists(GetFullFilePathOnFileServer(filePath));
        }


        /// <summary>
        /// Delete file from file server
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="fileName">Name of the file.</param>
        public void Delete(string fileName, FileTypes? fileType = null)
        {
            try
            {
                if (null == fileType)
                {
                    File.Delete(GetFullFilePathOnFileServer(fileName));
                }
                else
                {
                    File.Delete(GetFullFilePathOnFileServer((FileTypes)fileType, fileName));
                }
            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception er)
            {
                throw new Exception("Error in deleting file" + Environment.NewLine + er.Message);
            }
        }
    }

    public enum FileTypes
    {
        ProductPhoto
    }
}