using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Data
{
    public class Blog
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public Guid AuthorId { get; set; }
        public long CategoryId { get; set; }
        public Guid ThumbnailId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Author Author { get; set; }
        public Option Option { get; set; }
        public Image Thumbnail { get; set; }
    }
}
