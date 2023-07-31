using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class GameResultPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _result;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _backToPrepare;

        private EventBus _eventBus;
        private LevelConfig _levelConfig;

        private LevelsData _levelsData;
        private PlayerMoneyData _moneyData;

        private int _scorePoints;
        private int _minScore;

        public void Initialize(EventBus eventBus, LevelConfig levelConfig, LevelsData data, PlayerMoneyData moneyData)
        {
            _eventBus = eventBus;

            _levelConfig = levelConfig;

            _levelsData = data;
            _moneyData = moneyData;

            Subscribe();
            InitAllButtons();
        }

        public void ShowDefeatResult(PlayerDead signal)
        {
            ShowScore();
            _result.text = "Result: Defeat";
            gameObject.SetActive(true);
        }

        public void ShowEndResult(AllEnemyDeath signal)
        {
            ShowScore();
            if(_scorePoints >= _minScore)
            {
                _result.text = "Result: Success";
                _moneyData.money += _scorePoints;
                _levelsData.maxReachedLevel = _levelsData.CurrentLevelId + 1;
            }
            else
            {
                _result.text = "Result: Fail";
            }
            gameObject.SetActive(true);
        }

        private void OnBackButton()
        {
            _eventBus.Invoke<EnterToPrepare>(new EnterToPrepare());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }
        private void OnRestartButton()
        {
            _eventBus.Invoke<EnterRestarting>(new EnterRestarting());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void InitAllButtons()
        {
            _backToPrepare.onClick.AddListener(OnBackButton);
            _restart.onClick.AddListener(OnRestartButton);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<PlayerDead>(ShowDefeatResult);
            _eventBus.Subscribe<AllEnemyDeath>(ShowEndResult);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<PlayerDead>(ShowDefeatResult);
            _eventBus.Unsubscribe<AllEnemyDeath>(ShowEndResult);
        }

        private void ShowScore()
        {
            _scorePoints = _levelsData.lastScore;
            _minScore = _levelConfig.minWinScore;

            _score.text = $"{_scorePoints}/{_minScore}";
        }
    }
}