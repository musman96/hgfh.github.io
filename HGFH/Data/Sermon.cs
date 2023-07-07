using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Data
{
    public class Sermon
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string YoutubeLink { get; set; }
        public string Subtitle { get; set; }
        public string Preacher { get; set; }
        public Guid ThumbnailId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Image Thumbnail { get; set; }
    }
}
