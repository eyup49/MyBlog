using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Data.Abstract
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        IArticleRepository Articles { get; }//unitwork.Articles
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }
       
        //_unitOfWork.Categories.AddAsync();
                                      // _unitOfWork.Categories.AddAsync(category);
                                      // _unitOfWork.Categories.AddAsync(user);
                                      //_unitOfWork.SaveAsync();
        Task<int> SaveAsync();
    }
}
