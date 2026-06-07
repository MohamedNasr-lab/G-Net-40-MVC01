using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Entities
{
    public class MemberShip:BaseEntity
    {
        public Member Member { get; set; } = null!;

        public int MemberId { get; set; }

        public Plan Plan { get; set; }=null!;

        public int PlanId { get; set; }

        public DateTime EndDate { get; set; }

        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";

        public bool IsActive=> EndDate > DateTime.Now;


    }
}
