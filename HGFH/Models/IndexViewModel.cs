using HGFH.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Models
{
    public class IndexViewModel
    {
        public List<Blog> Blogs { get; set; }
        public List<Sermon> Sermons { get; set; }
        public Subscriber Subscriber { get; set; }
        public BlogViewModel YearlyDeclarationBlog { get; set; }
        public BlogViewModel MonthylDeclarationBlog { get; set; }
    }
}
