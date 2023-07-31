using UnityEngine;

namespace SpinalPlay
{
    internal class EntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            CompositionRoot composition = new CompositionRoot();
            composition.RegisterAllServices();

            GameStateMachine stateMachine = composition.CreateGameStateMachine();
            stateMachine.StartGame();
        }
    }
}