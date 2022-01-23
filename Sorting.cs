using System;
using System.Collections.Generic;
using Sigma.Models;

namespace Sigma
{
    public class Sorting
    {
        public static List<Project> Selected_sort(List<Project> array)
        {
            List<Project> result = new List<Project>();

            foreach(var project in array)
            {
                result.Add(project);
            }

            for (int i = 0; i < result.Count - 1; i++)
            {
                for (int j = i + 1; j < result.Count; j++)
                {
                    if (Convert.ToByte(result[i].selected) < Convert.ToByte(result[j].selected))
                    {
                        var temp = result[i];
                        result[i] = result[j];
                        result[j] = temp;
                    }
                }
            }
            return result;
        }
    }
}
