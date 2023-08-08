using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using CAT_Library.SQL_functions;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace CAT_Library.ft_Selenium
{
    /// <summary>
    /// Library for open and close Firefox.
    /// </summary>
    public static class Firefox_connection
    {
        const string CATcon = @"Data Source=EU1RAPW313;Initial Catalog=CATDepartment;Persist Security Info=True;User ID=web_sql;Password=Ottobre1985";

        /// <summary>
        /// Function to open Firefox.
        /// Check the path of firefox.exe.
        /// Set arguments of firefox.
        /// </summary>
        /// <param name="userName">Inform the user name.</param>
        public static IWebDriver ConfigFirfeox(string userName)
        {
            var FirefoxDriver = FirefoxDriverService.CreateDefaultService();
            FirefoxDriver.HideCommandPromptWindow = true;
            FirefoxOptions options = new FirefoxOptions();
            string path = @"C:\Users\" + userName + @"\AppData\Local\Mozilla Firefox\firefox.exe";
            bool exist = System.IO.File.Exists(path);
            if (!exist)
            {
                path = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            }
            options.BrowserExecutableLocation = path;
            options.AddArgument("--disable-infobars");
            //options.AddArguments("--headless");
            options.AddArguments("no-sandbox");
            return new FirefoxDriver(FirefoxDriver, options);
        }

        /// <summary>
        /// Function to open 'Lease' in the assigned Opco and in the type of production on OPCO side.
        /// </summary>
        /// <param name="x">Row of the DataGrid</param>
        /// <param name="type">Production or Not Production</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns></returns>
        public static (int, string) OpenLeaseWebOpco (DataGridViewRow x, string type, IWebDriver driver)
        {
            string opco = x.Cells["OPCO"].Value.ToString();
            SqlConnection con = new SqlConnection(CATcon);
            SqlCommand cmd = new SqlCommand
            {
                Connection = SQL_connection.Opencon(con),
                CommandText = "SearchOpCoURL",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Opco", opco);
            cmd.Parameters.AddWithValue("@Type", type);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string opcoUrl = reader["URL"] as string;
                driver.Navigate().GoToUrl(opcoUrl);
                reader.Close();
                cmd.Dispose();
                con.Dispose();
                SQL_connection.Closecon(con);
                return (0, opcoUrl);
            }
            else
            {
                return (1, "");
            }
        }

        /// <summary>
        /// Function to open 'Lease' in the assigned Opco and in the type of production on RCAP side.
        /// </summary>
        /// <param name="x">Row of the DataGrid</param>
        /// <param name="type">Production or Not Production</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns></returns>
        public static int OpenLeaseWebRcap(DataGridViewRow x, string type, IWebDriver driver)
        {
            string opco = x.Cells["OPCO"].Value.ToString();
            SqlConnection con = new SqlConnection(CATcon);
            SqlCommand cmd = new SqlCommand
            {
                Connection = SQL_connection.Opencon(con),
                CommandText = "SearchRcapURL",
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Opco", opco);
            cmd.Parameters.AddWithValue("@Type", type);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string opcoUrl = reader["URL"] as string;
                driver.Navigate().GoToUrl(opcoUrl);
                reader.Close();
                cmd.Dispose();
                con.Dispose();
                SQL_connection.Closecon(con);
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Process GetGeckoDriverProcess()
        {
            Process[] processes = Process.GetProcessesByName("geckodriver");
            if (processes.Length > 0)
            {
                return processes[0];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Process GetConsoleWindowsHost()
        {
            Process[] processes = Process.GetProcessesByName("Console Windows Host");
            if (processes.Length > 0)
            {
                return processes[0];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oracleUrl"></param>
        /// <returns></returns>
        public static async Task KeepSessionAliveAsync(string oracleUrl, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Realizar una petición sencilla al servidor de Oracle
                    using (var httpClient = new HttpClient())
                    {
                        // Hacer una petición GET a una página cualquiera de Oracle
                        var response = await httpClient.GetAsync(oracleUrl, cancellationToken);
                        response.EnsureSuccessStatusCode();
                    }
                    // Esperar antes de realizar la próxima petición (por ejemplo, cada 10 minutos)
                    await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Capturar la excepción que ocurre cuando se solicita la cancelación
                    // Puedes hacer algo aquí si lo deseas, o simplemente salir del bucle.
                    break;
                }
                catch (Exception ex)
                {
                    // Manejar cualquier error que pueda ocurrir durante la petición
                    Console.WriteLine($"Error al mantener la sesión activa: {ex.Message}");
                }
            }
        }


    }
}
