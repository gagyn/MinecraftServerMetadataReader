﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinecraftServerStatus.Integrations.MongoDB
{
    public interface ISession
    {
        Task AddAsync<T>(IEnumerable<T> items) where T : Entity;
        Task AddAsync<T>(T item) where T : Entity;
        Task DeleteAsync<T>(T item) where T : Entity;
        Task DeleteAsync<T, TW>(T item, Func<T, TW> finder) where T : Entity;
        IEnumerable<T> Get<T>() where T : Entity;
        Task<IEnumerable<T>> GetAsync<T>() where T : Entity;
        Task ReplaceByTypeAsync<T>(T item) where T : Entity;
        Task ReplaceAsync<T>(T oldItem, T newItem) where T : Entity;
        Task UpdateAsync<T>(T item) where T : Entity;
    }
}