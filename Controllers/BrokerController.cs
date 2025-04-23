using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using Broker;
using Models;
using System.Text;

namespace BrokerCommandAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokerController : ControllerBase
{
    private readonly ILogger<BrokerController> _logger;

    public BrokerController(ILogger<BrokerController> logger)
    {
        _logger = logger;
    }

    [HttpPost("File/Access")]
    public async Task<IActionResult> SendCommand([FromBody] CommandRequest request)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5000");
        var client = new BrokerProto.BrokerProtoClient(channel);

        var grpcRequest = new ClientCommand
        {
            ClientId = request.ClientId,
            Command = new FileCommand
            {
                Action = request.Action,
                FilePath = request.FilePath,
                Content = Google.Protobuf.ByteString.CopyFromUtf8(request.Content ?? ""),
            }
        };

        var response = await client.ForwardToClientAsync(grpcRequest);

        var data = ((FileResponse)response.Response).Content.ToByteArray();

        return Ok(new
        {
            response.ClientId,
            data
        });
    }
}

