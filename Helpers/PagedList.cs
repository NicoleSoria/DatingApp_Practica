using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set;}
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreatedAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();

            // Skip ignora una cierta cantidad de elementos, el parametro que le pasamos son la cantidad de elementos que salteará,
            // es decir si tenemos 13 usuarios y tamaño de la pagina 5 y solicitamos la pagina 3 seria (3-1) * 5 = 10, se omiten los 10 primeros elementos y se traen 5
            // Luego se toma el tamaño de la pagina y haga una lista con esos elementos
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
