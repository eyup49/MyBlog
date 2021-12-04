using Microsoft.EntityFrameworkCore;
using MyPageBlog.Data.Abstract;
using MyPageBlog.Entities.Concreate;
using MyPageBlog.Shared.Data.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Data.Concrete.EntityFreamework.Repositories
{
    public class EfCommentRepository : EfEntityRepositoryBase<Comment>, ICommentRepository
    {
        public EfCommentRepository(DbContext context) : base(context)
        {
        }
    }
}
