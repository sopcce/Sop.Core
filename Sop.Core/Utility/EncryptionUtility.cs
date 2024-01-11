using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Sop.Core.Utility
{
    /// <summary>
    /// 数据加密类，提供各种加密解密算法及防篡改散列算法
    /// </summary>·
    public static class EncryptionUtility
    {

        #region 私有属性

        private static readonly string EncryptKey = "ddsa234sf./zxc/asdsdh$%(&*&(@f";
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private static readonly int BufferSize = 128 * 1024;
        /// <summary>
        /// 密钥salt
        /// </summary>
        private static readonly byte[] Salt = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };
        /// <summary>
        /// 初始化向量
        /// </summary>
        private static readonly byte[] Iv = { 134, 216, 7, 36, 88, 164, 91, 227, 174, 76, 191, 197, 192, 154, 200, 248 };

        /// <summary>
        /// 初始化并返回对称加密算法
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);
            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256; 
            sma.Key = pdb.GetBytes(32); 
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }


        /// <summary>
        /// 随机密钥
        /// </summary>
        private static readonly byte[] Keys = new byte[]
        {
            239,171,86,20,144,52,205,182,22,38,123,137,215
        };



        #endregion

        #region 数据的防篡改验证

        /// <summary>
        ///  数据的防篡改验证码(散列值)生成函数，采用非通用的多种散列值混合算法
        /// </summary>
        /// <param name="sValue">原始数据</param>
        /// <param name="activeKey">动态附加数据。如：用户的IP地址，这样可防止验证信息被盗用</param>
        /// <returns>生成的防篡改验证码</returns>
        public static string EncryptString(string sValue, string activeKey)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(activeKey))
            {
                stringBuilder.Append(activeKey);
            }
            stringBuilder.Append(sValue);
            stringBuilder.Append(EncryptKey);
            var srcValue = stringBuilder.ToString();
            var text = Md5Encode(srcValue, 32);
            var value = Sha1Encode(srcValue);
            stringBuilder.Remove(0, stringBuilder.Length);
            int length = text.Length;
            int num = 0;
            for (int i = 1; i < length; i++)
            {
                if (i % 3 == 0)
                {
                    stringBuilder.Append(text.Substring(i, 1));
                    num++;
                    if (num >= 5)
                    {
                        break;
                    }
                }
            }
            stringBuilder.Append(value);
            num = 0;
            for (int j = length - 1; j >= 1; j--)
            {
                if (j % 3 != 0)
                {
                    stringBuilder.Append(text.Substring(j, 1));
                    num++;
                    if (num >= 5)
                    {
                        break;
                    }
                }
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 使用加密服务提供程序实现加密生成 MachineKey做密钥
        /// </summary>
        /// <param name="length"></param>
        /// <returns>16进制格式字符串</returns>
        public static string CreateMachineKey(int length)
        {
            // 要返回的字符格式为16进制,byte最大值255
            // 需要2个16进制数保存1个byte,因此除2
            byte[] random = new byte[length / 2];

            // 使用加密服务提供程序 (CSP) 提供的实现来实现加密随机数生成器 (RNG)
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // 用经过加密的强随机值序列填充字节数组
            rng.GetBytes(random);

            StringBuilder machineKey = new StringBuilder(length);
            for (int i = 0; i < random.Length; i++)
            {
                machineKey.Append(string.Format("{0:X2}", random[i]));
            }
            return machineKey.ToString();
        }
        #endregion

        #region MD5加密

        /// <summary>
        /// 生成指定字符串的MD5散列值，返回大写串
        /// </summary>
        /// <param name="srcValue">源字符串</param>
        /// <param name="encodeType">type类型：16位还是32位</param>
        /// <param name="encoding"></param>
        /// <returns>MD5值</returns>
        public static string Md5Encode(string srcValue, int encodeType, EncodingEnum encoding)
        {
            byte[] bytes;
            switch (encoding)
            {
                case EncodingEnum.Utf8:
                    bytes = Encoding.UTF8.GetBytes(srcValue);
                    break;
                case EncodingEnum.Gb2312:
                    bytes = Encoding.GetEncoding(936).GetBytes(srcValue);
                    break;
                default:
                    bytes = Encoding.Default.GetBytes(srcValue);
                    break;
            }
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] value = mD5CryptoServiceProvider.ComputeHash(bytes);
            if (encodeType == 16)
            {
                return BitConverter.ToString(value).Replace("-", "").ToUpper().Substring(8, 16);
            }
            return BitConverter.ToString(value).Replace("-", "").ToUpper();
        }
        /// <summary>
        /// 生成指定字符串的MD5散列值，返回大写串(默认default)
        /// </summary>
        /// <param name="srcValue">源字符串</param>
        /// <param name="encodeType">type类型：16位还是32位</param>
        /// <returns>MD5值</returns>
        public static string Md5Encode(string srcValue, int encodeType = 32)
        {
            return Md5Encode(srcValue, encodeType, EncodingEnum.Default);
        }
        #endregion

        #region SHA加密

        /// <summary>
        /// 生成指定字符串的SHA1散列值，返回大写串
        /// </summary>
        /// <param name="srcValue">源字符串字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>SHA1值</returns>
        public static string Sha1Encode(string srcValue, EncodingEnum encoding)
        {
            byte[] array;
            switch (encoding)
            {
                case EncodingEnum.Utf8:
                    array = Encoding.UTF8.GetBytes(srcValue);
                    break;
                case EncodingEnum.Gb2312:
                    array = Encoding.GetEncoding(936).GetBytes(srcValue);
                    break;
                default:
                    array = Encoding.Default.GetBytes(srcValue);
                    break;
            }
            HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
            array = hashAlgorithm.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var b in array)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString().ToUpper();
        }
        /// <summary>
        /// 生成指定字符串的SHA1散列值，返回大写串
        /// </summary>
        /// <param name="srcValue">源字符串字符串</param>
        /// <returns>SHA1值</returns>
        public static string Sha1Encode(string srcValue)
        {
            return Sha1Encode(srcValue, EncodingEnum.Default);
        }
        /// <summary>
        /// 生成指定字符串的SHA256散列值
        /// </summary>
        /// <param name="srcValue">源字符串</param>
        /// <returns>SHA256值</returns>
        public static string Sha256Encode(string srcValue)
        {
            SHA256 sHa = new SHA256Managed();
            byte[] inArray = sHa.ComputeHash(Encoding.Default.GetBytes(srcValue));
            sHa.Clear();
            return Convert.ToBase64String(inArray);
        }
        /// <summary>
        /// 生成指定字符串的SHA384散列值
        /// </summary>
        /// <param name="srcValue">源字符串</param>
        /// <returns>SHA384值</returns>
        public static string Sha384Encode(string srcValue)
        {
            SHA384 sHa = new SHA384Managed();
            var inArray = sHa.ComputeHash(Encoding.Default.GetBytes(srcValue));
            sHa.Clear();
            return Convert.ToBase64String(inArray);
        }
        /// <summary>
        /// 生成指定字符串的SHA512散列值(不可逆)
        /// </summary>
        /// <param name="srcValue">源字符串</param>
        /// <returns>SHA512值</returns>
        public static string Sha512Encode(string srcValue)
        {
            SHA512 sHa = new SHA512Managed();
            byte[] inArray = sHa.ComputeHash(Encoding.Default.GetBytes(srcValue));

            sHa.Clear();
            return Convert.ToBase64String(inArray);
        }


        #endregion

        #region DES加密、解密
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string DES_Encrypt(string encryptString, string encryptKey)
        {
            string result;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] keys = Keys;
                byte[] bytes2 = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dEsCryptoServiceProvider = new DESCryptoServiceProvider();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dEsCryptoServiceProvider.CreateEncryptor(bytes, keys), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes2, 0, bytes2.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    result = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch
            {
                result = encryptString;
            }
            return result;
        }
        /// <summary>
        ///  DES解密字符串
        /// </summary>
        /// <param name="decryptString"> 待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同 </param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DES_Decrypt(string decryptString, string decryptKey)
        {
            string result;
            try
            {
                var bytes = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                var keys = Keys;
                var array = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider dEsCryptoServiceProvider = new DESCryptoServiceProvider();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, dEsCryptoServiceProvider.CreateDecryptor(bytes, keys), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(array, 0, array.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    result = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch
            {
                result = decryptString;
            }
            return result;
        }

        #endregion

        #region AES加密、解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptStr">待加密的字符串</param>
        /// <param name="key">加密密钥</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string AES_Encrypt(string encryptStr, string key)
        {
            try
            {
                Byte[] bKey = new Byte[32];
                string str = key.PadRight(bKey.Length);
                byte[] keyArray = Encoding.UTF8.GetBytes(str);
                Array.Copy(keyArray, bKey, bKey.Length);

                byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
                var aes = new AesManaged()
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 256,
                    Key = bKey
                };
                var resultArray = aes.CreateEncryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                aes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                return encryptStr;
            }

        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">待解密的字符串</param>
        /// <param name="key">加密密钥,Key是24位</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string AES_Decrypt(string decryptStr, string key)
        {

            try
            {
                Byte[] bKey = new Byte[32];
                string str = key.PadRight(bKey.Length);
                byte[] keyArray = Encoding.UTF8.GetBytes(str);
                Array.Copy(keyArray, bKey, bKey.Length);

                byte[] toEncryptArray = Convert.FromBase64String(decryptStr);

                var aes = new AesManaged()
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 256,
                    Key = bKey
                };
                byte[] resultArray = aes.CreateDecryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                aes.Clear();
                return Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception ex)
            {
                //
                Console.WriteLine(ex.Message);
                return decryptStr;
            }

        }

        #endregion

        #region Base64加密、解密

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">待编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        public static string Base64_Encode(string str)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="str">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string Base64_Decode(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(decbuff);
        }



        #endregion

        #region RSA 加密解密 



        /// <summary>
        /// RSA 的密钥产生
        /// </summary>
        /// <param name="xmlKeys">xml产生私钥</param>
        /// <param name="xmlPublicKey">xml公钥</param>
        public static void RsaKey(out string xmlKeys, out string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }


        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="xmlPublicKey">xml公钥</param>
        /// <param name="mStrEncryptString">加密字符串</param>
        /// <returns>加密 字符串</returns>
        public static string RsaEncrypt(string xmlPublicKey, string mStrEncryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            var plainTextBArray = (new UnicodeEncoding()).GetBytes(mStrEncryptString);
            var cypherTextBArray = rsa.Encrypt(plainTextBArray, false);
            var result = Convert.ToBase64String(cypherTextBArray);
            return result;

        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="xmlPublicKey">xml公钥</param>
        /// <param name="encryptString">加密byte[]数组</param>
        /// <returns></returns>
        public static string RsaEncrypt(string xmlPublicKey, byte[] encryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            var cypherTextBArray = rsa.Encrypt(encryptString, false);
            var result = Convert.ToBase64String(cypherTextBArray);
            return result;

        }


        #region RSA解密

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="xmlPrivateKey">私有密钥</param>
        /// <param name="mStrDecryptString">加密字符串</param>
        /// <returns></returns>
        public static string RsaDecrypt(string xmlPrivateKey, string mStrDecryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            var plainTextBArray = Convert.FromBase64String(mStrDecryptString);
            var dypherTextBArray = rsa.Decrypt(plainTextBArray, false);
            var result = (new UnicodeEncoding()).GetString(dypherTextBArray);
            return result;

        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="xmlPrivateKey">私有密钥</param>
        /// <param name="decryptString">byte[]数组解密</param>
        /// <returns></returns>
        public static string RsaDecrypt(string xmlPrivateKey, byte[] decryptString)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            var dypherTextBArray = rsa.Decrypt(decryptString, false);
            var result = (new UnicodeEncoding()).GetString(dypherTextBArray);
            return result;

        }
        #endregion

        #endregion

        #region RSA数字签名 

        #region 获取Hash描述表 
        /// <summary>
        /// 获取Hash描述表 
        /// </summary>
        /// <param name="mStrSource"></param>
        /// <param name="hashData"></param>
        /// <returns></returns>
        public static bool GetHash(string mStrSource, ref byte[] hashData)
        {
            //从字符串中取得Hash描述 
            HashAlgorithm md5 = HashAlgorithm.Create("MD5");
            var buffer = Encoding.GetEncoding("GB2312").GetBytes(mStrSource);
            if (md5 != null)
                hashData = md5.ComputeHash(buffer);

            return true;
        }

        /// <summary>
        /// 获取Hash描述表 
        /// </summary>
        /// <param name="mStrSource"></param>
        /// <param name="strHashData"></param>
        /// <returns></returns>
        public static bool GetHash(string mStrSource, ref string strHashData)
        {
            if (strHashData == null)
                throw new ArgumentNullException(nameof(strHashData));
            //从字符串中取得Hash描述 
            HashAlgorithm md5 = HashAlgorithm.Create("MD5");
            var buffer = Encoding.GetEncoding(936).GetBytes(mStrSource);
            if (md5 != null)
            {
                var hashData = md5.ComputeHash(buffer);
                strHashData = Convert.ToBase64String(hashData);
            }
            return true;

        }

        /// <summary>
        /// 获取Hash描述表 获取Hash描述表 
        /// </summary>
        /// <param name="objFile"></param>
        /// <param name="hashData"></param>
        /// <returns></returns>
        public static bool GetHash(FileStream objFile, ref byte[] hashData)
        {
            using (HashAlgorithm md5 = HashAlgorithm.Create("MD5"))
            {
                if (md5 != null)
                    hashData = md5.ComputeHash(objFile);
            }
            return true;
        }



        /// <summary>
        /// 获取Hash描述表 
        /// </summary>
        /// <param name="objFile"></param>
        /// <param name="strHashData"></param>
        /// <returns></returns>
        public static bool GetHash(FileStream objFile, ref string strHashData)
        {
            if (strHashData == null) throw new ArgumentNullException(nameof(strHashData));

            //从文件中取得Hash描述 
            HashAlgorithm md5 = HashAlgorithm.Create("MD5");
            if (md5 != null)
            {
                var hashData = md5.ComputeHash(objFile);
                objFile.Close();

                strHashData = Convert.ToBase64String(hashData);
            }

            return true;

        }
        #endregion

        #region RSA签名 
        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="pStrKeyPrivate"></param>
        /// <param name="hashbyteSignature"></param>
        /// <param name="encryptedSignatureData"></param>
        /// <returns></returns>

        public static bool SignatureFormatter(string pStrKeyPrivate, byte[] hashbyteSignature, ref byte[] encryptedSignatureData)
        {
            if (encryptedSignatureData == null) throw new ArgumentNullException(nameof(encryptedSignatureData));

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            return true;

        }


        /// <summary>
        ///  RSA签名
        /// </summary>
        /// <param name="pStrKeyPrivate"></param>
        /// <param name="hashbyteSignature"></param>
        /// <param name="mStrEncryptedSignatureData"></param>
        /// <returns></returns>
        public static bool SignatureFormatter(string pStrKeyPrivate, byte[] hashbyteSignature, ref string mStrEncryptedSignatureData)
        {
            if (mStrEncryptedSignatureData == null) throw new ArgumentNullException(nameof(mStrEncryptedSignatureData));
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            var encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            mStrEncryptedSignatureData = Convert.ToBase64String(encryptedSignatureData);

            return true;

        }

        /// <summary>
        /// RSA签名 
        /// </summary>
        /// <param name="pStrKeyPrivate"></param>
        /// <param name="mStrHashbyteSignature"></param>
        /// <param name="encryptedSignatureData"></param>
        /// <returns></returns>
        public static bool SignatureFormatter(string pStrKeyPrivate, string mStrHashbyteSignature, ref byte[] encryptedSignatureData)
        {
            if (encryptedSignatureData == null) throw new ArgumentNullException(nameof(encryptedSignatureData));
            var hashbyteSignature = Convert.FromBase64String(mStrHashbyteSignature);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            return true;

        }



        /// <summary>
        /// RSA签名 
        /// </summary>
        /// <param name="pStrKeyPrivate"></param>
        /// <param name="mStrHashbyteSignature"></param>
        /// <param name="mStrEncryptedSignatureData"></param>
        /// <returns></returns>
        public static bool SignatureFormatter(string pStrKeyPrivate, string mStrHashbyteSignature, ref string mStrEncryptedSignatureData)
        {
            if (mStrEncryptedSignatureData == null) throw new ArgumentNullException(nameof(mStrEncryptedSignatureData));
            var hashbyteSignature = Convert.FromBase64String(mStrHashbyteSignature);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            var encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            mStrEncryptedSignatureData = Convert.ToBase64String(encryptedSignatureData);

            return true;

        }
        #endregion

        #region RSA 签名验证 

        /// <summary>
        /// RSA 签名验证
        /// </summary>
        /// <param name="strKeyPublic"></param>
        /// <param name="hashbyteDeformatter"></param>
        /// <param name="deformatterData"></param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string strKeyPublic, byte[] hashbyteDeformatter, byte[] deformatterData)
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(strKeyPublic);
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");
            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// RSA 签名验证
        /// </summary>
        /// <param name="pStrKeyPublic"></param>
        /// <param name="pStrHashbyteDeformatter"></param>
        /// <param name="deformatterData"></param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string pStrKeyPublic, string pStrHashbyteDeformatter, byte[] deformatterData)
        {
            var hashbyteDeformatter = Convert.FromBase64String(pStrHashbyteDeformatter);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// RSA 签名验证
        /// </summary>
        /// <param name="pStrKeyPublic"></param>
        /// <param name="hashbyteDeformatter"></param>
        /// <param name="pStrDeformatterData"></param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string pStrKeyPublic, byte[] hashbyteDeformatter, string pStrDeformatterData)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            var deformatterData = Convert.FromBase64String(pStrDeformatterData);

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// RSA 签名验证
        /// </summary>
        /// <param name="pStrKeyPublic"></param>
        /// <param name="pStrHashbyteDeformatter"></param>
        /// <param name="pStrDeformatterData"></param>
        /// <returns></returns>
        public static bool SignatureDeformatter(string pStrKeyPublic, string pStrHashbyteDeformatter, string pStrDeformatterData)
        {
            var hashbyteDeformatter = Convert.FromBase64String(pStrHashbyteDeformatter);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            var deformatterData = Convert.FromBase64String(pStrDeformatterData);

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        #endregion


        #endregion


        #region TripleDES加密
        /// <summary>
        /// TripleDES加密
        /// </summary>
        public static string TripleDesEncrypting(string strSource)
        {
            try
            {
                byte[] bytIn = Encoding.Default.GetBytes(strSource);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };  //定义偏移量
                TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();
                tripleDes.IV = IV;
                tripleDes.Key = key;
                ICryptoTransform encrypto = tripleDes.CreateEncryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                byte[] bytOut = ms.ToArray();
                return System.Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
            }
        }
        #endregion

        #region TripleDES解密
        /// <summary>
        /// TripleDES解密
        /// </summary>
        public static string TripleDesDecrypting(string source)
        {
            try
            {
                byte[] bytIn = System.Convert.FromBase64String(source);
                byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
                byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };   //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateDecryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader strd = new StreamReader(cs, Encoding.Default);
                return strd.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
            }
        }
        #endregion

        #region 文件加密、解密


        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void EncryptFile(string inFile, string outFile, string password)
        {
            using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.Open(outFile, FileMode.OpenOrCreate))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, Salt))
            {
                algorithm.IV = Iv;
                using (CryptoStream cryptoStream = new CryptoStream(outFileStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] bytes = new byte[BufferSize];
                    int readSize = -1;
                    while ((readSize = inFileStream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cryptoStream.Write(bytes, 0, readSize);
                    }
                    cryptoStream.Flush();
                }
            }
        }

        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void DecryptFile(string inFile, string outFile, string password)
        {
            using (FileStream inFileStream = File.OpenRead(inFile), outFileStream = File.OpenWrite(outFile))
            using (SymmetricAlgorithm algorithm = CreateRijndael(password, Salt))
            {
                algorithm.IV = Iv;
                using (CryptoStream cryptoStream = new CryptoStream(inFileStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] bytes = new byte[BufferSize];
                    int readSize = -1;
                    int numReads = (int)(inFileStream.Length / BufferSize);
                    int slack = (int)(inFileStream.Length % BufferSize);
                    for (int i = 0; i < numReads; ++i)
                    {
                        readSize = cryptoStream.Read(bytes, 0, bytes.Length);
                        outFileStream.Write(bytes, 0, readSize);
                    }
                    if (slack > 0)
                    {
                        readSize = cryptoStream.Read(bytes, 0, (int)slack);
                        outFileStream.Write(bytes, 0, readSize);
                    }
                    outFileStream.Flush();
                }
            }
        }


        #endregion
 
    }
}

