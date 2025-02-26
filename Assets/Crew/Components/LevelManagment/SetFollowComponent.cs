using Cinemachine;
using Crew.Creatures.Hero;
using UnityEngine;

namespace Crew.Components.LevelManagment
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class SetFollowComponent : MonoBehaviour
    {
        private void Start()
        {
            // var vCamera = GetComponent<CinemachineVirtualCamera>();
            // vCamera.Follow = FindObjectOfType<Hero>().transform;
            var vCamera = GetComponent<CinemachineVirtualCamera>();
            var hero = FindObjectOfType<Hero>();
            if (hero != null)
            {
                vCamera.Follow = hero.transform;
            }
            else
            {
                Debug.LogWarning("Hero object not found for camera follow!");
            }
        }

        private void Update()
        {
            Start();
        }
    }
}
