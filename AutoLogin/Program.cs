using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace AutoLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            const string name = "USERNAME";
            const string password = "PASSWORD";
            Uri url = new Uri("https://relesys.relesysapp.net");

            try
            {
                // Open Chrome
                ChromeDriver chrome = new ChromeDriver();
                chrome.Navigate().GoToUrl(url);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Logging In");

                // Username (if not already filled)
                IWebElement userField = chrome.FindElementById("Username");
                userField.Clear();
                userField.SendKeys(name);

                // Password
                IWebElement passwordField = chrome.FindElementById("Password");
                passwordField.Clear();
                passwordField.SendKeys(password);

                // Login
                IWebElement login = chrome.FindElementByName("button");
                login.Click();

                Thread.Sleep(3000);

                IWebElement skip = chrome.FindElementByCssSelector("button[ng-click='Continue()']");
                skip.Click();

                Console.WriteLine("Login Successful");
                Console.WriteLine("-----------------------");
                Console.WriteLine("Responding to workmood!");

                Thread.Sleep(3000);

                // Toggle burger menu
                IWebElement toggleMenu = chrome.FindElementByCssSelector("button[ng-click='BaseCtrl.ToggleMenu()']");
                toggleMenu.Click();

                Thread.Sleep(3000);

                // Find Work Mood module
                IWebElement workMood = chrome.FindElement(By.XPath("//span[contains(text(),'Work Mood')]"));
                workMood.Click();

                Thread.Sleep(3000);

                // Press nr. 5
                IWebElement happy = chrome.FindElement(By.XPath("//img[@src='/Content/Graphics/WorkMood/5.png']"));
                happy.Click();

                Thread.Sleep(2500);
                Console.WriteLine("Successfully responded to Work Mood!");
                Thread.Sleep(2500);
                chrome.Close();
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}