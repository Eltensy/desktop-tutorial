using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace wpf_backend.Data
{
    public class Task
    {
        public int id { get; set; }
        public int fk_user_id { get; set; }
        public int? fk_project_id { get; set; }
        public string title { get; set; }
        public string? description { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? deadline { get; set; }
        public TimeSpan? estimate { get; set; }
        public TimeSpan? progress { get; set; }
        public StateType state { get; set; }
        public PriorityType priority { get; set; }
        public short? project_sequence_num { get; set; }
    }

}
