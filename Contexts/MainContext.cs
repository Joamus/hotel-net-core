using HotelBackendApi;
using Microsoft.EntityFrameworkCore;

namespace HotelBackendApi;

public class MainContext : DbContext {
    public MainContext(DbContextOptions<MainContext> options) : base(options) {}
	
	public DbSet<Guest> Guests { get; set;}
	public DbSet<Room> Rooms { get; set;}
	public DbSet<RoomReservation> RoomReservations { get; set;}
	public DbSet<Order> Orders { get; set;}
	public DbSet<OrderItem> OrderItems { get; set;}
}