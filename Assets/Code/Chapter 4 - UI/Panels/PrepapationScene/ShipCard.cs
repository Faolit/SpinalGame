using UnityEngine;

namespace SpinalPlay
{
    public class ShipCard : ShopCard
    {
        public override void UpdateCard(ConfigBase config, GameObject item)
        {
            ShipConfig shipConfig = (ShipConfig)config;

            _name.text = shipConfig.shipName;
            _description.text = shipConfig.shipDescription;
            _cost.text = "Cost:" + shipConfig.shipCost.ToString();

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
