using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UUMS.IdentityServer.Api
{
    public class Config
    {
        /// <summary>
        /// 作用域范围
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("TestA")
            };
        }

        /// <summary>
        /// 应用
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client-TestA",
                    ClientSecrets =
                    {
                        new Secret("testAs-secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={ "TestA" }
                }
            };
        }
    }
}
