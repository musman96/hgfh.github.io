using HGFH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Services
{
    public interface IAuthorService
    {

        ResponseModel CreateAuthor(AuthorViewModel author);
        ResponseModel EditAuthor(AuthorViewModel author);
        AuthorViewModel ViewAuthor(Guid id);

    }
}
