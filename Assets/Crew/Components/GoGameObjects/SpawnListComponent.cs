using System;
using System.Linq;
using UnityEngine;

namespace Crew.Components.GoGameObjects
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners; // Массив данных о спаунерах.

        /// <summary>
        /// Спаунит все объекты, указанные в массиве _spawners.
        /// </summary>
        public void SpawnAll()
        {
            foreach (var spawnData in _spawners)
            {
                spawnData.Component.Spawn(); // Вызов метода спауна для каждого компонента.
            }
        }

        /// <summary>
        /// Спаунит объект по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта.</param>
        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id); // Поиск спаунера по идентификатору.
            spawner?.Component.Spawn(); // Вызов метода спауна, если спаунер найден.
        }

        /// <summary>
        /// Класс, содержащий данные о спаунере.
        /// </summary>
        [Serializable]
        public class SpawnData
        {
            public string Id; // Идентификатор объекта.
            public SpawnComponent Component; // Компонент для спауна объекта.
        }
    }
}
