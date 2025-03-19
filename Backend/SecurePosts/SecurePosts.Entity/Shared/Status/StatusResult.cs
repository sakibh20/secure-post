using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurePosts.Entity.Shared.Status
{
    public class StatusResult<T>
    {
        public StatusResult()
        {
            Status = "FAILED";
        }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "Result")]
        public T Result{ get; set; }
}
}
