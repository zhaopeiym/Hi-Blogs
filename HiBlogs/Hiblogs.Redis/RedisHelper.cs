using HiBlogs.Definitions.Config;
using HiBlogs.Infrastructure.Extensions;
using Serilog;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Hiblogs.Redis
{
    public class RedisHelper
    {
        /// <summary>
        /// ConnectionMultiplexer是线程安全的，且是昂贵的。所以我们尽量重用。 
        /// </summary>
        public readonly static ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(AppConfig.RedisConnection);

        /// <summary>
        /// 存储字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">有效期</param>
        /// <returns></returns>
        public static async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            try
            {
                var db = connectionMultiplexer.GetDatabase();
                await db.StringSetAsync(RedisTypePrefix.String.GetDescription() + key, value, expiry);
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string key)
        {
            try
            {
                var db = connectionMultiplexer.GetDatabase();
                return await db.StringGetAsync(RedisTypePrefix.String.GetDescription() + key);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 计数器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">只有第一次设置有效期生效</param>
        /// <returns></returns>
        public static async Task<long> SetStringIncrAsync(string key, long value, TimeSpan? expiry = null)
        {
            try
            {
                key = RedisTypePrefix.String.GetDescription() + key;
                var db = connectionMultiplexer.GetDatabase();

                var nubmer = await db.StringIncrementAsync(key, value);
                if (nubmer == 1 && expiry != null)//只有第一次设置有效期（防止覆盖）
                    await db.KeyExpireAsync(key, expiry);//设置有效期
                return nubmer;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 计数器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<long> SetStringIncrAsync(string key, TimeSpan? expiry = null)
        {
            return await SetStringIncrAsync(key, 1, expiry);
        }

        /// <summary>
        /// 读取计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> GetStringIncrAsync(string key)
        {
            var value = await GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            else
            {
                return long.Parse(value);
            }
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExistsAsync(string key, RedisTypePrefix redisTypePrefix)
        {
            var db = connectionMultiplexer.GetDatabase();
            return await db.KeyExistsAsync(redisTypePrefix.GetDescription() + key);
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteKeyAsync(string key, RedisTypePrefix redisTypePrefix)
        {
            var db = connectionMultiplexer.GetDatabase();
            return await db.KeyDeleteAsync(redisTypePrefix.GetDescription() + key);
        }
    }
}
