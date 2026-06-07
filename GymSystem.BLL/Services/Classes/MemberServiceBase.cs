using GymSystem.DAL.Entities;
using GymSystemG03.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServiceBase
    {
        private readonly IGenericRepository<Booking> bookingRepository;
    }
}