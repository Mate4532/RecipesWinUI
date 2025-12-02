using System.Collections.Generic;

namespace Receptek.Models
{
    public class IdManagerDto<T>
    {
        public Dictionary<int, T> Items { get; set; } = new();
        public Queue<int> FreeIds { get; set; } = new();
        public int NextId { get; set; }

    }
}
