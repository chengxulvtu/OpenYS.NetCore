using System;
using System.Collections.Generic;
using System.Text;

namespace OpenYS.NetCore
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int Total { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
