using Microsoft.EntityFrameworkCore;
using MyPageBlog.Data.Abstract;
using MyPageBlog.Entities.Concreate;
using MyPageBlog.Shared.Data.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Data.Concrete.EntityFreamework.Repositories
{
    public class EfArticleRepository : EfEntityRepositoryBase<Article>,IArticleRepository
    {
        public EfArticleRepository(DbContext context) : base(context)
        {
        }
    }
}
