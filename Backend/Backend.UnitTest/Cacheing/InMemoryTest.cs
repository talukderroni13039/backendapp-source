using Backend.Infrastructure.Cacheing.InMemory.Backend.Infrastructure.Cacheing.InMemory;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.UnitTest.Cacheing
{
    public class InMemoryTests
    {

        private InMemoryCache _cache;
        [SetUp]
        public void Setup()
        {
            _cache = new InMemoryCache(new MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()));
        }


        [TestCase("testKey", "testValue")]
        [TestCase("", "testValue")]
        public async Task SetAndGet_Data(string key ,string value )
        {
            // Act
            await _cache.SetData(key, value, DateTimeOffset.UtcNow.AddMinutes(10));
            var retrievedValue = await _cache.GetData<string>(key);

            // Assert
            Assert.AreEqual(value, retrievedValue);
        }
        [TestCase("NonKey")]
        public async Task GetData_NonexistentKey_ShouldReturnNull(string key)
        {
            // Act
            var retrievedValue = await _cache.GetData<string>(key);
            // Assert
            Assert.IsNull(retrievedValue);
        }



    }
}
