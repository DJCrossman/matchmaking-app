using System;

namespace ExpressConnect
{
    [Serializable]
    public class Player
    {
        public Guid PlayerGuid { get; private set; }
        public double Mu { get; private set; }
        public double Sigma { get; private set; }

        internal Player(Guid playerGuid, double mu, double sigma)
        {
            this.PlayerGuid = playerGuid;
            this.Mu = mu;
            this.Sigma = sigma;
        }
    }
}
