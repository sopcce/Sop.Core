using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace ItemDoc.ConsoleBot
{
    class Program
    {
        static void Main(string[] args)
        {
            //启动程序，测试功能
            //初始化Id
            //搜索引擎爬虫，商业爬虫，内容取者爬虫和监控爬虫

            // 所有log 日志全部存储到数据库，方便记录。



            //重试、超时处理、缓存、返回、

            var fallBackPolicy =
                Policy<string>
                    .Handle<Exception>()
                    .Fallback("执行失败，返回Fallback");

            var fallBack = fallBackPolicy.Execute(() =>
            {
                return "zhe shi yi ge ce shi";
            });
            Console.WriteLine(fallBack);

            var politicaWaitAndRetry =
                Policy<string>
                    .Handle<Exception>()
                    .Retry(3, (ex, count) =>
                    {
                        Console.WriteLine("执行失败! 重试次数 {0}", count);
                        Console.WriteLine("异常来自 {0}", ex.GetType().Name);
                    });

            var mixedPolicy = Policy.Wrap(fallBackPolicy, politicaWaitAndRetry);
            var mixedResult = mixedPolicy.Execute(ThrowException);
            Console.WriteLine($"执行结果: {mixedResult}");


        }
        static string ThrowException()
        {
            throw new Exception();
        }


        static int Compute()
        {
            var a = 0;
            return 1 / a;
        }
    }
}
