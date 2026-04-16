using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Registrar nuevo usuario
        public async Task<bool> RegisterAsync(string fullName, string email, string password)
        {
            // Verificar si el email ya existe
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            if (exists) return false;

            var user = new User
            {
                FullName = fullName,
                Email = email,
                // Nunca guardes contraseñas en texto plano — esto las encripta
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Validar login
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;

            // Verifica la contraseña encriptada
            bool valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return valid ? user : null;
        }
    }
}