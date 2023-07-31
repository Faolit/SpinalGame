using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class ChooseLevelContainer : MonoBehaviour
    {
        [SerializeField] private Button[] _buttonsList;
        [SerializeField] private TMP_Text _levelCaution;

        private LevelsData _levelsData;
        private EventBus _eventBus;

        public void Initialize(LevelsData currentLevel, EventBus eventBus)
        {
            _eventBus = eventBus;
            _levelsData = currentLevel;
            _levelCaution.text = $"Choosed Level: {currentLevel.CurrentLevelId}";
            OnInitialized();
            SetActiveLevels();
        }

        private void OnInitialized()
        {
            int i = 0;
            foreach(var button in _buttonsList)
            {
                int copy = i;
                button.onClick.AddListener(() => OnChooseLevel(copy));
                i++;
            }
        }

        private void SetActiveLevels()
        {
            int disactiveAftThis = Mathf.Clamp(_levelsData.maxReachedLevel + 1 , 1, _buttonsList.Length);
            foreach (var button in _buttonsList[disactiveAftThis..])
            {
                button.gameObject.SetActive(false);
            }
        }

        private void OnChooseLevel(int levelID)
        {
            _levelsData.CurrentLevelId = levelID;
            _levelCaution.text = $"Choosed Level: {levelID}";
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }
    }
}