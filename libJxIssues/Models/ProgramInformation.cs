using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libJxIssues.Models
{
    public class ProgramInformation
    {
        [Key, Required, Column(TypeName = "int")]
        public int id { get; set; }

        [Required, Column(TypeName = "int")]
        public int project_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? lastSyncGIT { get; set; }
    }
}
