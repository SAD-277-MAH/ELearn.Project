using ELearn.Data.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Data.Dtos.Common.Pagination
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 0;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > Constants.MaxPageSize) ? Constants.MaxPageSize : value; }
        }

        public string Filter { get; set; }

        public string SortHeader { get; set; }

        public string SortDirection { get; set; }
    }
}
