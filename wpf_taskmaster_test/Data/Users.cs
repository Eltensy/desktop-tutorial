using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace wpf_backend.Data
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public DateTime? date_joined { get; set; }
    }

}
