using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Carts
{
     public interface ICartRepository
    {
        List<Cart> GetCarts();
        void AddCart(Cart c);

        void UpdateCart(Cart c);

        Cart GetCartByRecordId(int id);
        void DeleteCart(Cart p);
    }
}
