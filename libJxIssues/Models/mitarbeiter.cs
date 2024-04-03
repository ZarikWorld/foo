#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libJxIssues.Models;

public class Mitarbeiter
{
    [Column(TypeName = "varchar(512)")]
    public string? avatar_url { get; set; }
    
    [Required, Column(TypeName = "int")]
    public int gitlab_id { get; set; }

    [Required, Column(TypeName = "varchar(100)")]
    public string gitUsername { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string gitToken { get; set; }

    [Key, Required, Column(TypeName = "int")]
    public int id { get; set; }

    [Required, Column(TypeName = "varchar(100)")]
    public string name { get; set; }

    [Column(TypeName = "int")]
    public int? mitarbeiterRole { get; set; }

    [Required, Column(TypeName = "int")]
    public int team_id { get; set; }

    [Column(TypeName = "int")]
    public int montag { get; set; }
    [Column(TypeName = "int")]
    public int dienstag { get; set; }
    [Column(TypeName = "int")]
    public int mittwoch { get; set; }
    [Column(TypeName = "int")]
    public int donnerstag { get; set; }
    [Column(TypeName = "int")]
    public int freitag { get; set; }
}