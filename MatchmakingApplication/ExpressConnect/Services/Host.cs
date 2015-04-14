using System;
using UnityEngine;

namespace ExpressConnect
{
    [Serializable]
    public class Host
    {
        public Players PlayersList { get; private set; }
        public string Comment { get; private set; }
        public int MaxConnections { get; private set; }
        public string GameName { get; private set; }
        public string GameType { get; private set; }
        public string Guid { get; private set; }
        public string[] IP { get; private set; }
        public bool PasswordProtected { get; private set; }
        public int PlayerLimit { get; private set; }
        public int Port { get; private set; }
        public bool UseNat { get; private set; }

        public Host(Players players, string comment, int maxConnections, int connectedPlayers, string gameName, string gameType, string guid, string[] ip, bool passwordProtected, int playerLimit, int port, bool useNat)
        {
            this.PlayersList = players;
            this.Comment = comment;
            this.MaxConnections = maxConnections;
            this.GameName = gameName;
            this.GameType = gameType;
            this.Guid = guid;
            this.IP = ip;
            this.PasswordProtected = passwordProtected;
            this.PlayerLimit = playerLimit;
            this.Port = port;
            this.UseNat = useNat;
        }

        public Host() { }

        public static Host HostDataToHost(Players players, HostData data, int maxConnections)
        {
            return new Host(players, data.comment, maxConnections, data.connectedPlayers, data.gameName, data.gameType, data.guid, data.ip, data.passwordProtected, data.playerLimit, data.port, data.useNat);
        }

        public HostData ToHostData()
        {
            return new HostData()
            {
                comment = this.Comment,
                gameName = this.GameName,
                gameType = this.GameType,
                guid = this.Guid,
                ip = this.IP,
                passwordProtected = this.PasswordProtected,
                playerLimit = this.PlayerLimit,
                port = this.Port,
                useNat = this.UseNat
            };
        }
    }
}
