using MyPageBlog.Entities.Concreate;
using MyPageBlog.Shared.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Data.Abstract
{
    public interface IArticleRepository:IEntityRepository<Article>
    {
        
    }
}
