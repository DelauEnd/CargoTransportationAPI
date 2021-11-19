using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.RequestFeautures
{
    public class RequestParameters
    {
        public string Search { get; set; }

        public string OrderBy { get; set; }

        public string Fields { get; set; }

        const int maxPageSize = 50;
        public int Page { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }   
    }
}
