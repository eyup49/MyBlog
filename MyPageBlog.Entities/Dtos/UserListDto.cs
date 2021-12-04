﻿using MyPageBlog.Entities.Concreate;
using MyPageBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Entities.Dtos
{
    public class UserListDto:DtoGetBase
    {
        public IList<User>Users  { get; set; }
    }
}
