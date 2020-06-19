﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.Extensions.Configuration;

namespace NetPro.RedisManager
{
    internal class CsRedisManager : IRedisManager
    {
        private readonly RedisCacheOption _option;
        private readonly ConnectionMultiplexer _connection;
        public CsRedisManager(RedisCacheOption option, ConnectionMultiplexer connection)
        {
            _connection = connection;
            _option = option;
        }

        public T GetOrCreate<T>(string key, Func<T> func = null, int expiredTime = -1)
        {
            Common.CheckKey(key);
            var result = RedisHelper.Get<T>(key);
                                                             
            if (result == null)
            {
                if (func == null || func.Invoke() == null) return default(T);
                RedisHelper.Set(key, func.Invoke(), expiredTime);

                return func.Invoke();
            }

            return result;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<T> func = null, int expiredTime = -1)
        {
            Common.CheckKey(key);
            var result = await RedisHelper.GetAsync<T>(key);

            if (result == null)
            {
                if (func == null || func.Invoke() == null) return default(T);
                await RedisHelper.SetAsync(key, func.Invoke(), expiredTime);

                return func.Invoke();
            }

            return result;
        }

        public object GetByLuaScript(string script, object obj)
        {
            var result = RedisHelper.Eval(script, key: "lock_name", args: obj);
            return result;
        }

        public T GetDistributedLock<T>(string resource, int timeoutSeconds, Func<T> func)
        {
            if (timeoutSeconds <= 0 || string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentException($"The timeout is not valid with a distributed lock object--key:{resource}--expiryTime--{timeoutSeconds}");
            }
            using (var lockObject = RedisHelper.Lock($"Lock:{resource?.Trim()}", timeoutSeconds))
            {
                if (lockObject == null)
                {
                    return default;
                }
                var result = func();
                lockObject.Unlock();
                return result;
            }
        }

        /// <summary>
        /// 获得使用原生stackexchange.redis的能力，用于pipeline (stackExchange.redis专用)
        /// </summary>
        /// <returns></returns>
        public IDatabase GetIDatabase()
        {
            Console.WriteLine("当前CSRedis默认驱动下调用Stackexchange方法");
            return _connection.GetDatabase();
        }

        public T HGet<T>(string key, string field)
        {
            Common.CheckKey(key);
            return RedisHelper.HGet<T>(key, field);
        }

        public async Task<T> HGetAsync<T>(string key, string field)
        {
            Common.CheckKey(key);

            return await RedisHelper.HGetAsync<T>(key, field);
        }

        public bool HSet<T>(string key, string field, T value, int expirationMinute = 1)
        {
            bool isSet = RedisHelper.HSet(key, field, value);
            if (isSet && expirationMinute > 0)
                RedisHelper.Expire(key, TimeSpan.FromMinutes(expirationMinute));
            return isSet;
        }

        public async Task<bool> HSetAsync<T>(string key, string field, T value, int expirationMinute = 1)
        {
            bool isSet = await RedisHelper.HSetAsync(key, field, value);
            if (isSet && expirationMinute > 0)
                await RedisHelper.ExpireAsync(key, TimeSpan.FromMinutes(expirationMinute));
            return isSet;
        }

        public bool IsSet(string key)
        {
            return RedisHelper.Exists(key);
        }

        public bool Remove(string key)
        {
            return RedisHelper.Del(key) > 0 ? true : false;
        }

        public bool Remove(string[] keys)
        {
            return RedisHelper.Del(keys) > 0 ? true : false;
        }

        public bool Set(string key, object data, int cacheTime = -1)
        {
            return RedisHelper.Set(key, data, cacheTime);
        }

        public async Task<bool> SetAsync(string key, object data, int cacheTime)
        {
            return await RedisHelper.SetAsync(key, data, cacheTime);
        }

        public bool ZAdd<T>(string key, T obj, decimal score)
        {
            return RedisHelper.ZAdd(key, (score, obj)) > 0 ? true : false;
        }

        public List<T> ZRange<T>(string key)
        {
            return RedisHelper.ZRange<T>(key, 0, -1)?.ToList();
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Subscribe<T>(params (string, Action<CSRedisClient.SubscribeMessageEventArgs>)[] channels)
        {
            RedisHelper.Subscribe(channels);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        public void Publish<T>(string key, Func<string> func)
        {
            RedisHelper.Publish(key, func.Invoke());
        }

    }
}
