using System;
using System.Collections.Generic;
using Crew.Definitions;
using Crew.Model.Data.Properties;
using Crew.Utils;
using Crew.Utils.Disposables;

namespace Crew.Model.Data
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        public readonly Cooldown Cooldown = new Cooldown();
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));

            if (!string.IsNullOrEmpty(_data.Perks.Used.Value))
                SelectPerk(_data.Perks.Used.Value);
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public string Used => _data.Perks.Used.Value;
        public bool IsDoubleJumpSupported => _data.Perks.IsUnlocked("double-jump") && Cooldown.IsReady;
        public bool IsFireballSupported => _data.Perks.IsUnlocked("fireball") && Cooldown.IsReady;


        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);

                OnChanged?.Invoke();
            }
        }

        public void SelectPerk(string selected)
        {
            var perkDef = DefsFacade.I.Perks.Get(selected);
            Cooldown.Value = perkDef.Cooldown;
            _data.Perks.Used.Value = selected;
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        // Метод для получения ID перков
        public List<string> GetPerkIds()
        {
            List<string> perkIds = new List<string>(_data.Perks._unlocked);
            return perkIds;
        }

        public void ApplySavedPerks(List<string> savedPerkIds)
        {
            // Очистить текущие перки
            _data.Perks._unlocked.Clear();

            // Добавить сохраненные перки
            foreach (string perkId in savedPerkIds)
            {
                _data.Perks._unlocked.Add(perkId);
            }
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}