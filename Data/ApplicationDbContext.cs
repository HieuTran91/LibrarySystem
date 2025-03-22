using Microsoft.EntityFrameworkCore;
using LibraryProject.Models;
using OrderApiProject_week2.Models;
namespace LibraryProject.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
		public DbSet<User> Users { get; set; }
		public DbSet<Book> Books { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<Genre> Genres { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            // Thiết lập quan hệ User - Role (1 Role có nhiều Users)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Thiết lập quan hệ Borrowing - User
            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Borrowing)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BorrowingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Thiết lập quan hệ Notification - User (gửi thông báo cá nhân)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.RecipientUser)
                .WithMany()
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Borrowing)
                .WithOne(b=> b.Payment)
                .HasForeignKey<Payment>(p => p.BorrowingID)
                .OnDelete(DeleteBehavior.Restrict);



            // Seed dữ liệu Role mặc định
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Librarian" },
                new Role { RoleId = 3, RoleName = "Reader" }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, GenreName = "History" },
                new Genre { GenreId = 2, GenreName = "Literature" },
                new Genre { GenreId = 3, GenreName = "Economy" },
                new Genre { GenreId = 4, GenreName = "Science" },
                new Genre { GenreId = 5, GenreName = "Children" },
                new Genre { GenreId = 6, GenreName = "Manga-Anime" },
                new Genre { GenreId = 7, GenreName = "Law" },
                new Genre { GenreId = 8, GenreName = "Other" }
            );

            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodId = 1, MethodName = "Cash" },
                new PaymentMethod { PaymentMethodId = 2, MethodName = "Credit Card" },
                new PaymentMethod { PaymentMethodId = 3, MethodName = "Bank Transfer" },
                new PaymentMethod { PaymentMethodId = 4, MethodName = "Momo" }
            );
        }
	}
}
