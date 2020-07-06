using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image icon; // иконка предмета
    public Button unequipButton; // кнопка слота
    public Equipment equipment; // ссылка на экипировку

    Item item; // предмет в слоте

    public void SetItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        unequipButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        unequipButton.interactable = false;
    }

    public void Unequip()
    {
        equipment.UnequipItem(item);
    }
}
