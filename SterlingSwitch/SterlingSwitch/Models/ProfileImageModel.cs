using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SterlingSwitch.Models
{
   
    public class ProfileImageModel 
    {
        public string Email { get; set; }
        public string Base64Image { get; set; }
        /// <summary>
        /// Image Extention
        /// </summary>
        public string FileType { get; set; }

        
    }
}
