
using Microsoft.EntityFrameworkCore;
using Grpc.Core;
using Microservicio_Administracion.Data;
using Microservicio_Administracion.Administracion;

namespace Microservicio_Administracion.Protos
{
    public class AdministracionServiceImpl : AdministracionService.AdministracionServiceBase
    {
        private readonly AppDbContext _context;

        public AdministracionServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public override async Task<Administracion.Empleado> GetEmpleado(EmpleadoGet request, ServerCallContext context)
        {

            if (request == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "El Empleado no puede ser null"));
            }

            var e = await _context.Empleados
                .Include(e => e.Centro_Medico)
                .Include(e => e.Especialidad)
                .Include(e => e.Tipo_Empleado)
                .FirstOrDefaultAsync(e => e.Id == request.Id);



            return new Administracion.Empleado
            {
                Email=e.email,
                Id=e.Id,
                Cedula=e.cedula,
                Nombre=e.nombre,
                CentroMedicoID=e.centro_medicoID,
                EspecialidadID=e.especialidadID,
                TipoEmpleadoID=e.tipo_empleadoID,
                Salario=e.salario,
                Telefono=e.telefono,
                Especialidad=new Administracion.Especialidad
                {
                    Especialidad_=e.Especialidad.especialidad,
                    Id=e.Especialidad.Id

                },
                TipoEmpleado=new Administracion.Tipo_Empleado
                {
                    Id=e.Tipo_Empleado.Id,
                    Tipo=e.Tipo_Empleado.tipo
                },
                CentroMedico=new Administracion.Centro_Medico
                {
                    Id=e.Centro_Medico.Id,
                    Ciudad=e.Centro_Medico.ciudad,
                    Direccion=e.Centro_Medico.direccion,
                    Nombre=e.Centro_Medico.nombre
                }

            };
        }
        
    }
}
