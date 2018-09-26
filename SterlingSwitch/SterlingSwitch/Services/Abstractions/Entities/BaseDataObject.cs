using Newtonsoft.Json;
using System;

namespace SterlingSwitch.Services.Abstractions.Entities
{
    public interface IBaseDataObject
    {
        string Id { get; set; }
    }
    public class BaseDataObject : IBaseDataObject
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        
        public string RefrenceId { get; set; }
        public string RequestType { get; set; }

        public string Translocation { get; set; }

        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
            RefrenceId = "00055"+ Guid.NewGuid().ToString();
            //Translocation = App.GetUserLocation;
        }
    }
}
