﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace HLib.File.Environment
{
    public class EnvironmentHandler
    {

        #region Objects

        #endregion Objects


        #region Properties


        /// <summary>
        /// Returns the location of the All Users Profile.
        /// </summary>
        public static String ALLUSERSPROFILE
        {
            get { return System.Environment.GetEnvironmentVariable("ALLUSERSPROFILE"); }
        }

        /// <summary>
        /// Returns the location where applications store data by default.
        /// </summary>
        public static String APPDATA
        {
            get { return System.Environment.GetEnvironmentVariable("APPDATA"); }
        }

        /// <summary>
        /// Returns the current directory String.
        /// </summary>
        public static String CD
        {
            get { return System.Environment.GetEnvironmentVariable("CD"); }
        }

        /// <summary>
        /// Returns the exact command line used to start the current Cmd.exe.
        /// </summary>
        public static String CMDCMDLINE
        {
            get { return System.Environment.GetEnvironmentVariable("CMDCMDLINE"); }
        }

        /// <summary>
        /// Returns the version number of the current Command Processor Extensions.
        /// </summary>
        public static String CMDEXTVERSION
        {
            get { return System.Environment.GetEnvironmentVariable("CMDEXTVERSION"); }
        }

        /// <summary>
        /// Returns the name of the computer.
        /// </summary>
        public static String COMPUTERNAME
        {
            get { return System.Environment.GetEnvironmentVariable("COMPUTERNAME"); }
        }

        /// <summary>
        /// Returns the exact path to the command shell executable.
        /// </summary>
        public static String COMSPEC
        {
            get { return System.Environment.GetEnvironmentVariable("COMSPEC"); }
        }

        /// <summary>
        /// Returns the current date. Uses the same format as the date /t command. Generated by Cmd.exe.
        /// </summary>
        public static String DATE
        {
            get { return System.Environment.GetEnvironmentVariable("DATE"); }
        }

        /// <summary>
        /// Returns the error code of the most recently used command. A non zero value usually indicates an error.
        /// </summary>
        public static String ERRORLEVEL
        {
            get { return System.Environment.GetEnvironmentVariable("ERRORLEVEL"); }
        }

        /// <summary>
        /// Returns which local workstation drive letter is connected to the user's home directory. Set based on the value of the home directory. The user's home directory is specified in Local Users and Groups.
        /// </summary>
        public static String HOMEDRIVE
        {
            get { return System.Environment.GetEnvironmentVariable("HOMEDRIVE"); }
        }

        #endregion Properties

    }
}
