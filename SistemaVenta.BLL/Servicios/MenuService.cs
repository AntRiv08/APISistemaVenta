using AutoMapper;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IGenericRepository<MenuRol> _menuRolRepositorio;
        private readonly IGenericRepository<Menu> _menuRepositorio;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepositorio,
            IGenericRepository<MenuRol> menuRolRepositorio,
            IGenericRepository<Menu> menuRepositorio,
            IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _menuRolRepositorio = menuRolRepositorio;
            _menuRepositorio = menuRepositorio;
            _mapper = mapper;
        }
        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tblUsuario = await _usuarioRepositorio.Consultar(u => u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tblMenuRol = await _menuRolRepositorio.Consultar();
            IQueryable<Menu> tblMenu = await _menuRepositorio.Consultar();

            try
            {
                IQueryable<Menu> tblResultado = (from u in tblUsuario
                                                 join mr in tblMenuRol on
                                                 u.IdRol equals mr.IdRol
                                                 join m in tblMenu on
                                                 mr.IdMenu equals m.IdMenu
                                                 select m).AsQueryable();
                var listaMenu = tblResultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listaMenu);
            }
            catch
            {
                throw;
            }
        }
    }
}
