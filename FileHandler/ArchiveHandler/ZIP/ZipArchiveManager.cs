
/*
 * Filename: ZipArchiveManager.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * Description: Exposes methods to archive files.
 */

// System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Packaging;
using System.IO;
using System.Net.Mime;
using System.IO.Compression;
using HLib.File.ArchiveHandler;

namespace HLib.File.Archive.ZIP
{
    public class ZipArchiveManager : IArchiveHandler
    {

        #region Objects

        private Package    package;
        private FileStream packageStream;

        #endregion Objects

        #region Constructor

        public ZipArchiveManager(String _path)
        {     
            // get the stream of the archive
            // if the file doesn't exits create it
            this.packageStream = System.IO.File.Open(_path, FileMode.Create);

            // get the archive
            // if the archive doesn't exits create it
            this.package = ZipPackage.Open(packageStream, FileMode.Create);
        }

        #endregion Constructor

        #region Methods

        public void Add(String _path, String Name, String _contentType)
        {
            try
            {
                // initialize a stream to the archive.
                FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);

                // create the relative Uri of the package
                Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(Name, UriKind.Relative));

                // create a a package part which will be added to the archive
                PackagePart packagePartDocument = package.CreatePart(partUriDocument, _contentType);
                
                // add the package part to the archive
                CopyStream(stream, packagePartDocument.GetStream());
                stream.Close();
            }
            catch (Exception _ex)
            {
                throw _ex;
            }
        }

        public void Add(Stream stream, String Name)
        {
            Uri partUriDocument = PackUriHelper.CreatePartUri(new Uri(Name, UriKind.Relative));
            PackagePart packagePartDocument = package.CreatePart(partUriDocument, String.Empty);

            CopyStream(stream, packagePartDocument.GetStream());
            stream.Close();
        }
 
        public void Close()
        {
            this.package.Close();
            this.packageStream.Close();
        }

        /// <summary> Copies data from a source stream to a target stream.</summary>
        /// <param name="source"> The source stream to copy from.</param>
        /// <param name="target"> The destination stream to copy to.</param>
        private static void CopyStream(Stream source, Stream target)
        {
            // specify the buffer size
            const Int32 bufSize = 0x1000;

            // initialize the buffer
            byte[] buf = new byte[bufSize];

            // initialize the help variable
            Int32 bytesRead = new Int32();

            // Copies data from a source stream to a target stream
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            { target.Write(buf, 0, bytesRead); }
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        public static void OpenArchive(String _path, String _outPutPath)
        {
            using (Package package = Package.Open(_path, FileMode.Open, FileAccess.Read))
            {
                DirectoryInfo dir = Directory.CreateDirectory(_outPutPath);

                foreach (PackagePart part in package.GetParts())
                {
                    String target = Path.Combine(dir.FullName, CreateFilenameFromUri(part.Uri));

                    using (Stream source = part.GetStream(FileMode.Open, FileAccess.Read))
                    using (Stream destination = System.IO.File.OpenWrite(target))
                    {
                        byte[] buffer = new byte[0x1000];
                        Int32 read;

                        while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            destination.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }

        private static String CreateFilenameFromUri(Uri _uri)
        {
            String uri = _uri.OriginalString;

            char[] invalidChars = Path.GetInvalidFileNameChars();
            
            StringBuilder sb = new StringBuilder(uri.Length);

            if(uri.StartsWith("/") || uri.StartsWith("\\"))
            {
                uri = uri.Remove(0,1);
            }

            foreach (char c in uri)
            {
                sb.Append(Array.IndexOf(invalidChars, c) < 0 ? c : '_');
            }
            return sb.ToString();
        }

        #endregion Methods

    }
}
