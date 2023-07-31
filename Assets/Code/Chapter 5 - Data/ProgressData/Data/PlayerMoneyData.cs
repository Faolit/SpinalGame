namespace SpinalPlay
{
    public class PlayerMoneyData : DataBase
    {
        public int money;

        public PlayerMoneyData()
        {
            Reset();
        }

        public override void Reset()
        {
            money = 20;
        }
    }
}
