using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    public class PageName
    {
        public string Page { get; set; }
        public int PageCount { get; set; }
        public string PageIcon
        {
            get;
            set;
        }

        public string PageAlias
        {
            get;
            set;
        }
    }
}
