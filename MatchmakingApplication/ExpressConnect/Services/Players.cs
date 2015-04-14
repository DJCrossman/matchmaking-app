using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressConnect
{
    [Serializable]
    public class Players
    {
        private List<Player> _playerList = new List<Player>();

        public int Count { get { return this._playerList.Count; } }

        public void Add(Player item)
        {
            this._playerList.Add(item);
        }

        public void AddRange(Player item)
        {
            this._playerList.Add(item);
        }

        public void Remove(Player item)
        {
            this._playerList.Remove(item);
        }

        public void Clear()
        {
            this._playerList.Clear();
        }

        public bool Any(Func<Player,bool> func)
        {
            return this._playerList.Any(func);
        }

        public void ForEach(Action<Player> action)
        {
            this._playerList.ForEach(action);
        }

        public double MeanOfPlayers()
        {
            var playerMuList = new List<double>();
            this._playerList.ForEach(p => playerMuList.Add(p.Mu));
            return playerMuList.Count > 0 ? playerMuList.Average() : 0.0;
        }

        public double StdDevOfPlayers()
        {
            var playerMuList = new List<double>();
            this._playerList.ForEach(p => playerMuList.Add(p.Mu));
            return playerMuList.Count > 0 ? playerMuList.Average() : 0.0;
        }

        public List<Player> GetPlayerList()
        {
            return _playerList;
        } 
    }
}
