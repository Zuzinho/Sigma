using System.Collections.Generic;
using Sigma.Models;

namespace Sigma
{
    public class Sorting
    {
        public static List<Project> Selected_sort(List<Project> projects,int range = 0,Project excessProject = null)
        {
            if (range == 0) range = projects.Count;
            List<Project> selectedProjects = new List<Project>(),unselectedProjects = new List<Project>();
            _sortLists(ref selectedProjects,ref unselectedProjects,projects,ref range,excessProject);
            List<Project> result = new List<Project>(selectedProjects);
            if (range == 0) return result;
            for (int i = 0; i < range; i++) result.Add(unselectedProjects[i]);
            return result;
        }
        private static void _sortLists(ref List<Project> selectedProjects,ref List<Project> unselectedProjects,List<Project> projects,ref int range,Project excessProject)
        {
            foreach (var project in projects)
            {
                if (project == excessProject) continue;
                if (!project.Selected)
                {
                    unselectedProjects.Add(project);
                    continue;
                }
                selectedProjects.Add(project);
                if (--range == 0) break;
            }
        }
    }
}
