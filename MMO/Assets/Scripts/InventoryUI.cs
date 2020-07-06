using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Singleton

    public static InventoryUI instance;

    private void Awake()
    {
        inventoryUI.SetActive(false);
        if (instance != null)
        {
            Debug.LogError("More than one instance of InventoryUI found!");
            return;
        }
        instance = this;
    }

    #endregion

    [SerializeField] Transform itemsParent; // объект, в котором должны храниться слоты

    [SerializeField] InventorySlot slotPrefab; // префаб слота

    [SerializeField] GameObject inventoryUI;

    InventorySlot[] slots;     // массив слотов для текущего инвентаря

    Inventory inventory;

    private void Update()
    {
        if (Input.GetButtonUp("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            inventoryUI.SetActive(false);
        }
    }

    public void SetInventory(Inventory newInventory)
    {
        inventory = newInventory;
        inventory.onItemChanged += ItemChanged;
        // получение старых слотов
        InventorySlot[] childs = itemsParent.GetComponentsInChildren<InventorySlot>();
        // удаление старых слотов
        for (int i = 0; i < childs.Length; i++) Destroy(childs[i].gameObject);
        // выделение места под новые слоты
        slots = new InventorySlot[inventory.space];
        for (int i = 0; i < inventory.space; i++)
        {
            // создание нового слота
            slots[i] = Instantiate(slotPrefab, itemsParent);
            // настройка слота в соответствии со списком предметов из инвентаря
            slots[i].inventory = inventory;
            if (i < inventory.items.Count) slots[i].SetItem(inventory.items[i]);
            else slots[i].ClearSlot();
        }
    }

    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count) slots[i].SetItem(inventory.items[i]);
            else slots[i].ClearSlot();
        }
    }
}
