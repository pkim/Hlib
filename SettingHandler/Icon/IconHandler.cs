/*
 * Filename: IconManager.cs
 * Author: Lukas Bernreiter, Patrik Kimmeswenger
 * Last change: 21.11.2011
 * 
 * Description: 
 * 
 * This class returns the icon of an application based on the fileextension
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace SettingsHandler.Icon
{
    public enum IconSize : int
    {
        Large = 0x000000000,
        Small = 0x000000001
    }

    public enum FolderType
    {
        Closed,
        Open
    }

    public static class IconManager
    {


        #region Properties

        public static System.Drawing.Icon FolderOpenSmall
        { get { return IconManager.GetFolderIcon(IconSize.Small, FolderType.Open); } }

        public static System.Drawing.Icon FolderOpenLarge
        { get { return IconManager.GetFolderIcon(IconSize.Large, FolderType.Open); } }

        public static System.Drawing.Icon FolderClosedSmall
        { get { return IconManager.GetFolderIcon(IconSize.Small, FolderType.Closed); } }

        public static System.Drawing.Icon FolderClosedLarge
        { get { return IconManager.GetFolderIcon(IconSize.Large, FolderType.Closed); } }

        public static System.Drawing.Icon UnknownFileTypeSmall
        { get { return IconManager.GetUnkownFileTypeIcon(IconSize.Small); } }

        public static System.Drawing.Icon UnknownFileTypeLarge
        { get { return IconManager.GetUnkownFileTypeIcon(IconSize.Large); } }

        #endregion Properties


        public static ImageSource ToImageSource(System.Drawing.Icon _icon)
        {
            return ShellIcon.toImageSource(_icon);
        }

        /// <summary>
        /// Generates the ImageSource which is used for folders
        /// </summary>
        /// <param name="_iconSize">The size of the icon</param>
        /// <param name="_folderType">The type of the folder icon</param>
        /// <returns>Retunrs the ImageSource of the folder icon</returns>
        private static ImageSource GetFolderImageSource(IconSize _iconSize, FolderType _folderType)
        {
            System.Drawing.Icon icon = IconManager.GetFolderIcon(_iconSize, _folderType);
            
            return ShellIcon.toImageSource(icon);
        }

        /// <summary>
        /// Generates the Icon which is used for folders
        /// </summary>
        /// <param name="_iconSize"></param>
        /// <param name="_folderType"></param>
        /// <returns></returns>
        private static System.Drawing.Icon GetFolderIcon(IconSize _iconSize, FolderType _folderType)
        {
            // Need to add size check, although errors generated at present!    
            API.SHGFI flags = API.SHGFI.ICON | API.SHGFI.USEFILEATTRIBUTES;

            if (FolderType.Open == _folderType)
            {
                flags |= API.SHGFI.OPENICON;
            }

            switch (_iconSize)
            {
                case IconSize.Small:

                    flags |= API.SHGFI.SMALLICON;
                    break;

                case IconSize.Large:

                    flags |= API.SHGFI.LARGEICON;
                    break;
            }

            // Get the folder icon    
            var shfi = new API.SHFILEINFO();

            var res = API.SHGetFileInfo(@"C:\Windows",
                API.FileAttributes.ATTRIBUTE_DIRECTORY,
                ref shfi,
                (uint)Marshal.SizeOf(shfi),
                flags);

            if (res == IntPtr.Zero)
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());

            // Load the icon from an HICON handle  
            System.Drawing.Icon.FromHandle(shfi.hIcon);

            // Now clone the icon, so that it can be successfully used
            var icon = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();

            // Cleanup
            API.DestroyIcon(shfi.hIcon);      

            return icon;
        }


        public static ImageSource GetFileImageSourceFromExtension(String _extension, IconSize _size)
        {
            System.Drawing.Icon icon = IconManager.GetFileIconFromExtension(_extension, _size);

            return ShellIcon.toImageSource(icon);
        }

        public static System.Drawing.Icon GetUnkownFileTypeIcon(IconSize _iconSize)
        {

            String iconLocation = String.Empty;
            System.Drawing.Icon icon = null;


            //opens the registry for the wanted key.
            RegistryKey registryKeyRoot        = Registry.ClassesRoot;
            RegistryKey registryKeyUnknown     = registryKeyRoot.OpenSubKey("Unknown");
            RegistryKey registryKeyDefaultIcon = registryKeyUnknown.OpenSubKey("DefaultIcon");

            if (registryKeyDefaultIcon != null)
            {
                //Get the file contains the icon and the index of the icon in that file.
                Object value = registryKeyDefaultIcon.GetValue("");

                if (value != null)
                {
                    // Clear all unnecessary " sign in the string to avoid error.
                    iconLocation = value.ToString().Replace("\"", "");
                }

                registryKeyDefaultIcon.Close();
            }

            registryKeyUnknown.Close();
            registryKeyRoot.Close();

            String[] iconPath = iconLocation.Split(',');


            IntPtr[] large = null;
            IntPtr[] small = null;

            int iIconPathNumber = 0;

            if (iconPath.Length > 1)
                iIconPathNumber = 1;
            else
                iIconPathNumber = 0;


            if (iconPath[iIconPathNumber] == null)
                iconPath[iIconPathNumber] = "0";

            large = new IntPtr[1];
            small = new IntPtr[1];

            //extracts the icon from the file.
            if (iIconPathNumber > 0)
            {
                API.ExtractIconEx(iconPath[0], Convert.ToInt16(iconPath[iIconPathNumber]), large, small, 1);
            }
            else
            {
                API.ExtractIconEx(iconPath[0], Convert.ToInt16(0), large, small, 1);
            }

            try
            {
                switch (_iconSize)
                {
                    case IconSize.Small:
                        icon = System.Drawing.Icon.FromHandle(small[0]);
                        break;

                    case IconSize.Large:
                        icon = System.Drawing.Icon.FromHandle(large[0]);
                        break;

                }
            }
            catch (Exception)
            {
                return null;
            }

            return icon;

            return icon;
        }

        public static System.Drawing.Icon GetFileIconFromExtension(String _extension, IconSize _size)
        {
            String iconLocation = String.Empty;

            // Add the '.' to the extension if needed
            if (_extension[0] != '.')
                _extension = String.Format(".{0}", _extension);
            

            //opens the registry for the wanted key.
            RegistryKey registryKeyRoot = Registry.ClassesRoot;
            RegistryKey registryKeyFileType = registryKeyRoot.OpenSubKey(_extension);

            if (registryKeyFileType == null)
                return IconManager.GetUnkownFileTypeIcon(_size);

            //Gets the default value of this key that contains the information of file type.
            Object defaultValue = registryKeyFileType.GetValue("");

            if (defaultValue == null)
                return IconManager.GetUnkownFileTypeIcon(_size);

            //Go to the key that specifies the default icon associates with this file type.
            String defaultIcon = String.Format("{0}\\DefaultIcon", defaultValue.ToString());

            RegistryKey registryKeyFileIcon = registryKeyRoot.OpenSubKey(defaultIcon);

            if (registryKeyFileIcon != null)
            {
                //Get the file contains the icon and the index of the icon in that file.
                Object value = registryKeyFileIcon.GetValue("");

                if (value != null)
                {
                    // Clear all unnecessary " sign in the string to avoid error.
                    iconLocation = value.ToString().Replace("\"", "");
                }

                registryKeyFileIcon.Close();
            }

            else
            {
                return IconManager.GetUnkownFileTypeIcon(_size);
            }

            registryKeyFileType.Close();
            registryKeyRoot.Close();

            String[] iconPath = iconLocation.Split(',');


            IntPtr[] large = null;
            IntPtr[] small = null;

            int iIconPathNumber = 0;

            if (iconPath.Length > 1)
                iIconPathNumber = 1;
            else
                iIconPathNumber = 0;


            if (iconPath[iIconPathNumber] == null) 
                iconPath[iIconPathNumber] = "0";
            
            large = new IntPtr[1];
            small = new IntPtr[1];

            //extracts the icon from the file.
            if (iIconPathNumber > 0)
            {
                API.ExtractIconEx(iconPath[0], Convert.ToInt16(iconPath[iIconPathNumber]), large, small, 1);
            }
            else
            {
                API.ExtractIconEx(iconPath[0], Convert.ToInt16(0), large, small, 1);
            }

            System.Drawing.Icon icon = null;

            try
            {
                switch (_size)
                {
                    case IconSize.Small:
                        icon = System.Drawing.Icon.FromHandle(small[0]);
                        break;

                    case IconSize.Large:
                        icon = System.Drawing.Icon.FromHandle(large[0]);
                        break;

                }
            }
            catch (Exception)
            {
                return IconManager.GetUnkownFileTypeIcon(_size);
            }

            return icon;
        }


    }
}
