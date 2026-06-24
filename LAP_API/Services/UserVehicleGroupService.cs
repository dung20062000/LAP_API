using LAP_API.DTOs.UserVehicleGroup;
using LAP_API.Models;
using LAP_API.Repositories;
using Microsoft.Extensions.Logging;

namespace LAP_API.Services;

/// <summary>
/// Service xử lý nghiệp vụ quản lý gán nhóm xe cho người dùng.
/// Bao gồm: lấy danh sách user, lấy nhóm đã/chưa gán dạng cây, và cập nhật gán nhóm.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
/// <seealso cref="LAP_API.Services.BaseService" />
/// <seealso cref="LAP_API.Services.IUserVehicleGroupService" />
public class UserVehicleGroupService : BaseService, IUserVehicleGroupService
{
    public UserVehicleGroupService(IUnitOfWork unitOfWork, ILogger<UserVehicleGroupService> logger)
        : base(unitOfWork, logger)
    {
    }


    /// <summary>
    /// Lấy danh sách người dùng đang hoạt động của công ty (CompanyId=15076).
    /// Lọc: không bị khóa, không bị xóa. Sắp xếp theo Fullname.
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _unitOfWork.UserVehicleGroups.GetActiveUsersAsync();

        return users.Select(u => new UserDto
        {
            UserId = u.Id,
            Username = u.Username,
            Fullname = u.Fullname,
        }).ToList();
    }


    /// <summary>
    /// Lấy danh sách nhóm xe chưa được gán cho người dùng cụ thể, dưới dạng cây.
    /// Chỉ lấy các nhóm thuộc CompanyId=15076, không bị xóa,
    /// và chưa tồn tại trong Admin.UserVehicleGroup của user đó.
    /// </summary>
    /// <param name="userId">ID người dùng cần kiểm tra.</param>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task<List<VehicleGroupNodeDto>> GetUnassignedGroupsAsync(Guid userId)
    {
        var allGroups = (await _unitOfWork.Groups.GetAllActiveAsync()).ToList();
        var assignedIds = (await _unitOfWork.UserVehicleGroups.GetAssignedGroupIdsAsync(userId)).ToHashSet();

        // Lọc ra nhóm chưa được gán
        var unassignedGroups = allGroups
            .Where(g => !assignedIds.Contains(g.Id))
            .ToList();

        return BuildTree(unassignedGroups, allGroups);
    }


    /// <summary>
    /// Lấy danh sách nhóm xe đã gán cho người dùng cụ thể, dưới dạng cây.
    /// </summary>
    /// <param name="userId">ID người dùng cần kiểm tra.</param>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task<List<VehicleGroupNodeDto>> GetAssignedGroupsAsync(Guid userId)
    {
        var allGroups = (await _unitOfWork.Groups.GetAllActiveAsync()).ToList();
        var assignedIds = (await _unitOfWork.UserVehicleGroups.GetAssignedGroupIdsAsync(userId)).ToHashSet();

        // Lọc ra nhóm đã được gán
        var assignedGroups = allGroups
            .Where(g => assignedIds.Contains(g.Id))
            .ToList();

        return BuildTree(assignedGroups, allGroups);
    }


    /// <summary>
    /// Lưu danh sách gán nhóm xe mới cho người dùng (chiến lược replace-all).
    /// </summary>
    /// <param name="userId">ID người dùng.</param>
    /// <param name="request">Request chứa danh sách GroupId mới cần gán.</param>
    /// <Modified>
    /// Name     Date         Comments
    /// dungbt   6/11/2026    created
    /// </Modified>
    public async Task AssignGroupsAsync(Guid userId, AssignGroupsRequest request)
    {
        var allGroups = await _unitOfWork.Groups.GetAllActiveAsync();
        await _unitOfWork.UserVehicleGroups.UpdateAssignedGroupsAsync(userId, request.GroupIds, allGroups);
    }


    /// <summary>
    /// Chuyển đổi danh sách nhóm phẳng thành cây phân cấp.
    /// Nếu node cha không nằm trong tập hợp được truyền vào nhưng tồn tại trong allGroups,
    /// thì node đó được coi là node gốc ảo - các con vẫn hiển thị đúng vị trí.
    /// </summary>
    /// <param name="targetGroups">Danh sách nhóm cần xây cây (assigned hoặc unassigned).</param>
    /// <param name="allGroups">Toàn bộ nhóm của công ty (dùng để tra cứu cha).</param>
    private static List<VehicleGroupNodeDto> BuildTree(List<Group> targetGroups, List<Group> allGroups)
    {
        if (!targetGroups.Any())
            return new List<VehicleGroupNodeDto>();

        var allGroupsDict = allGroups.ToDictionary(g => g.Id);
        var nodesToInclude = new HashSet<int>();

        // Xây dựng tập hợp các node cần hiển thị (bao gồm cả các node cha bị thiếu)
        foreach (var group in targetGroups)
        {
            var current = group;
            nodesToInclude.Add(current.Id);

            // Truy ngược lên để thêm các node cha ảo nếu chưa có
            while (current.ParentVehicleGroupID.HasValue)
            {
                var parentId = current.ParentVehicleGroupID.Value;
                if (!nodesToInclude.Add(parentId))
                    break; // Cha này đã được thêm

                if (allGroupsDict.TryGetValue(parentId, out var parentGroup))
                {
                    current = parentGroup;
                }
                else
                {
                    break;
                }
            }
        }

        // Lọc danh sách gốc để giữ đúng thứ tự
        var orderedNodes = allGroups.Where(g => nodesToInclude.Contains(g.Id)).ToList();

        // Tạo dictionary nhanh để tra cứu node
        var nodeDict = orderedNodes.ToDictionary(
            g => g.Id,
            g => new VehicleGroupNodeDto
            {
                Key = g.Id.ToString(),
                Label = g.GroupName,
                Data = g.Id,
                ParentId = g.ParentVehicleGroupID,
                Children = new List<VehicleGroupNodeDto>()
            });

        var rootNodes = new List<VehicleGroupNodeDto>();

        // Xây dựng cây
        foreach (var node in orderedNodes)
        {
            var dto = nodeDict[node.Id];

            if (dto.ParentId.HasValue && nodeDict.TryGetValue(dto.ParentId.Value, out var parentNode))
            {
                // Gắn vào cha
                parentNode.Children.Add(dto);
            }
            else
            {
                // Node gốc
                rootNodes.Add(dto);
            }
        }

        return rootNodes;
    }
}
