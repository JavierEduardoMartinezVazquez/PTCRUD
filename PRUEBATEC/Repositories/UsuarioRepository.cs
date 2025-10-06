using PRUEBATEC.Data;
using PRUEBATEC.Models;

namespace PRUEBATEC.Repositories
{
    public interface IUsuarioRepository
    {
        UsuarioModel ValidarUsuario(string usuario, string password);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public UsuarioModel ValidarUsuario(string usuario, string password)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Usuario == usuario && u.Password == password);
        }
    }
}
