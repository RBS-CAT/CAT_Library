using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAT_Library
{
    /// <summary>
    /// 
    /// </summary>
    public static class Settings
    {
        private static Point dragCursorPoint;
        private static Point dragFormPoint;
        static Form form;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dragging"></param>
        /// <param name="e"></param>
        /// <param name="form"></param>
        public static void DraggingPoint(bool dragging, MouseEventArgs e, Form form )
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = form.Location;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dragging"></param>
        /// <param name="form"></param>
        public static void DraggingMouse(bool dragging, Form form)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                form.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void NewSizePosition(Control control, int width, int height, int x, int y)
        {
            control.Size = new Size(width, height);
            control.Location = new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string ReadEncryptedFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                string encryptedFilePath = Path.Combine(fullPath);
                byte[] encryptedData = File.ReadAllBytes(encryptedFilePath);
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    encryptedData, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return (Encoding.Unicode.GetString(decryptedData));
            }
            return (null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formchild"></param>
        /// <param name="panelMain"></param>
        public static void OpenChildForm(object formchild, Panel panelMain)
        {
            if (panelMain.Controls.Count > 0)
            {
                panelMain.Controls.RemoveAt(0);
            }
            form = formchild as Form;
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelMain.Controls.Add(form);
            panelMain.Tag = form;
            form.Show();
        }

        /// <summary>
        /// Function to stop down the code for a certain time.
        /// </summary>
        /// <param name="time">'TimeSpan.' and select the amount of time.</param>
        public static void Waiting(TimeSpan time)
        {
            Thread.Sleep(time);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void fileLogMenu(string path)
        {
            if (File.Exists(path))
            {
                Cursor.Current = Cursors.WaitCursor;
                Process.Start(path);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("The file does not exist in the folder.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void excelLogMenu(string path)
        {
            if (File.Exists(path))
            {
                Cursor.Current = Cursors.WaitCursor;
                Process.Start(path);
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("The Excel file does not exist in the folder.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void folderLogMenu(string fileName)
        {
            if (Directory.Exists(fileName))
            {
                Cursor.Current = Cursors.WaitCursor;
                Process.Start("explorer.exe", "\"" + fileName + "\"");
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("The folder you are trying to open does not exist.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExtractText(string text)
        {
            Match match = Regex.Match(text, "\"(.*?)\"");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return text;
        }
    }
}
