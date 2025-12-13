using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.Interfaces
{
    public interface IPagedList<T>
    {
        int PageNumber { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int PageCount { get; }

        bool HasNextPage { get; }
        bool HasPreviousPage { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }

        List<T> Items { get; }
        List<T> ToList();
    }
}
