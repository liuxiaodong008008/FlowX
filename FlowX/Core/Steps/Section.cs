using FlowX.Core.Steps;

namespace FlowX.Core
{
    public class Section : IStep
    {
        private readonly string _name;

        public Section(string name)
        {
            _name = name;
        }

        public void RunStep(IFlow flow)
        {
            
        }
    }
}