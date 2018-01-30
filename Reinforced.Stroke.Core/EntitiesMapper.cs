using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Reinforced.Stroke.Core
{
    /// <summary>
    /// Mapper that helps to determine table/column name by type and property using EF's DbContext
    /// </summary>
    internal static class EntitiesMapper
    {
        private static readonly Dictionary<Type, Dictionary<Type, MappingCacheEntry>> _tableNamesCache = new Dictionary<Type, Dictionary<Type, MappingCacheEntry>>();
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();

        private static MappingCacheEntry GetCachedMapping(Type contextType, Type t)
        {
            if (!_tableNamesCache.ContainsKey(contextType))
            {
                lock (_locker)
                {
                    if (!_tableNamesCache.ContainsKey(contextType))
                    {
                        _tableNamesCache[contextType] = new Dictionary<Type, MappingCacheEntry>();
                    }
                }
            }

            var cached = _tableNamesCache[contextType];

            if (!cached.ContainsKey(t))
            {
                lock (_locker2)
                {
                    if (cached.ContainsKey(t)) return cached[t];
                    cached[t] = new MappingCacheEntry();
                }
            }
            return cached[t];
        }

        private static EntityType GetMapping(DbContext context, Type t)
        {
            return context.Model.FindEntityType(t).AsEntityType();
        }

        /// <summary>
        /// Retrieves table name by type of particular context
        /// </summary>
        /// <param name="context">EF DbContext</param>
        /// <param name="t">Mapped type</param>
        /// <returns>Table name</returns>
        public static string GetTableName<T>(this T context, Type t) where T : DbContext
        {
            var cm = GetCachedMapping(typeof(T), t);
            var tName = cm.GetTableName(t);
            if (string.IsNullOrEmpty(tName))
            {
                tName = cm.GetTableName(t, GetMapping(context, t));
            }
            return tName;
        }

        /// <summary>
        /// Retrieves DB column name for Type's property
        /// </summary>
        /// <param name="context">EF DbContext</param>
        /// <param name="t">Mapped type</param>
        /// <param name="propertyName"></param>
        /// <returns>Column name</returns>
        public static string GetColumnName<T>(this T context, Type t, string propertyName) where T : DbContext
        {
            var cm = GetCachedMapping(typeof(T), t);
            var fName = cm.GetFieldName(t, propertyName);
            if (string.IsNullOrEmpty(fName))
            {
                fName = cm.GetFieldName(t, propertyName, GetMapping(context, t));
            }
            return fName;
        }
    }
}