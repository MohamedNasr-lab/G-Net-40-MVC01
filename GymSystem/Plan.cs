namespace GymSystem
{
    public class Plan
    {
        public int Id {  get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price {  get; set; }

        public int Duration {  get; set; }
        
        public bool IsActive {  get; set; }

    }
}
