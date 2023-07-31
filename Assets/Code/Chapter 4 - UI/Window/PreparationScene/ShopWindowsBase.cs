using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public abstract class ShopWindowsBase : WindowBase
    {
        [SerializeField] private Button _toNextCardButton;
        [SerializeField] private Button _toPreviosCardButton;

        [SerializeField] private Button _setItemButton;
        [SerializeField] private Button _buyItemButton;

        [SerializeField] protected ShopCard _shopCard;
        [SerializeField] protected TMP_Text _moneyView;

        protected int _itemIndex;
        protected int _maxIndex;
        protected bool _itemIsAvailable;

        protected abstract void UpdateShopCard();
        protected abstract bool CheckAvailables();
        protected abstract void TryBuyItem();
        protected abstract void SetItem();
        protected abstract Task GetAllView();
        protected abstract void UpdateMoneyView();
        protected new void Initialize(EventBus eventBus)
        {
            base.Initialize(eventBus);

            _toNextCardButton.onClick.AddListener(TryNextCard);
            _toPreviosCardButton.onClick.AddListener(TryPreviosCard);
            _setItemButton.onClick.AddListener(SetItem);
            _buyItemButton.onClick.AddListener(TryBuyItem);
        }

        protected void TryNextCard()
        {
            if (_itemIndex < _maxIndex)
            {
                _itemIndex++;
                UpdateShopCard();
                FitButtons(CheckAvailables());
            }
        }

        protected void TryPreviosCard()
        {
            if (_itemIndex > 0)
            {
                _itemIndex--;
                UpdateShopCard();
                FitButtons(CheckAvailables());
            }
        }

        protected void FitButtons(bool isAvailable)
        {
            if(isAvailable)
            {
                _buyItemButton.gameObject.SetActive(false);
                _setItemButton.gameObject.SetActive(true);
            }
            else
            {
                _buyItemButton.gameObject.SetActive(true);
                _setItemButton.gameObject.SetActive(false);
            }
        }
    }
}
