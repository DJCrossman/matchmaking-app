using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using ExpressConnect;
using MatchmakingApplication.Services;

namespace MatchmakingApplication.DataServices

{
    public class MatchmakingDataServices
    {
        /**************************************************
         * Local Data Services
         **************************************************/
        private string playerString = "C:/data/players.dat";
        private string hostString = "C:/data/hosts.dat";
        // TODO: Review Security of local storage

        public void CreateLocalService(MatchmakingService service)
        {
            var bf = new BinaryFormatter();

            var player = File.Create(playerString);
            bf.Serialize(player, service.GetPlayers());
            player.Close();

            var host = File.Create(hostString);
            bf.Serialize(host, service.GetHosts());
            host.Close();
        }

        public MatchmakingService LoadLocalService()
        {
            var bf = new BinaryFormatter();

            var player = File.Open(playerString, FileMode.Open);
            var playerData = (List<Player>)bf.Deserialize(player);
            player.Close();

            var host = File.Open(hostString, FileMode.Open);
            var hostData = (List<Host>)bf.Deserialize(host);
            host.Close();

            return new MatchmakingService(playerData, hostData);
        }

        public void UpdateLocalService(MatchmakingService service)
        {
            var bf = new BinaryFormatter();

            var player = File.Open(playerString, FileMode.Open);
            bf.Serialize(player, service.GetPlayers());
            player.Close();

            var host = File.Open(hostString, FileMode.Open);
            bf.Serialize(host, service.GetHosts());
            host.Close();
        }

        public void DeleteLocalService()
        {
            if (File.Exists(playerString) && File.Exists(hostString)) {
                File.Delete(playerString);
                File.Delete(hostString);
            }
        }

        public bool LocalServiceExist()
        {
            return File.Exists(playerString) && File.Exists(hostString);
        }
    }
}