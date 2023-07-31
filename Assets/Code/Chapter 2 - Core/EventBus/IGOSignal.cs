using UnityEngine;

namespace SpinalPlay
{
    public interface IGOSignal : ISignal
    {
        public GameObject Object { get; }
    }
}
