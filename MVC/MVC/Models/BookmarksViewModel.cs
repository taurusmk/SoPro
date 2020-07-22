using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class BookmarksViewModel
    {
        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string URL { get; set; }

        public string ShortDescription { get; set; }
    }
}