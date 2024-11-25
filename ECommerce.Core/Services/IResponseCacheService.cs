﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key,object response, TimeSpan timeToLive);
        Task<string> GetCacheResponseAsync(string key);
    }
}
