你已成功添加了一个 Office 外接程序。

若要将 Office 功能和样式应用于给定的 HTML 页，请将以下
引用添加到该 HTML 页的 <head> 区域，并根据需要修改相对路径：

    <!-- Office 引用： -->
    <link href="Content/Office.css" rel="stylesheet" type="text/css" />
    <script src="https://appsforoffice.microsoft.com/lib/1/hosted/office.js"></script>

    <!-- 要允许使用 Office.js 的本地引用进行脱机调试，请使用：                  -->
    <!--    <script src="Scripts/Office/MicrosoftAjax.js" type="text/javascript"></script>       -->
    <!--    <script src="" type="text/javascript"></script>          -->


注：在任何 JavaScript 与 Office API 交互之前， 
必须调用 Office 初始化函数（每页一次）。

    Office.initialize = function (reason) {
        $(document).ready(function () {
            // 在此添加初始化逻辑。
        });
    };
