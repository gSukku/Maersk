﻿using System;
using System.Collections.Generic;

namespace Maersk.Sorting.Api
{
    public class SortJob
    {
        public SortJob(Guid id, SortJobStatus status, TimeSpan? duration, IReadOnlyCollection<int> input, IReadOnlyCollection<int>? output)
        {
            Id = id;
            Status = status;
            Duration = duration;
            Input = input;
            Output = output;
        }

        public Guid Id { get; set;}
        public SortJobStatus Status { get; set;}
        public TimeSpan? Duration { get;  set; }
        public IReadOnlyCollection<int> Input { get; }
        public IReadOnlyCollection<int>? Output { get; }
    }
}
