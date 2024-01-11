﻿using System;
using System.Threading;

namespace Sop.Core.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SnowflakeIdUtility
    {
        private static long machineId;//机器ID
        private static long datacenterId = 0L;//数据ID
        private static long sequence = 0L;//计数从零开始

        private static long twepoch = 687888001020L; //唯一时间随机量

        private static long machineIdBits = 5L; //机器码字节数
        private static long datacenterIdBits = 5L;//数据字节数
        private static long maxMachineId = -1L ^ -1L << (int)machineIdBits; //最大机器ID
        private static long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits);//最大数据ID

        private static long sequenceBits = 12L; //计数器字节数，12个字节用来保存计数码        
        private static long machineIdShift = sequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private static long datacenterIdShift = sequenceBits + machineIdBits;
        private static long timestampLeftShift = sequenceBits + machineIdBits + datacenterIdBits; //时间戳左移动位数就是机器码+计数器总字节数+数据字节数

        /// <summary>
        ///一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        /// </summary>
        private static long sequenceMask = -1L ^ -1L << (int)sequenceBits;
        private static long lastTimestamp = -1L;//最后时间戳

        private static object syncRoot = new object();//加锁对象
        static SnowflakeIdUtility snowflake;

        public static SnowflakeIdUtility Instance()
        {
            if (snowflake == null)
                snowflake = new SnowflakeIdUtility();
            return snowflake;
        }

        public SnowflakeIdUtility()
        {
            Snowflakes(0L, -1);
        }

        public SnowflakeIdUtility(long machineId)
        {
            Snowflakes(machineId, -1);
        }

        public SnowflakeIdUtility(long machineId, long datacenterId)
        {
            Snowflakes(machineId, datacenterId);
        }

        private void Snowflakes(long machineId, long datacenterId)
        {
            if (machineId >= 0)
            {
                if (machineId > maxMachineId)
                {
                    throw new Exception("机器码ID非法");
                }
                SnowflakeIdUtility.machineId = machineId;
            }
            if (datacenterId >= 0)
            {
                if (datacenterId > maxDatacenterId)
                {
                    throw new Exception("数据中心ID非法");
                }
                SnowflakeIdUtility.datacenterId = datacenterId;
            }
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns>毫秒</returns>
        private static long GetTimestamp()
        {
            //让他2000年开始
            return (long)(DateTime.UtcNow - new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetTimestamp();
            int count = 0;
            while (timestamp <= lastTimestamp)//这里获取新的时间,可能会有错,这算法与comb一样对机器时间的要求很严格
            {
                count++;
                if (count > 10)
                    throw new Exception("机器的时间可能不对");
                Thread.Sleep(1);
                timestamp = GetTimestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            lock (syncRoot)
            {
                long timestamp = GetTimestamp();
                if (SnowflakeIdUtility.lastTimestamp == timestamp)
                { //同一微妙中生成ID
                    sequence = (sequence + 1) & sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (sequence == 0)
                    {
                        //一微秒内产生的ID计数已达上限，等待下一微秒
                        timestamp = GetNextTimestamp(SnowflakeIdUtility.lastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID
                    sequence = 0L;
                }
                if (timestamp < lastTimestamp)
                {
                    throw new Exception("时间戳比上一次生成ID时时间戳还小，故异常");
                }
                SnowflakeIdUtility.lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long Id = ((timestamp - twepoch) << (int)timestampLeftShift)
                          | (datacenterId << (int)datacenterIdShift)
                          | (machineId << (int)machineIdShift)
                          | sequence;
                return Id;
            }
        }

    }
}
