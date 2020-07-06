using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon; // иконка слота
    public Button removeButton; // кнопка удаления предмета из слота
    public Inventory inventory; // инвентарь, с которым работает слот

    Item item; // предмет, хранящийся в слоте

    // установка предмета в слот
    public void SetItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    // очистка слота от предмета
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    // обработчик для нажатия на кнопку удаления предмета
    public void OnRemoveButton()
    {
        // вызываем метод инвентаря, удаляющий предмет из слота
        inventory.DropItem(item);
    }

    // обработчик для нажатия на предмет
    public void UseItem()
    {
        if (item != null) inventory.UseItem(item);
    }
}
