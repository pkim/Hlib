/*
 * Filename: ItemPrivileges.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 08.01.2012
 * Description: Represents the Privileges of a FTPItem if included
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handler.Interface.HLib.Network.IP.FTP
{
    public class ItemPrivileges
    {
        //public SpecialPrivilege SpecialPrivilege { get; set; }
        
        public ItemPrivilege USER_NAME   { get; set; }

        public ItemPrivilege Group  { get; set; }

        public ItemPrivilege Others { get; set; }


        public override String ToString()
        {
            return String.Format("{0}{1}{2}",
                                    this.USER_NAME.ToString(),
                                    this.Group.ToString(),
                                    this.Others.ToString()
                                    );
        }

        public static ItemPrivileges GetItemPrivileges(String _privileges)
        {
            ItemPrivileges itemPrivileges = new ItemPrivileges();

            itemPrivileges.USER_NAME   = ItemPrivilege.GetItemPrivilege(_privileges.Substring(0, 3));
            itemPrivileges.Group  = ItemPrivilege.GetItemPrivilege(_privileges.Substring(3, 3));
            itemPrivileges.Others = ItemPrivilege.GetItemPrivilege(_privileges.Substring(6, 3));

            return itemPrivileges;
        }
    }
}
