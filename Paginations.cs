using System.Collections.Generic;

namespace Sigma.Models
{
    public class Paginations
    {
        public static List<int> GetPagination(int pagesCount, int currentPage)
        {
            if(pagesCount == 0) return new List<int> { 0 };
            List<int> pagination = new List<int>();
            bool has_left_ellipsis = currentPage > 4;
            bool has_right_ellipsis = (pagesCount - currentPage) > 3;
            int middle_section_start;
            int middle_section_end;
            if (has_left_ellipsis)
            {
                middle_section_start = currentPage - 2;
            }
            else
            {
                middle_section_start = 2;
            }
            if (has_right_ellipsis)
            {
                middle_section_end = currentPage + 3;
            }
            else
            {
                middle_section_end = pagesCount;
            }

            if (pagesCount < 8)
            {
                for (int page = 1; page <= pagesCount; page++)
                {
                    pagination.Add(page);
                }
            }
            else
            {
                pagination.Add(1);
                if (has_left_ellipsis)
                {
                    pagination.Add(0);
                }
                for (int page = middle_section_start; page < middle_section_end; page++)
                {
                    pagination.Add(page);
                }
                if (has_right_ellipsis)
                {
                    pagination.Add(0);
                }
                pagination.Add(pagesCount);
            }
            return pagination;
        }
    }
}
