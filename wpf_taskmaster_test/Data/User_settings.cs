using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_backend.Data
{
    public class User_settings
    {
        public int id { get; set; }
        public int fk_user_id { get; set; }
        public LangType lang { get; set; }
        public ThemeType theme { get; set; }
        public TimeFormatType time_format { get; set; }
        public DateFormatType date_format { get; set; }

    }
}
