using AutoMapper;
using Firebase.Auth;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;

namespace LibraryProject.Services.NotificationService
{
    public class NotificationService : INotificationObserver
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        public NotificationService(IRepository<Notification> notificationRepository, IMapper mapper, IRepository<Role> roleRepository)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotifications()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsOfReader(int userId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationsOfReader = notifications.Where(n => n.RecipientUserId == userId);
            return _mapper.Map<IEnumerable<NotificationDTO>>(notificationsOfReader);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsRole(int roleId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationsOfRole = notifications.Where(n => n.RecipientRoleId == roleId);
            return _mapper.Map<IEnumerable<NotificationDTO>>(notificationsOfRole);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsReaded()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationsReaded = notifications.Where(n => n.IsRead).ToList();
            return _mapper.Map<IEnumerable<NotificationDTO>>(notificationsReaded);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsForUserAsync(int userId, int? roleId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationsReaded = notifications.Where(n => n.RecipientUserId == userId || (roleId != null && n.RecipientRoleId == roleId));
            return _mapper.Map<IEnumerable<NotificationDTO>>(notificationsReaded);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsForRole(int roleId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationsReaded = notifications.Where(n => n.RecipientRoleId == roleId);
            return _mapper.Map<IEnumerable<NotificationDTO>>(notificationsReaded);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsForUser(int userId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var notificationsReaded = notifications.Where(n =>  n.RecipientUserId == userId);
            return _mapper.Map<IEnumerable<NotificationDTO>>(notificationsReaded);
        }

        public async Task<NotificationDTO> UpdateNotificationReaded(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if(notification!=null)
            {
                notification.IsRead = true;
                await _notificationRepository.UpdateAsync(notification);
                return _mapper.Map<NotificationDTO>(notification);
            }
            return null;
        }
        public async Task<NotificationDTO> AddNotificationToBorrowBook(BorrowingDTO borrowingDTO, string username, int roleId)
        {
            var notification = new Notification
            {
                Message = $"User {username} has borrowed the book '{borrowingDTO.BookTitle}'.",
                RecipientRoleId = roleId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _notificationRepository.AddAsync(notification);
            return _mapper.Map<NotificationDTO> (notification);
        }

        public async Task<NotificationDTO> AddNotificationToReturnBook(BorrowingDTO borrowingDTO, string username, int roleId)
        {
            var notification = new Notification
            {
                Message = $"User {username} has returned the book '{borrowingDTO.BookTitle}'.",
                RecipientRoleId = roleId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _notificationRepository.AddAsync(notification);
            return _mapper.Map<NotificationDTO>(notification);
        }

        public async Task SendNotificationToRoleAsync(string message, int roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null) return;

            var notification = new Notification
            {
                Message = message,
                RecipientRoleId = roleId,
                RecipientRole = role,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _notificationRepository.AddAsync(notification);
        }

        
    }
}
