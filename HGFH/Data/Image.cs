using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Data
{
    public class Image
    {
        public Guid Id { get; set; }
        public byte[] Thumbnail { get; set; }
        public string ThumbnailContentType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
