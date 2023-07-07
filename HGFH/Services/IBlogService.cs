using HGFH.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Services
{
    public interface IBlogService
    {
        Option GetCategories();
        Blog CreateBlog();
    }
}
