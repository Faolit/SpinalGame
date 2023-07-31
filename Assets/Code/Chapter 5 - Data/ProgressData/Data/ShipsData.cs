using System.Collections.Generic;

namespace SpinalPlay
{
    public class ShipsData : DataBase
    {
        public int currentShipId;
        public Dictionary<int, int> shipToWeaponId;
        public  List<int> availableShips;

        public int ShipToWeapID(int id)
        {
            if (shipToWeaponId.ContainsKey(id))
            {
                return shipToWeaponId[id];
            }
            return 0;
        }

        public override void Reset()
        {
            currentShipId = 0;
            shipToWeaponId = new Dictionary<int, int> { { 0, 0 } };
            availableShips = new List<int> { 0 };
        }

        public ShipsData()
        {
            Reset();
        }
    }
}