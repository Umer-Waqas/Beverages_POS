using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Pagination
{
    public class PagedList<T> : IPagedList<T>, IEnumerable<T>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int PageCount { get; private set; }

        public bool HasNextPage => PageNumber < PageCount;
        public bool HasPreviousPage => PageNumber > 1;
        public bool IsFirstPage => PageNumber == 1;
        public bool IsLastPage => PageNumber == PageCount;


        public List<T> Items { get; private set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = (int)Math.Ceiling(count / (double)pageSize);
        }

        public List<T> ToList()
        {
            return Items.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
