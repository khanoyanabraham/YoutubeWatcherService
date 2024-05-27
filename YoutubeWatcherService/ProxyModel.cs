using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeWatcherService
{
    public class ProxyAddress
    {
        public string ip { get; set; }
        public int port_http { get; set; }
        public string country { get; set; }
    }
    public class ProxyData
    {
        public List<ProxyAddress> items { get; set; }
    }
    public class ProxyModel
    {
        public string status { get; set; }
        public ProxyData data { get; set; }
    }
}
