using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GitManager.Entity
{
    [Table("MergeRequest")]
    public class MergeRequestEntity
    {
        [Key] public long Id { get; set; }

        public long RequestId { get; set; }

        public string User { get; set; }
    }
}