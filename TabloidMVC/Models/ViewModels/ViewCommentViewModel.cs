using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class ViewCommentViewModel
    {
        public Post Post { get; set; }
        public int PostId { get; set; }
           
        public List<Comment> Comment { get; set; }

       
    }
}


