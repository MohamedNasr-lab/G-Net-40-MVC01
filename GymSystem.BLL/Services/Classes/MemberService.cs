using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystemG03.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberService : IMemberServices
    {
        private readonly IUnitOfWork UnitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(
            CancellationToken ct = default)
        {
            var members = await UnitOfWork.GetRepository<Member>()
                .GetAll(false, ct);

            if (!members.Any())
                return Enumerable.Empty<MemberViewModel>();

            return members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo
            });
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(
            int memberId,
            CancellationToken ct = default)
        {
            var member = await UnitOfWork.GetRepository<Member>()
                .GetById(memberId, ct);

            if (member is null)
                return null;

            var memberVM = new MemberViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = member.Address is not null
                    ? $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
                    : string.Empty
            };

            var activeMembership =
                await UnitOfWork.GetRepository<MemberShip>()
                .FirstOrDefaultAsync(
                    mb => mb.MemberId == memberId &&
                          mb.EndDate > DateTime.Now,
                    false,
                    ct);

            if (activeMembership is not null)
            {
                var activePlan = await UnitOfWork
                    .GetRepository<Plan>()
                    .GetById(activeMembership.PlanId, ct);

                if (activePlan is not null)
                {
                    memberVM.PlanName = activePlan.Name;
                }

                memberVM.MembershipStartDate =
                    activeMembership.CreatedAt.ToShortDateString();

                memberVM.MembershipEndDate =
                    activeMembership.EndDate.ToShortDateString();
            }

            return memberVM;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(
            int memberId,
            CancellationToken ct = default)
        {
            var record = await UnitOfWork.GetRepository<HealthRecord>()
                .FirstOrDefaultAsync(
                    r => r.MemberId == memberId,
                    false,
                    ct);

            if (record is null)
                return null;

            return new HealthRecordViewModel
            {
                Weight = record.Weight,
                Height = record.Height,
                BloodType = record.BloodType,
                Note = record.Note
            };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(
            int memberId,
            CancellationToken ct = default)
        {
            var member = await UnitOfWork.GetRepository<Member>()
                .GetById(memberId, ct);

            if (member is null)
                return null;

            return new MemberToUpdateViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address?.Street,
                City = member.Address?.City,
                BuildingNumber = member.Address?.BuildingNumber ?? 0,
                Photo = member.Photo
            };
        }

        public async Task<bool> CreateMemberAsync(
            CreateMemberViewModel model,
            CancellationToken ct = default)
        {
            var emailExists = await UnitOfWork.GetRepository<Member>()
                .AnyAsync(m => m.Email == model.Email);

            var phoneExists = await UnitOfWork.GetRepository<Member>()
                .AnyAsync(m => m.Phone == model.Phone);

            if (emailExists || phoneExists)
                return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,

                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },

                HealthRecord = new HealthRecord
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            UnitOfWork.GetRepository<Member>().Add(member);

            var result = await UnitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(
            int memberId,
            CancellationToken ct = default)
        {
            var member = await UnitOfWork.GetRepository<Member>()
                .GetById(memberId, ct);

            if (member is null)
                return false;

            var hasFutureSessions =
                await UnitOfWork.GetRepository<Booking>()
                .AnyAsync(
                    b => b.MemberId == memberId &&
                         b.Session.EndDate > DateTime.Now);

            if (hasFutureSessions)
                return false;

            UnitOfWork.GetRepository<Member>()
                .Delete(memberId);

            var result = await UnitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(
            int id,
            MemberToUpdateViewModel model,
            CancellationToken ct = default)
        {
            var member = await UnitOfWork.GetRepository<Member>()
                .GetById(id, ct);

            if (member is null)
                return false;

            if (await UnitOfWork.GetRepository<Member>()
                .AnyAsync(m => m.Email == model.Email && m.Id != id))
                return false;

            if (await UnitOfWork.GetRepository<Member>()
                .AnyAsync(m => m.Phone == model.Phone && m.Id != id))
                return false;

            member.Name = model.Name;
            member.Email = model.Email;
            member.Phone = model.Phone;

            if (member.Address is not null)
            {
                member.Address.Street = model.Street;
                member.Address.City = model.City;
                member.Address.BuildingNumber = model.BuildingNumber;
            }

            member.UpdatedAt = DateTime.Now;

            UnitOfWork.GetRepository<Member>()
                .Update(member);

            var result = await UnitOfWork.CompleteAsync();

            return result > 0;
        }
    }
}