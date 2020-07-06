using UnityEngine.Networking;
using UnityEngine;
using System.Linq;

public class Equipment : NetworkBehaviour
{
    public event SyncList<Item>.SyncListChanged onItemChanged;
    public SyncListItem items = new SyncListItem();

    public Player player;

    UserData data;

    public void Load(UserData data)
    {
        this.data = data;
        for (int i = 0; i < data.equipment.Count; i++)
        {
            EquipmentItem item = (EquipmentItem)ItemBase.GetItem(data.equipment[i]);
            items.Add(item);
            item.Equip(player);
        }
    }

    public override void OnStartLocalPlayer()
    {
        items.Callback += ItemChanged;
    }

    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        onItemChanged(op, itemIndex);
    }

    public EquipmentItem EquipItem(EquipmentItem item)
    {
        EquipmentItem oldItem = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (((EquipmentItem)items[i]).equipSlot == item.equipSlot)
            {
                oldItem = (EquipmentItem)items[i];
                oldItem.Unequip(player);
                data.equipment.Remove(ItemBase.GetItemId(items[i]));
                items.RemoveAt(i);
                break;
            }
        }
        items.Add(item);
        item.Equip(player);
        CmdEquipItem(item); // вызываем метод смены снаряжения
        data.equipment.Add(ItemBase.GetItemId(item));

        return oldItem;
    }

    [Command]
    void CmdEquipItem(EquipmentItem item)
    {
        EquipHolder(player.character._rightHolder, item.prefab);
    }

    public void UnequipItem(Item item)
    {
        CmdUnequipItem(items.IndexOf(item));
    }

    [Command]
    void CmdUnequipItem(int index)
    {
        if (items[index] != null && player.inventory.AddItem(items[index]))
        {
            ((EquipmentItem)items[index]).Unequip(player);
            data.equipment.Remove(ItemBase.GetItemId(items[index]));
            items.RemoveAt(index);
            UnequipHolder(player.character._rightHolder);
        }
    }

    // включает отображение предмета снаряжения
    public GameObject EquipHolder(GameObject holder, GameObject prefab)
    {
        UnequipHolder(holder);

        var item = Instantiate(prefab, holder.transform.position, Quaternion.Euler(0, 0, 0), holder.transform);
        item.transform.localPosition = prefab.transform.position;
        item.transform.rotation = holder.transform.rotation;
        NetworkServer.Spawn(item);
        return item;
    }

    // скрывает отображение предмета снаряжения
    public void UnequipHolder(GameObject holder)
    {
        if (holder.transform.childCount != 0)
        {
            var children = (from Transform child in holder.transform select child.gameObject).ToList();
            children.ForEach(Destroy);
        }
    }
}
