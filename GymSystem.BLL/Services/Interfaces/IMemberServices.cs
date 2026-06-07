using GymSystem.BLL.ViewModels.MembersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IMemberServices
    {   
        
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);

        
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);

        //POST
        
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        
        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
