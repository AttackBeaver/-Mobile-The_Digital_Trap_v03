using Crew.Model;
using UnityEngine;

namespace Crew.Components.GoGameObjects
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var isDestroyed = _session.RestoreState(Id);
            if (isDestroyed)
                Destroy(gameObject);
        }
    }
}
