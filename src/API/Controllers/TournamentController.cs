using API.CQRS.Commands;
using API.CQRS.Queries;
using API.Data.Entities;
using API.Data.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TournamentController : ApiController
{
    private readonly IMediator _mediator;

    public TournamentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tournament>>> GetTournaments()
    {
        var query = new SearchAllTournamentsQuery(x => x.ParentId == null);

        var tournaments = await _mediator.Send(query);
        return Ok(tournaments.Adapt<IEnumerable<TournamentResponse>>());
    }

    [HttpGet("{tournamentId}")]
    public async Task<ActionResult<Tournament>> GetTournamentById(int tournamentId)
    {
        var query = new GetTournamentByIdQuery(tournamentId);

        var tournaments = await _mediator.Send(query);

        if (tournaments is null)
            return NotFound();

        return Ok(tournaments.Adapt<TournamentResponse>());

    }

    [HttpPost]
    public async Task<ActionResult<Tournament>> CreateTournament([FromBody] CreateTournamentCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpPut("{tournamentId}")]
    public async Task<ActionResult<Tournament>> UpdateTournament(int tournamentId, [FromBody] UpdateTournamentCommand command)
    {
        command.Id = tournamentId;
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Tournament with id: {tournamentId} updated");
    }

    [HttpDelete("{tournamentId}")]
    public async Task<ActionResult> DeleteTournament(int tournamentId)
    {
        var command = new DeleteTournamentCommand(tournamentId);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Tournament with id: {tournamentId} deleted");
    }

    [HttpPost("{tournamentId}/players/{playerId}")]
    public async Task<ActionResult> AddPlayerToTournament(int tournamentId, int playerId)
    {
        var command = new AddPlayerToTournamentCommand(tournamentId, playerId);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Player with id: {playerId} added to tournament with id: {tournamentId}");
    }

    [HttpDelete("{tournamentId}/players/{playerId}")]
    public async Task<ActionResult> RemovePlayerFromTournament(int tournamentId, int playerId)
    {
        var command = new RemovePlayerFromTournamentCommand(tournamentId, playerId);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Player with id: {playerId} removed from tournament with id: {tournamentId}");
    }

    [HttpPost("{tournamentId}/subtournaments/{subTournamentId}")]
    public async Task<ActionResult> AddSubTournament(int tournamentId, int subTournamentId)
    {
        var command = new AddSubTournamentCommand(tournamentId, subTournamentId);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Sub tournament with id: {subTournamentId} added to tournament with id: {tournamentId}");
    }

    [HttpDelete("{tournamentId}/subtournaments/{subTournamentId}")]
    public async Task<ActionResult> RemoveSubTournament(int tournamentId, int subTournamentId)
    {
        var command = new RemoveSubTournamentCommand(tournamentId, subTournamentId);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok($"Sub tournament with id: {subTournamentId} removed from tournament with id: {tournamentId}");
    }
}