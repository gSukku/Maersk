using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    public class SortController : ControllerBase
    {
        private readonly ISortJobProcessor _sortJobProcessor;

        public SortController(ISortJobProcessor sortJobProcessor)
        {
            _sortJobProcessor = sortJobProcessor;
        }

        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }

        [HttpPost]
        public ActionResult<SortJob> EnqueueJob(int[] values)
        {
            // TODO: Should enqueue a job to be processed in the background.
            var enqueuejob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: new TimeSpan(0, 1, 0),
                input: values,
                output: null);

            var enqueuedjob = _sortJobProcessor.ProcessAsync(enqueuejob);

            if(enqueuedjob.Status == TaskStatus.RanToCompletion)
            {
                return  Ok(enqueuedjob);
            }
            return  Ok(enqueuejob);
        }

        [HttpGet]
        public ActionResult<List<SortJob>> GetJobs()
        {
            // TODO: Should return all jobs that have been enqueued (both pending and completed).
           var allJobs= _sortJobProcessor.GetAllJobs().Result;
            if(allJobs.Count() != 0)            
            return allJobs;
            else
            return BadRequest("Not items found");
        }

        [HttpGet("{jobId}")]
        public  ActionResult<SortJob> GetJob(Guid jobId)
        {
            // TODO: Should return a specific job by ID.
            var allJobs= _sortJobProcessor.GetAllJobs().Result;
            if(allJobs.Count != 0)            
            return _sortJobProcessor.GetAllJobs().Result.First(SortJob => SortJob.Id == jobId);
            else
            return BadRequest("Not items found");
        }
    }
}
