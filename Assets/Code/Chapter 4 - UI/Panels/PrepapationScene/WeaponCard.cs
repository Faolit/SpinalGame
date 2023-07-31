using UnityEngine;

namespace SpinalPlay
{
    public class WeaponCard : ShopCard
    {
        public override void UpdateCard(ConfigBase config, GameObject item)
        {
            WeaponConfig weapConfig = (WeaponConfig)config;

            _name.text = weapConfig.weaponName;
            _description.text = weapConfig.description;
            _cost.text = "Cost:" + weapConfig.cost.ToString();

            _currentItem?.SetActive(false);

            if (!_initializedItems.Contains(config.ID))
            {
                item.transform.position = _position.transform.position;
                item.transform.SetParent(_position.transform);
                Resize(item.GetComponent<SpriteRenderer>());

                _initializedItems.Add(config.ID);
            }

            item.SetActive(true);
            _currentItem = item;
        }
    }
}