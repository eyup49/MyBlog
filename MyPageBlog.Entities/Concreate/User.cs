using Microsoft.AspNetCore.Identity;
using MyPageBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Entities.Concreate
{
    public class User:IdentityUser<int>
    {   
        public string Picture { get; set; }     
        public ICollection<Article> Articles { get; set; }
    }
}
