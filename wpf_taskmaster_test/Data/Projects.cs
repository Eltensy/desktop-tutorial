using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_backend.Data
{
    public class Project
    {
        public int id { get; set; }
        public int fk_user_id { get; set; }
        public string title { get; set; }
        public string? description { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? deadline { get; set; }
        public TimeSpan? estimate { get; set; }
        public TimeSpan? progress { get; set; }

        [Column(TypeName = "state_enum")]
        public StateType state { get; set; }

        [Column(TypeName = "priority_enum")]
        public PriorityType priority { get; set; }
    }
}
