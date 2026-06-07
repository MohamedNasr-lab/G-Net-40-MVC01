using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Entities.Enums;
using GymSystem.DAL.Repositories.Classes;
using GymSystemG03.DAL.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Numerics;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberService : IMemberServices
    {
        private readonly IGenericRepository<Member> memberRepository;
        private readonly IGenericRepository<MemberShip> membershipRepository;
        private readonly IGenericRepository<Plan> planRepository;
        private readonly IGenericRepository<HealthRecord> healthRecordRepository;
        private readonly IGenericRepository<Booking> bookingRepository;

        public MemberService(IGenericRepository<Member> memberRepository, IGenericRepository<MemberShip> membershipRepository, IGenericRepository<Plan> PlanRepository,
            IGenericRepository<HealthRecord> HealthRecordRepository, IGenericRepository<Booking> bookingRepository)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            planRepository = PlanRepository;
            healthRecordRepository = HealthRecordRepository;
            this.bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(
            CancellationToken ct = default)
        {
            var members = await memberRepository.GetAll(false, ct);

            if (!members.Any())
                return Enumerable.Empty<MemberViewModel>();

            var membersViewModel = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo
       
            });

            return membersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            
            var member = await memberRepository.GetById(memberId, ct);

            if (member is null) return null;

            var MemberVM = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
            var ActiveMemberShip = await membershipRepository.FirstOrDefaultAsync(mb=> mb.MemberId== memberId&& mb.EndDate>DateTime.Now
            ,false,ct);

            if(ActiveMemberShip is not null)
            {
                var activeplan = await planRepository.GetById(ActiveMemberShip.PlanId,ct);

                MemberVM.PlanName= activeplan.Name;
                MemberVM.MembershipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                MemberVM.MembershipEndDate= ActiveMemberShip.EndDate.ToShortDateString();
            }
            return MemberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await healthRecordRepository.FirstOrDefaultAsync(r=> r.MemberId==memberId,false,ct);

            if(record is null) return null;

            return new HealthRecordViewModel()
            { 
                Weight = record.Weight,
                Height = record.Height,
                BloodType = record.BloodType,
                Note = record.Note,
            };

        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(memberId, ct);

            if (member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber=member.Address.BuildingNumber,
                Photo=member.Photo,

            };
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists= await memberRepository.AnyAsync(m=>m.Email==model.Email);
            var PhoneExists= await memberRepository.AnyAsync(p => p.Phone == model.Phone);

            if (PhoneExists || emailExists)
                return false;

            var memeber = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street=model.Street,
                },
                HealthRecord= new HealthRecord()
                {
                    Weight=model.HealthRecordViewModel.Weight,
                    Height=model.HealthRecordViewModel.Height,
                    BloodType=model.HealthRecordViewModel.BloodType,
                    Note=model.HealthRecordViewModel.Note,
                }
            };

            memberRepository.Add(memeber);
            var result = await memberRepository.CompleteAsync();

            return result > 0;
        }


        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(memberId, ct);

            if (member is null) return false;

            var HasFutureSessions = await bookingRepository.AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now);
            if(!HasFutureSessions) return false;

            memberRepository.Delete(member.Id);

            var result= await memberRepository.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await memberRepository.GetById(id, ct);

            if (member == null) return false;

            if (await memberRepository.AnyAsync(m => m.Email == model.Email && m.Id != id)) return false;
            if (await memberRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != id)) return false;

            member.Email= model.Email;
            member.Phone= model.Phone;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;
            memberRepository.Update(member);

            var result=await memberRepository.CompleteAsync();

            return result > 0;
        }
    }
}