using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcCookieAuthSample.ViewModels
{
    public class InputConsentViewModel
    {
        public string Button { get; set; }
        public  IEnumerable<string> ScopecConsented { get; set; }
        public bool RemeberConsent { get; set; }
        public string ReturnUrl{ get; set; }
    }
}

