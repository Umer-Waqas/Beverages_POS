using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Pagination
{
    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(
            this IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(
            this IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static IPagedList<T> ToPagedList<T>(
        this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var enumerable = source as T[] ?? source.ToArray();
            var count = enumerable.Length;
            var items = enumerable.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(
            this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            // For IEnumerable, we can make it async by using Task.Run
            // or simply call the synchronous version asynchronously
            return await Task.Run(() => source.ToPagedList(pageNumber, pageSize));
        }
    }

}
