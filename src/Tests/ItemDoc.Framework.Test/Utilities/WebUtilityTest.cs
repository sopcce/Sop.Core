using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sop.Framework.Test;

namespace ItemDoc.Framework.Test.Utilities
{
    [TestClass]
    public class WebUtilityTest
    {
        [TestInitialize]
        public void asdasda()
        {
            MockHelper.GetHttpContext();
            MockHelper.GetRequestContext();
        }


        [TestMethod]
        [Description("测试IP")]
        public void IP_Test()
        {
            //HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost", ""), new HttpResponse(new StringWriter(new StringBuilder())));
            //string ipstr1 = WebUtility.GetIp();


           // var asd = new WebUtility().IsConnectedInternet();
        }
    }
}
