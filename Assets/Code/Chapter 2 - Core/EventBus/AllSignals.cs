using System;
using UnityEngine;

namespace SpinalPlay
{
    public class NewGame : ISignal { }

    public class EnterToMainMenu : ISignal { }
    public class EnterToGameLoop : ISignal { }
    public class EnterToPrepare : ISignal { }
    public class EnterRestarting: ISignal { }

    public class InvokeSound : ISignal 
    {
        public AssetType sound;

        public InvokeSound(AssetType sound)
        {
            this.sound = sound;
        }
    }

    public class GamePaused : ISignal { }
    public class OpenShipShopWindow : ISignal { }
    public class OpenWeaponShopWindow : ISignal { }
    public class OpenSettingWindow : ISignal { }
    public class OpenNotificationWindow : ISignal
    {
        public string message;
        public OpenNotificationWindow( string message)
        {
            this.message = message;
        }
    }
    public class OpenWarningWindow : ISignal 
    {
        public Action callback;
        public string message;
        public OpenWarningWindow(Action callback, string message) 
        {
            this.callback = callback;
            this.message = message;
        }
    }

    public class AllEnemyDeath : ISignal { }
    public class PlayerDamaged : ISignal 
    {
        public int damage;
        public PlayerDamaged(int damage)
        {
            this.damage = damage;
        }
    }
    public class PlayerDead : ISignal { }
    public class SpawnEnd : ISignal { }
    public class EnemyDead : IGOSignal
    {
        public GameObject Object => _object;
        private GameObject _object;
        public EnemyDead(GameObject enemy)
        {
            _object = enemy;
        }
    }
    public class ProjectileDestroy : IGOSignal
    {
        public GameObject Object => _object;
        private GameObject _object;
        public ProjectileDestroy(GameObject projectile)
        {
            _object = projectile;
        }
    }
}