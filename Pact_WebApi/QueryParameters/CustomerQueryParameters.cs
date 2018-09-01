using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pact_WebApi.QueryParameters
{
    public class CustomerQueryParameters
    {
        private const int MaxPageCount = 100;
        private int _pageCount = 100;

        public int Page { get; set; } = 1;

        public int PageCount
        {
            get => _pageCount;
            set => _pageCount = (value > MaxPageCount) ? MaxPageCount : value;
        }

        [BindNever]
        public bool HasQuery => !string.IsNullOrEmpty(Query);

        public string Query { get; set; }

        public string OrderBy { get; set; } = "FirstName";

        [BindNever]
        public bool Descending
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(OrderBy))
                {
                    return OrderBy.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
                }

                return false;
            }
        }
    }
}
