﻿using System.ComponentModel.DataAnnotations;

namespace MG.Utils.Pagination
{
    public record PageModel
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = DefaultPage;

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = DefaultPageSize;

        public int ToSkip => (Page - 1) * PageSize;

        public PageModel()
        {
        }

        public PageModel(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public static PageModel Default => new (DefaultPage, DefaultPageSize);
    }
}
