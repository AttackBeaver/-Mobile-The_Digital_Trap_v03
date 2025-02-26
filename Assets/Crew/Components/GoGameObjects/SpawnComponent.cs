using Crew.Utils;
using UnityEngine;

namespace Crew.Components.GoGameObjects
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instantiate = SpawnUtils.Spawn(_prefab, _target.position);
            instantiate.transform.localScale = transform.lossyScale;
        }
    }
}
