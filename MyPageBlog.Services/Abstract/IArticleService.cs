using MyPageBlog.Entities.Concreate;
using MyPageBlog.Entities.Dtos;
using MyPageBlog.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> Get(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeleted();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive();
        Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId);
        Task<IResult> Add(ArticleAddDto articleAddDto, string createByName);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedxByName);
        Task<IResult> Delete(int articleId, string modifiedxByName);

        Task<IResult> HardDelete(int articleId);
    }
}
