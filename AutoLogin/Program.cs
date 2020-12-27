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
            Console.ForegroundColor = ConsoleColor.Green;

            // Randomize the time to run this
            Random random = new Random();
            int minutesToWait = random.Next(0, 60);
            int millisecondsToWait = (minutesToWait * 60000) + 1000;

            Console.WriteLine("Waiting " + (millisecondsToWait / 60000).ToString() + " minutes before continuing!");
            Console.WriteLine("Triggered at: " + DateTime.Now.ToString("HH:mm") + " - Executing at: " + DateTime.Now.AddMinutes(minutesToWait).ToString("HH:mm"));
            Thread.Sleep(millisecondsToWait);

            try
            {
                // Open Chrome
                ChromeDriver chrome = new ChromeDriver();
                chrome.Navigate().GoToUrl(url);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Logging In");
                Console.ForegroundColor = ConsoleColor.Red;

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

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login Successful");
                Console.WriteLine("-----------------------");
                Console.WriteLine("Responding to workmood!");
                Console.ForegroundColor = ConsoleColor.Red;

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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully responded to Work Mood!");
                chrome.Close();
                chrome.Dispose();
                Environment.Exit(0);
                return;
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