using AutoMapper;
using MyPageBlog.Entities.Concreate;
using MyPageBlog.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Services.AutoMapper.Profiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddDto, Category>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<CategoryUpdateDto, Category>().ForMember(dest => dest.ModifiedData, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Category, CategoryUpdateDto>();
        }
    }
}
