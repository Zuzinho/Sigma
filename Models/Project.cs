//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sigma.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string Link { get; set; }
        public string About { get; set; }
        public Nullable<int> user_id { get; set; }
        public string technology { get; set; }
        public string user_name { get; set; }
        public Nullable<bool> selected { get; set; }

    }
}
