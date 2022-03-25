using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sigma.Models;
using Sigma;

namespace Sigma
{
    public class Reference
    {
        public static List<Project> GetRange(List<Project> item_list, Project item)
        {
            List<Project> other_projects = new List<Project>();
            if (item_list.Count >= 5)
            {
                other_projects = Sorting.Selected_sort(item_list).GetRange(0, 4);
                if (other_projects.Contains(item))
                {
                    other_projects = Sorting.Selected_sort(item_list).GetRange(0, 5);
                    other_projects.Remove(item);
                }
            }
            else
            {
                other_projects = Sorting.Selected_sort(item_list);
                other_projects.Remove(item);
            }
            return other_projects;
        }
    }
}