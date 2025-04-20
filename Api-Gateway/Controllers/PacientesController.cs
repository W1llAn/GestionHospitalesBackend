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
    [Route("CentroMedico/Pacientes")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        IConfiguration _configuration;
        public PacientesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<GetPacienteListaResponse>> GetPacientes()
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

                var cliente = new PacienteService.PacienteServiceClient(canal);
                    var pacientesLista = await cliente.GetAllPacienteAsync(new EmptyResponse { }, callOptionsToken());                

                return pacientesLista;
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


        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteModel>> GetPaciente(int id)
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
                var cliente = new PacienteService.PacienteServiceClient(canal);
                var paciente = await cliente.GetPacienteAsync(new GetPacienteRequest { IdPaciente=id}, callOptionsToken());

                return paciente.Paciente;
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
        public async Task<ActionResult<PacienteModel>> PostPaciente(CrearPacienteRequest paciente)
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
                var cliente = new PacienteService.PacienteServiceClient(canal);
                var pacienteCrear = await cliente.CrearPacienteAsync(paciente, callOptionsToken());

                return pacienteCrear.Paciente;
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

        [HttpPut("{id}")]
        public async Task<ActionResult<PacienteModel>> PutPaciente(int id, ActualizarPacienteRequest paciente)
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
                var cliente = new PacienteService.PacienteServiceClient(canal);
                var pacientesLista = await cliente.ActualizarPacienteAsync(paciente, callOptionsToken());

                return pacientesLista.Paciente;
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
                var cliente = new PacienteService.PacienteServiceClient(canal);
                var pacientesLista = await cliente.EliminarPacienteAsync(new EliminarPacienteRequest {IdPaciente=id }, callOptionsToken());

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
