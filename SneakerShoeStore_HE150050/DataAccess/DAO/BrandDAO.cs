using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class BrandDAO
    {
        public static List<Brand> GetBrands()
        {
            var listBrand = new List<Brand>();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    listBrand = context.Brands.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listBrand;
        }
        public static Brand FindBrandById(int brandId)
        {
            Brand b = new Brand();
            try
            {
                using (var context = new SneakerShoeStoreContext())
                {
                    b = context.Brands.SingleOrDefault(x => x.BrandId == brandId);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return b;
        }

        //public static void SaveBook(Book b)
        //{
        //    try
        //    {
        //        using (var context = new EBookStoreContext())
        //        {
        //            context.Books.Add(b);
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex) { }
        //}

        //public static void UpdateBook(Book b)
        //{
        //    try
        //    {
        //        using (var context = new EBookStoreContext())
        //        {
        //            context.Entry<Book>(b).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public static void DeleteBook(Book b)
        //{
        //    try
        //    {
        //        using (var context = new EBookStoreContext())
        //        {
        //            var b1 = context.Books.SingleOrDefault(
        //                c => c.book_id == b.book_id);
        //            context.Books.Remove(b1);
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
