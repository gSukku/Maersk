using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Maersk.Sorting.Api
{
    public class SortJobProcessor : ISortJobProcessor
    {
        private readonly ILogger<SortJobProcessor> _logger;
        public List<SortJob> allSortJobs = new List<SortJob>();
        public SortJobProcessor(ILogger<SortJobProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<SortJob> Process(SortJob job)
        {
            _logger.LogInformation("Processing job with ID '{JobId}'.", job.Id);

            var stopwatch = Stopwatch.StartNew();

            var output = job.Input.OrderBy(n => n).ToArray();
            await Task.Delay(5000); // NOTE: This is just to simulate a more expensive operation

            var duration =  stopwatch.Elapsed;

            _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", job.Id, duration);

            return new SortJob(
                id: job.Id,
                status: SortJobStatus.Completed,
                duration: duration,
                input: job.Input,
                output: output);
        }

        public async Task<SortJob> ProcessAsync(SortJob job)
        {
            var newjob = job;
            allSortJobs.Add(job);
            newjob = await Task.Run(()=> CompleteJobAsync(job));
            var itemToRemove = allSortJobs.FirstOrDefault(r => r.Id == newjob.Id);
            if (itemToRemove != null)
            allSortJobs.Remove(itemToRemove);
            allSortJobs.Add(newjob);
            return newjob;
        }

        public SortJob CompleteJobAsync(SortJob job)
        {
            var stopwatch = Stopwatch.StartNew();
            TimeSpan value =job.Duration == null? TimeSpan.FromSeconds(0): job.Duration.Value;
            Task.Delay(value).Wait();
            var duration =  stopwatch.Elapsed;
            var output = job.Input.OrderBy(n => n).ToArray();
            return new SortJob(id:job.Id, status: SortJobStatus.Completed,duration: duration,input: job.Input,output: output);
        } 
        public async Task<List<SortJob>> GetAllJobs()
        {
           return await Task.Run(() => allSortJobs);
        }              
    }
}
