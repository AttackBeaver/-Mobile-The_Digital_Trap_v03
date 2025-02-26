using Crew.Definitions;
using UnityEngine;
using UnityEngine.UI;
using static Crew.Model.Definitions.Repositories.PerkRepository;

namespace Crew.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount price)
        {
            var def = DefsFacade.I.Items.Get(price.ItemId);
            _icon.sprite = def.Icon;

            _value.text = price.Count.ToString();
        }
    }
}
