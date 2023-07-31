namespace SpinalPlay
{
    public class BarrageFactory
    {
        private readonly PoolService _poolService;
        private readonly ProjectileConfig _config;
        private readonly UnitTag _damageTag;
        private readonly EventBus _eventBus;

        public BarrageFactory(PoolService poolService, ProjectileConfig config, UnitTag damageTag, EventBus eventBus)
        {
            _poolService = poolService;
            _config = config;
            _damageTag = damageTag;
            _eventBus = eventBus;
        }
        public BarrageBase CreateBarrage(BarrageType type)
        {
            switch (type)
            {
                case BarrageType.ForvardFire:
                    return new ForvardFire(_poolService, _config, _damageTag, _eventBus);

                case BarrageType.BackTriple:
                    return new BackTriple(_poolService, _config, _damageTag, _eventBus);

                case BarrageType.BackvardFire:
                    return new BackvardFire(_poolService, _config, _damageTag, _eventBus);

                case BarrageType.TripleFire:
                    return new TripleFire(_poolService, _config, _damageTag, _eventBus);

                case BarrageType.CircleWave:
                    return new CircleWave(_poolService, _config, _damageTag, _eventBus);

                default:
                    return new ForvardFire(_poolService, _config, _damageTag, _eventBus);
            }
        }
    }
}