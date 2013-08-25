/*
 * Name: Handler.FileHandler
 * Date: 20 Februar 2011
 * Author: Patrik Kimmeswenger
 * Description: serves some methods to handle file and directory communication
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Handler.File.Text
{
    public static class TextFileManager
    {

        ///<summary>
        /// Writes a _text to _destinationFilePath
        /// and returns <returns>true</returns> if writting _text has been successfull
        /// otherwise <returns>false</returns> if it has failed
        /// 
        /// <param name="_destinationFilePath"> destination path where the file should be created</param>
        /// <param name="_text"> text which should be written into the file</param>
        ///</summary>
        public static bool _write_TextFile(string _destinationFilePath, string _text)
        {
            StreamWriter streamWriter = new StreamWriter(_destinationFilePath);

            try
            {
                streamWriter.Write(_text);
                streamWriter.Flush();
                streamWriter.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }    
        }


        ///<summary>
        /// Reads a _text from _sourceFilePath
        /// and returns <returns>true</returns> if reading _text has been successfull
        /// otherwise <returns>false</returns> if it has failed
        /// 
        /// <param name="_sourceFilePath"> source path where of the file</param>
        /// <param name="_text"> text which should be read from the file</param>
        ///</summary>
        public static bool _read_TextFile(string _sourceFilePath, ref string _text)
        {
            StreamReader streamReader;

            if (System.IO.File.Exists(_sourceFilePath))
            {
                streamReader = new StreamReader(_sourceFilePath);

                try
                {
                    _text = streamReader.ReadToEnd();
                    streamReader.Close();

                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }

            return false;

        }

        ///<summary>
        /// Reads a _text from _sourceFilePath
        /// and returns <returns>true</returns> if reading _text has been successfull
        /// otherwise <returns>false</returns> if it has failed
        /// 
        /// <param name="_sourceFilePath"> source path where of the file</param>
        /// <param name="_text"> text which should be read from the file</param>
        ///</summary>
        public static string _read_TextFile(string _sourceFilePath)
        {
            StreamReader streamReader;

            if (System.IO.File.Exists(_sourceFilePath))
            {
                streamReader = new StreamReader(_sourceFilePath);

                try
                {
                    return streamReader.ReadToEnd();
                }
                catch (IOException)
                {
                    return null;
                }
            }

            return null;

        }


        public static string _ConvertToStringFilePath(string _filePath)
        {
            char searchChar = '\\';
            int index = new Int32();

            while((index = _filePath.IndexOf(searchChar, index)) != -1)
            {
                if ((index + 1) < _filePath.Length && _filePath[index + 1] != searchChar)
                {
                    _filePath.Insert(index, searchChar.ToString());
                    index++;
                }

                index++;
            }

                
            
            return _filePath;
        }


        public static string _ConvertToNormalFilePath(string _filePath)
        {
            string searchString = "\\\\";
            int index = new Int32();

            
            while((index = _filePath.IndexOf(searchString, index)) != -1)
            {
                _filePath.Remove(index);
                index++;
            }
                
            return _filePath;
        }

        public enum IOType { File, Directory, DontExist };

        /// <summary>
        /// Gibt die Endung einer Datei zurück
        /// <param name="_path">Pfad der Datei</param>
        /// <returns>Endung ohne Punkt</returns>
        /// </summary>
        public static string _Extenstion(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path).Extension.Substring(1);

            else
                throw new FileNotFoundException("Datei existiert nicht.");

        }


        /// <summary>
        /// Gibt die Dateigröße zurück
        /// </summary>
        /// <param name="_path">Pfad der Datei</param>
        /// <returns>Dateigröße</returns>
        public static long _FileSize(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path).Length;


            else
                throw new FileNotFoundException("Datei existiert nicht.");
        }



        /// <summary>
        /// Gibt die Ordnergröße zurück
        /// </summary>
        /// <param name="_path">Pfad zum Ordner</param>
        /// <param name="_recursiv">Unterordner auch dazu zählen</param>
        /// <returns>Ordnergröße</returns>
        public static long _FolderSize(this String _path, bool _recursiv)
        {
            if (Directory.Exists(_path))
            {
                if (_recursiv)
                    return FolderSize(new DirectoryInfo(_path));

                else
                {
                    long retValue = new Int32();

                    foreach (FileInfo fi in new DirectoryInfo(_path).GetFiles())
                        retValue += fi.Length;

                    return retValue;
                }
            }
            else
                throw new DirectoryNotFoundException("Ordner existiert nicht.");
        }


        /// <summary>
        /// Erzeigt aus einem Pfad ein FileInfo
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>FileInfo</returns>
        public static FileInfo _ConvertToFileInfo(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path);

            else
                throw new FileNotFoundException("Datei existiert nicht.");
        }


        /// <summary>
        /// Erzeugt aus einem Pfad ein DirectoryInfo
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>DirectoryInfo</returns>
        public static DirectoryInfo _ConvertToDirectoryInfo(this String _path)
        {
            if (Directory.Exists(_path))
                return new DirectoryInfo(_path);

            else
                throw new DirectoryNotFoundException("Ordner existiert nicht.");
        }


        /// <summary>
        /// Ermittelt ob der Pfad zu eine Datei oder zu einem Ordner zeigt
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>Datei/Ordner/Datei existiert nicht</returns>
        public static IOType _Type(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return IOType.File;

            else if (Directory.Exists(_path))
                return IOType.Directory;

            return IOType.DontExist;
        }


        /// <summary>
        /// Letzter Schreibzugriff auf den Pfad
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>Zeit</returns>
        public static DateTime _LastWriteTime(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path).LastWriteTime;

            else if (Directory.Exists(_path))
                return new DirectoryInfo(_path).LastWriteTime;

            else
                throw new IOException("Kein Ziel zu diesem Pfad Vorhanden");
        }


        /// <summary>
        /// Letzter Lesezugriff auf den Pfad
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>Zeit</returns>
        public static DateTime _LastAccessTime(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path).LastAccessTime;

            else if (Directory.Exists(_path))
                return new DirectoryInfo(_path).LastAccessTime;

            else
                throw new IOException("Kein Ziel zu diesem Pfad Vorhanden");
        }


        /// <summary>
        /// Zeit an dem die Datei/der Ordner erstellt wurde
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>Zeit</returns>
        public static DateTime _CreationTime(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path).CreationTime;

            else if (Directory.Exists(_path))
                return new DirectoryInfo(_path).CreationTime;

            else
                throw new IOException("Kein Ziel zu diesem Pfad Vorhanden");
        }


        /// <summary>
        /// Gibt den Übergeordneten Ordner an
        /// </summary>
        /// <param name="_path">Pfad</param>
        /// <returns>Übergeordneter Ordner</returns>
        public static string _Directory(this String _path)
        {
            if (System.IO.File.Exists(_path))
                return new FileInfo(_path).DirectoryName;

            else if (Directory.Exists(_path))
                return new DirectoryInfo(_path).Parent.Name;

            else
                throw new IOException("Kein Ziel zu diesem Pfad Vorhanden");
        }


        /// <summary>
        /// Liste mit untergeordneten Dateien
        /// </summary>
        /// <param name="_pfad">Pfad des Ordners</param>
        /// <returns>Liste</returns>
        public static List<string> _ListFiles(this String _path)
        {
            List<string> retValue = new List<string>();

            if (!Directory.Exists(_path))
                throw new DirectoryNotFoundException("Ordner existiert nicht.");

            foreach (FileInfo fi in new DirectoryInfo(_path).GetFiles())
                retValue.Add(fi.FullName);

            return retValue;
        }


        /// <summary>
        /// Liste mit untergeordneten Ordner
        /// </summary>
        /// <param name="_pfad">Pfad des Ordners</param>
        /// <returns>Liste</returns>
        public static List<string> _ListDirectories(this String _path)
        {
            List<string> retValue = new List<string>();

            if (!Directory.Exists(_path))
                throw new DirectoryNotFoundException("Ordner existiert nicht.");

            foreach (DirectoryInfo di in new DirectoryInfo(_path).GetDirectories())
                retValue.Add(di.FullName);

            return retValue;
        }


        #region Hilfmethoden

        /// <summary>
        /// Ermittelt die Ordnergröße samt Unterordner
        /// </summary>
        /// <param name="_di">Ordner</param>
        /// <returns>Größe</returns>
        private static long FolderSize(DirectoryInfo _di)
        {
            long retValue = new Int32();

            foreach (DirectoryInfo di in _di.GetDirectories())
                retValue += FolderSize(di);

            foreach (FileInfo fi in _di.GetFiles())
                retValue += fi.Length;

            return retValue;
        }
        #endregion



    }
}


