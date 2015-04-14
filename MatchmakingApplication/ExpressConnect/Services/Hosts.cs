using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressConnect
{
    [Serializable]
    public class Hosts
    {
        private List<Host> _hostList = new List<Host>();

        public int Count { get { return this._hostList.Count; } }

        public void Add(Host item)
        {
            this._hostList.Add(item);
        }

        public void Remove(Host item)
        {
            this._hostList.Remove(item);
        }

        public void Clear()
        {
            this._hostList.Clear();
        }

        public void AddRange(List<Host> dataList)
        {
            this._hostList.AddRange(dataList);
        }

        public Host First(Func<Host, bool> func)
        {
            try {
                return this._hostList.First(func);
            } catch (Exception) {
                throw new Exception("There are no avaiable hosts.");
            }
        }

        public void AddPlayerToHost(Host host, Player player)
        {
            _hostList.ForEach(h =>
            {
                if (h.Guid == host.Guid) {
                    h.PlayersList.Add(player);
                }
            });
        }

        public bool Any()
        {
            return this._hostList.Any();
        }

        public void ForEach(Action<Host> action)
        {
            this._hostList.ForEach(action);
        }

        public List<Host> GetHostList()
        {
            return _hostList;
        }
    }
}
