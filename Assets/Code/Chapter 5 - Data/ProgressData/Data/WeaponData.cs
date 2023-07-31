using System.Collections.Generic;

namespace SpinalPlay
{
    public class WeaponData : DataBase
    {
        public List<int> activeWeapon;

        public WeaponData() 
        {
            Reset();
        }

        public override void Reset()
        {
            activeWeapon = new List<int> { 0 };
        }
    }
}
