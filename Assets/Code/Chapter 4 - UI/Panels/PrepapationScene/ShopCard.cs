using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public abstract class ShopCard : MonoBehaviour
    {
        private const float SHIP_SCALE = 2.5f;

        [SerializeField] protected TMP_Text _name;
        [SerializeField] protected TMP_Text _cost;
        [SerializeField] protected TMP_Text _description;
        [SerializeField] protected Image _position;

        protected GameObject _currentItem;
        protected List<int> _initializedItems = new List<int>();

        public abstract void UpdateCard(ConfigBase config, GameObject item);

        protected void Resize(SpriteRenderer target)
        {
            float scaleX = target.gameObject.transform.localScale.x * SHIP_SCALE;
            float scaleY = target.gameObject.transform.localScale.y * SHIP_SCALE;
            target.gameObject.transform.localScale = new Vector2(scaleX, scaleY);
        }
    }
}