using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerLukaN.Client
{
    public class Config
    {
        public const string ApiRootUrl = "https://localhost:44365/";
        public const string ApiResourceUrl = ApiRootUrl + "api/";
        public const string TokenUrl = ApiRootUrl + "get-token";
        
    }
}
