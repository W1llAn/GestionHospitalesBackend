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
                var token = Request.Headers["Authorization"].ToString();
                var payload = leerPayload(token);

                if (payload == null || !payload.TryGetValue("TipoEmpleado", out var tipoEmpleado))
                {
                    return BadRequest("Token inválido o sin tipo de empleado.");
                }

                if (tipoEmpleado.ToString() == "Administrador")
                {
                    var consultas = new List<Consulta>();
                    var centros = _configuration.GetSection("grcp:centrosMedicos").Get<List<string>>();

                    foreach (var clave in centros)
                    {
                        var url = _configuration[$"grcp:{clave}"];
                        if (string.IsNullOrEmpty(url)) continue;

                        using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                        {
                            HttpHandler = new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            }
                        });

                        var cliente = new ConsultasService.ConsultasServiceClient(canal);
                        try
                        {
                            var respuesta = await cliente.GetAllConsultasAsync(new ConsultasMedicas.EmptyResponse { }, callOptionsToken());
                            if (respuesta?.Consultas != null)
                            {
                                consultas.AddRange(respuesta.Consultas);
                            }
                        }
                        catch { continue; }
                    }

                    if (consultas.Count == 0)
                        return NotFound("Consulta no encontrada.");

                    return Ok(consultas);
                }

                // Si NO es administrador, consultar solo su centro
                if (!payload.TryGetValue("CentroMedico", out var centroMedico))
                {
                    return BadRequest("No se pudo determinar el centro médico.");
                }

                var claveCentro = $"centroMedico-{centroMedico}";
                var urlCentro = _configuration[$"grcp:{claveCentro}"];
                if (string.IsNullOrEmpty(urlCentro))
                {
                    return BadRequest("Centro médico no configurado.");
                }

                var httpHandlerSolo = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var canalSolo = GrpcChannel.ForAddress(urlCentro, new GrpcChannelOptions { HttpHandler = httpHandlerSolo });
                var clienteSolo = new ConsultasService.ConsultasServiceClient(canalSolo);

                var consultasBuscar = await clienteSolo.GetAllConsultasAsync(new ConsultasMedicas.EmptyResponse { }, callOptionsToken());

                if (consultasBuscar == null)
                {
                    return NotFound("Consulta no encontrada.");
                }
                return consultasBuscar;

            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception)
            {
                return BadRequest("Error general.");
            }

        }
        [HttpGet("Paciente/{cedula}")]
        public async Task<ActionResult<ConsultaList>> GetConsultasCedula(string cedula)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString();
                var payload = leerPayload(token);

                if (payload == null || !payload.TryGetValue("TipoEmpleado", out var tipoEmpleado))
                {
                    return BadRequest("Token inválido o sin tipo de empleado.");
                }

                if (tipoEmpleado.ToString() == "Administrador")
                {
                    var consultas = new List<Consulta>();
                    var centros = _configuration.GetSection("grcp:centrosMedicos").Get<List<string>>();

                    foreach (var clave in centros)
                    {
                        var url = _configuration[$"grcp:{clave}"];
                        if (string.IsNullOrEmpty(url)) continue;

                        using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                        {
                            HttpHandler = new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            }
                        });

                        var cliente = new ConsultasService.ConsultasServiceClient(canal);
                        try
                        {
                            var respuesta = await cliente.GetConsultasReporteAsync(new ConsultaCedulaRequest {Cedula=cedula }, callOptionsToken());
                            if (respuesta?.Consultas != null)
                            {
                                consultas.AddRange(respuesta.Consultas);
                            }
                        }
                        catch { continue; }
                    }

                    if (consultas.Count == 0)
                        return NotFound("Consulta no encontrada.");

                    return Ok(consultas);
                }

                // Si NO es administrador, consultar solo su centro
                if (!payload.TryGetValue("CentroMedico", out var centroMedico))
                {
                    return BadRequest("No se pudo determinar el centro médico.");
                }

                var claveCentro = $"centroMedico-{centroMedico}";
                var urlCentro = _configuration[$"grcp:{claveCentro}"];
                if (string.IsNullOrEmpty(urlCentro))
                {
                    return BadRequest("Centro médico no configurado.");
                }

                var httpHandlerSolo = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var canalSolo = GrpcChannel.ForAddress(urlCentro, new GrpcChannelOptions { HttpHandler = httpHandlerSolo });
                var clienteSolo = new ConsultasService.ConsultasServiceClient(canalSolo);

                var consultasBuscar =await clienteSolo.GetConsultasReporteAsync(new ConsultaCedulaRequest { Cedula = cedula }, callOptionsToken());

                if (consultasBuscar == null)
                {
                    return NotFound("Consulta no encontrada.");
                }
                return consultasBuscar;

            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception)
            {
                return BadRequest("Error general.");
            }
        }

        [HttpGet("Fechas")]
        public async Task<ActionResult<ConsultaList>> GetConsultasFecha([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString();
                var payload = leerPayload(token);

                if (payload == null || !payload.TryGetValue("TipoEmpleado", out var tipoEmpleado))
                {
                    return BadRequest("Token inválido o sin tipo de empleado.");
                }

                if (tipoEmpleado.ToString() == "Administrador")
                {
                    var consultas = new List<Consulta>();
                    var centros = _configuration.GetSection("grcp:centrosMedicos").Get<List<string>>();

                    foreach (var clave in centros)
                    {
                        var url = _configuration[$"grcp:{clave}"];
                        if (string.IsNullOrEmpty(url)) continue;

                        using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                        {
                            HttpHandler = new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            }
                        });

                        var cliente = new ConsultasService.ConsultasServiceClient(canal);
                        try
                        {
                            var respuesta = await cliente.GetConsultasByFechaAsync(new ConsultaFechaRequest { FechaDesde=desde.ToString(),FechaHasta=hasta.ToString()}, callOptionsToken());
                            if (respuesta?.Consultas != null)
                            {
                                consultas.AddRange(respuesta.Consultas);
                            }
                        }
                        catch { continue; }
                    }

                    if (consultas.Count == 0)
                        return NotFound("Consulta no encontrada.");

                    return Ok(consultas);
                }

                // Si NO es administrador, consultar solo su centro
                if (!payload.TryGetValue("CentroMedico", out var centroMedico))
                {
                    return BadRequest("No se pudo determinar el centro médico.");
                }

                var claveCentro = $"centroMedico-{centroMedico}";
                var urlCentro = _configuration[$"grcp:{claveCentro}"];
                if (string.IsNullOrEmpty(urlCentro))
                {
                    return BadRequest("Centro médico no configurado.");
                }

                var httpHandlerSolo = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var canalSolo = GrpcChannel.ForAddress(urlCentro, new GrpcChannelOptions { HttpHandler = httpHandlerSolo });
                var clienteSolo = new ConsultasService.ConsultasServiceClient(canalSolo);

                var consultasBuscar = await clienteSolo.GetConsultasByFechaAsync(new ConsultaFechaRequest { FechaDesde = desde.ToString(), FechaHasta = hasta.ToString() }, callOptionsToken());

                if (consultasBuscar == null)
                {
                    return NotFound("Consulta no encontrada.");
                }
                return consultasBuscar;

            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
            catch (Exception)
            {
                return BadRequest("Error general.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Consulta>> PostPaciente(CreateConsultaRequest consulta)
        {
            try
            {
                var ciudad = this.GetCentro_Medico(consulta.IdCentroMedico).Result.Ciudad;

                var url = _configuration[$"grcp:centroMedico-{ciudad}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions { HttpHandler = httpHandler });
                var cliente = new ConsultasService.ConsultasServiceClient(canal);

                var respuesta = await cliente.CreateConsultaAsync(consulta, callOptionsToken());
                return respuesta;
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id,UpdateConsultaRequest consulta)
        {
            try
            {
                var ciudad = this.GetCentro_Medico(consulta.IdCentroMedico).Result.Ciudad;

                var url = _configuration[$"grcp:centroMedico-{ciudad}"];

                var httpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                using var canal = GrpcChannel.ForAddress(url, new GrpcChannelOptions { HttpHandler = httpHandler });
                var cliente = new ConsultasService.ConsultasServiceClient(canal);

                var respuesta = await cliente.DeleteConsultaAsync(new DeleteConsultaRequest { IdConsultaMedica= consulta.IdConsultaMedica }, callOptionsToken());
                return Ok();
            }
            catch (RpcException ex)
            {
                return erroresGrpc(ex.StatusCode, ex.Status.Detail);
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
        private async Task<Centro_Medico> GetCentro_Medico(int id_centro_medico)
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            using var canal = GrpcChannel.ForAddress(_configuration["grcp:administracion"], new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            var cliente = new AdministracionService.AdministracionServiceClient(canal);

            var centro_Medico = await cliente.GetCentro_MedicoAsync(new Centro_MedicoGet { Id = id_centro_medico }, this.callOptionsToken());

            if (centro_Medico == null)
            {
                throw new RpcException(new Status(Grpc.Core.StatusCode.NotFound, "Centro Medico no encontrado"));
            }

            return centro_Medico;
        }
    }
}
