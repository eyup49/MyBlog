using MyPageBlog.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPageBlog.Mvc.Areas.Admin.Models
{
    public class CategoryUpdateAjaxViewModel
    {
        public CategoryUpdateDto categoryUpdateDto { get; set; }
        public string CategoryUpdatePartial { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
