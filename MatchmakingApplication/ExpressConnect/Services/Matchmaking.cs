using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.ExpressConnect;

namespace ExpressConnect
{
    public class Matchmaking
    {
        private Hosts _hostList;
        private readonly double _beta;

        public Matchmaking(int beta)
        {
            _hostList = new Hosts();
            _beta = beta;
        }

        /************************************************************
         * Player Handlers
         ************************************************************/

        static public Player CalculatePlayerStat(Guid guid, List<double> stats)
        {
            return new Player(guid, stats.Any() ? stats.Average() : 0, Numberics.CalculateStdDev(stats));
        }

        static public Player InitializePlayer(Guid guid, double mu, double sigma)
        {
            return new Player(guid, mu, sigma);
        }

        /************************************************************
         * Matchmaking Algorithms
         ************************************************************/

        public Host RequestBestMatchNetworkHostData(Player player)
        {
            if(!_hostList.Any())
                throw new Exception("No Servers to join.");
            // Real Matchmaking code
            double probability = 0.0;
            var bestHost = _hostList.First(h => h.PlayersList.Count < h.MaxConnections);
            _hostList.ForEach(h =>
            {
                if (h.PlayersList.Count < h.MaxConnections) {
                    double temp = RequestQualityMatch(_beta, player.Mu,
                        h.PlayersList.Count > 0 ? h.PlayersList.MeanOfPlayers() : player.Mu,
                        player.Sigma, h.PlayersList.Count > 0 ? h.PlayersList.StdDevOfPlayers() : player.Sigma);
                    probability = temp >= probability ? temp : probability;
                    bestHost = temp >= probability ? h : bestHost;
                }
            });
            return bestHost;
        }

        private double RequestQualityMatch(double beta, double mu, double meanOfPlayers, double sigma, double stdDevOfPlayers)
        {
            var x = 2*(Math.Pow(beta, 2));
            var y = (x + (Math.Pow(sigma, 2)) + (Math.Pow(stdDevOfPlayers, 2)));
            return Math.Sqrt(x / y) * Math.Exp(-(Math.Pow(mu - meanOfPlayers, 2)) / (2 * y));
        }

        /************************************************************
         * Host Handlers
         ************************************************************/

        public void AddHostToList(Host host)
        {
            _hostList.Add(host);
        }

        public void RefreshHostList(List<Host> dataList)
        {
            _hostList.Clear();
            _hostList.AddRange(dataList);
        }

        public void AddPlayerToHost(Host host, Player player)
        {
            if(!host.PlayersList.Any(p => p.PlayerGuid == player.PlayerGuid))
                _hostList.AddPlayerToHost(host, player);
        }

        public List<Host> GetListOfHosts()
        {
            return _hostList.GetHostList();
        }

        public int HostListCount()
        {
            return _hostList.Count;
        }
    }
}
