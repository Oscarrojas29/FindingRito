using System;
using Newtonsoft.Json;

namespace FindingRito.Models
{
    public class Member
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("CreatedUtcTime")]
        public string CreatedUtcTime { get; set; }
    }
}
