using MyPageBlog.Entities.Concreate;
using MyPageBlog.Shared.Entities.Abstract;
using MyPageBlog.Shared.Utilities.Results.Abstract.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Entities.Dtos
{
    public class ArticleDto:DtoGetBase
    {
        public Article Article { get; set; }
        //public override ResultStatus ResultStatus { get; set; } = ResultStatus.Success;

    }
}
