using UnityEngine;

namespace SpinalPlay
{
    public class LoadingCurtain : IService
    {
        private GameObject curtain;
        public LoadingCurtain() 
        {
            Init();
            Show();
        }

        public void Show()
        {
            curtain.SetActive(true);    
        }

        public void Hide()
        {
            curtain.SetActive(false);
        }

        private void Init()
        {
            curtain = GameObject.FindObjectOfType<LoadingCurtainTag>().gameObject;
            GameObject.DontDestroyOnLoad(curtain);
        }
    }
}
