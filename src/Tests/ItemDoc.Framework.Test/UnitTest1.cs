using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemDoc.Framework.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
         


        }

        private string GenerateCompanyCode(long id)
        {
            //用异或计算生成初始的企业号
            int seed = 999999999;
            long code = id ^ seed;

            //根据映射关系对二级制数进行位交换，使企业号无序
            var mappingArray = new int[] { 0, 21, 22, 23, 24, 25, 26, 27, 28, 29, 20 };
            var charArray = Convert.ToString(code, 2).ToCharArray();
            for (int i = 0; i < mappingArray.Length; i++)
            {
                int mappingIndex = mappingArray[i];
                if (mappingIndex != i)
                {
                    var temp = charArray[i];
                    charArray[i] = charArray[mappingIndex];
                    charArray[mappingIndex] = temp;
                }
            }

            code = Convert.ToInt32(new string(charArray), 2);

            return code.ToString();
        }
    }
}
