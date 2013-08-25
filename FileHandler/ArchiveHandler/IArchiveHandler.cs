using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Handler.File.ArchiveHandler
{
    public interface IArchiveHandler : IDisposable
    {
        void Add(String _path, String Name, String _contentType);

        void Add(Stream stream, String Name);

        void Close();
    }
}
