using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }

        public List<Category> CategoryOptions { get; set; }
    }
}


