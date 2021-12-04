using MyPageBlog.Data.Abstract;
using MyPageBlog.Data.Concrete.EntityFreamework.Contexts;
using MyPageBlog.Data.Concrete.EntityFreamework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Data.Concrete
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly MyPageBlogContext _context;
        private EfArticleRepository _efArticleRepository;
        private EfCategoryRepository _categoryRepository;
        private EfCommentRepository _commentRepository;
       

        public UnitOfWork(MyPageBlogContext context)
        {
            _context = context;
        }

        public IArticleRepository Articles => _efArticleRepository ?? new EfArticleRepository(_context);

        public ICategoryRepository Categories => _categoryRepository ?? new EfCategoryRepository(_context);

        public ICommentRepository Comments => _commentRepository ?? new EfCommentRepository(_context);

        

    

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
