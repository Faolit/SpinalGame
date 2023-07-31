using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class GameContainer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private HearthPanel _hearth;
        [SerializeField] private Button _pauseButton;

        private EventBus _eventBus;
        private LevelsData _levelsData;

        private int _scorePoints;
        private LevelConfig _levelConfig;

        public void Initialize(EventBus eventBus, LevelsData levelsData, int playerHealth, LevelConfig levelConfig)
        {
            _eventBus = eventBus;
            _scorePoints = 0;

            _levelsData = levelsData;

            _levelConfig = levelConfig;

            _hearth.Initialize(eventBus, playerHealth);
            InitAllButtons();  
            UpdateScore(0);
            Subscribe();
        }

        public void OnEnemyDead(EnemyDead signal)
        {
            int points = signal.Object.GetComponent<PointGiver>().Points;
            UpdateScore(points);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            _eventBus.Invoke<GamePaused>(new GamePaused());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void UpdateScore(int score)
        {
            _scorePoints += score;
            _levelsData.lastScore = _scorePoints;

            if (_scoreText != null)
            {
                _scoreText.text = $"{_scorePoints} / {_levelConfig.minWinScore}";
            }
            if(_scorePoints > _levelsData.IDToScore(_levelConfig.ID))
            {
                _levelsData.levelIDToScore[_levelConfig.ID] = _scorePoints;
            }
        }

        private void InitAllButtons()
        {
            _pauseButton.onClick.AddListener(PauseGame);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<EnemyDead>(OnEnemyDead);
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<EnemyDead>(OnEnemyDead);
        }
    }
}