using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.healper;

namespace API.Extensions
{
    public static class HttpExtension
    {
        public static void addPaginationHeader(this HttpResponse res,
        int currentPage,int itemsPerPage
        ,int totalItems,int totalPages)
        {
            var paginatorHeader=new PaginatorHeader(currentPage,itemsPerPage,totalItems,totalPages);
            res.Headers.Add("pagination",JsonSerializer.Serialize(paginatorHeader));
            res.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}