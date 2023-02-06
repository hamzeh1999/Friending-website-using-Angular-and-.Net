using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.healper
{
    public class PageList<T> : List<T>
    {


        public PageList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            this.currentPage = pageNumber;
            this.totalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.pageSize = pageSize;
            this.totalCount = count;
            AddRange(items);
        }

        public int currentPage { set; get; }

        public int totalPages { set; get; }

        public int pageSize { set; get; }

        public int totalCount { set; get; }

        public static async Task<PageList<T>> createAsync(IQueryable<T> source,
         int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);

        }
    }
}