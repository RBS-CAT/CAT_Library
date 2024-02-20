using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Diagnostics;
using System.Threading;

namespace CAT_Library.ft_Selenium
{
    /// <summary>
    /// Functions that are done within the 'Lease' page.
    /// </summary>
    public static class Ft_Firefox
    {
        /// <summary>
        /// Function to wait for a web element to be available and action can be taken.
        /// </summary>
        /// <param name="elementLocator">Element to be searched for.</param>
        /// <param name="timeout">'TimeSpan.' and select the amount of time.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns></returns>
        public static bool WaitUntilElementClickable(By elementLocator, TimeSpan timeout, IWebDriver driver)
        {
            try
            {
                //IWebElement findElement = driver.FindElement(elementLocator);
                //IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                //je.ExecuteScript("arguments[0].scrollIntoView(true);", findElement);
                var wait = new WebDriverWait(driver, timeout);
                wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        /// <summary>
        /// Function that will try to find an element on a web page using a locator.
        /// </summary>
        /// <param name="elementLocator">Element to be searched for.</param>
        /// <param name="element">Output parameter that will contain the element found if the function is successful.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Returns 'true' if the search is successful and the found element is assigned, or 'false' if there was an error in the code.</returns>
        public static bool TryFindElement(By elementLocator, out IWebElement element, IWebDriver driver)
        {
            try
            {
                element = driver.FindElement(elementLocator);
            }
            catch (NoSuchElementException)
            {
                element = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Click action.
        /// The code will search for the element on the active page where it needs to perform the click.
        /// </summary>
        /// <param name="elementLocator">Element to be searched for.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        public static void ClickAction(By elementLocator, IWebDriver driver)
        {
            int click = 0;
            while (click != 1)
            {
                try
                {
                    IWebElement btnClick = driver.FindElement(elementLocator);
                    IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                    je.ExecuteScript("arguments[0].scrollIntoView(true);", btnClick);
                    btnClick.Click();
                    click = 1;
                }
                catch
                {
                    Settings.Waiting(TimeSpan.FromSeconds(10));
                    continue;
                }
            }
        }

        /// <summary>
        /// Text input action.
        /// The code will search for the element on the active page where it needs to enter the assigned text.
        /// </summary>
        /// <param name="elementLocator">Element to be searched for.</param>
        /// <param name="value">Text to be written.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        public static void InputText(By elementLocator, string value, IWebDriver driver)
        {
            int check = 0;
            while (check != 1)
            {
                try
                {
                    IWebElement inputText = driver.FindElement(elementLocator);
                    IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                    je.ExecuteScript("arguments[0].scrollIntoView(true);", inputText);
                    inputText.Clear();
                    inputText.SendKeys(value);
                    check = 1;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Function that will wait for a specific text at the indicated location.
        /// </summary>
        /// <param name="elementLocator">Element to be searched for.</param>
        /// <param name="value">Text to search for and wait until found.</param>
        /// <param name="time">Wait time.<br>
        /// It can be in milliseconds, seconds, minutes, hours or days.</br></param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Returns True if the specified text has been found. False if there has been an error.</returns>
        public static bool WaitingText(By elementLocator, string value, TimeSpan time, IWebDriver driver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, time);
                string status = driver.FindElement(elementLocator).Text;
                wait.Until(ExpectedConditions.TextToBePresentInElementLocated((elementLocator), value));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Function that will wait for two specific texts at the specified location.
        /// </summary>
        /// <param name="elementLocator">Element to be searched for.</param>
        /// <param name="value1">Text to search for and wait until found.</param>
        /// <param name="value2">Text to search for and wait until found.</param>
        /// <param name="time">Wait time.<br> 
        /// It can be in milliseconds, seconds, minutes, hours or days.</br></param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns></returns>
        public static bool WaitingTextBetween(By elementLocator, string value1, string value2, TimeSpan time, IWebDriver driver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, time);
                var element = driver.FindElement(elementLocator);
                wait.Until(d => {
                    string currentText = element.Text;
                    return currentText.CompareTo(value1) >= 0 && currentText.CompareTo(value2) <= 0;
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Function to log in to the Lease website.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Returns 0 if the function has been successful, 1 if there has been an error in the code and 2 if there has been an error when logging into the Lease website.</returns>
        public static int LoginLease(string password, IWebDriver driver)
        {
            try
            {
                string InputContract = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[3]/td[2]/table/tbody/tr[2]/td[2]/input";
                string Origination = "/html/body/table[3]/tbody/tr[1]/td[2]/table/tbody/tr[2]/td[1]/a/span";
                string ErrorAutentication = "/html/body/center/form/table/tbody/tr/td/blockquote/blockquote/table[1]/tbody/tr/td[1]/font/nobr";
                string btnOk = "/html/body/center/form/center[2]/table/tbody/tr[2]/td/table/tbody/tr/td[1]/input";

                //UserName
                WaitUntilElementClickable(By.XPath(btnOk), TimeSpan.FromMinutes(3), driver);
                IWebElement inputName = driver.FindElement(By.Name("ssousername"));
                inputName.Clear();
                inputName.SendKeys(Environment.UserName.ToString().ToLower());
                
                Thread.Sleep(1000);
                //UserPasword
                IWebElement inputPassword = driver.FindElement(By.Name("password"));
                inputPassword.Clear();
                inputPassword.SendKeys(password);
                Thread.Sleep(1000);
                //ClickOk
                ClickAction(By.XPath("/html/body/center/form/center[2]/table/tbody/tr[2]/td/table/tbody/tr/td[1]/input"), driver);
                //IWebElement clickOK = driver.FindElement(By.XPath("/html/body/center/form/center[2]/table/tbody/tr[2]/td/table/tbody/tr/td[1]/input"));
                //clickOK.Click();
                if (TryFindElement(By.XPath(ErrorAutentication), out IWebElement element, driver))
                {
                    if (driver.FindElement(By.XPath(ErrorAutentication)).Text == "Error: ")
                    {
                        return 2;
                    }
                }
                else
                {
                    WaitUntilElementClickable(By.XPath(Origination), TimeSpan.FromMinutes(3), driver);
                    ClickAction(By.XPath(Origination), driver);
                    WaitUntilElementClickable(By.XPath(InputContract), TimeSpan.FromMinutes(3), driver);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        /// <summary>
        /// Function to add the contract number..
        /// </summary>
        /// <param name="contractNumber">Contract number.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Returns 0 if the function was successful and 1 if there was an error in the code.</returns>
        public static int InputContract(string contractNumber, IWebDriver driver)
        {
            try
            {
                string InputContract = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[3]/td[2]/table/tbody/tr[2]/td[2]/input";
                string Origination = "/html/body/table[3]/tbody/tr[1]/td[2]/table/tbody/tr[2]/td[1]/a/span";
                string inputContract = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[3]/td[2]/table/tbody/tr[2]/td[2]/input";
                string goButton = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[3]/td[2]/table/tbody/tr[3]/td[2]/a/img";
                string linkContract = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[6]/td[2]/table[2]/tbody/tr[2]/td[2]/a";

                ClickAction(By.XPath(Origination), driver);
                WaitUntilElementClickable(By.XPath(InputContract), TimeSpan.FromMinutes(3), driver);
                IWebElement contract = driver.FindElement(By.XPath(inputContract));
                contract.Clear();
                contract.SendKeys(contractNumber);
                ClickAction(By.XPath(goButton), driver);
                WaitUntilElementClickable(By.XPath(linkContract), TimeSpan.FromMinutes(3), driver);

                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
            
        }

        /// <summary>
        /// Function to check the status of the contract before to open the contract.
        /// </summary>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Will return 0 if the state of the contract is not activated, otherwise it will return 1.</returns>
        public static int StatusContract(IWebDriver driver)
        {
            string status = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[6]/td[2]/table[2]/tbody/tr[2]/td[4]";
            string linkContract = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[6]/td[2]/table[2]/tbody/tr[2]/td[2]/a";
            string btnAdditional = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table/tbody/tr[60]/td[2]/a[4]/img";

            try
            {
                string statusContract = driver.FindElement(By.XPath(status)).Text;
                if (driver.FindElement(By.XPath(status)).Text == "Booked")
                {
                    throw new Exception();
                }
                else
                {
                    ClickAction(By.XPath(linkContract), driver);
                    WaitUntilElementClickable(By.XPath(btnAdditional), TimeSpan.FromMinutes(3), driver);
                    return 0;
                }
                
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        /// Function to know the total assets of the contract.
        /// </summary>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Returns the total number of assets in the contract or 0 if there has been an error in the code.</returns>
        public static int NumOfAssets(IWebDriver driver)
        {
            string linkContractUpdate = "/html/body/table[4]/tbody/tr/td[1]/table/tbody/tr[17]/td[4]/a";
            string btnQuote = "/html/body/table[4]/tbody/tr/td[2]/table/tbody/tr/td[2]/form/table[1]/tbody/tr[2]/td[2]/a/img";
            string assetNumText = "/html/body/table[4]/tbody/tr/td[2]/table/tbody/tr/td[2]/form/table[3]/tbody/tr[2]/td[2]/b";

            try
            {
                ClickAction(By.XPath(linkContractUpdate), driver);
                Settings.Waiting(TimeSpan.FromSeconds(10));
                WaitUntilElementClickable(By.XPath(btnQuote), TimeSpan.FromMinutes(3), driver);
                return Convert.ToInt32(driver.FindElement(By.XPath(assetNumText)).Text);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Function to go to the Payment section and count the number of rows.
        /// </summary>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Returns the total number of lines or 0 if there was a bug in the code.</returns>
        public static int PaymentBlock(IWebDriver driver)
        {
            string linkPayment = "/html/body/table[4]/tbody/tr/td[1]/table/tbody/tr[9]/td[4]/a";
            string btnCreatePayment = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[6]/tbody/tr/td/a/img";
            string tableAssets = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[8]/tbody/tr/td/table[2]/tbody/tr";
            string btnUpdate = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[10]/tbody/tr/td/a[1]/img";
            int lines;

            try
            {
                ClickAction(By.XPath(linkPayment), driver);
                WaitUntilElementClickable(By.XPath(btnCreatePayment), TimeSpan.FromMinutes(3), driver);
                ClickAction(By.XPath(btnUpdate), driver);
                Settings.Waiting(TimeSpan.FromSeconds(5));
                lines = driver.FindElements(By.XPath(tableAssets)).Count;
                return lines;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Function to click in the 'Details' link on the 'Payments' part.
        /// </summary>
        /// <param name="line">Number of the row.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Return 0 when the function works properly or 1 if there was a bug in the code.</returns>
        public static int PaymentDetail(int line, IWebDriver driver)
        {
            try
            {
                string detailsLinks = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[8]/tbody/tr/td/table[2]/tbody/tr[" + line + "]/td[5]/a";
                string btnUpdate = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[3]/tbody/tr[19]/td/div/a[1]/img";
                WaitUntilElementClickable(By.XPath(detailsLinks), TimeSpan.FromMinutes(3), driver);
                ClickAction(By.XPath(detailsLinks), driver);
                WaitUntilElementClickable(By.XPath(btnUpdate), TimeSpan.FromMinutes(3), driver);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        /// Function to click the 'Update' button within the Payments details.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="count"></param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns>Return 0 if the function has executed correctly and found the message 'Successfully Updated'; otherwise, return 1.</returns>
        public static int ft_ButtonUpdate(string step, int count, IWebDriver driver)
        {
            string confirmationText = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[1]/tbody/tr[2]/td[2]/div/span";
            string confirmationProcess = "/html/body/table[4]/tbody/tr/td[2]/table[1]/tbody/tr[2]/td[2]/div/div/span[1]";
            string clickBtnUpdate = "/html/body/table[4]/tbody/tr/td[2]/form[2]/table[11]/tbody/tr/td/a[1]/img";
            string Updatebutton = "Update";

            try
            {
                switch (step)
                {
                    case "first":
                        Ft_Firefox.ClickAction(By.CssSelector($"img[title='{Updatebutton}']"), driver);
                        WaitingText(By.XPath(confirmationText), "Successfully Updated.", TimeSpan.FromMinutes(1), driver);
                        Settings.Waiting(TimeSpan.FromSeconds(1));
                        ft_ButtonUpdate("second", 0, driver);
                        break;
                    case "second":
                        try
                        {
                            Ft_Firefox.ClickAction(By.XPath(clickBtnUpdate), driver);
                            Settings.Waiting(TimeSpan.FromSeconds(1));
                            WaitingText(By.XPath(confirmationProcess), "Successfully Processed.", TimeSpan.FromMinutes(1), driver);
                        }
                        catch
                        {
                            if (count < 3)
                            {
                                Settings.Waiting(TimeSpan.FromSeconds(10));
                                ft_ButtonUpdate("second", count++, driver);
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        break;
                }
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string ft_yieldNumber(IWebDriver driver)
        {
            try
            {
                string implicitInteresRateNum = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[23]/td[3]";
                return driver.FindElement(By.XPath(implicitInteresRateNum)).Text;
            }
            catch (Exception)
            {
                throw new Exception("Function 'ft_yieldNumber', failed");
            }
        }

        /// <summary>
        /// Function for protected close for GeckoDriver
        /// </summary>
        /// <param name="isDriverStarted">Check if FirefoxDriver is openend</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        /// <returns></returns>
        public static int ft_GeckoDriverClose(bool isDriverStarted)
        {
            Process driverProcess = Firefox_connection.GetGeckoDriverProcess();
            if (isDriverStarted && driverProcess != null)
            {
                driverProcess.Kill();
                driverProcess.Dispose();
                return 0;
            }
            return 1;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDriverStarted"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static int closeFirefox(bool isDriverStarted, IWebDriver driver)
        {
            try
            {
                driver.Quit();
                ft_GeckoDriverClose(isDriverStarted);
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}
