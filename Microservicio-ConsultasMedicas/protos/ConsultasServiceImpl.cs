using Grpc.Core;
using Grpc.Net.Client;
using Microservicio_Administracion.Data;
using Microsoft.EntityFrameworkCore;
using ConsultasMedicas;


namespace Microservicio_Administracion.protos
{
    public class ConsultasServiceImpl :ConsultasService.ConsultasServiceBase
    {
        private readonly DataContext _context;

        public ConsultasServiceImpl(DataContext context)
        {
            _context = context;
        }

        public override Task<Consulta> GetConsultaCedula(ConsultaCedulaRequest request, ServerCallContext context)
        {



            return base.GetConsultaCedula(request, context);
        }
    }
}