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
    public class ArticleProfile:Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleAddDto, Article>().ForMember(dest=>dest.CreatedDate,opt=>opt.MapFrom(x=>DateTime.Now));
            CreateMap<ArticleUpdateDto, Article>().ForMember(dest=>dest.ModifiedData,opt=>opt.MapFrom(c=>DateTime.Now));
        }
    }
}
