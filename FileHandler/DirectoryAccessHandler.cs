using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Security.AccessControl;
using System.Diagnostics;

namespace HLib.FileHandler
{
    internal class CheckDirectory
    {
        #region generics

        [StructLayout(LayoutKind.Sequential)]
        internal struct GENERIC_MAPPING
        {
            internal uint GenericRead;
            internal uint GenericWrite;
            internal uint GenericExecute;
            internal uint GenericAll;
        }

        [DllImport("advapi32.dll", SetLastError = false)]
        static extern void MapGenericMask([In, MarshalAs(UnmanagedType.U4)] ref TokenAccessLevels AccessMask,
            [In] ref GENERIC_MAPPING map);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]


        public static extern Boolean DuplicateToken(IntPtr ExistingTokenHandle,
                [MarshalAs(UnmanagedType.U4)] TokenImpersonationLevel level,
                out Int32 DuplicateTokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern Boolean AccessCheck(
            [MarshalAs(UnmanagedType.LPArray)]
        byte[] pSecurityDescriptor,
                IntPtr ClientToken,

            [MarshalAs(UnmanagedType.U4)]
        TokenAccessLevels accessmask,
              [In] ref GENERIC_MAPPING GenericMapping,
              IntPtr PrivilegeSet,
              ref Int32 PrivilegeSetLength,
              out uint GrantedAccess,
              [MarshalAs(UnmanagedType.Bool)]
      out Boolean AccessStatus);

        [DllImport("kernel32")]
        static extern void CloseHandle(IntPtr ptr);

        #endregion

        internal static Boolean hasReadAccess(String path)
        {
            // Obtain the authenticated user's Identity
            
            
            WindowsIdentity winId = WindowsIdentity.GetCurrent(TokenAccessLevels.Duplicate | TokenAccessLevels.Query);

            WindowsImpersonationContext ctx = null;
            Int32 statError = new Int32();

            IntPtr dupToken = IntPtr.Zero;
            try
            {
                // Start impersonating
                //ctx = winId.Impersonate(); works but AccessCheck does not like this

                Int32 outPtr;
                //AccessCheck needs a duplicated token!
                DuplicateToken(winId.Token, TokenImpersonationLevel.Impersonation, out outPtr);

                dupToken = new IntPtr(outPtr);
                ctx = WindowsIdentity.Impersonate(dupToken);
                GENERIC_MAPPING map = new GENERIC_MAPPING();
                map.GenericRead = 0x80000000;
                map.GenericWrite = 0x40000000;
                map.GenericExecute = 0x20000000;
                map.GenericAll = 0x10000000;
                TokenAccessLevels required = TokenAccessLevels.Query | TokenAccessLevels.Read | TokenAccessLevels.AssignPrimary | (TokenAccessLevels)0x00100000; // add synchronization
                MapGenericMask(ref required, ref map);

                uint status = new Int32();
                Boolean accesStatus = false;
                // dummy area the size should be 20 we don't do anything with it
                Int32 sizeps = 20;
                IntPtr ps = Marshal.AllocCoTaskMem(sizeps);

                //AccessControlSections.Owner | AccessControlSections.Group MUST be included,
                //otherwise the descriptor would be seen with ERROR 1338
                var ACE = Directory.GetAccessControl(path,
                    AccessControlSections.Access | AccessControlSections.Owner |
                        AccessControlSections.Group);

                Boolean success = AccessCheck(ACE.GetSecurityDescriptorBinaryForm(), dupToken, required, ref map,
                        ps, ref sizeps, out status, out accesStatus);
                Marshal.FreeCoTaskMem(ps);
                if (!success)
                {
                    statError = Marshal.GetLastWin32Error();
                }
                else
                {
                    return accesStatus;
                }
            }
            // Prevent exceptions from propagating
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }

            finally
            {
                // Revert impersonation

                if (ctx != null)
                    ctx.Undo();
                CloseHandle(dupToken);
            }

            if (statError != 0)
            {
                throw new System.ComponentModel.Win32Exception(statError);
            }

            return false;
        }
    }
}
