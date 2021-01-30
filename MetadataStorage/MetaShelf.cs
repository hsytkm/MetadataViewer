using System;
using System.Collections.Generic;
using System.Linq;

namespace MetadataStorage
{
    public class MetaShelf
    {
        private readonly Dictionary<string, MetaBook> _booksMap = new();

        public MetaShelf()
        {

        }

        public MetaBook GetOrAdd(string filePath)
        {
            if (!_booksMap.TryGetValue(filePath, out var book))
            {
                book = new MetaBook(filePath);
                _booksMap.Add(filePath, book);
            }
            return book;
        }
    }
}
