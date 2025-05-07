using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Enums;
using RestaurantManager.Models;
using Xunit;

namespace RestaurantManager.UnitTests.BackEnd.Database{

    public class OrderDbContext: DbContext {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { 

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders {get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderMenuItem> OrderMenuItems { get; set; }
        public DbSet<UserDietaryTag> UserDietaryTags { get; set; }
        public DbSet<DietaryTag> DietaryTags { get; set; }
        public DbSet<MenuItemDietaryTag> MenuItemDietaryTags { get; set; }
        

       protected override void OnModelCreating(ModelBuilder builder) {

            //Has combined key made up of OrderID and MenuItemID
            builder.Entity<OrderMenuItem>()
                .HasKey(om => new {om.OrderId, om.MenuItemId});

            //Tell database that each OrdermenuItem belongs to one Order
            //One Order can have many OrderMenuItems
            builder.Entity<OrderMenuItem>()
                .HasOne(om => om.Order)
                .WithMany(mi => mi.OrderMenuItems)
                .HasForeignKey(om => om.OrderId);

            //Tell database that each OrderMenuItem has one MenuItem
            //MenuItems can appear in many OrderMenuItems
            builder.Entity<OrderMenuItem>()
                .HasOne(om => om.MenuItem)
                .WithMany(mi => mi.OrderMenuItems)
                .HasForeignKey(om => om.MenuItemId);

            //Combined key made up of UserID and TadId
            builder.Entity<UserDietaryTag>()
                .HasKey(udt => new { udt.UserId, udt.TagId });

            //Combined key made up of MenuItemId and TagId
            builder.Entity<MenuItemDietaryTag>()
                .HasKey(mdt => new { mdt.MenuItemId, mdt.TagId });                
        }
    }

    public class OrderRelationshipTest : IDisposable {

        private readonly DbContextOptions<OrderDbContext> _options;
        private readonly OrderDbContext _context;

        public OrderRelationshipTest() {
            //Create options for an in memory testing database
            _options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderRelationshipsTest")
                .Options;

            //Create the database with options
            _context = new OrderDbContext(_options);

            //Fill the database with dummy data
            SeedDatabase();
        }

        private void SeedDatabase() {

            //Create User to test with
            var user = new User {
                Id = 1,
                FirstName = "Ted",
                LastName = "Sheckler",
                Email = "TedS@emporium.com",
                Phone = "780-911-4444",
                PasswordHash = "hash",
                PasswordSalt = "salt",
                RewardsPoints = 50,
                Role = UserRole.Customer
            };
            _context.Users.Add(user);
            
                //Creat MenuItems
                var fish = new MenuItem {
                    Id = 1,
                    Name = "Green Tuna",
                    Description = "The Greenest of Tuna in all the sea",
                    Price = 14.99m,
                    Category = MenuItemCategory.MainCourse,
                    IsAvailable = true
                };
                
                var mozza = new MenuItem {
                    Id = 2,
                    Name = "Mozza Sticks",
                    Description = "Deep fried battered mozza. Stick form.",
                    Price = 3.99m,
                    Category = MenuItemCategory.Appetizer,
                    IsAvailable = true
                };
                _context.MenuItems.AddRange(fish, mozza);
                
                //Create Order
                var order = new Order {
                    Id = 1,
                    UserId = 1,
                    Type = OrderType.DineIn,
                    Status = OrderStatus.InProgress,
                    Subtotal = 42.98m,
                    Tax = 1.40m,
                    TipAmount = 2.80m,
                    Total = 47.18m,
                    User = user
                };
                _context.Orders.Add(order);
                
                //Add menu items to Order
                var orderItem1 = new OrderMenuItem {
                    OrderId = 1,
                    MenuItemId = 1,
                    Quantity = 1,
                    Order = order,
                    MenuItem = fish
                };
                
                var orderItem2 = new OrderMenuItem {
                    OrderId = 1,
                    MenuItemId = 2,
                    Quantity = 1,
                    Order = order,
                    MenuItem = mozza
                };
                _context.OrderMenuItems.AddRange(orderItem1, orderItem2);
                
                //Save test data to database
                _context.SaveChanges();
        }

        //Add MenuItems to an Order and checks if they've been added correctly
        [Fact]
        public async Task AbleToLoadMenuItemsCorrectly(){

            var order = await _context.Orders
                .Include(o => o.OrderMenuItems!)
                .ThenInclude(omi => omi.MenuItem)
                .FirstOrDefaultAsync(o => o.Id ==1);
            
            //Check that order is loaded
            //Order is DineIn
            //Order is InProgress
            Assert.NotNull(order);
            Assert.Equal(OrderType.DineIn, order.Type);
            Assert.Equal(OrderStatus.InProgress, order.Status);
            

            //Check MenuItems have loaded
            //2 menu items added
            Assert.NotNull(order.OrderMenuItems);
            Assert.Equal(2, order.OrderMenuItems.Count());

            //Check first MenuItem added to Order
            //Make sure it's loaded
            //That it's the Green Tuna and priced correctly
            var fish = order.OrderMenuItems.FirstOrDefault(o => o.MenuItemId == 1);
            Assert.NotNull(fish);
            Assert.Equal("Green Tuna", fish.MenuItem.Name);
            Assert.Equal(14.99M, fish.MenuItem.Price);

            //Check second MenuItem added to Order
            //Make sure it's loaded
            //That it's Mozza Sticks and priced correctly
            var mozza = order.OrderMenuItems.FirstOrDefault(o => o.MenuItemId == 2);
            Assert.NotNull(mozza);
            Assert.Equal("Mozza Sticks", mozza.MenuItem.Name);
            Assert.Equal(3.99M, mozza.MenuItem.Price);            
        }

        //Clean up
        public void Dispose(){
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}