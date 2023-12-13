using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_backend.Data
{
    public class Progress_log
    {
        public int id { get; set; }
        public int fk_user_id { get; set; }
        public int fk_task_id { get; set; }
        public DateTime logged_at { get; set; }
        public string? description { get; set; }
        public TimeSpan logged_work { get; set; }

    }
}
