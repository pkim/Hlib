/*
 * Filename: ItemPrivilege
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 19.01.2012
 * Description: Defines properties and methods to handle privileges. 
 * For example: rwx
 * 
 * This privilege is given for user, group, others: rwxrwxrwx
 * The privilges of ugo are stored in ItemPrivileges
 */

// System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handler.Interface.HLib.Network.IP.FTP
{
    public class ItemPrivilege
    {

        #region Objects

        public const Char ReadPermission    = 'r';
        public const Char WritePermission   = 'w';
        public const Char ExecutePermission = 'x';
        public const Char NoPermission      = '-';

        #region Properties

        public Boolean Read { get; set; }

        public Boolean Write { get; set; }

        public Boolean Execute { get; set; }

        #endregion Properties

        #region Enums

        public enum PrivilegeAttribute
        {
            Read = 0,
            Write = 1,
            Execute = 2,
        }

        #endregion Enums

        #endregion Objects

        #region Methods

        public override String ToString()
        {

            Char read    = NoPermission;
            Char write   = NoPermission;
            Char execute = NoPermission;


            if (this.Read)
                read = ReadPermission;

            if (this.Write)
                write = WritePermission;

            if (this.Execute)
                execute = ExecutePermission;


            return String.Format("{0}{1}{2}",
                                read,
                                write,
                                execute
                                );
        }

        public static ItemPrivilege GetItemPrivilege(String _privilege)
        {
            ItemPrivilege itemPrivilege = new ItemPrivilege();

            // Read Permission
            if (_privilege[(Int32)PrivilegeAttribute.Read] == ReadPermission)
                itemPrivilege.Read = true;
            else
                itemPrivilege.Read = false;

            // Write Permission
            if (_privilege[(Int32)PrivilegeAttribute.Write] == WritePermission)
                itemPrivilege.Write = true;
            else
                itemPrivilege.Write = false;

            // Execute Permission
            if (_privilege[(Int32)PrivilegeAttribute.Execute] == ReadPermission)
                itemPrivilege.Execute = true;
            else
                itemPrivilege.Execute = false;

            return itemPrivilege;
        }

        #endregion Methods
       
    }
}
