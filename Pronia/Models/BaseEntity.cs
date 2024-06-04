namespace Pronia.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool isDeleted {  get; set; } = false;
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set;}
    }
}
