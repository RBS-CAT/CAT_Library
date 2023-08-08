using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CAT_Library.ft_Selenium
{
    /// <summary>
    /// 
    /// </summary>
    public static class BookingContract
    {
        private static string statusContractText;

        /// <summary>
        /// Function to go to the activation section and set the contract status to 'Approved' or 'Booked'.
        /// </summary>
        /// <param name="totalAssets">The number of Assets the contract has.</param>
        /// <param name="checkStatus">The current state of the checkBox.</param>
        /// <param name="driver">Geckodriver browser driver.</param>
        public static string MainBooking(int totalAssets, bool checkStatus , IWebDriver driver)
        {
            string status = null; 
            try
            {
                var result = GoToBooking(driver);
                statusContractText = result.Item2;
                switch (result.Item1)
                {
                    case 0:
                        switch (totalAssets)
                        {
                            case var n when totalAssets < 11:
                                status = Booking(statusContractText, 0, checkStatus, driver);
                                break;
                            case var n when totalAssets >= 11:
                                status = Booking(statusContractText, 1, checkStatus, driver);
                                break;
                        }
                        break;
                    case 1:
                        throw new Exception("Function GoToBooking, failed.");
                }
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static (int, string) GoToBooking(IWebDriver driver)
        {
            try
            {
                string bookingLink = "/html/body/table[4]/tbody/tr/td[1]/table/tbody/tr[11]/td[4]/a";
                string btnValidate = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[30]/td[2]/a[1]/img";
                string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";

                Ft_Firefox.ClickAction(By.XPath(bookingLink), driver);
                Ft_Firefox.WaitUntilElementClickable(By.XPath(btnValidate), TimeSpan.FromMinutes(3), driver);
                string statusText = driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
                return (0, statusText);
            }
            catch (Exception)
            {
                return (1, "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="group"></param>
        /// <param name="checkStatus"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string Booking(string status, int group, bool checkStatus, IWebDriver driver)
        {
            string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";
            bool finish = true;
            try
            {
                switch (status)
                {
                    case "Incomplete":
                    case "New":
                        try
                        {
                            status = ValidateButton(driver);
                            finish = true;
                        }
                        catch (Exception)
                        {
                            throw new Exception("Function Incomplete or New, failed");
                        }
                        break;
                    case "Passed":
                        try
                        {
                            status = PassedButton(group, driver);
                            finish = true;
                        }
                        catch (Exception)
                        {
                            throw new Exception("Function Passed, failed");
                        }
                        break;
                    case "Complete":
                        try
                        {
                            status = ApprovalButton(group, driver);
                            finish = true;
                        }
                        catch (Exception)
                        {
                            throw new Exception("Function Complete, failed");
                        }
                        break;
                    case "Approved":
                        try
                        {
                            if (checkStatus)
                            {
                                status = ActivateButton(group, driver);
                                Settings.Waiting(TimeSpan.FromSeconds(1));
                            }
                            finish = false;
                        }
                        catch (Exception)
                        {
                            throw new Exception("Function Approved, failed");
                        }
                        break;
                }
                if (finish)
                {
                    Booking(status, group, checkStatus, driver);
                }
                return driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string ValidateButton(IWebDriver driver) 
        {
            string validateButton = "Validate Contract";
            string backToBooking = "Back To Booking Screen";
            string tableValidation = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[4]/td[2]/table/tbody/tr/td/table/tbody/tr";
            string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";
            int count = 2;
            int rows;

            Ft_Firefox.ClickAction(By.CssSelector($"img[title='{validateButton}']"), driver);
            Ft_Firefox.WaitUntilElementClickable(By.CssSelector($"img[title='{backToBooking}']"), TimeSpan.FromMinutes(3), driver);

            rows = (driver.FindElements(By.XPath(tableValidation)).Count) - 1;
            while(count != rows)
            {
                string line = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[4]/td[2]/table/tbody/tr/td/table/tbody/tr[" + count + "]/td[3]";
                string severity = driver.FindElement(By.XPath(line)).Text.ToString();
                if (severity == "Error")
                {
                    string explanation = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[4]/td[2]/table/tbody/tr/td/table/tbody/tr[" + count + "]/td[4]";
                    Ft_Firefox.ClickAction(By.CssSelector($"img[title='{backToBooking}']"), driver);
                    throw new Exception(driver.FindElement(By.XPath(explanation)).Text);
                }
                count++;
            }
            Ft_Firefox.ClickAction(By.CssSelector($"img[title='{backToBooking}']"), driver);
            Ft_Firefox.WaitUntilElementClickable(By.CssSelector($"img[title='{validateButton}']"), TimeSpan.FromMinutes(3), driver);
            return driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string PassedButton(int group, IWebDriver driver)
        {
            string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";
            string streamsButton = "Streams";

            switch (group)
            {
                case 0: //Less than 11
                    Ft_Firefox.ClickAction(By.CssSelector($"img[title='{streamsButton}']"), driver);
                    Ft_Firefox.WaitingText(By.XPath(statusContract), " Complete", TimeSpan.FromMinutes(3), driver);
                    break;
                case 1: //Greater than 11
                    SubmitButtonToApproved(driver);
                    break;
            }
            return driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string ApprovalButton(int group, IWebDriver driver)
        {
            string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";
            string submitApproval = "Submit for Approval";

            switch (group)
            {
                case 0: //Less than 11
                    Ft_Firefox.ClickAction(By.CssSelector($"img[title='{submitApproval}']"), driver);
                    Ft_Firefox.WaitingText(By.XPath(statusContract), " Approved", TimeSpan.FromMinutes(3), driver);
                    break;
                case 1: //Greater than 11
                    SubmitButtonToApproved(driver);
                    break;
            }
            return driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string ActivateButton(int group, IWebDriver driver)
        {
            string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";
            string contractStage = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[32]/td[3]/select";
            string activateButton = "Activate";
            string submitButton = "Submit";
            string refreshButton = "Refresh";
            string status;

            switch (group)
            {
                case 0: //Less than 11
                    Ft_Firefox.ClickAction(By.CssSelector($"img[title='{activateButton}']"), driver);
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector($"img[title='{activateButton}']")));
                    break;
                case 1: //Greater than 11
                    IWebElement dropDownMenu = driver.FindElement(By.XPath(contractStage));
                    SelectElement select = new SelectElement(dropDownMenu);
                    select.SelectByValue("BOOKED");

                    Ft_Firefox.ClickAction(By.CssSelector($"img[title='{submitButton}']"), driver);
                    status = driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
                    while (status == "Booked" || status == "Abandoned")
                    {
                        Ft_Firefox.ClickAction(By.CssSelector($"img[title='{refreshButton}']"), driver);
                        Settings.Waiting(TimeSpan.FromMinutes(3));
                        status = driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
                    }
                    break; 
            }
            Settings.Waiting(TimeSpan.FromSeconds(3));
            return driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string SubmitButtonToApproved(IWebDriver driver)
        {
            string statusContract = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[4]";
            string contractStage = "/html/body/table[4]/tbody/tr/td[2]/form/table/tbody/tr[32]/td[3]/select";
            string submitButton = "Submit";
            string refreshButton = "Refresh";
            string status;

            //Select drop Down Menu
            IWebElement dropDownMenu = driver.FindElement(By.XPath(contractStage));
            SelectElement select = new SelectElement(dropDownMenu);
            select.SelectByValue("APPROVED");
            
            Ft_Firefox.ClickAction(By.CssSelector($"img[title='{submitButton}']"), driver);
            status = driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
            while (status != "Approved")
            {
                Ft_Firefox.ClickAction(By.CssSelector($"img[title='{refreshButton}']"), driver);
                Settings.Waiting(TimeSpan.FromSeconds(10));
                status = driver.FindElement(By.XPath(statusContract)).Text.ToString().Trim();
            }
            return status;
        }
    }
}
