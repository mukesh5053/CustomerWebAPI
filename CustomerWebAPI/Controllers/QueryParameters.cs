using System;

namespace CustomerWebAPI.Controllers
{
    public class QueryParameters
    {
        private const int _maxSize = 100;
        private int _size = 10;

        public int Page { get; set; }

        public int Size
        {
            get
            {

                return _size;
            }
            set
            {
                _size = Math.Min(_size, value);

            }
        }
    }
}