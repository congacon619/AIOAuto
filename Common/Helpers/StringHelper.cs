using System;
using System.Text;

namespace AIOAuto.Common.Helpers
{
    public static class StringHelper
    {
        public static string Base64Decode(string base64Encoded)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64Encoded);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}