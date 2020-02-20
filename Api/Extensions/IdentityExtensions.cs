using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Api.Extensions
{
    public static class IdentityExtension
    {
        public static string UserId(this ClaimsIdentity identity)
        {
            return identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static List<string> GetRules(this ClaimsIdentity identity)
        {
            var rules = identity.Claims.Where(x => x.Type.Equals(ClaimTypes.Role)).Select(s => s.Value).ToList();
            return rules;
        }
    }
}