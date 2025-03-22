using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using OrderApiProject_week2.Models;

namespace LibraryProject.Repositories.LoginRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewUserAsync(User user)
        {
            //if (user is Reader reader)
            //{
            //    await _context.Readers.AddAsync(reader);
            //}
            //else if (user is Librarian librarian)
            //{
            //    await _context.Librarians.AddAsync(librarian);
            //}
            //else
            //{
            //    throw new InvalidOperationException("User type is not supported.");
            //}

            await _context.SaveChangesAsync();
        }


        public async Task<User> GetLoginAsync(string username)
        {
            var user = await _context.Users
                            .Where(x => x.Username == username)
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync();
            Console.WriteLine(user);
            Console.WriteLine(username);
            return user;
        }
        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshToken(string refreshToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        }
        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users
                    .Include(u => u.RefreshTokens)
                    .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken && !rt.IsRevoked));
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
