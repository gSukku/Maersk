using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public interface ISortJobProcessor
    {
        Task<SortJob> Process(SortJob job);
        Task<SortJob> ProcessAsync (SortJob job);
        Task<List<SortJob>> GetAllJobs();
    }
}