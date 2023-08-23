using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using CAT_Library.SQL_functions;
using CAT_Library.ft_Excel;

namespace CAT_Library.ft_Excel
{
    /// <summary>Library to make changes or move within an Excel file.</summary>
    public static class CatExcel 
    {
        /// <summary>
        /// Check if the connection is opened, if not the connection will be opened.
        /// </summary>
        /// <param name="con">A unique connection to a data source.</param>
        /// <returns>Connection status.</returns>
        public static OleDbConnection Opencon(OleDbConnection con)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            return con;
        }

        /// <summary>
        /// Check if the connection is closed, if not the connection will be closed.
        /// </summary>
        /// <param name="con">A unique connection to a data source.</param>
        /// <returns>Connection status.</returns>
        public static OleDbConnection Closecon(OleDbConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            return con;
        }

        /// <summary>
        /// Allows load all data from Excel to the DataGridView.
        /// </summary>
        /// <param name="dataGrid">Name of the DataGridView.</param>
        /// <param name="path">Full path where find the Excel file.</param>
        /// <param name="sheet">Name of the Excel tab to load.</param>
        public static void Load(DataGridView dataGrid, string path, string sheet)
        {
            OleDbConnection excelCon = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR=YES;'");
            Opencon(excelCon);
            string query = "SELECT * FROM [" + sheet + "$]";
            OleDbDataAdapter data = new OleDbDataAdapter(query, excelCon);
            DataTable table = new DataTable();
            data.Fill(table);
            DataGridView dataGridView = new DataGridView();
            dataGrid.DataSource = table;
            Closecon(excelCon);
        }
        
        /// <summary>
        /// Set the width and the alignment of the column.
        /// </summary>
        /// <param name="ws">WorkSheet variable.</param>
        /// <param name="row">Row position (num).</param>
        /// <param name="column">Column position (num).</param>
        /// <param name="title">Tittle of the column.</param>
        /// <param name="vAlign">Vertical Alignment.</param>
        /// <param name="hAlign">Horizontal Alignment.</param>
        /// <param name="width">Column dimension (num).</param>
        public static void SetColumn(Excel.Worksheet ws, int row, int column, string title, Excel.XlVAlign vAlign, Excel.XlHAlign hAlign, int width)
        {
            LetterColumns lt = new LetterColumns();
            ws.Cells[row, column].Value = title;
            string letter = lt.InLetter(column.ToString());
            ws.Columns[letter].ColumnWidth = width;
            ws.Columns[letter].VerticalAlignment = vAlign;
            ws.Columns[letter].HorizontalAlignment = hAlign;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        /// <param name="excelFile"></param>
        public static void ExportToExcel(DataGridView dgv, string dirPath, string fileName, string excelFile)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                Excel.Workbook wb = excelApp.Workbooks.Open(excelFile);
                Excel.Worksheet ws = wb.Sheets[1];

                ws.Range["A1"].Select();
                int colCount = dgv.Columns.Count;
                int rowCount = dgv.Rows.Count;
                // Exportar los encabezados de las columnas
                for (int i = 1; i <= dgv.Columns.Count; i++)
                {
                    ws.Cells[1, i] = dgv.Columns[i - 1].HeaderText;
                }

                // Exportar los datos de las celdas
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        // Verificar si el valor de la celda es null y proporcionar un valor predeterminado ("") en su lugar
                        string cellValue = dgv.Rows[i].Cells[j].Value?.ToString() ?? "";
                        ws.Cells[i + 2, j + 1] = cellValue;
                    }
                }

                ws.Columns.AutoFit();
                ws.Range["A1"].EntireRow.Font.Bold = true;
                ws.Range["A1"].EntireRow.Font.Underline = true;
                var range = ws.Range[ws.Cells[1, 1], ws.Cells[rowCount + 1, colCount]];
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Guardar el archivo de Excel
                string excelName = dirPath + fileName;
                wb.SaveAs(excelName);

                // Cerrar Excel
                wb.Close();
                excelApp.Quit();

                // Liberar recursos COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Llamar al recolector de basura para liberar memoria
                GC.Collect(); 
            }
            catch (Exception ex)
            {
                throw new Exception("The Excel file could not be saved properly.");
            }
        }
    }
}
