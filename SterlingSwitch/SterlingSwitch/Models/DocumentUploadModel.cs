using System;
using System.Collections.Generic;
using System.Text;

namespace SterlingSwitch.Models
{
    public class DocumentUploadModel:IBSRequest
    {
        public string NUBAN { get; set; }
        public string ImageByte { get; set; }       
        public string FileType { get; set; }
        public DocumentType DocumentType { get; set; }
    }

}
