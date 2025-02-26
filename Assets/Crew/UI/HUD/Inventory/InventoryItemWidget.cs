using System;
using Crew.Definitions;
using Crew.Model;
using Crew.Model.Data;
using Crew.Model.Definitions;
using Crew.Model.Definitions.Repositories.Items;
using Crew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace Crew.UI.HUD.Inventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        /// <summary>
        /// Быстрая ячейка
        /// </summary>
        // [SerializeField] private GameObject _selection;

        [SerializeField] private Text _value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _index;

        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            var index = session.QuickInventory.SelectedIndex;
            _trash.Retain(index.SubscribeAndInvoke(OnIdexChanged));
        }

        private void OnIdexChanged(int newValue, int _)
        {
            /// <summary>
            /// Быстрая ячейка
            /// </summary>
            // _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
