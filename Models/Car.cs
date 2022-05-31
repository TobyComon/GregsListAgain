using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GregsListAgain.Models
{
    public class Car
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        [MinLength(3)]
        public string Make { get; set; }
        public string Model { get; set; }

        [Url]
        public string ImgUrl { get; set; }
        public string CreatorId { get; set; }
        public Profile Creator { get; set; }
    }
}