﻿namespace DreamFactory.Demo.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using DreamFactory.Api;
    using DreamFactory.Model.System.Role;
    using DreamFactory.Rest;

    public static class SystemRoleTest
    {
        private const string NewRole = "NewRole";

// ReSharper disable PossibleMultipleEnumeration
        public static async Task Run(IRestContext context)
        {
            ISystemApi systemApi = context.Factory.CreateSystemApi();

            IEnumerable<RoleResponse> roles = await systemApi.GetRolesAsync();
            Console.WriteLine("GetRolesAsync(): {0}", roles.Select(x => x.name).ToStringList());

            RoleResponse role = roles.SingleOrDefault(x => x.name == NewRole);
            if (role != null)
            {
                await DeleteRole(role, systemApi);
            }

            RoleRequest newRole = new RoleRequest
            {
                name = NewRole,
                description = "new role",
                is_active = true
            };
                
            roles = await systemApi.CreateRolesAsync(newRole);
            role = roles.Single(x => x.name == NewRole);
            Console.WriteLine("CreateRolesAsync(): {0}", context.ContentSerializer.Serialize(role));

            newRole.id = role.id;
            newRole.description = "new description";
            role = (await systemApi.UpdateRolesAsync(newRole)).Single(x => x.name == NewRole);
            Console.WriteLine("UpdateUsersAsync(): new description={0}", role.description);

            await DeleteRole(role, systemApi);
        }

        private static async Task DeleteRole(RoleResponse role, ISystemApi systemApi)
        {
            Debug.Assert(role.id.HasValue, "Role ID must be set");
            await systemApi.DeleteRolesAsync(role.id.Value);
            Console.WriteLine("DeleteRolesAsync():: id={0}", role.id);
        }
    }
}