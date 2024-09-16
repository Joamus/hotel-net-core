
namespace HotelBackendApi;

public class Order {
	public long Id { get; set; }
	public DateTime PurchaseTime { get; }
	
	public ICollection<OrderItem> Items { get; set; } = null!;
	
	public float DiscountPercentage { get; set; } = 0;
	
	public decimal Total { 
		get {
            decimal total = 0;

			foreach (var item in Items) {
				total += item.Price;
			}
			return total - (total * (decimal) DiscountPercentage);
		}
	}
}