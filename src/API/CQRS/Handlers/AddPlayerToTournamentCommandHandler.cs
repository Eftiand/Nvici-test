
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class AddPlayerToTournamentCommandHandler : IRequestHandler<AddPlayerToTournamentCommand, Result>
{
    private readonly IRepository<Tournament> _tournamentRepository;
    private readonly IRepository<Player> _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPlayerToTournamentCommandHandler(IRepository<Tournament> tournamentRepository, IRepository<Player> playerRepository, IUnitOfWork unitOfWork)
    {
        _tournamentRepository = tournamentRepository;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddPlayerToTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, cancellationToken);
        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        var player = await _playerRepository.GetByIdAsync(request.PlayerId, cancellationToken);
        if (player == null)
        {
            return PlayerErrors.NotFound(request.PlayerId);
        }

        if(tournament.ParentId is not null)
        {
            var parentTournament = await _tournamentRepository.GetByIdAsync(tournament.ParentId.Value, cancellationToken);
            if(parentTournament is null)
            {
                return TournamentErrors.NotFound(tournament.ParentId.Value);
            }

            if(!parentTournament.Players.Exists(p => p.Id == player.Id))
            {
                return PlayerErrors.NotRegisteredInParentTournament(player.Id, parentTournament.Id);
            }
        }



        tournament.Players.Add(player);
        _tournamentRepository.Update(tournament);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}