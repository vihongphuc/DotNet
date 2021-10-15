using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetSPODataListCallBackURL.Models
{
    public class AzureActiveDirectory
    {
        public string Url { get; set; }
        public string TanentId { get; set; }
        public string AppClientId { get; set; }
        public string AppClientSecret { get; set; }
        public string CallBack { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
