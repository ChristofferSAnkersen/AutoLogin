using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Reflection;
using System.Threading;

namespace AutoLogin
{
    class Program
    {
        private static int _retries = 0;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .Build();

            string name = config["Username"];
            string password = config["Password"];
            Uri url = new Uri("https://relesys.relesysapp.net");
            Console.ForegroundColor = ConsoleColor.Green;

            // Randomize the time to run this
            Random random = new Random();
            int minutesToWait = random.Next(0, 20);
            int millisecondsToWait = (minutesToWait * 1000) + 1000;
            TimeSpan timeout = new TimeSpan(0, 0, 30);

#if DEBUG
            millisecondsToWait = 0;
#endif

            Console.WriteLine("Waiting " + (millisecondsToWait / 1000).ToString() + " minutes before continuing!");
            Console.WriteLine("Triggered at: " + DateTime.Now.ToString("HH:mm") + " - Executing at: " + DateTime.Now.AddMinutes(minutesToWait).ToString("HH:mm"));
            Thread.Sleep(millisecondsToWait);

            try
            {
                // Open Chrome
                ChromeDriver chrome = new ChromeDriver();
                var awaiter = new WebDriverWait(chrome, timeout);
                chrome.Navigate().GoToUrl(url);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Logging In");
                Console.ForegroundColor = ConsoleColor.Red;

                // Username (if not already filled)
                IWebElement loginElement = awaiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Username")));
                loginElement.Clear();
                loginElement.SendKeys(name);

                // Password
                IWebElement passwordField = awaiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("Password")));
                passwordField.Clear();
                passwordField.SendKeys(password);

                // Login
                IWebElement login = awaiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("button")));
                login.Click();

                IWebElement skip = awaiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button[ng-click='Continue()']")));
                skip.Click();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login Successful");
                Console.WriteLine("-----------------------");
                Console.WriteLine("Responding to workmood!");
                Console.ForegroundColor = ConsoleColor.Red;


                // Toggle burger menu
                IWebElement toggleMenu = awaiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button[ng-click='BaseCtrl.ToggleMenu()']")));
                toggleMenu.Click();

                Thread.Sleep(5000);

                // Find Work Mood module
                IWebElement workMood = chrome.FindElement(By.XPath("//span[contains(text(),'Work Mood')]"));
                workMood.Click();

                // Press nr. 5
                IWebElement happy = awaiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//img[@src='/Content/Graphics/WorkMood/5.png']")));
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
                if (_retries < 3)
                {
                    Main(new string[0]);
                }
            }
        }
    }
}