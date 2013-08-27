/*
 * Filename: FTPItem.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * Description: Represents a ftp item.
 */

// System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HLib.Network.IP.FTP
{
    public class FTPItem
    {

        #region Objects

        private static FTPItem rootFTPItem;

        private Boolean isDirectory;

        private ItemPrivileges itemPrivileges;

        private Int32 node;

        private String user;
        private String group;

        private Int64 size;

        private DateTime dateModified;

        private String name;

        #endregion Objects

        #region Properties

        public static FTPItem RootFTPItem
        {
            get
            {
                if (FTPItem.rootFTPItem == null)
                    FTPItem.rootFTPItem = FTPItem.gernerateRootItem();

                return FTPItem.rootFTPItem;
            }
        }

        public Boolean IsDirectory
        { get { return this.isDirectory; } }

        public ItemPrivileges ItemPrivileges
        { get { return this.itemPrivileges; } }

        public Int32 Node
        { get { return this.node; } }

        public String USER_NAME
        { get { return this.user; } }

        public String Group
        { get { return this.group; } }

        public Int64 Size
        { get { return this.size; } }

        public DateTime DateModified
        { get { return this.dateModified; } }

        public String Name
        { get { return this.name; } }

        #endregion Properties;

        #region Constructor

        private FTPItem() { }

        #endregion Constructor

        #region Methods

        public static FTPItem GetFTPItem(String _item, params Char[] _delimiter)
        {
            List<String> attributes = new List<String>();
            FTPItem ftpItem = new FTPItem();

            // split items
            foreach (String attribute in _item.Split(_delimiter))
            {
                if (attribute.Length != 0)
                    attributes.Add(attribute);
            }

            DateTime dateTime;
            String dateTimeString = String.Empty;

            // if the ftpItem starts with modified Date ( Microsoft IIS Format )
            if (attributes[0].Contains('-') && !(attributes[0].Contains('r') || attributes[0].Contains('w') || attributes[0].Contains('x')))
            {
                try
                {
                    List<String> dateTimeParts = attributes[0].Insert(attributes[0].LastIndexOf('-') + 1, "20").Split('-').ToList();

                    dateTimeString = String.Format("{0}.{1}.{2}", dateTimeParts[1], dateTimeParts[0], dateTimeParts[2]);
                }
                catch{};
            }

            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                Int32 size;
                String datetime = String.Format("{0} {1}", dateTimeString, attributes[1]);
                DateTime dateModified;


                if (DateTime.TryParse(datetime, out dateModified))
                {
                    ftpItem.dateModified = dateModified;
                }

                if (attributes[2].Equals("<DIR>"))
                {
                    ftpItem.isDirectory = true;

                    ftpItem.name = attributes[3];
                }

                else
                {

                    if (Int32.TryParse(attributes[2], out size))
                    {
                        ftpItem.size = size;
                    }

                    ftpItem.name = attributes[3];
                }

            }

            else
            {
                if (_item[0] == 'd' || _item[0] == 'l')
                    ftpItem.isDirectory = true;

                else
                    ftpItem.isDirectory = false;

                _item = _item.Remove(0, 1);

                String dateTimeFormatString = String.Empty;

                if (attributes[(Int32)Attribute.DateModified_TimeOrYear].Contains(':'))
                    dateTimeFormatString = "{0}.{1}.2011 {2}:00";

                else
                    dateTimeFormatString = "{0}.{1}.{2}";


                String dateModified = String.Format(dateTimeFormatString,
                                           attributes[(Int32)Attribute.DateModified_Day],
                                           attributes[(Int32)Attribute.DateModified_Month],
                                           attributes[(Int32)Attribute.DateModified_TimeOrYear]
                                           );

                ftpItem.itemPrivileges = ItemPrivileges.GetItemPrivileges(attributes[(Int32)Attribute.ItemPrivileges]);
                ftpItem.node           = Int32.Parse(attributes[(Int32)Attribute.Node]);
                ftpItem.user           = attributes[(Int32)Attribute.USER_NAME];
                ftpItem.group          = attributes[(Int32)Attribute.Group];
                ftpItem.size           = Int32.Parse(attributes[(Int32)Attribute.Size]);
                ftpItem.dateModified   = DateTime.Parse(dateModified);

                ftpItem.name = attributes[(Int32)Attribute.Name];

                for (Int32 i = (Int32)Attribute.Name + 1; i < attributes.Count; i++)
                {
                    ftpItem.name = String.Format("{0} {1}", ftpItem.name, attributes[i]);
                }
            }

            return ftpItem;

        }

        private static Boolean specifyIsDirectory(List<String> _attributes)
        {
            if (_attributes.Count > 0)
            {
                if (_attributes[0].StartsWith("d") || _attributes.Contains("<DIR>"))
                {
                    return true;
                }
            }

            return false;
        }

        private static Int32 specifyNode(ref List<String> _attributes)
        {
            Int32 node = new Int32();

            if (_attributes.Count == 0)
                return 0;

            foreach (String attribute in _attributes)
            {
                if (Int32.TryParse(attribute, out node))
                {
                    _attributes.Remove(attribute);
                    break;
                }
            }

            return node;
            
        }

        private static DateTime specifyModifiedDate(List<String> _attributes)
        {
            DateTime modifiedDate = new DateTime();

            if (_attributes.Count == 0)
                return modifiedDate;

            foreach (String attribute in _attributes)
            {
                if (!DateTime.TryParse(attribute, out modifiedDate))
                    continue;
            }

            return modifiedDate;
        }

        private static Int32 specifySize(List<String> _attributes)
        {
            Int32 size = new Int32();

            if (_attributes.Count == 0)
                return 0;

            foreach(String attribute in _attributes)
            {
                if (!Int32.TryParse(attribute, out size))
                    continue;
            }

            return size;
        }

        private static ItemPrivileges specifyItemPrivileges(ref List<String> _attributes)
        {
            ItemPrivileges itemPrivileges = new ItemPrivileges();

            DateTime dateTime;

            if (_attributes.Count == 0)
                return itemPrivileges;

            // if no privileges are setted
            if (DateTime.TryParse(_attributes[0], out dateTime))
                return itemPrivileges;

            else
            {
                itemPrivileges = ItemPrivileges.GetItemPrivileges(_attributes[0]);
            }


            return itemPrivileges;
        }

        private static FTPItem gernerateRootItem()
        {
            FTPItem rootFTPItem = new FTPItem();

            rootFTPItem.name = @"\";
            rootFTPItem.node = new Int32();
            rootFTPItem.size = new Int32();

            rootFTPItem.user  = String.Empty;
            rootFTPItem.group = String.Empty;

            rootFTPItem.itemPrivileges = new ItemPrivileges();
            rootFTPItem.isDirectory    = true;

            rootFTPItem.dateModified = DateTime.Now;

            return rootFTPItem;
        }

        public override String ToString()
        {
            String dateTimeModified = String.Empty;
            Char isDirectory = '-';

            if (IsDirectory)
                isDirectory = 'd';

            if (this.DateModified.TimeOfDay.ToString() == "00:00:00")
                dateTimeModified = String.Format("{0} {1} {2} ",
                                        this.getMonth(this.DateModified.Month),
                                        this.addHeadingZero(this.DateModified.Day),
                                        this.DateModified.Year
                                        );
                                            
            else dateTimeModified = String.Format("{0} {1} {2}:{3}",
                                        this.getMonth(this.DateModified.Month),
                                        this.addHeadingZero(this.DateModified.Day),
                                        this.addHeadingZero(this.DateModified.Hour),
                                        this.addHeadingZero(this.DateModified.Minute)
                                        );


            return String.Format("{0}{1} {2} {3} {4}\t\t{5} {6} {7}",
                            isDirectory,
                            this.ItemPrivileges.ToString(),
                            this.Node,
                            this.USER_NAME,
                            this.Group,
                            this.Size.ToString(),
                            dateTimeModified,
                            this.Name
                            );
        }

        private String getMonth(Int32 _month)
        {
            switch (_month)
            {
                case 1:
                    return "Jan";

                case 2:
                    return "Feb";

                case 3:
                    return "Mar";

                case 4:
                    return "Apr";

                case 5:
                    return "May";

                case 6:
                    return "Jun";

                case 7:
                    return "Jul";

                case 8:
                    return "Aug";

                case 9:
                    return "Sep";

                case 10:
                    return "Oct";

                case 11:
                    return "Nov";

                case 12:
                    return "Dec";

                default:
                    return String.Empty;
            }
        }

        private String addHeadingZero(Int32 _timedate)
        {
            if (_timedate-10 < 0)
                return String.Format("0{0}", _timedate);

            return _timedate.ToString();
        }



        private enum Attribute
        {
            ItemPrivileges          = 0,
            Node                    = 1,
            USER_NAME                    = 2,
            Group                   = 3,
            Size                    = 4,
            DateModified_Month      = 5,
            DateModified_Day        = 6,
            DateModified_TimeOrYear = 7,
            Name                    = 8,
        }

        #endregion Methods

    }
}
