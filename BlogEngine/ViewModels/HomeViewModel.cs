using BlogEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.ViewModels
{
    public class HomeViewModel
    {
        public List<Post> Posts { get; set; }
        public List<Category> Categories { get; set; }
    }
}
