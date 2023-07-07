using HGFH.Data;
using HGFH.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Services
{
    public class AuthorService : IAuthorService
    {
        private ApplicationDbContext _database;
        private IConfiguration Configuration;
        public AuthorService(ApplicationDbContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            _database = context;
        }
        public ResponseModel CreateAuthor(AuthorViewModel author)
        {
            var responseModel = new ResponseModel();
            try
            {
                var Author = new Author()
                {
                    Id = Guid.NewGuid(),
                    Name = author.Name,
                    CreatedBy = author.CreatedBy,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Description = author.Description,
                    ModifiedBy = author.ModifiedBy
                };

                var entity = _database.Authors.Add(Author).Entity;
                _database.Entry(author).State = EntityState.Added;
                var added = _database.SaveChanges();

                if (added > 0)
                {
                    responseModel = new ResponseModel()
                    {
                        code = "200",
                        isSuccessful = true,
                        message = "Author created"
                    };
                }
            }
            catch ( Exception ex)
            {

                return new ResponseModel()
                {
                    isSuccessful = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message,
                    innerExceptionStackTrace = ex.InnerException?.StackTrace
                };
            }

            return responseModel;
        }

        public ResponseModel EditAuthor(AuthorViewModel author)
        {
            var responseModel = new ResponseModel();
            try
            {
                var Author = new Author()
                {
                    Id = Guid.NewGuid(),
                    Name = author.Name,
                    CreatedBy = author.CreatedBy,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Description = author.Description,
                    ModifiedBy = author.ModifiedBy
                };

                var entity = _database.Authors.Add(Author).Entity;
                _database.Entry(author).State = EntityState.Modified;
                var added = _database.SaveChanges();

                if (added > 0)
                {
                    responseModel = new ResponseModel()
                    {
                        code = "200",
                        isSuccessful = true,
                        message = "Author created"
                    };
                }
            }
            catch (Exception ex)
            {

                return new ResponseModel()
                {
                    isSuccessful = false,
                    message = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message,
                    innerExceptionStackTrace = ex.InnerException?.StackTrace
                };
            }

            return responseModel;
        }

        public AuthorViewModel ViewAuthor(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
