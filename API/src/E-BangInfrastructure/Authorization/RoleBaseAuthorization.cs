using E_BangDomain.Settings;
using Microsoft.AspNetCore.Authorization;

namespace E_BangInfrastructure.Authorization
{
    public static class PolicyConstant
    {
        public const string RequireAdmin = "RequireAdmin";

        public const string RequireModerator = "RequireModerator";

        public const string RequireHeadTeacher = "RequireHeadTeacher";

        public const string RequireTeacher = "RequireTeacher";

        public const string RequireStudent = "RequireStudent";
    }
    public static class PolicyFactory
    {
        public static AuthorizationPolicy RequireAdmin()
        {
            return new AuthorizationPolicyBuilder()
            .RequireRole(Enum.GetNames<EAuthorizationRole>().Take((int)EAuthorizationRole.admin))
                .Build();
        }

        public static AuthorizationPolicy RequireModerator()
        {
            return new AuthorizationPolicyBuilder()
            .RequireRole(Enum.GetNames<EAuthorizationRole>().ToList().Take((int)EAuthorizationRole.moderator))
            .Build();
        }

        public static AuthorizationPolicy RequireHeadTeacher()
        {
            return new AuthorizationPolicyBuilder()
            .RequireRole(Enum.GetNames<EAuthorizationRole>().ToList().Take((int)EAuthorizationRole.premium_user))
            .Build();
        }

        public static AuthorizationPolicy ReqiureTeacher()
        {
            return new AuthorizationPolicyBuilder()
            .RequireRole(Enum.GetNames<EAuthorizationRole>().ToList().Take((int)EAuthorizationRole.user))
            .Build();
        }

        public static AuthorizationPolicy ReqiureStudent()
        {
            return new AuthorizationPolicyBuilder()
            .RequireRole(Enum.GetNames<EAuthorizationRole>().ToList().Take((int)EAuthorizationRole.unknown))
               .Build();
        }
    }
}
