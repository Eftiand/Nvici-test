
using System.Net;
using System.Text;
using System.Text.Json;
using API.CQRS.Commands;
using API.Data.Entities;
using API.Data.Responses;
using FluentAssertions;
using IntegrationTests.Init;
using Xunit;

namespace IntegrationTests;

public class TournamentTests : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly string _baseUrl = "/api/v1/tournament";

    public TournamentTests(IntegrationWebAppFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTournaments_ShouldReturn_ListOfAllTournaments()
    {
        var response = await _client.GetAsync($"{_baseUrl}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tournaments = JsonSerializer.Deserialize<List<TournamentResponse>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        tournaments.Should().HaveCountGreaterThan(10);
    }

    [Fact]
    public async Task GetTournamentById_ShouldReturn_SingleTournament()
    {
        var response = await _client.GetAsync($"{_baseUrl}/1");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tournament = JsonSerializer.Deserialize<TournamentResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        tournament.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTournamentById_ShouldReturn_NotFound()
    {
        var response = await _client.GetAsync($"{_baseUrl}/1000");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTournament_ShouldReturn_NewTournament()
    {
        var newTournament = new CreateTournamentCommand("New Tournament");

        var json = JsonSerializer.Serialize(newTournament);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"{_baseUrl}", content);
        response.EnsureSuccessStatusCode();

        var contentResponse = await response.Content.ReadAsStringAsync();
        var tournament = JsonSerializer.Deserialize<TournamentResponse>(contentResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        tournament.Should().NotBeNull();
        tournament!.Name.Should().Be(newTournament.Name);
    }

    [Fact]
    public async Task UpdateTournament_ShouldReturn_Success()
    {
        var updateTournament = new UpdateTournamentCommand("Updated Tournament");

        var json = JsonSerializer.Serialize(updateTournament);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{_baseUrl}/19", content);
        response.EnsureSuccessStatusCode();
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateTournament_ShouldReturn_BadRequest()
    {
        var updateTournament = new UpdateTournamentCommand("Updated Tournament");

        var json = JsonSerializer.Serialize(updateTournament);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"{_baseUrl}/1000", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteTournament_ShouldReturn_Success()
    {
        var response = await _client.DeleteAsync($"{_baseUrl}/1");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Tournament with id: 1 deleted");
    }

    [Fact]
    public async Task AddSubTourToTournament_ShouldReturn_Success()
    {
        var response = await _client.PostAsync($"{_baseUrl}/10/subtournaments/20", null);
        response.EnsureSuccessStatusCode();
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task AddSubTourToTournament_WithMaxDepthShouldReturn_Error()
    {
        var response = await _client.PostAsync($"{_baseUrl}/5/subtournaments/6", null);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}