using Microsoft.EntityFrameworkCore;
using TheGioiViTinh.Data;
using TheGioiViTinh.Models.Entities;
using TheGioiViTinh.Services.IServices;

namespace TheGioiViTinh.Services
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly ApplicationDbContext _context;
		public ShoppingCartService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<ShoppingCart> AddToCartAsync(ShoppingCart cart)
		{
			var existingCart = await _context.ShoppingCarts.Include(p=>p.Product).FirstOrDefaultAsync(p => p.ApplicationUserId == cart.ApplicationUserId && p.ProductId == cart.ProductId);
			if(existingCart != null)
			{
				existingCart.Count += cart.Count;
				await _context.SaveChangesAsync();
				return existingCart;
			}
			else
			{
				_context.ShoppingCarts.Add(cart);
				await _context.SaveChangesAsync();
				return cart;
			}
		}

		public async Task ClearCartAsync(string userId)
		{
			var items = await _context.ShoppingCarts.Include(p => p.Product).Where(p => p.ApplicationUserId == userId).ToListAsync();
			if (items.Any()) {
				_context.ShoppingCarts.RemoveRange(items);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<ShoppingCart?> GetCartByIdAsync(int cartId)
		{
			return await _context.ShoppingCarts.Include(p=>p.Product).ThenInclude(p=>p.ProductDiscounts).ThenInclude(p => p.Discount).FirstOrDefaultAsync(p => p.Id == cartId);
		}

		public async Task<int> GetCartCountAsync(string userId)
		{
			return await _context.ShoppingCarts.Where(p => p.ApplicationUserId == userId).SumAsync(p => p.Count);
		}

		public async Task<IEnumerable<ShoppingCart>> GetUserCartItemsAsync(string userId)
		{
			return await _context.ShoppingCarts.Include(p=>p.Product).ThenInclude(p => p.ProductDiscounts).ThenInclude(p=>p.Discount).Where(p => p.ApplicationUserId == userId).ToListAsync();
		}

		public async Task UpdateCartAsync(ShoppingCart cart)
		{
			if(cart.Count <= 0)
			{
				_context.ShoppingCarts.Remove(cart);
			}
			else
			{
				_context.ShoppingCarts.Update(cart);
			}
			await _context.SaveChangesAsync();
		}
	}
}
