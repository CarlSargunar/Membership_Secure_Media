using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
namespace MemberTest.Code
{
    public class AssignMembersToPremiumRoleHandler : INotificationHandler<MemberSavedNotification>
    {
        private const string RoleName = "Premium";

        private readonly IMemberService _memberService;
        private readonly ILogger<AssignMembersToPremiumRoleHandler> _logger;

        public AssignMembersToPremiumRoleHandler(
            IMemberService memberService,
            ILogger<AssignMembersToPremiumRoleHandler> logger)
        {
            _memberService = memberService;
            _logger = logger;
        }

        public void Handle(MemberSavedNotification notification)
        {
            foreach (IMember member in notification.SavedEntities)
            {
                if (_memberService.GetAllRoles(member.Id).Contains(RoleName))
                {
                    continue;
                }
                _logger.LogInformation("Automatically assigning member with ID: {memberId} to role: {roleName}", member.Id, RoleName);
                _memberService.AssignRole(member.Id, RoleName);
            }
        }
    }
}