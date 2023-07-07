using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Data
{
    public class SubComment 
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public Guid BlogPostId { get; set; }
        public virtual Blog BlogPost { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid CommentId { get; set; }
        public virtual Comment Comment {get;set;}
    }
}
