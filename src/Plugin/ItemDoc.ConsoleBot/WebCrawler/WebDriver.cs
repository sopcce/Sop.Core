using System;
using System.Collections.Generic;
using QA = OpenQA.Selenium;
using UI = OpenQA.Selenium.Support.UI;
 

namespace ItemDoc.ConsoleBot.WebCrawler
{
    public class WebDriver
    {
        private QA.IWebDriver wd = null;
        private Browsers _browser = Browsers.Chrome;
        public WebDriver(Browsers theBrowser)
        {
            this._browser = theBrowser;
            wd = InitWebDriver();
        }

        private QA.IWebDriver InitWebDriver()
        {
            QA.IWebDriver theDriver = null;
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        QA.IE.InternetExplorerOptions _ieOptions = new QA.IE.InternetExplorerOptions();
                        _ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                        theDriver = new QA.IE.InternetExplorerDriver(_ieOptions);
                    }; break;
                case Browsers.Chrome:
                    {
                        theDriver = new QA.Chrome.ChromeDriver();
                    }; break;
                case Browsers.Firefox:
                    {
                        theDriver = new QA.Firefox.FirefoxDriver();
                    }; break;
                case Browsers.Safari:
                    {
                        theDriver = new QA.Safari.SafariDriver();
                    }; break;
                case Browsers.PhantomJS:
                    {
                        theDriver = new QA.PhantomJS.PhantomJSDriver();
                    }; break;
                default:
                    {
                        QA.IE.InternetExplorerOptions _ieOptions = new QA.IE.InternetExplorerOptions();
                        _ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                        theDriver = new QA.IE.InternetExplorerDriver(_ieOptions);
                    }; break;
            }
            return theDriver;
        }

        #region public members
        /// <summary> 
        ///             
        /// </summary>
        /// <param name="seconds"></param>
        public void ImplicitlyWait(double seconds)
        {
            //获取或设置隐式等待超时，这是驱动程序在搜索元素时应该等待的时间（如果元素不立即存在）
            //wd.Manage().Timeouts().ImplicitWait(TimeSpan.FromSeconds(seconds));
            wd.Manage()?.Timeouts()?.ImplicitWait.Add(TimeSpan.FromSeconds(seconds));
           

        }

        /// <summary>
        /// Wait for the expected condition is satisfied, return immediately
        /// </summary>
        /// <param name="expectedCondition"></param>
        public void WaitForPage(string title)
        {


            var _wait = new UI.WebDriverWait(wd, TimeSpan.FromSeconds(10));
            _wait.Until((d) => { return d.Title.ToLower().StartsWith(title.ToLower()); });
            //to do
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="we"></param>
        public void WaitForElement(string id)
        {
            UI.WebDriverWait _wait = new UI.WebDriverWait(wd, TimeSpan.FromSeconds(10));
            _wait.Until((d) => { return OpenQA.Selenium.Support.UI.ExpectedConditions.ElementExists(QA.By.Id(id)); });
        }


        /// <summary>
        /// Load a new web page in current browser
        /// </summary>
        /// <param name="url"></param>
        public void GoToUrl(string url)
        {
            wd.Navigate().GoToUrl(url);
        }

        public void Refresh()
        {
            wd.Navigate().Refresh();
        }

        public void Back()
        {
            wd.Navigate().Back();
        }

        public void Forward()
        {
            wd.Navigate().Forward();
        }

        /// <summary>
        /// Get the url of current browser window
        /// </summary>
        /// <returns></returns>
        public string GetUrl()
        {
            return wd.Url;
        }

        /// <summary>
        /// Get page title of current browser window
        /// </summary>
        /// <returns></returns>
        public string GetPageTitle()
        {
            return wd.Title;
        }

        /// <summary>
        /// Get all cookies defined in the current page
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllCookies()
        {
            Dictionary<string, string> cookies = new Dictionary<string, string>();
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        var allCookies = ((QA.IE.InternetExplorerDriver)wd).Manage().Cookies.AllCookies;
                        foreach (QA.Cookie cookie in allCookies)
                        {
                            cookies[cookie.Name] = cookie.Value;
                        }
                    }; break;
                case Browsers.Chrome:
                    {
                        var allCookies = ((QA.Chrome.ChromeDriver)wd).Manage().Cookies.AllCookies;
                        foreach (QA.Cookie cookie in allCookies)
                        {
                            cookies[cookie.Name] = cookie.Value;
                        }
                    }; break;
                case Browsers.Firefox:
                    {
                        var allCookies = ((QA.Firefox.FirefoxDriver)wd).Manage().Cookies.AllCookies;
                        foreach (QA.Cookie cookie in allCookies)
                        {
                            cookies[cookie.Name] = cookie.Value;
                        }
                    }; break;
                case Browsers.Safari:
                    {
                        var allCookies = ((QA.Safari.SafariDriver)wd).Manage().Cookies.AllCookies;
                        foreach (QA.Cookie cookie in allCookies)
                        {
                            cookies[cookie.Name] = cookie.Value;
                        }
                    }; break;
                case Browsers.PhantomJS:
                    {
                        var allCookies = ((QA.PhantomJS.PhantomJSDriver)wd).Manage().Cookies.AllCookies;
                        foreach (QA.Cookie cookie in allCookies)
                        {
                            cookies[cookie.Name] = cookie.Value;
                        }
                    }; break;
                default:
                    {
                        var allCookies = ((QA.IE.InternetExplorerDriver)wd).Manage().Cookies.AllCookies;
                        foreach (QA.Cookie cookie in allCookies)
                        {
                            cookies[cookie.Name] = cookie.Value;
                        }
                    }; break;
            }

            return cookies;
        }

        /// <summary>
        /// Delete all cookies from the page
        /// </summary>
        public void DeleteAllCookies()
        {
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).Manage().Cookies.DeleteAllCookies();
                    }; break;
                case Browsers.Chrome:
                    {
                        ((QA.Chrome.ChromeDriver)wd).Manage().Cookies.DeleteAllCookies();
                    }; break;
                case Browsers.Firefox:
                    {
                        ((QA.Firefox.FirefoxDriver)wd).Manage().Cookies.DeleteAllCookies();
                    }; break;
                case Browsers.Safari:
                    {
                        ((QA.Safari.SafariDriver)wd).Manage().Cookies.DeleteAllCookies();
                    }; break;
                case Browsers.PhantomJS:
                    {
                        ((QA.PhantomJS.PhantomJSDriver)wd).Manage().Cookies.DeleteAllCookies();
                    }; break;
                default:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).Manage().Cookies.DeleteAllCookies();
                    }; break;
            }
        }

        /// <summary>
        /// Set focus to a browser window with a specified title
        /// </summary>
        /// <param name="title"></param>
        /// <param name="exactMatch"></param>
        public void GoToWindow(string title, bool exactMatch)
        {
            string theCurrent = wd.CurrentWindowHandle;
            IList<string> windows = wd.WindowHandles;
            if (exactMatch)
            {
                foreach (var window in windows)
                {
                    wd.SwitchTo().Window(window);
                    if (wd.Title.ToLower() == title.ToLower())
                    {
                        return;
                    }
                }
            }
            else
            {
                foreach (var window in windows)
                {
                    wd.SwitchTo().Window(window);
                    if (wd.Title.ToLower().Contains(title.ToLower()))
                    {
                        return;
                    }
                }
            }

            wd.SwitchTo().Window(theCurrent);
        }

        /// <summary>
        /// Set focus to a frame with a specified name
        /// </summary>
        /// <param name="name"></param>
        public void GoToFrame(string name)
        {
            QA.IWebElement theFrame = null;
            var frames = wd.FindElements(QA.By.TagName("iframe"));
            foreach (var frame in frames)
            {
                if (frame.GetAttribute("name").ToLower() == name.ToLower())
                {
                    theFrame = (QA.IWebElement)frame;
                    break;
                }
            }
            if (theFrame != null)
            {
                wd.SwitchTo().Frame(theFrame);
            }
        }

        public void GoToFrame(QA.IWebElement frame)
        {
            wd.SwitchTo().Frame(frame);
        }

        /// <summary>
        /// Switch to default after going to a frame
        /// </summary>
        public void GoToDefault()
        {
            wd.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Get the alert text
        /// </summary>
        /// <returns></returns>
        public string GetAlertString()
        {
            string theString = string.Empty;
            QA.IAlert alert = null;
            alert = wd.SwitchTo().Alert();
            if (alert != null)
            {
                theString = alert.Text;
            }
            return theString;
        }

        /// <summary>
        /// Accepts the alert
        /// </summary>
        public void AlertAccept()
        {
            QA.IAlert alert = null;
            alert = wd.SwitchTo().Alert();
            if (alert != null)
            {
                alert.Accept();
            }
        }

        /// <summary>
        /// Dismisses the alert
        /// </summary>
        public void AlertDismiss()
        {
            QA.IAlert alert = null;
            alert = wd.SwitchTo().Alert();
            if (alert != null)
            {
                alert.Dismiss();
            }
        }

        /// <summary>
        /// Move vertical scroll bar to bottom for the page
        /// </summary>
        public void PageScrollToBottom()
        {
            var js = "document.documentElement.scrollTop=10000";
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Chrome:
                    {
                        ((QA.Chrome.ChromeDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Firefox:
                    {
                        ((QA.Firefox.FirefoxDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Safari:
                    {
                        ((QA.Safari.SafariDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.PhantomJS:
                    {
                        ((QA.PhantomJS.PhantomJSDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Opera:
                    {
                        ((QA.Opera.OperaDriver)wd).ExecuteScript(js, null);
                    }; break;
                default:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
            }
        }

        /// <summary>
        /// Move horizontal scroll bar to right for the page
        /// </summary>
        public void PageScrollToRight()
        {
            var js = "document.documentElement.scrollLeft=10000";
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Chrome:
                    {
                        ((QA.Chrome.ChromeDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Firefox:
                    {
                        ((QA.Firefox.FirefoxDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Safari:
                    {
                        ((QA.Safari.SafariDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.PhantomJS:
                    {
                        ((QA.PhantomJS.PhantomJSDriver)wd).ExecuteScript(js, null);
                    }; break;
                default:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
            }
        }

        /// <summary>
        /// Move vertical scroll bar to bottom for an element
        /// </summary>
        /// <param name="element"></param>
        public void ElementScrollToBottom(QA.IWebElement element)
        {
            string id = element.GetAttribute("id");
            string name = element.GetAttribute("name");
            var js = "";
            if (!string.IsNullOrWhiteSpace(id))
            {
                js = "document.getElementById('" + id + "').scrollTop=10000";
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                js = "document.getElementsByName('" + name + "')[0].scrollTop=10000";
            }
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Chrome:
                    {
                        ((QA.Chrome.ChromeDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Firefox:
                    {
                        ((QA.Firefox.FirefoxDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Safari:
                    {
                        ((QA.Safari.SafariDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.PhantomJS:
                    {
                        ((QA.PhantomJS.PhantomJSDriver)wd).ExecuteScript(js, null);
                    }; break;
                default:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
            }
        }

        /// <summary>
        /// Get a screen shot of the current window
        /// </summary>
        /// <param name="savePath"></param>
        public void TakeScreenshot(string savePath)
        {
            QA.Screenshot theScreenshot = null;
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        theScreenshot = ((QA.IE.InternetExplorerDriver)wd).GetScreenshot();
                    }; break;
                case Browsers.Chrome:
                    {
                        theScreenshot = ((QA.Chrome.ChromeDriver)wd).GetScreenshot();
                    }; break;
                case Browsers.Firefox:
                    {
                        theScreenshot = ((QA.Firefox.FirefoxDriver)wd).GetScreenshot();
                    }; break;
                case Browsers.Safari:
                    {
                        theScreenshot = ((QA.Safari.SafariDriver)wd).GetScreenshot();
                    }; break;
                case Browsers.PhantomJS:
                    {
                        theScreenshot = ((QA.PhantomJS.PhantomJSDriver)wd).GetScreenshot();
                    }; break;
                default:
                    {
                        theScreenshot = ((QA.IE.InternetExplorerDriver)wd).GetScreenshot();
                    }; break;
            }
            if (theScreenshot != null)
            {
                //theScreenshot.SaveAsFile(savePath); 
                theScreenshot.SaveAsFile(savePath, QA.ScreenshotImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// Find the element of a specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QA.IWebElement FindElementById(string id)
        {
            QA.IWebElement theElement = null;
            theElement = (QA.IWebElement)wd.FindElement(QA.By.Id(id));
            return theElement;
        }

        /// <summary>
        /// Find the element of a specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public QA.IWebElement FindElementByName(string name)
        {
            QA.IWebElement theElement = null;
            theElement = (QA.IWebElement)wd.FindElement(QA.By.Name(name));
            return theElement;
        }

        /// <summary>
        /// Find the element by xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public QA.IWebElement FindElementByXPath(string xpath)
        {
            QA.IWebElement theElement = null;
            theElement = (QA.IWebElement)wd.FindElement(QA.By.XPath(xpath));
            return theElement;
        }

        public QA.IWebElement FindElementByLinkText(string text)
        {
            QA.IWebElement theElement = null;
            try
            {
                theElement = wd.FindElement(QA.By.LinkText(text));
            }
            catch { }
            return theElement;
        }

        public IList<QA.IWebElement> FindElementsByLinkText(string text)
        {
            IList<QA.IWebElement> theElement = null;
            theElement = (IList<QA.IWebElement>)wd.FindElements(QA.By.LinkText(text));
            return theElement;
        }

        public IList<QA.IWebElement> FindElementsByPartialLinkText(string text)
        {
            IList<QA.IWebElement> theElement = null;
            theElement = (IList<QA.IWebElement>)wd.FindElements(QA.By.PartialLinkText(text));
            return theElement;
        }

        public IList<QA.IWebElement> FindElementsByClassName(string clsName)
        {
            IList<QA.IWebElement> theElement = null;
            theElement = (IList<QA.IWebElement>)wd.FindElements(QA.By.ClassName(clsName));
            return theElement;
        }

        public IList<QA.IWebElement> FindElementsByTagName(string tagName)
        {
            IList<QA.IWebElement> theElement = null;
            theElement = (IList<QA.IWebElement>)wd.FindElements(QA.By.TagName(tagName));
            return theElement;
        }

        public IList<QA.IWebElement> FindElementsByCssSelector(string css)
        {
            IList<QA.IWebElement> theElement = null;
            theElement = (IList<QA.IWebElement>)wd.FindElements(QA.By.CssSelector(css));
            return theElement;
        }

        public IList<QA.IWebElement> FindElementsByXPathName(string xpath)
        {
            IList<QA.IWebElement> theElement = null;
            theElement = (IList<QA.IWebElement>)wd.FindElements(QA.By.XPath(xpath));
            return theElement;
        }

        /// <summary>
        /// Executes javascript
        /// </summary>
        /// <param name="js"></param>
        public void ExecuteJS(string js)
        {
            switch (this._browser)
            {
                case Browsers.IE:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Chrome:
                    {
                        ((QA.Chrome.ChromeDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Firefox:
                    {
                        ((QA.Firefox.FirefoxDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.Safari:
                    {
                        ((QA.Safari.SafariDriver)wd).ExecuteScript(js, null);
                    }; break;
                case Browsers.PhantomJS:
                    {
                        ((QA.PhantomJS.PhantomJSDriver)wd).ExecuteScript(js, null);
                    }; break;
                default:
                    {
                        ((QA.IE.InternetExplorerDriver)wd).ExecuteScript(js, null);
                    }; break;
            }
        }

        public void ClickElement(QA.IWebElement element)
        {
            (new QA.Interactions.Actions(wd)).Click(element).Perform();
        }

        public void DoubleClickElement(QA.IWebElement element)
        {
            (new QA.Interactions.Actions(wd)).DoubleClick(element).Perform();
        }

        public void ClickAndHoldOnElement(QA.IWebElement element)
        {
            (new QA.Interactions.Actions(wd)).ClickAndHold(element).Perform();
        }

        public void ContextClickOnElement(QA.IWebElement element)
        {
            (new QA.Interactions.Actions(wd)).ContextClick(element).Perform();
        }

        public void DragAndDropElement(QA.IWebElement source, QA.IWebElement target)
        {
            (new QA.Interactions.Actions(wd)).DragAndDrop(source, target).Perform();
        }

        public void SendKeysToElement(QA.IWebElement element, string text)
        {
            (new QA.Interactions.Actions(wd)).SendKeys(element, text).Perform();
        }

        /// <summary>
        /// Quit this server, close all windows associated to it
        /// </summary>
        public void Cleanup()
        {
            wd.Quit();
        }
        #endregion
    }

    public enum Browsers
    {
        IE,
        Firefox,
        Chrome,
        Safari,
        PhantomJS,
        Opera
    }
}