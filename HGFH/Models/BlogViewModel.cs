using HGFH.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Models
{
    public class BlogViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
        public long CateoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public List<Option> Categories { get; set; }
        public List<Blog> RecentBlogs { get; set; }
        public List<Blog> BlogsPerCategory { get; set; }
        public List<Comment> Comments { get; set; }
        public List<SubComment> SubComments { get; set; }
        public int CommentsCount { get; set; }
        public Comment Comment { get; set; }
        public SubComment SubComment { get; set; }
        public Author Author { get; set; }
        public Option Option { get; set; }
    } 
}
