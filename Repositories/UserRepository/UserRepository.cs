using AutoMapper;
using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repositories.UserRepository
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<Reader>> GetAllReadersAsync()
        //{
        //    return await _context.Readers.ToListAsync();
        //}

        //public async Task<IEnumerable<Librarian>> GetAllLibrariansAsync()
        //{
        //    return await _context.Librarians.ToListAsync();
        //}

        //public async Task<IEnumerable<User>> GetAllUsersAsync()
        //{
        //    return await _context.Users.IgnoreQueryFilters().ToListAsync();
        //}

        //public async Task<User> GetUserByNameAsync(string name)
        //{
        //    return await _context.Users.FirstOrDefaultAsync(u => u.Username == name);
        //}

        //public async Task<Reader> GetReaderByNameAsync(string name)
        //{
        //    return await _context.Readers.FirstOrDefaultAsync(u => u.Username == name);
        //}

        //public async Task<Librarian> GetLibrarianByNameAsync(string name)
        //{
        //    return await _context.Librarians.FirstOrDefaultAsync(u => u.Username == name);
        //}

        //public async Task<UserDTO> UpdateUserAsync(RegisterDTO registerDTO) // không thỏa tính chất
        //{
            //var role = registerDTO.Role;
            //Reader reader;
            //Librarian librarian;

            //reader = await _context.Readers.FirstOrDefaultAsync(r => r.Username == registerDTO.Username);
            //librarian = await _context.Librarians.FirstOrDefaultAsync(l => l.Username == registerDTO.Username);
            //if ((librarian == null && reader == null) || (librarian != null && reader != null))
            //{
            //    return null;
            //}
            //if(reader!=null)
            //{
            //    var newReader = _mapper.Map<Reader>(reader);
            //    newReader.Password = reader.Password;
            //    newReader.UserId = reader.UserId;
            //    _context.Entry(reader).CurrentValues.SetValues(newReader);
            //    return _mapper.Map<UserDTO>(newReader);
            //}
            //var newLibrarian = _mapper.Map<Librarian>(reader);
            //newLibrarian.Password = librarian.Password;
            //newLibrarian.UserId = librarian.UserId;
            //_context.Entry(librarian).CurrentValues.SetValues(newLibrarian);
            //return _mapper.Map<UserDTO>(newLibrarian);
        //}
    }
}
