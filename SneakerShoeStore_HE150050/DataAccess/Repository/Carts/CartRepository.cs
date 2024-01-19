using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Carts
{
    public class CartRepository : ICartRepository
    {
        public void AddCart(Cart c) => CartDAO.SaveCart(c);

        public void DeleteCart(Cart p) => CartDAO.DeleteCart(p);

        public Cart GetCartByRecordId(int id) => CartDAO.FindCartById(id);

        public List<Cart> GetCarts() => CartDAO.GetCarts();

        public void UpdateCart(Cart c) => CartDAO.UpdateCart(c);
    }
}
