using System;
using System.Collections.Generic;
using Crew.Model.Data.Properties;
using Crew.Model.Definitions.Repositories.Items;
using Crew.Utils.Disposables;
using UnityEngine;

namespace Crew.Model.Data
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public InventoryItemData[] Inventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();

        // private Model.PlayerData data;

        public event Action OnChanged;

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            _data.Inventory.OnChanged += OnChangedInventory;
            OnChanged?.Invoke();
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int value)
        {
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }

        // Метод для получения данных инвентаря в формате, удобном для сохранения
        public List<InventoryItemSaveData> GetInventorySaveData()
        {
            List<InventoryItemSaveData> saveDataList = new List<InventoryItemSaveData>();

            foreach (var item in _data.Inventory._inventory)
            {
                // Создать объект данных для сохранения
                InventoryItemSaveData saveData = new InventoryItemSaveData();
                saveData.itemId = item.Id;
                saveData.quantity = item.Value;

                saveDataList.Add(saveData);
            }
            return saveDataList;
        }

        public void ApplySavedInventory(List<InventoryItemSaveData> savedInventoryItems)
        {
            // Очистить текущий инвентарь
            _data.Inventory._inventory.Clear();

            // Добавить сохраненные предметы в инвентарь
            foreach (var savedItem in savedInventoryItems)
            {
                InventoryItemData itemData = new InventoryItemData(savedItem.itemId);
                itemData.Value = savedItem.quantity;
                _data.Inventory._inventory.Add(itemData);
                OnChanged?.Invoke();
            }
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;
        }
    }
}
