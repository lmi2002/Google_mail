using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace GoogleMail
{


    class PageMail
    {
        //values:
        IWebDriver driver;
        WebDriverWait wait;
        IWebElement elem_field_letter;
        IWebElement elem_input_email;
        IWebElement elem_button_go;
        IWebElement elem_input_pass;
        IWebElement elem_button_menu;
        IWebElement elem_button_mail_app;
        IWebElement elem_indraft;
        IWebElement elem_button_addletter;
        IWebElement elem_button_close;
        IWebElement elem_row;
        string value = "Data";
        string value_update = "Update";
        string email = "srgsrch1988@gmail.com";
        string pass = "qwe1asd1zx";
        string textbodyletter;


        // locators:
        string input_email = "input";
        string input_pass = "//input[@type='password']";
        string button_go = "//span[text()='Далее']";
        string button_menu = "//a[@aria-label='Приложения Google']";
        string button_mail_app = "//a//span[contains(text(), 'Почта')]";
        string indraft = "//*[contains(text(), 'Черновики')]";
        string button_addletter = "//*[contains(text(), 'Написать')]";
        string field_letter = "//*[contains(@role, 'textbox')]";
        string button_close = "//*[contains(@alt, 'Закрыть')]";
        string button_deleteletter = "//*[contains(text(), 'Удалить черновики')]";
        string row = "/html/body/div[7]/div[3]/div/div[2]/div[1]/div[2]/div/div/div/div/div[2]/div/div[1]/div/div[2]/div[3]/div[2]/div/table/tbody/tr";
        string checkbox = "/html/body/div[7]/div[3]/div/div[2]/div[1]/div[2]/div/div/div/div/div[1]/div[2]/div[1]/div[1]/div/div/div[1]/div/div[1]/span";
        string button_account = "//*[contains(@aria-label, 'Аккаунт Google')]";
        string popup_abautaccount = "//*[contains(@aria-label, 'Информация об аккаунте')]";
        string button_exit = "//a[contains(text(), 'Выйти')]";

        //functions:
        public void DeleteLetter(WebDriverWait wait)
        {
            IWebElement elem_checkbox = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(checkbox)));
            elem_checkbox.Click();
            IWebElement elem_button_deleteletter = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_deleteletter)));
            elem_button_deleteletter.Click();
        }

        public static WebDriverWait Wait(int sec, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
            return wait;
        }

        public bool LetterisPresent(WebDriverWait wait)
        {
            try
            {
                IWebElement elem_row = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(row)));
                return true;
            }
            catch (WebDriverTimeoutException) {
                return false;
            }
        }

        public void SingOut(WebDriverWait wait)
        {
            IWebElement elem_button_account = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_account)));
            elem_button_account.Click();
            IWebElement elem_popup_abautaccount = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(popup_abautaccount)));
            IWebElement elem_button_exit = elem_popup_abautaccount.FindElement(By.XPath(button_exit));
            elem_button_exit.Click();
        }

        public void isAlertPresent(IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert().Accept();
            }   
            catch(NoAlertPresentException)
            {

            }  
        }

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = Wait(30, driver);
        }

        [Test]
        public void test()
        {
            driver.Url = "https://accounts.google.com/";
            elem_input_email = driver.FindElement(By.TagName(input_email));
            elem_input_email.SendKeys(email);
            elem_button_go = driver.FindElement(By.XPath(button_go));
            elem_button_go.Click();
            elem_input_pass = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(input_pass)));
            elem_input_pass.SendKeys(pass);
            elem_button_go = driver.FindElement(By.XPath(button_go));
            elem_button_go.Click();
            elem_button_menu = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_menu)));
            elem_button_menu.Click();
            elem_button_mail_app = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_mail_app)));
            elem_button_mail_app.Click();
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            elem_indraft = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(indraft)));
            elem_indraft.Click();

            if (LetterisPresent(wait) == true)
            {
                DeleteLetter(wait);
            }

            elem_button_addletter = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_addletter)));
            elem_button_addletter.Click();
            elem_field_letter = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(field_letter)));
            elem_field_letter.SendKeys(value);
            elem_button_close = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_close)));
            elem_button_close.Click();

            //Check that the draft was created
            elem_row = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(row)));

            elem_row.Click();
            elem_field_letter = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(field_letter)));
            elem_field_letter.Clear();
            elem_field_letter.SendKeys(value_update);

            elem_row = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(row)));
            elem_row.Click();
            elem_field_letter = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(field_letter)));
            textbodyletter = elem_field_letter.Text;
            elem_button_close = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(button_close)));
            elem_button_close.Click();

            Assert.AreEqual(value_update, textbodyletter, "The draft wasn’t updated!");

            DeleteLetter(wait);
            SingOut(wait);
            isAlertPresent(driver);
        }
       
        [TearDown]
        public void closeBrowser()
        {         
            driver.Quit();
        }
        
    }
}
