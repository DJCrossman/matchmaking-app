using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ExpressConnect
{
    [Serializable]
    public class Numberics
    {
        public static double CalculateStdDev(List<double> values)
        {
            double ret = 0;
            if (values.Count() > 1)
            {
                double avg = values.Average();
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
    }
}
