using System;
using System.Threading.Tasks;
using Xunit;

namespace Hiblogs.Redis.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task SetStringIncrAsync_TestsAsync()
        {
            var number1 = await RedisHelper.GetStringIncrAsync("test");

            await RedisHelper.SetStringIncrAsync("test");
            await RedisHelper.SetStringIncrAsync("test");
            var num3 = await RedisHelper.SetStringIncrAsync("test");
            var number2 = await RedisHelper.GetStringIncrAsync("test");

            Assert.True(number1 == number2 - 3);
        }
    }
}
