using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPageBlog.Entities.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPageBlog.Data.Concrete.EntityFreamework.Mappings
{
    public class ArticleMap:IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a =>a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Title).IsRequired(true);
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(50);
            builder.Property(a => a.SeoDescription).HasMaxLength(50);
            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(70);
            builder.Property(a => a.ViewsCount).IsRequired();
            builder.Property(a => a.CommentCount).IsRequired();
            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedData).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.HasOne<Category>(a => a.Category).WithMany(c=> c.Articles).HasForeignKey(a=>a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);
            builder.ToTable("Articles");
            //builder.HasData(
            //new Article
            //{
            //    Id = 1,
            //    CategoryId=1,
            //    Title="C# 9.0 VE NET 5 YENİLİKLERİ",
            //    Content= "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir." +
            //    " Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere" +
            //    " bir yazı galerisini alarak karıştırdığı 1500'lerden beri " +
            //    "endüstri standardı sahte metinler olarak kullanılmıştır. Beşyüz yıl boyunca " +
            //    "varlığını sürdürmekle kalmamış, aynı zamanda pek değişmeden elektronik dizgiye de sıçramıştır." +
            //    " 1960'larda Lorem Ipsum pasajları da içeren Letraset yapraklarının yayınlanması ile" +
            //    " ve yakın zamanda Aldus PageMaker gibi Lorem Ipsum sürümleri içeren masaüstü yayıncılık " +
            //    "yazılımları ile popüler olmuştur.",
            //    Thumbnail="Default.jpg",
            //    SeoDescription= "C# 9.0 VE NET 5 YENİLİKLERİ",
            //    SeoTags="C#, C# 9, .NET5,.NET FREAMWORK,.NET CORE",
            //    SeoAuthor="Eyup kayak",
            //    Date=DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    CreatedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedByName = "InitialCreate",
            //    ModifiedData = DateTime.Now,
            //    Note = "C# Blog Kategorisi C# 9.0 VE NET 5 YENİLİKLERİ",
            //    UserId=1,
            //    ViewsCount=100,
            //    CommentCount=1

            //},
            //new Article
            //{
            //    Id = 2,
            //    CategoryId = 2,
            //    Title = "C++ 11 VE NET 19 YENİLİKLERİ",
            //    Content = "Yaygın inancın tersine, Lorem Ipsum rastgele sözcüklerden oluşmaz." +
            //    " Kökleri M.Ö. 45 tarihinden bu yana klasik Latin edebiyatına kadar uzanan 2000 " +
            //    "yıllık bir geçmişi vardır. Virginia'daki Hampden-Sydney College'dan Latince profesörü" +
            //    " Richard McClintock, bir Lorem Ipsum pasajında geçen ve anlaşılması en güç sözcüklerden" +
            //    " biri olan 'consectetur' sözcüğünün klasik edebiyattaki örneklerini incelediğinde kesin bir" +
            //    " kaynağa ulaşmıştır. Lorm Ipsum, Çiçero tarafından M.Ö. 45 tarihinde kaleme alınan " +
            //    "de Finibus Bonorum et Malorum" +
            //    " (İyi ve Kötünün Uç Sınırları) eserinin 1.10.32 ve 1.10.33 sayılı bölümlerinden gelmektedir. " +
            //    "Bu kitap, ahlak kuramı üzerine bir tezdir ve Rönesans döneminde çok popüler olmuştur." +
            //    " Lorem Ipsum pasajının ilk satırı olan"+
            //    "Lorem ipsum dolor sit amet1.10.32 sayılı bölümdeki bir satırdan gelmektedir.",
            //    Thumbnail = "Default.jpg",
            //    SeoDescription = "C# 9.0 VE NET 5 YENİLİKLERİ",
            //    SeoTags = "C++, C# 9, .NET5,.NET FREAMWORK,.NET CORE",
            //    SeoAuthor = "Eyup kayak",
            //    Date = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    CreatedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedByName = "InitialCreate",
            //    ModifiedData = DateTime.Now,
            //    Note = "C++ Blog Kategorisi C# 9.0 VE NET 5 YENİLİKLERİ",
            //    UserId = 1,
            //    ViewsCount = 250,
            //    CommentCount = 1

            //},
            //new Article
            //{
            //    Id = 3,
            //    CategoryId = 3,
            //    Title = "Javascript 9.0 VE NET 5 YENİLİKLERİ",
            //    Content = "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir." +
            //    " Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere" +
            //    " bir yazı galerisini alarak karıştırdığı 1500'lerden beri " +
            //    "endüstri standardı sahte metinler olarak kullanılmıştır. Beşyüz yıl boyunca " +
            //    "varlığını sürdürmekle kalmamış, aynı zamanda pek değişmeden elektronik dizgiye de sıçramıştır." +
            //    " 1960'larda Lorem Ipsum pasajları da içeren Letraset yapraklarının yayınlanması ile" +
            //    " ve yakın zamanda Aldus PageMaker gibi Lorem Ipsum sürümleri içeren masaüstü yayıncılık " +
            //    "yazılımları ile popüler olmuştur.",
            //    Thumbnail = "Default.jpg",
            //    SeoDescription = "Javascript 9.0 VE NET 5 YENİLİKLERİ",
            //    SeoTags = "Javascript, C# 9, .NET5,.NET FREAMWORK,.NET CORE",
            //    SeoAuthor = "Eyup kayak",
            //    Date = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    CreatedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedByName = "InitialCreate",
            //    ModifiedData = DateTime.Now,
            //    Note = "Javascript Blog Kategorisi C# 9.0 VE NET 5 YENİLİKLERİ",
            //    UserId = 1,
            //    ViewsCount = 120,
            //    CommentCount = 1

            //}
            //) ;





        }
    }
}
