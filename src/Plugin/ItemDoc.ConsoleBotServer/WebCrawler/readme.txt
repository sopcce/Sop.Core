下边的我们要讲的内容，涉及了众多开源软件。先别太紧张，
越是高级的东西通常都封装的越好，只要放开心态综合运用
就行了，我先假设你对下边这些工具都有过了解：

RabbitMQ：用于分布式消息传递。

Shadowsocks：用于代理加密。

PhantomJS：用于Web页面渲染。

Selenium：用于Web自动化控制。

一、什么是高级爬虫？

我们长谈到的高级爬虫，通常是说它具有浏览器的运行特征，
需要第三方的类库或工具的支持，比如说以下这些常见的东东：

Webkit

WebBrowser

PhantomJS + Selenium

很多人都觉得，分布式爬虫才能算是高级的爬虫。这绝对是一种错误的理解
，分布式只是我们实现爬虫架构的一种手段，而并非是用来定义它高级的因素。

PhantomJS：算是一个没有UI界面的浏览器，主要用来实现页面自动化测试
，我们则利用它的页面解析功能，执行网站内容的抓取。下载解压后将Bin文件夹中的phantomjs.exe文件复制到你爬虫项目下的任意文件夹，我们只需要这个。

下载地址：http://phantomjs.org/download.html

基于C#.NET的高端智能化网络爬虫（二）（攻破携程网）

Selenium：是一个自动化测试工具，封装了很多WebDriver用于跟浏览器内核通讯，
我用开发语言来调用它实现PhantomJS的自动化操作。它的下载页面里有很多东西，
我们只需要Selenium Client，它支持了很多语言（C#、JAVA、Ruby、Python、NodeJS）
，按自己所学语言下载即可。

下载地址：http://docs.seleniumhq.org/download/