using ItemDoc.Framework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemDoc.Framework.Test.Utilities
{
    /// <summary>
    /// 数据加密类，提供各种加密解密算法及防篡改散列算法
    /// </summary>
    [TestClass]
    public class EncryptionUtilityTest
    {

        #region 初始化类资源
        /// <summary>
        /// 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {

        }

        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        [ClassCleanup()]
        public static void MyClassCleanup()
        {

        }

        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {

        }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {

        }

        #endregion

        #region 数据的防篡改验证
        /// <summary>
        /// 测试数据的防篡改验证码(散列值)生成函数
        /// </summary>
        [Description("测试 数据的防篡改验证码(散列值)生成函数")]
        [TestMethod]
        public void EncryptString()
        {
            string str = EncryptionUtility.EncryptString("sopcce","sop");
            Assert.AreEqual(str, "1A014AF2903091028842D8307A0255E675B02DA38AF04DF990");
            Assert.IsNotNull(str);
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// 生成指定字符串的MD5散列值，返回大写串
        /// </summary>
        [Description("生成指定字符串的MD5散列值，返回大写串")]
        [TestMethod]
        public void MD5Encode()
        {
            string str16 = EncryptionUtility.Md5Encode("sop",16);
            string str160 = EncryptionUtility.Md5Encode("sop", 16, EncodingEnum.Default);
            string str161 = EncryptionUtility.Md5Encode("sop", 16, EncodingEnum.Utf8);
            string str162 = EncryptionUtility.Md5Encode("sop", 16, EncodingEnum.Gb2312);
            Assert.AreEqual(str16, "46F94F5EE51BCEE8");
            Assert.IsNotNull(str16);
            Assert.IsNotNull(str160);
            Assert.IsNotNull(str161);
            Assert.IsNotNull(str162);

            string str32 = EncryptionUtility.Md5Encode("sop", 32);
            string str320 = EncryptionUtility.Md5Encode("sop", 32,EncodingEnum.Default);
            string str321 = EncryptionUtility.Md5Encode("sop", 32, EncodingEnum.Default);            
            string str322 = EncryptionUtility.Md5Encode("sop", 32, EncodingEnum.Default);
            Assert.AreEqual(str32, "3F71916D46F94F5EE51BCEE817AD8FAE");
            Assert.IsNotNull(str32);
            Assert.IsNotNull(str320);
            Assert.IsNotNull(str321);
            Assert.IsNotNull(str322);
        }

        #endregion
        
        #region SHA加密
        /// <summary>
        /// 生成指定字符串的SHA1散列值，返回大写串
        /// </summary>
        [Description("生成指定字符串的SHA1散列值，返回大写串")]
        [TestMethod]
        public void SHA1Encode()
        {
            string str = EncryptionUtility.Sha1Encode("sopcce.com", EncodingEnum.Default);
            string str1 = EncryptionUtility.Sha1Encode("sopcce.com", EncodingEnum.Default);
            string str2 = EncryptionUtility.Sha1Encode("sopcce.com", EncodingEnum.Default);
            string str3 = EncryptionUtility.Sha1Encode("sopcce.com");
            Assert.AreEqual(str, "6833248587E32F9EC0F4717F7F8AC42A4555578A");
            Assert.IsNotNull(str);
            Assert.IsNotNull(str1);
            Assert.IsNotNull(str2);
            Assert.IsNotNull(str3);
        }

        /// <summary>
        /// 生成指定字符串的SHA256散列值
        /// </summary>
        [Description("生成指定字符串的SHA256散列值")]
        [TestMethod]
        public void SHA256Encode()
        {
            string str = EncryptionUtility.Sha256Encode("sopcce.com");
            Assert.AreEqual(str, "px8GeVUC0uXxgQeW8P4XqoeYw+dTVfV5TFo2pvFFFMA=");
            Assert.IsNotNull(str);
        }

        /// <summary>
        /// 生成指定字符串的SHA256散列值
        /// </summary>
        [Description("生成指定字符串的SHA256散列值")]
        [TestMethod]
        public void SHA384Encode()
        {
            string str = EncryptionUtility.Sha384Encode("sopcce.com");
            Assert.AreEqual(str, "Xbb0XzEqT75z31vpVwPmgjSyl7e3IB3BVKOJp4NUE8uO3kxI+RxdhEEUZ87wKdEM");
            Assert.IsNotNull(str);
        }
        /// <summary>
        /// 生成指定字符串的SHA512散列值
        /// </summary>
        [Description("生成指定字符串的SHA512散列值")]
        [TestMethod]
        public void SHA512Encode()
        {
            string str = EncryptionUtility.Sha512Encode("sopcce.com");
            Assert.AreEqual(str, "ZLOH4HaJuQVAmoYogalieAjNchL3DlIKVQ+VKXUFcA9QrdxFdEKM/jACPl6/V0+BDJIEBjJLk+U3l/yvTRLymw==");
            Assert.IsNotNull(str);
        }
        #endregion
        
        #region DES加密、解密
        /// <summary>
        /// DES加密字符串
        /// </summary>
        [Description("DES加密字符串")]
        [TestMethod]
        public void EncryptDES()
        {
            string str = EncryptionUtility.DES_Encrypt("sopcce.com","12345678");
            string str1 = EncryptionUtility.DES_Encrypt("sopcce.com", "123");

            Assert.AreEqual(str, "rURHNTjpUkzmPGqrC2f0DA==");
            Assert.IsNotNull(str);
            Assert.AreEqual(str1, "sopcce.com");
            Assert.IsNotNull(str1);
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        [Description("DES解密字符串")]
        [TestMethod]
        public void DecryptDES()
        {
            string str = EncryptionUtility.DES_Decrypt("rURHNTjpUkzmPGqrC2f0DA==", "12345678");
            string str1 = EncryptionUtility.DES_Decrypt("rURHNTjpUkzmPGqrC2f0DA==", "123");
            Assert.AreEqual(str, "sopcce.com");
            Assert.IsNotNull(str);
            Assert.AreEqual(str1, "rURHNTjpUkzmPGqrC2f0DA==");
            Assert.IsNotNull(str1);
        }
        #endregion
        
        #region  AES加密、解密
        /// <summary>
        /// AES加密
        /// </summary>
        [Description("AES加密")]
        [TestMethod]
        public void AES_Encrypt()
        {
            string str = EncryptionUtility.AES_Encrypt("sopcce.com");
            string str12 = EncryptionUtility.AES_Encrypt("sopcce.com", "1234");
            string str24 = EncryptionUtility.AES_Encrypt("sopcce.com", "123456789012345678901234");
            string str48 = EncryptionUtility.AES_Encrypt("sopcce.com", "123456789012345678901234123456789012345678901234");
            Assert.AreEqual(str, "08lZcVgN63Wi1rzQxPgUWw==");
            Assert.AreEqual(str12, "k45Va4jPjZnASLF6BlVL6A==");
            Assert.AreEqual(str24, "4QOpkmOO6xukAG+4PtOdQQ==");
            Assert.AreEqual(str48, "XYVhwmu6d3SaMphEfMepnw==");
            Assert.IsNotNull(str);
            Assert.IsNotNull(str24);
        }
        /// <summary>
        ///  AES解密
        /// </summary>
        [Description(" AES解密")]
        [TestMethod]
        public void AES_Decrypt()
        {
            string str = EncryptionUtility.AES_Decrypt("08lZcVgN63Wi1rzQxPgUWw==");
            string str12 = EncryptionUtility.AES_Decrypt("k45Va4jPjZnASLF6BlVL6A==","1234");
            string str12_1 = EncryptionUtility.AES_Decrypt("k45Va4jPjZnASLF6BlVL6A==", "false");
            string str24 = EncryptionUtility.AES_Decrypt("4QOpkmOO6xukAG+4PtOdQQ==", "123456789012345678901234");
            string str48 = EncryptionUtility.AES_Decrypt("XYVhwmu6d3SaMphEfMepnw==", "123456789012345678901234123456789012345678901234");
            string str48_1 = EncryptionUtility.AES_Decrypt("XYVhwmu6d3SaMphEfMepnw==", "false");
            Assert.AreEqual(str, "sopcce.com");
            Assert.AreEqual(str12, "sopcce.com");
            Assert.AreEqual(str12_1, "k45Va4jPjZnASLF6BlVL6A==");
            Assert.AreEqual(str24, "sopcce.com");
            Assert.AreEqual(str48, "sopcce.com");
            Assert.AreEqual(str48_1, "XYVhwmu6d3SaMphEfMepnw==");
            Assert.IsNotNull(str);
        }
        #endregion

        #region Base64加密、解密
        /// <summary>
        /// 生成指定字符串的SHA256散列值
        /// </summary>
        [Description("生成指定字符串的SHA256散列值")]
        [TestMethod]
        public void Base64_Encode()
        {
            string str = EncryptionUtility.Base64_Encode("sopcce.com");
            Assert.AreEqual(str, "c29wY2NlLmNvbQ==");
            Assert.IsNotNull(str);
        }
        #endregion


        /// <summary>
        /// 生成指定字符串的SHA256散列值
        /// </summary>
        [Description("生成指定字符串的SHA256散列值")]
        [TestMethod]
        public void SHA256Encode123()
        {
            string str = EncryptionUtility.Sha256Encode("sopcce.com");
            Assert.AreEqual(str, "px8GeVUC0uXxgQeW8P4XqoeYw+dTVfV5TFo2pvFFFMA=");
            Assert.IsNotNull(str);
        }

        

        
    }
}
