using System;
using MatchmakingApplication.Services;
using Nancy.Responses.Negotiation;
using System.Collections;

namespace MatchmakingApplication
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        private readonly MatchmakingService _matchmakingService;
        public IndexModule()
        {
            _matchmakingService = new MatchmakingService();
            Get["/"] = x => GetIndexView();
            Get["/players/{amount}"] = x => GetListOfPlayers(x.amount);
            Get["/hosts/{amount}"] = x => GetNumberOfHosts(x.amount);
            Get["/host/match/{id}"] = x => GetPlayersByMatch(x.id);
            Post["/host/match/"] = x => StartMatchmaking();
        }

        private object GetPlayersByMatch(Guid id)
        {
            var players = _matchmakingService.GetPlayersByHostID(id);
            return Response.AsJson(players);
        }

        private object StartMatchmaking()
        {
            _matchmakingService.StartMatchmaking();
            return HttpStatusCode.NoContent;
        }

        private object GetNumberOfHosts(int amount)
        {
            var hosts = _matchmakingService.GetAmountOfHosts(amount);
            return hosts;
        }

        private Negotiator GetIndexView()
        {
            return View["Index"];
        }

        private object GetListOfPlayers(int amount)
        {
            var players = _matchmakingService.GetPlayers();
            return Response.AsJson(players);
        }

        
    }
}