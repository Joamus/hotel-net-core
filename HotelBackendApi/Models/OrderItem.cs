namespace HotelBackendApi;
public class OrderItem {
	public long Id { get; set; }
	private decimal price = 0;
	public decimal Price { 
		get {
			return price / (1 + (decimal) DiscountPercentage);
		}
		set {
			price = value;
		}
	}
	
	public float DiscountPercentage { get; set; } = 0;
	
	public decimal PriceWithVat {
		get {
			return Price * (1 + (decimal) VatPercentage);
		}
	}

	public string? ProductCode { get; set; }
	
	public string? ProductName { get; set; }

	public float VatPercentage {
		get;
	}
}