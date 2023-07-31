using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class HearthPanel : MonoBehaviour
    {
        [SerializeField] private Image _heartImage;

        private EventBus _eventBus;

        private int _maxHealth;
        private int _playerHealth;
        private float _flashDelay;
        private Vector4 _sourceColor;

        public void Initialize(EventBus eventBus, int playerHealth)
        {
            _eventBus = eventBus;
            _maxHealth = playerHealth;
            _playerHealth = playerHealth;

            _sourceColor = _heartImage.color;

            SetFlashDelay();

            Subscribe();

            StartCoroutine(ShowDamageRoutine());
        }

        private IEnumerator ShowDamageRoutine()
        {
            while(_flashDelay == 1)
            {
                yield return null;
            }
            while (_playerHealth != 0)
            {
                _heartImage.color = new Vector4(0, 0, 0 ,0);
                yield return new WaitForSeconds(_flashDelay);
                _heartImage.color = _sourceColor;
                yield return new WaitForSeconds(_flashDelay);
            }
            _heartImage.color = new Vector4(0, 0, 0, 0);
        }

        private void CalcDamage(PlayerDamaged signal) 
        {
            _playerHealth -= signal.damage;
            SetFlashDelay();
        }

        private void SetFlashDelay()
        {
            _flashDelay = _playerHealth / _maxHealth;
            _flashDelay = Mathf.Clamp(_flashDelay, 0.2f, 1f);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<PlayerDamaged>(CalcDamage);
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe<PlayerDamaged>(CalcDamage);
        }
    }
}