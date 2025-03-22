using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Services.NotificationService
{
    public class OverdueNotificationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OverdueNotificationService> _logger;

        public OverdueNotificationService(IServiceScopeFactory scopeFactory, ILogger<OverdueNotificationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope()) 
                {
                    var borrowingRepository = scope.ServiceProvider.GetRequiredService<IRepository<Borrowing>>();
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<IRepository<Notification>>();
                    var roleRepository = scope.ServiceProvider.GetRequiredService<IRepository<Role>>();
                    var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
                    var bookRepository = scope.ServiceProvider.GetRequiredService<IRepository<Book>>();

                    var borrowings = await borrowingRepository.GetAllAsync();
                    var overdueBorrowings = borrowings
                        .Where(b => (b.DueDate.AddDays(1)) < DateTime.UtcNow && b.ReturnDate == null)
                        .ToList();

                    foreach(var borrowing in overdueBorrowings)
                    {
                        var user = await userRepository.GetByIdAsync(borrowing.UserId);
                        var book = await bookRepository.GetByIdAsync(borrowing.BookID);
                        borrowing.Book = book;
                        borrowing.User = user;
                    }    

                    foreach (var borrowing in overdueBorrowings)
                    {
                        var roles = await roleRepository.GetAllAsync();
                        var admin = roles.FirstOrDefault(r => r.RoleName == "Admin");
                        var librarian = roles.FirstOrDefault(r => r.RoleName == "Librarian");
                        var notificationAdmin = new Notification
                        {
                            Message = $"User {borrowing.User.Username} did not return '{borrowing.Book.Title}', OVERDUE!",
                            RecipientRoleId = admin.RoleId,
                            CreatedAt = DateTime.UtcNow
                        };

                        var notificationLibrarian = new Notification
                        {
                            Message = $"User {borrowing.User.Username} did not return '{borrowing.Book.Title}', OVERDUE!",
                            RecipientRoleId = librarian.RoleId,
                            CreatedAt = DateTime.UtcNow
                        };

                        var notificationUser = new Notification
                        {
                            Message = $"Your book '{borrowing.Book.Title}' is overdue! Please return it as soon as possible.",
                            RecipientUserId = borrowing.UserId,
                            CreatedAt = DateTime.UtcNow
                        };

                        await notificationRepository.AddAsync(notificationAdmin);
                        await notificationRepository.AddAsync(notificationLibrarian);
                        await notificationRepository.AddAsync(notificationUser);
                    }
                }

                _logger.LogInformation("Checked overdue books and sent notifications.");

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
