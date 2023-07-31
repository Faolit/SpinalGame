namespace SpinalPlay
{
    public interface IState
    {
        public void OnEnter();
    }

    public interface IStateWithExit : IState
    {
        public void OnExit();
    }

}