namespace CRUDapi.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CurrentTime { get; set; }
    }

}
