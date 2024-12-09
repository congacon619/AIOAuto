using System.Collections.Generic;
using System.Linq;

namespace AIOAuto.Common.Helpers
{
    public static class ListHelper
    {
        public static List<string> RemoveEmptyItems(List<string> lst)
        {
            return lst.Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => item.Trim()).ToList();
        }
    }
}