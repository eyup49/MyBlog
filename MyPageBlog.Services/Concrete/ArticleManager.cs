using AutoMapper;
using MyPageBlog.Data.Abstract;
using MyPageBlog.Entities.Concreate;
using MyPageBlog.Entities.Dtos;
using MyPageBlog.Services.Abstract;
using MyPageBlog.Shared.Utilities.Results.Abstract;
using MyPageBlog.Shared.Utilities.Results.Abstract.ComplexTypes;
using MyPageBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Services.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleManager(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createByName;
            article.ModifiedByName = createByName;
            article.UserId = 1;
            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{articleAddDto.Title} başlıklı makale başarıyla eklenmiştir. ");
        }

        public async Task<IResult> Delete(int articleId, string modifiedxByName)
        {
            var result=await _unitOfWork.Articles.AnyAsync(c => c.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(c => c.Id == articleId);
                article.IsDeleted = true;
                article.ModifiedByName = modifiedxByName;
                article.ModifiedData = DateTime.Now;
                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{article.Title} adli makale başarıyla silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Hiçbir makale bulunamadı");
        }

        public async Task<IDataResult<ArticleDto>> Get(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.Category);
            if (article!=null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
                {
                    Article = article,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, null, "Böyle bir makale bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
            if (articles.Count>-1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "makale bulunamadı");
        }
          
        public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var articles = await _unitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, ar => ar.User, ar => ar.Category);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, null, "makaleler bulunamadı");
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Böyle bir kategori  bulunamadı");

        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeleted()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a=>!a.IsDeleted, ar => ar.User, ar => ar.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "makaleler bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a =>!a.IsDeleted &&a.IsActive, ar => ar.User, ar => ar.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "makaleler bulunamadı");
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var result = await _unitOfWork.Articles.AnyAsync(c => c.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(c => c.Id == articleId);
                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{article.Title} adli makale başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Hiçbir makale bulunamadı");
        }

        public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedxByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifiedxByName;
            await _unitOfWork.Articles.UpdateAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{articleUpdateDto.Title} başlıklı makale başarıyla guncellenmiştir. ");
        }
    }
}
