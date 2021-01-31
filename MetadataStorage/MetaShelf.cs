using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MetadataStorage
{
    public class MetaShelf
    {
        private readonly IDictionary<string, MetaBook> _booksDict = new ConcurrentDictionary<string, MetaBook>();

        public MetaShelf()
        {

        }

        public MetaBook GetOrAdd(string filePath)
        {
            var key = filePath;

            if (!_booksDict.TryGetValue(key, out var value))
            {
                value = new MetaBook(key);
                _booksDict.Add(key, value);
            }
            return value;
        }
    }
}
