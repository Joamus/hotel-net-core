using HotelBackendApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelBackendApi;

public class MainContext : IdentityDbContext<User> {
    public MainContext(DbContextOptions<MainContext> options) : base(options) {}
	
	public DbSet<Guest> Guests { get; set;}
	public DbSet<Room> Rooms { get; set;}
	public DbSet<RoomReservation> RoomReservations { get; set;}
	public DbSet<Order> Orders { get; set;}
	public DbSet<OrderItem> OrderItems { get; set;}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		// no extra stuff yet - just in case
	}
}