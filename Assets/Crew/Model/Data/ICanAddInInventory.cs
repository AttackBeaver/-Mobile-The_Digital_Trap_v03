using UnityEngine;

namespace Crew.Model.Data
{
    public interface ICanAddInInventory
    {
        void AddInInventory(string id, int value);
    }
}