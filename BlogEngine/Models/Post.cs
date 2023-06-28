using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogEngine.Models
{
    public class Post
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Publication Date field is required.")]
        [Display(Name = "Pub Date")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "The Content field is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        [Display(Name = "Category")]
        public long CategorieId { get; set; } 
    }
}
