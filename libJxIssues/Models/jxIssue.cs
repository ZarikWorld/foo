#nullable disable
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libJxIssues.Models;

public class jxIssue
{
    [Key, Required, Column(TypeName = "int")]
    public int id { get; set; }
    
    [Required, Column(TypeName = "int")]
    public int iid { get; set; }

    [Required, Column(TypeName = "int")]
    public int project_id { get; set; }

    [Required, Column(TypeName = "bit")]
    public bool erledigt { get; set; }
    
    [Column(TypeName = "int")]
    public int? prioPunkte { get; set; }

    [Required, Column(TypeName = "varchar(500)")]
    public string titel { get; set; }

    [Required, Column(TypeName = "bit")]
    public bool git { get; set; }

    [Required, Column(TypeName = "varchar(100)")]
    public string web_url { get; set; }

    [Column(TypeName = "int")]
    public int? sortOrder { get; set; }

    [Required, Column(TypeName = "int")]
    public int creator_id { get; set; }
    
    [Column(TypeName = "int")]
    public int? mitarbeiter_id { get; set; }

    [Column(TypeName = "int")]
    public int? schaetzung { get; set; }

    [Column(TypeName = "int")]
    public int? schaetzungOffiziell { get; set; }

    [Required, Column(TypeName = "datetime")]
    public DateTime created_at { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? start { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime? ende { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? deadline { get; set; }
    
    [Required, Column(TypeName = "int")]
    public int typ { get; set; }
    
    [Required, Column(TypeName = "int")]
    public int status { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    public string? aktenzahl{ get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? kunde { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? anmerkung { get; set; }
}