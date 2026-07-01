using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.Models
{
    public class AdminPagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}
