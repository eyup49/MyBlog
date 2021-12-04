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
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork uInitOfWork ,IMapper mapper)
        {
            _unitOfWork = uInitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createByName)
        {
         

            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createByName;
            category.ModifiedByName = createByName;
            var addedCategory= await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success,new CategoryDto { 
          
            ResultStatus=ResultStatus.Success,
            Category = addedCategory,
            Message= $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir."

            },$"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir.");
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedxByName)
        {
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = _mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);
            category.ModifiedByName = modifiedxByName;
             var updateCategory= await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
            {

                ResultStatus = ResultStatus.Success,
                Category = updateCategory,
                Message = $"{categoryUpdateDto.Name} adlı kategori başarıyla guncellenmiştir."

            }, $"{categoryUpdateDto.Name} adlı kategori başarıyla guncellenmiştir.");
           

        }

        public async Task<IDataResult<CategoryDto>> Delete(int categoryId, string modifiedxByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category!=null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedxByName;
                category.ModifiedData = DateTime.Now;
                var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {

                    ResultStatus = ResultStatus.Success,
                    Category = deletedCategory,
                    Message = $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir."

                }, $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir.");
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, new CategoryDto
            {

                ResultStatus = ResultStatus.Error,
                Category = null,
                Message = $"Böyle bir kategori bulunamadı."

            }, $"Böyle bir kategori bulunamadı.");
        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
            if (category!=null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto { 
                Category=category,
                ResultStatus=ResultStatus.Success
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error,new CategoryDto { 
            
                Category=null,
                ResultStatus=ResultStatus.Error,
                Message= "böyle bir kategori bulunamadı"


            },"böyle bir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);
            if (categories.Count>-1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto{ 
                Categories=categories,
                ResultStatus=ResultStatus.Success
                
              
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message= "Hiçbir kategori bulunamadı"

            }, "Hiçbir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted,c=>c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success


                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = "Hiçbir kategori bulunamadı"

            }, "Hiçbir kategori bulunamadı");
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                await _unitOfWork.Categories.DeleteAsync(category);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{category.Name} adli kategori başarıyla veritabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Hiçbir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted&&c.IsActive, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, null, "Hiçbir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Error,null,"Böyle bir kategori bulunamadı");
            }
        }
    }
}
