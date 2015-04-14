using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ExpressConnect;
using MatchmakingApplication.DataServices;

namespace MatchmakingApplication.Services
{
    [Serializable]
    public class MatchmakingService
    {
        private Matchmaking _matchmaking;
        private MatchmakingDataServices _dataServices;
        private List<Player> _activePlayers;
        private List<Host> _activeHosts;
        private const int MaxPlayers = 200;
        private const int MaxConnections = 8;

        public MatchmakingService(List<Player> players, List<Host> hosts)
        {
            this._activeHosts = hosts;
            this._activePlayers = players;
        }

        public MatchmakingService()
        {
            _dataServices = new MatchmakingDataServices();
            _matchmaking = new Matchmaking(4);
            _activePlayers = new List<Player>();
            _activeHosts = new List<Host>();

            if (!_dataServices.LocalServiceExist()) {
                // if it doesn't exist create it
                var rand = new Random();
                for (int i = 1; i <= MaxPlayers; i++)
                    _activePlayers.Add(Matchmaking.InitializePlayer(Guid.NewGuid(), rand.Next(10, 40), 8.3));
                _dataServices.CreateLocalService(this);
            } else {
                // if it does exist grab it
                GetMatchmakingServiceData();
            }
        }

        public List<Host> GetHosts()
        {
            return _activeHosts;
        }

        public List<Player> GetPlayers()
        {
            return _activePlayers;
        }

        public List<Host> GetAmountOfHosts(int amount)
        {
            if (amount == _matchmaking.HostListCount()) {
                // if the count hasn't changed return old list
                return _matchmaking.GetListOfHosts();
            }
            else {
                // if the count has changed return new list
                var hosts = new List<Host>();
                _activeHosts.Clear();
                for (int i = 1; i <= amount; i++) 
                    hosts.Add(new Host(new Players(), "Test Comment " + i, MaxConnections, 0, "Server " + i, "Terminal offense", Guid.NewGuid().ToString(), RandomIpGenerator.GetRandomIp(), false, MaxConnections, 23466, true));
                
                _matchmaking.RefreshHostList(hosts);
                _activeHosts.AddRange(hosts);
                _dataServices.UpdateLocalService(this);
                return _matchmaking.GetListOfHosts();
            }
        }

        public void StartMatchmaking()
        {
            var matched = _matchmaking.GetListOfHosts().Any(h => h.PlayersList.Count > 0);
            if (!matched) {
                _activePlayers.ForEach(p =>
                {
                    _matchmaking.AddPlayerToHost(_matchmaking.RequestBestMatchNetworkHostData(p), Matchmaking.InitializePlayer(p.PlayerGuid, p.Mu, p.Sigma));
                });
                _dataServices.UpdateLocalService(this);
            }
        }

        public void GetMatchmakingServiceData()
        {
            var service = _dataServices.LoadLocalService();
            _activePlayers = service.GetPlayers();
            _activeHosts = service.GetHosts();
            _matchmaking.RefreshHostList(_activeHosts);
        }

        public List<Player> GetPlayersByHostID(Guid id)
        {
            return _matchmaking.GetListOfHosts().First(h => Guid.Parse(h.Guid) == id).PlayersList.GetPlayerList();
        }
    }
    public static class RandomIpGenerator
    {
        private static Random _random = new Random();
        public static string[] GetRandomIp()
        {
            var strList = new List<string>();
            strList.Add(string.Format("{0}.{1}.{2}.{3}", _random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255)));
            return strList.ToArray();
        }
    }
}