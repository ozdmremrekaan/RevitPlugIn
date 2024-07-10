using System;
namespace RevitPlugIn
{
    public static class RoleMapper
    {
        public static UserRole MapUserRoleToRole(UserRole userRole)
        {
            switch (userRole)
            {
                case UserRole.UNLOCKED:
                    return UserRole.UNLOCKED;
                case UserRole.USER:
                    return UserRole.USER;
                case UserRole.ADMIN:
                    return UserRole.ADMIN;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userRole), userRole, null);
            }
        }
    }

}
