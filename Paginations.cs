using System.Collections.Generic;

namespace Sigma.Models
{
    public class Paginations
    {
        public static List<int> show_pagination(int pages_count, int current_page)
        {
            List<int> pagination = new List<int>();
            bool arrow_left_active = current_page > 1;
            bool has_left_ellipsis = current_page > 4;
            bool arrow_right_active = current_page < pages_count;
            bool has_right_ellipsis = (pages_count - current_page) > 3;
            int middle_section_start;
            int middle_section_end;
            if (has_left_ellipsis)
            {
                middle_section_start = current_page - 2;
            }
            else
            {
                middle_section_start = 2;
            }
            if (has_right_ellipsis)
            {
                middle_section_end = current_page + 3;
            }
            else
            {
                middle_section_end = pages_count;
            }

            if (pages_count < 8)
            {
                for (int page = 1; page <= pages_count; page++)
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
                pagination.Add(pages_count);
            }
            return pagination;
        }
        public static List<List<int>> paginations_list(int pages_count)
        {
            List<List<int>> paginations_list = new List<List<int>>();
            for (int page = 0; page < pages_count; page++)
            {
                paginations_list.Add(show_pagination(pages_count, page + 1));
            }
            return paginations_list;
        }
    }
}
