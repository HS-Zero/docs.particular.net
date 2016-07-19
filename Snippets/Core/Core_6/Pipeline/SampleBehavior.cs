﻿namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region SamplePipelineBehavior

    public class SampleBehavior :
        Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            // custom logic before calling the next step in the pipeline.

            return next();

            // custom logic after all inner steps in the pipeline completed.
        }
    }

    #endregion
}