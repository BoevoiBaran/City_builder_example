using System.Collections.Generic;

namespace Code.Main.Data
{
    public enum ResourceType
    {
        Unknown,
        Gas,
        Minerals
    }

    public class ResourceHolder
    {
        public int Limit;
        public int Value;
    }
    
    public class ResourceData
    {
        public List<(ResourceType type, int count, int limit)> Data;
    }
}
