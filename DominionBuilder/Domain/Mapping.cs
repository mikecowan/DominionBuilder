using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Domain
{
    public static class Mapping
    {
        private static Dictionary<string, int> TypeIds;
        private static Dictionary<string, int> CategoryIds;
        
        public static void SetTypeIds(Dictionary<string, int> typeIds)
        {
            TypeIds = typeIds;
        }

        public static void SetCategoryIds(Dictionary<string, int> categoryIds)
        {
            CategoryIds = categoryIds;
        }

        public static int GetTypeId(string type)
        {
            return TypeIds[type];
        }

        public static int GetCategoryId(string category)
        {
            return CategoryIds[category];
        }
    }
}
