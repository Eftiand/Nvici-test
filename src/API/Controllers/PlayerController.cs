
using API.CQRS.Commands;
using API.CQRS.Queries;
using API.Data.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PlayerController : ApiController
{
    private readonly IMediator _mediator;

    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlayers()
    {
        var players = await _mediator.Send(new GetAllPlayersQuery());

        return Ok(players.Adapt<IEnumerable<PlayerResponse>>());
    }

    [HttpGet("{playerId}")]
    public async Task<IActionResult?> GetPlayerById(int playerId)
    {
        var player = await _mediator.Send(new GetPlayerByIdQuery(playerId));

        if (player is null)
            return NotFound();

        return Ok(player.Adapt<PlayerResponse>());
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);


        return Ok(result.Value.Adapt<PlayerResponse>());
    }

    [HttpPut("{playerId}")]
    public async Task<IActionResult> UpdatePlayer(int playerId, [FromBody] UpdatePlayerCommand command)
    {
        command.Id = playerId;
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value.Adapt<PlayerResponse>());
    }

    [HttpDelete("{playerId}")]
    public async Task<IActionResult> DeletePlayer(int playerId)
    {
        var result = await _mediator.Send(new DeletePlayerCommand(playerId));

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Player with id: {playerId} deleted");
    }
}

