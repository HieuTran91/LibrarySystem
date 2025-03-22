using LibraryProject.DTOs;
using LibraryProject.Models;

namespace LibraryProject.Services.NotificationService
{
    public interface INotificationObserver
    {
        Task<IEnumerable<NotificationDTO>> GetNotifications();
        Task<IEnumerable<NotificationDTO>> GetNotificationsReaded();
        Task<NotificationDTO> UpdateNotificationReaded(int id);
        //Task<NotificationDTO> AddNotificationToBorrowBook(BorrowingDTO borrowingDTO, string username);
        Task<IEnumerable<NotificationDTO>> GetNotificationsOfReader(int userId);
        Task<IEnumerable<NotificationDTO>> GetNotificationsRole(int roleId);
        Task<IEnumerable<NotificationDTO>> GetNotificationsForUserAsync(int userId, int? roleId);
        Task<NotificationDTO> AddNotificationToBorrowBook(BorrowingDTO borrowingDTO, string username, int roleId);
        Task<NotificationDTO> AddNotificationToReturnBook(BorrowingDTO borrowingDTO, string username, int roleId);
        Task SendNotificationToRoleAsync(string message, int roleId);
        Task<IEnumerable<NotificationDTO>> GetNotificationsForRole(int roleId);
        Task<IEnumerable<NotificationDTO>> GetNotificationsForUser(int userId);
    }
}
