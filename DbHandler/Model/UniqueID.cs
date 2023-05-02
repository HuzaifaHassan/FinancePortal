using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHandler.Model
{
    [Table("t_UniqueId")]
    public class UniqueID
    {
        [Key]
        public string Id { get; set; }
        public string Route { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
