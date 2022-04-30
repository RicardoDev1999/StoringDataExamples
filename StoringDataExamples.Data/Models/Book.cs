using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Data.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(6)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(6)]
        public string? Author { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
