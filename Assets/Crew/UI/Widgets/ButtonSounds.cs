using Crew.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Crew.UI.Widgets
{
    public class ButtonSounds : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _audioClip;

        private AudioSource _source;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_source == null)
            {
                _source = AudioUtils.FindSfxSource();
            }

            _source.PlayOneShot(_audioClip);
        }
    }
}
