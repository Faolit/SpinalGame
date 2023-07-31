using System;
using System.Collections.Generic;

namespace SpinalPlay
{
    public class AllData
    {
        [NonSerialized] public Dictionary<Type, DataBase> progressData;

        public LevelsData levelData;
        public ShipsData shipsData;
        public WeaponData weaponData;
        public PlayerMoneyData playerMoneyData;
        public SettingData settingData;

        public AllData()
        {
            progressData = new Dictionary<Type, DataBase>();

            AddToDict(levelData = new LevelsData());
            AddToDict(shipsData = new ShipsData());
            AddToDict(weaponData = new WeaponData());
            AddToDict(playerMoneyData = new PlayerMoneyData());
            AddToDict(settingData = new SettingData());
        }

        private void AddToDict(DataBase data)
        {
            progressData.Add(data.GetType(), data);
        }
    }
}