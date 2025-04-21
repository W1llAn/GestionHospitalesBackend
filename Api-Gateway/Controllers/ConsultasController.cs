using ConsultasMedicas;
using Grpc.Core;
using Grpc.Net.Client;
using Microservicio_Administracion.Administracion;
using Microservicio_ConsultasMedicas.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Api_Gateway.Controllers
{
    [Route("CentroMedico/Consultas")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        IConfiguration _configuration;
        public ConsultasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<ConsultaList>> GetConsultas()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var payload = leerPayload(token);
                var centroMedico = payload.TryGetValue("CentroMedico", out var cm) ? cm.ToString() : null;

                    var clave = $"centroMedico-{centroMedico}";
                    var url = _configuration[$"grcp:{clave}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });

                var cliente = new ConsultasService.ConsultasServiceClient(canal);
                    var consultaLista = await cliente.GetAllConsultasAsync(new ConsultasMedicas.EmptyResponse { }, callOptionsToken());                

                return consultaLista;
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Sin Token");
            }
        }
        [HttpGet("Paciente/{cedula}")]
        public async Task<ActionResult<ConsultaList>> GetConsultasCedula(string cedula)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var payload = leerPayload(token);
                var centroMedico = payload.TryGetValue("CentroMedico", out var cm) ? cm.ToString() : null;

                var clave = $"centroMedico-{centroMedico}";
                var url = _configuration[$"grcp:{clave}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });

                var cliente = new ConsultasService.ConsultasServiceClient(canal);
                var consultaLista = await cliente.GetConsultasReporteAsync(new ConsultaCedulaRequest { Cedula=cedula}, callOptionsToken());

                return consultaLista;
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Sin Token");
            }
        }

        [HttpGet("Fechas")]
        public async Task<ActionResult<ConsultaList>> GetConsultasFecha([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var payload = leerPayload(token);
                var centroMedico = payload.TryGetValue("CentroMedico", out var cm) ? cm.ToString() : null;

                var clave = $"centroMedico-{centroMedico}";
                var url = _configuration[$"grcp:{clave}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });

                var cliente = new ConsultasService.ConsultasServiceClient(canal);
                var consultaLista = await cliente.GetConsultasByFechaAsync(new ConsultaFechaRequest {FechaDesde=desde.ToString("yyyy/MM/dd"),FechaHasta=hasta.ToString("yyyy/MM/dd") }, callOptionsToken());

                return consultaLista;
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Sin Token");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Consulta>> PostPaciente(CreateConsultaRequest consulta)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var payload = leerPayload(token);
                var centroMedico = payload.TryGetValue("CentroMedico", out var cm) ? cm.ToString() : null;


                var clave = $"centroMedico-{centroMedico}";
                var url = _configuration[$"grcp:{clave}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });
                var cliente = new ConsultasService.ConsultasServiceClient(canal);
                var consultaCreada = await cliente.CreateConsultaAsync(consulta, callOptionsToken());

                return consultaCreada;
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Sin Token");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var payload = leerPayload(token);
                var centroMedico = payload.TryGetValue("CentroMedico", out var cm) ? cm.ToString() : null;

                var clave = $"centroMedico-{centroMedico}";
                var url = _configuration[$"grcp:{clave}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });
                var cliente = new ConsultasService.ConsultasServiceClient(canal);
                var pacientesLista = await cliente.DeleteConsultaAsync(new DeleteConsultaRequest{IdConsultaMedica=id }, callOptionsToken());

                return Ok();
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Sin Token");
            }
        }


        private CallOptions callOptionsToken()
        {
            var token = Request.Headers["Authorization"];
            var metadata = new Metadata {
                { "Authorization",token}
            };
            return new CallOptions
            (
                headers: metadata
            );
        }
        private ObjectResult erroresGrpc(Grpc.Core.StatusCode codigo, string mensaje)
        {
            switch (codigo)
            {
                case Grpc.Core.StatusCode.InvalidArgument:
                    return BadRequest(new { mensaje = mensaje });

                case Grpc.Core.StatusCode.Unauthenticated:
                    return Unauthorized(new { mensaje = mensaje });

                case Grpc.Core.StatusCode.NotFound:
                    return NotFound(new { mensaje = mensaje });

                case Grpc.Core.StatusCode.AlreadyExists:
                    return Conflict(new { mensaje = mensaje });

                default:
                    return StatusCode(500, new { mensaje = "Error interno: " + mensaje });
            }
        }

        private IDictionary<string, object> leerPayload(string token)
        {

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            var handler = new JwtSecurityTokenHandler();
            var jwt=handler.ReadJwtToken(token);
            return jwt.Payload;
        }
    }
}
