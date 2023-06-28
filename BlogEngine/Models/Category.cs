using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEngine.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }
    }
}
