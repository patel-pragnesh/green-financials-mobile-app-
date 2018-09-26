using Newtonsoft.Json;
using SterlingSwitch.Services.Abstractions.Entities;

namespace switch_mobile.Services.Abstractions.Entities
{
    [JsonObject(Title = "Gender")]
 
    public class Gender: BaseDataObject
    {
        public string Description { get; set; }
    }
}
