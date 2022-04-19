using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсач.Models
{
    public class Subscription
    {
        public int SUBSCRIPTION_ID { get; set; }

        public DateTime SUBSCRIPTION_DATE { get; set; }

        public Nullable<int> LENGTH { get; set; }
        public Subscription(DateTime date, Nullable<int> length)
        {
            SUBSCRIPTION_DATE = date;
            LENGTH = length;
        }
    }
}
