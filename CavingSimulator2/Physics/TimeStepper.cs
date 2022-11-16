using BepuPhysics;
using BepuUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Physics
{
    public struct TimeStepper : ITimestepper
    {
        public event TimestepperStageHandler BeforeCollisionDetection;
        public event TimestepperStageHandler CollisionsDetected;

        public void Timestep(Simulation simulation, float dt, IThreadDispatcher threadDispatcher = null)
        {
            
        }
    }
}
