using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Sop.FileServer.WebCrawler.Extensions
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static IWebElement TryFindElement(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="locator"></param>
        /// <param name="elementCondition"></param>
        /// <param name="timeOutInceconds"></param>
        /// <returns></returns>
        public static IWebElement TryFindElement(ISearchContext sc, By locator, Func<IWebElement, bool> elementCondition = null, int timeOutInceconds = 20)
        {
            DefaultWait<ISearchContext> wait = new DefaultWait<ISearchContext>(sc);
            wait.Timeout = TimeSpan.FromSeconds(timeOutInceconds);
            wait.PollingInterval = TimeSpan.FromSeconds(3);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            return wait.Until(x => GetElement(x, locator, elementCondition));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="url"></param>
        public static void GoToUrl(this IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="cssSelector"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static IWebElement FindBySelector(this IWebDriver driver, string cssSelector, int timeoutInSeconds = 30)
        {
            return FindElement(driver, By.CssSelector(cssSelector), timeoutInSeconds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="cssSelector"></param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> FindAllBySelector(this IWebDriver driver, string cssSelector)
        {
            return driver.FindElements(By.CssSelector(cssSelector));
        }

        public static IWebElement FindElement(this IWebDriver driver, By @by, IWebElement parent, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => parent.FindElement(by));
            }

            return parent.FindElement(by);
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="selector"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static IWebElement WaitUntilElementIsVisible(this IWebDriver driver, string selector, int timeoutInSeconds)
        {
            return driver.WaitUntil(By.CssSelector(selector), drv => drv.GetIfVisible(By.CssSelector(selector)), timeoutInSeconds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="selector"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static IWebElement WaitUntilElementIsEnabled(this IWebDriver driver, string selector, int timeoutInSeconds)
        {
            return driver.WaitUntil(By.CssSelector(selector), drv => drv.GetIfEnabled(selector), timeoutInSeconds);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="func"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static IWebElement WaitUntil(this IWebDriver driver, By by, Func<IWebDriver, IWebElement> func, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(func);
            }

            return driver.FindElement(by);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <param name="expectedWindowCount"></param>
        public static void WaitUntilWindowOpens(this IWebDriver driver, int timeoutInSeconds, int expectedWindowCount = 2)
        {
            var startTime = DateTime.Now;

            while (driver.WindowHandles.Count < expectedWindowCount && (DateTime.Now - startTime).TotalSeconds < timeoutInSeconds)
            {
                Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static bool IsVisible(this IWebDriver driver, string selector)
        {
            var elements = driver.FindElements(By.CssSelector(selector));
            return elements.Any() && elements.All(x => x.Displayed);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static IWebElement GetIfVisible(this IWebDriver driver, By by)
        {
            var element = driver.FindElement(by);
            return element != null && element.Displayed ? element : null;
        }

        public static IWebElement GetIfEnabled(this IWebDriver driver, string selector)
        {
            try
            {
                var element = driver.FindElement(By.CssSelector(selector));
                return element != null && element.Enabled ? element : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="locator"></param>
        /// <param name="elementCondition"></param>
        /// <returns></returns>
        public static IWebElement GetElement(ISearchContext sc, By locator, Func<IWebElement, bool> elementCondition = null)
        {
            IWebElement webElement = sc.FindElement(locator);
            if (elementCondition != null)
            {
                if (elementCondition(webElement))
                    return webElement;
                else
                    return null;
            }
            else
            {
                return webElement;
            }
        }

        /// <summary>
        /// Locates an element using the given <see cref="ISearchContext"/> and list of <see cref="By"/> criteria.
        /// </summary>
        /// <param name="searchContext">The <see cref="ISearchContext"/> object within which to search for an element.</param>
        /// <param name="bys">The list of methods by which to search for the element.</param>
        /// <returns>An <see cref="IWebElement"/> which is the first match under the desired criteria.</returns>
        public static IWebElement LocateElement(ISearchContext searchContext, IEnumerable<By> bys)
        {
            if (searchContext == null || bys == null)
            {
                return null;
            }
            string errorString = null;
            foreach (var by in bys)
            {
                try
                {
                    return searchContext.FindElement(by);
                }
                catch (NoSuchElementException)
                {
                    errorString = (errorString == null ? "Could not find element by: " : errorString + ", or: ") + by;
                }
            }
            return null;
        }
    }
}