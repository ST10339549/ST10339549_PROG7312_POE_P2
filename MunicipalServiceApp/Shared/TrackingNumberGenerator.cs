using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServiceApp.Shared
{
    public static class TrackingNumberGenerator
    {
        // Short, tracking token based on time + randomness.
        public static string NewToken()
        {
            var time = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            Span<byte> random = stackalloc byte[4];
            RandomNumberGenerator.Fill(random);
            var suffix = Convert.ToHexString(random).Substring(0, 6); // 6 hex chars
            return $"T{time}-{suffix}";
        }
    }
}
