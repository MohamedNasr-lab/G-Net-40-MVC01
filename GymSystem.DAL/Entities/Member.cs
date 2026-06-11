namespace GymSystem.DAL.Entities
{
    public class Member:GymUser
    {
        public string? Photo { get; set; } = null!;

        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<MemberShip> Memberships = new HashSet<MemberShip>();

        public ICollection<Booking> Bookings = new HashSet<Booking>();
    }
}