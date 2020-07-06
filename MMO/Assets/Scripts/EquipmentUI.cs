using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    #region Singleton 
    public static EquipmentUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of InventoryUI found!");
            return;
        }
        instance = this;
    }
    #endregion

    // окно экипировки
    [SerializeField] GameObject equipmentUI;

    // привязка к слотам экипировки
    [SerializeField] EquipmentSlot headSlot;
    [SerializeField] EquipmentSlot chestSlot;
    [SerializeField] EquipmentSlot legsSlot;
    [SerializeField] EquipmentSlot righHandSlot;
    [SerializeField] EquipmentSlot leftHandSlot;

    // массив для упорядочивания слотов
    EquipmentSlot[] slots;

    private void Start()
    {
        equipmentUI.SetActive(false);
        // инициализация массива слотов
        slots = new EquipmentSlot[System.Enum.GetValues(typeof(EquipmentSlotType)).Length];
        // заполнение массива выбранными слотами по соответствующим
        // их типу экипировки индексам
        slots[(int)EquipmentSlotType.Chest] = chestSlot;
        slots[(int)EquipmentSlotType.Head] = headSlot;
        slots[(int)EquipmentSlotType.LeftHand] = leftHandSlot;
        slots[(int)EquipmentSlotType.Legs] = legsSlot;
        slots[(int)EquipmentSlotType.RighHand] = righHandSlot;
    }

    // переключение активности окна при нажатии на клавишу Equipment
    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            equipmentUI.SetActive(!equipmentUI.activeSelf);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            equipmentUI.SetActive(false);
        }
    }

    // поле для отображаемой экипировки
    Equipment equipment;

    // установка экипировки
    public void SetEquipment(Equipment newEquipment)
    {
        equipment = newEquipment;
        equipment.onItemChanged += ItemChanged;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].equipment = equipment;
            }
        }
        ItemChanged(0, 0);
    }

    // отрисовка экипировки после изменения
    private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
    {
        // очистка слотов
        for (int i = 0; i < slots.Length; i++) slots[i].ClearSlot();
        // установка предметов в слоты по индексу, равному их типу слота
        for (int i = 0; i < equipment.items.Count; i++)
        {
            int slotIndex = (int)((EquipmentItem)equipment.items[i]).equipSlot;
            slots[slotIndex].SetItem(equipment.items[i]);
        }
    }
}
