using System.Linq;
using Crew.Collectables;
using Crew.Model.Data;
using Crew.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConsoleComponent : AnimatedWindow
{
    public InputField inputField;

    protected override void Start()
    {
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    public void OnInputEndEdit(string input)
    {
        // Обработка введенной команды
        ProcessCommand(input);

        // Очистка поля ввода после обработки команды
        inputField.text = "";
    }

    private void ProcessCommand(string command)
    {
        // Обработка команды для загрузки уровня
        if (command.StartsWith("loadlevel "))
        {
            string levelName = command.Substring("loadlevel ".Length);
            LoadLevel(levelName);
        }
        // Обработка команды для добавления предметов в инвентарь
        else if (command.StartsWith("additem "))
        {
            string[] args = command.Substring("additem ".Length).Split(' ');
            if (args.Length != 2)
            {
                Debug.Log("Invalid additem command format. Usage: additem [id] [count]");
                return;
            }

            string itemId = args[0];
            int itemCount;

            if (!int.TryParse(args[1], out itemCount))
            {
                Debug.Log("Invalid item count specified.");
                return;
            }

            // Находим все объекты в сцене, имеющие интерфейс ICanAddInInventory
            var heroes = FindObjectsOfType<MonoBehaviour>().OfType<ICanAddInInventory>();

            // Проверяем каждый объект на наличие интерфейса и добавляем предмет
            foreach (var hero in heroes)
            {
                // Получаем GameObject из объекта, реализующего интерфейс ICanAddInInventory
                var heroGameObject = hero as MonoBehaviour;
                if (heroGameObject == null)
                {
                    Debug.LogError("Failed to get GameObject from ICanAddInInventory.");
                    continue;
                }

                // Создаем объект InventoryAddComponent
                var inventoryAddComponent = gameObject.AddComponent<InventoryAddComponent>();
                // Устанавливаем параметры
                inventoryAddComponent._id = itemId;
                inventoryAddComponent._count = itemCount;

                // Вызываем метод Add для добавления предмета в инвентарь героя
                inventoryAddComponent.Add(heroGameObject.gameObject);

                // Удаляем временный InventoryAddComponent
                Destroy(inventoryAddComponent);
            }
        }
        // Обработка команды для изменения GravityScale
        else if (command.StartsWith("setgravity "))
        {
            string[] args = command.Substring("setgravity ".Length).Split(' ');
            if (args.Length != 2)
            {
                Debug.Log("Invalid setgravity command format. Usage: setgravity [heroName] [gravityScale]");
                return;
            }

            string heroName = args[0];
            float gravityScale;

            if (!float.TryParse(args[1], out gravityScale))
            {
                Debug.Log("Invalid gravityScale value specified.");
                return;
            }

            // Находим объект героя по его имени
            GameObject hero = GameObject.Find(heroName);
            if (hero == null)
            {
                Debug.Log("Hero with name " + heroName + " not found.");
                return;
            }

            // Получаем компонент Rigidbody2D объекта героя
            Rigidbody2D rb2D = hero.GetComponent<Rigidbody2D>();
            if (rb2D == null)
            {
                Debug.Log("Rigidbody2D component not found on hero object.");
                return;
            }

            // Устанавливаем GravityScale
            rb2D.gravityScale = gravityScale;
        }
        else
        {
            Debug.Log("Unknown command: " + command);
        }
    }

    private void LoadLevel(string levelName)
    {
        // Загрузка уровня по его имени
        SceneManager.LoadScene(levelName);
    }
}