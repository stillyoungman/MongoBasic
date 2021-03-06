﻿using MongoBasic.Core.Abstract;
using MongoDB.Bson.Serialization;

namespace MongoBasic.Core
{
    public abstract class MongoClassMap<T> : IMongoClassMap
    {
        private static readonly object ClassMapLock;
        protected readonly IIdGenerator IdGenerator;

        static MongoClassMap()
        {
            ClassMapLock = new object();
        }

        protected MongoClassMap(IIdGenerator idGenerator = null)
        {
            if (idGenerator != null)
            {
                this.IdGenerator = idGenerator;
            }

            //we need to synchronize access to this resource because we don want multiple mongo class maps to be registerd;
            lock (ClassMapLock)
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof (T)))
                {
                    BsonClassMap.RegisterClassMap<T>(MapEntity);
                }
            }
        }

        /// <summary>
        /// Can be ovveridden by concrete map implementations
        /// </summary>
        protected abstract void MapEntity(BsonClassMap<T> cm);
    }
}
