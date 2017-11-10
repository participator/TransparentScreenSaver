using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class Transparent : Form
    {
        #region WinAPI

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        #region Fields and Properties

        private bool IsPreviewMode = false;

        #endregion
        public Transparent()
        {
            InitializeComponent();
        }

        public Transparent(Rectangle Bounds)
        {
            InitializeComponent();
            SetEvents();
            this.Bounds = Bounds;
            Cursor.Hide();
        }

        /// <summary>
        /// Used when preview is called
        /// </summary>
        /// <param name="PreviewHandle"></param>
        public Transparent(IntPtr PreviewWndHandle)
        {
            InitializeComponent();
            SetEvents();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            IsPreviewMode = true;
        }

        public void SetEvents()
        {
            this.Click += new EventHandler(Transparent_Click);
            this.KeyDown += new KeyEventHandler(Transparent_KeyDown);
            this.MouseMove += new MouseEventHandler(Transparent_MouseMove);
        }

        #region User Event Handlers

        private void Transparent_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        private void Transparent_Click(object sender, EventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                Application.Exit();
            }
        }

        //start off OriginalLoction with an X and Y of int.MaxValue, because
        //it is impossible for the cursor to be at that position. That way, we
        //know if this variable has been set yet.
        Point OriginalLocation = new Point(int.MaxValue, int.MaxValue);

        private void Transparent_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsPreviewMode) //disable exit functions for preview
            {
                //see if originallocation has been set
                if (OriginalLocation.X == int.MaxValue &
                    OriginalLocation.Y == int.MaxValue)
                {
                    OriginalLocation = e.Location;
                }
                //see if the mouse has moved more than 20 pixels 
                //in any direction. If it has, close the application.
                if (Math.Abs(e.X - OriginalLocation.X) > 20 |
                    Math.Abs(e.Y - OriginalLocation.Y) > 20)
                {
                    Application.Exit();
                }
            }
        }
        #endregion
    }
}
