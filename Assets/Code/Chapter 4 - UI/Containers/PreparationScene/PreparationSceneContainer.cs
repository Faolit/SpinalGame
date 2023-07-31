using UnityEngine;

namespace SpinalPlay
{
    public class PreparationSceneContainer : MonoBehaviour
    {
        [SerializeField] private FooterPreparationContainer _footerPreparationContainer;
        [SerializeField] private ChooseLevelContainer _chooseLevelContainer;

        public void Initialize(EventBus eventBus, LevelsData choosedLevel)
        {
            _footerPreparationContainer.Initialize(eventBus);
            _chooseLevelContainer.Initialize(choosedLevel, eventBus);
        }
    }
}