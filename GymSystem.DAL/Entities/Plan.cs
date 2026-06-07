using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Entities
{
    public class Plan: BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public bool IsActive { get; set; }

        public ICollection<MemberShip> Memberships = new HashSet<MemberShip>();
    }
}
