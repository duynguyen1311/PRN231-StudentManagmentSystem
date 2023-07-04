using System.Runtime.Serialization;

namespace BusinessObject.Utility
{
    public class PagedList<T> where T : class
    {
        public PagedList()
        {
            Data = new List<T>();
        }

        public PagedList(IEnumerable<T> _Data, int _TotalCount)
        {
            Data = _Data;
            TotalCount = _TotalCount;
        }

        public IEnumerable<T> Data { get; set; }

        public int TotalCount { get; set; }
    }
}
