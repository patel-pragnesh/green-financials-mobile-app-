using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SterlingSwitch.Services.Constants
{
    public class URLConstants
    {
        

        private static string testUrl
        {
            get => "https://pass.sterlingbankng.com/SPay/api/";
        }
        public static string SwitchNewApiBaseUrl
        {
            get
            {
               return "https://mobile.switch-ng.com/SwitchAPi/";
              // return "https://pass.sterlingbankng.com/SwitchAPI/";
            }
        }

        public static string SwitchApiBaseUrl
        {
            get
            {

 return "https://mobile.switch-ng.com/SwitchAPi/";

            }
        }

        public static string SwitchApiTestBaseUrl
        {
            get
            {
#if DEBUG
                return testUrl;
#else
                return testUrl;
#endif

            }
        }

       public static string switchAPINewBaseURL
        {
            get
            {
               // return "https://pass.sterlingbankng.com/SwitchAPi/api/";
                return "https://mobile.switch-ng.com/SwitchAPi/api/";
            }
        }

        public static string SwitchUrl
        {
            get
            {
                return testUrl;

                //return "https://mobile.switch-ng.com/spay/";
            }
        }

        //public static string SwitchApiLiveBaseUrl { get { return "https://pass.sterlingbankng.com/spay/api/"; } }
        public static string SwitchApiLiveBaseUrl { get { return "https://mobile.switch-ng.com/spay/api/"; } }// made a copy for test
        public static string SpectaAPILiveBaseUrl { get { return "https://www.myspecta.com/SpectaApi/api/Specta/"; } } 
        public static string ExceptionURL { get { return "https://mobile.switch-ng.com/AppAnalytics/api/"; } }
        public static string ExceptionEndPoint { get { return "ExceptionLogs/exceptionlogs"; } }
       
        public static string AppId = "69";

        
    }
}
