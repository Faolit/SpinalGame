namespace SpinalPlay
{
    public class TrajectoryFactory
    {
        public ITrajectory CreateTrajectoryCalc(TrajectoryType type)
        {
            switch (type)
            {
                case TrajectoryType.LineType:
                    return new LineTrajectory();
                case TrajectoryType.SinType: 
                    return new SinTrajectory();
                default:
                    return new LineTrajectory();
            }
        }
    }
}
