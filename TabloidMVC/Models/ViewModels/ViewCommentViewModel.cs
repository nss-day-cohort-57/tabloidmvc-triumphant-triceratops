using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class ViewCommentViewModel
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }   
        public List<Comment> Comment { get; set; }

       
    }
}


