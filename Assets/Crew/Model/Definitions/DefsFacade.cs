using Crew.Model.Definitions;
using Crew.Model.Definitions.Repositories;
using Crew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace Crew.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {

        [SerializeField] private ItemsRepository _items;
        [SerializeField] private PerkRepository _perks;
        [SerializeField] private PlayerDef _player;


        public ItemsRepository Items => _items;
        public PerkRepository Perks => _perks;
        public PlayerDef PLayer => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        public static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}