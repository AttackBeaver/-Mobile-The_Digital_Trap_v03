using Crew.Creatures.Hero;
using Crew.Model.Data;
using Crew.Model.Definitions;
using Crew.Model.Definitions.Repositories.Items;
using UnityEngine;

namespace Crew.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] public string _id;
        [SerializeField] public int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventory(_id, _count);
        }
    }
}
