using System.Collections.Generic;
using FlowX.Core.Steps;

namespace FlowX.Core
{
    public class Sequence : IStep
    {
        private readonly List<IStep> _steps;

        public Sequence(params IStep[] steps): this(new List<IStep>(steps))
        {
        }
        
        public Sequence(List<IStep> steps)
        {
            _steps = steps;
        }

        public void Add(IStep step)
        {
            _steps.Add(step);
        }

        public void RunStep(IFlow flow)
        {
            foreach (var step in _steps)
            {
                step.Run(flow);
            }
        }
    }
}