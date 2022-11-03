using System.ComponentModel.DataAnnotations.Schema;

namespace SeriLogDemo_Net6.Model
{
    public class Work
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedTime { get; set; }

    }
}
