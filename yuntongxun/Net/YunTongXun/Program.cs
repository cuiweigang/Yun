using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunTongXun
{
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using CCPRestSDK;

    class Program
    {
        static void Main(string[] args)
        {
            //AccountInfo();
            //SendSms();
            //GetSubAccounts();
            // TemplateSms();
            VoiceVerify();
            

            Console.ReadKey();
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        public static void SendSms()
        {
            var api = GetApi();
            var result = api.SendSMS("手机号", "你好,我是测试用的,不要扣我钱!");
            Console.WriteLine(getDictionaryData(result));
        }

        /// <summary>
        /// 祝账户信息
        /// </summary>
        public static void AccountInfo()
        {
            var api = GetApi();
            Dictionary<string, object> retData = api.QueryAccountInfo();
            var ret = getDictionaryData(retData);
            Console.WriteLine(ret);
        }

        /// <summary>
        /// 获取子账户信息
        /// </summary>
        public static void GetSubAccounts()
        {
            string ret = null;
            var api = GetApi();

            try
            {
                if (true)
                {
                    Dictionary<string, object> retData = api.GetSubAccounts(0, 100);
                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {
                Console.Write(ret);
            }
        }

        /// <summary>
        /// 模版短信
        /// </summary>
        public static void TemplateSms()
        {
            string ret = null;

            CCPRestSDK api = new CCPRestSDK();

            bool isInit = api.init("sandboxapp.cloopen.com", "8883");
            api.setAccount("8a48b55146472691014689aa664f2", "f1258a0cc1a7492aa55e9eafb4b05f");
            api.setAppId("8a48b5514647269101468a07a0d428c");

            try
            {
                if (isInit)
                {
                    Dictionary<string, object> retData = api.SendTemplateSMS("手机号", "1783", new string[] { "123456,2" });
                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {
                Console.WriteLine(ret);
            }
        }

        /// <summary>
        /// 语音验证码
        /// </summary>
        public static void VoiceVerify()
        {
            string ret = null;

            CCPRestSDK api = new CCPRestSDK();

            bool isInit = api.init("sandboxapp.cloopen.com", "8883");
            api.setAccount("8a48b55146472691014689aa664f2", "f1258a0cc1a7492aa55e9eafb4b05");
            api.setAppId("8a48b5514647269101468a07a0d42");

            try
            {
                if (isInit)
                {
                    Dictionary<string, object> retData = api.VoiceVerify("手机号", "123456", null, "3", null);
                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {
                Console.WriteLine(ret);
            }
        }



        private static CCPRestSDK GetApi()
        {
            CCPRestSDK api = new CCPRestSDK();

            bool isInit = api.init("sandboxapp.cloopen.com", "8883");
            api.setAccount("8a48b55146472691014689aa664f2", "f1258a0cc1a7492aa55e9eafb4b05");
            api.setAppId("aaf98f8946471bb0014689b7fa4927");
            api.setSubAccount("aaf98f8946471bb0014689b7fa6327", "ffbd6baa633346edb04b8159c56049", "81617100000001", "kalyrn0i");

            return api;
        }


        private static string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key + "={";
                    ret += getDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else if (item.Value != null && item.Value.GetType() == typeof(List<object>))
                {
                    var list = item.Value as List<object>;

                    ret += item.Key + "[";

                    foreach (var i in list)
                    {
                        if (i is Dictionary<string, object>)
                        {
                            ret += getDictionaryData((Dictionary<string, object>)i);
                        }
                    }

                    ret += "]";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5String"></param>
        /// <returns></returns>
        private static string Md5(string md5String)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(md5String), 0, md5String.Length);
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}
