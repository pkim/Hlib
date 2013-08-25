using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Handler.GUIHandler.Cursor
{

    public class CursorEditor
    {

        #region Struct

        private struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        #endregion


        #region DLLImport

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        #endregion


        #region Methods

        private static System.Windows.Forms.Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new System.Windows.Forms.Cursor(CreateIconIndirect(ref tmp));
        }

        public static void changeCursorIcon(Form frame, String cursorImageSource, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(Image.FromFile(cursorImageSource), new Rectangle(0, 0, width, height));

            frame.Cursor = CreateCursor(bitmap, 0, 0);

            bitmap.Dispose();
        }

        #endregion


    }
}
