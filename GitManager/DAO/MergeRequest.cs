using System;
using Newtonsoft.Json;

namespace GitManager.DAO
{
    public class MergeRequest
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string State { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("target_branch")] public string TargetBranch { get; set; }

        [JsonProperty("source_branch")] public string SourceBranch { get; set; }

        public User Author { get; set; }
        public User Assignee { get; set; }

        [JsonProperty("created_at")] public DateTime CreateAt { get; set; }

        [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
    }
}