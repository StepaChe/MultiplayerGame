using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{
    public int space = 15;
    public SyncListItem items = new SyncListItem();
    public Player player;

    UserData data;

    public void Load(UserData data)
    {
        this.data = data;
        for (int i = 0; i < data.inventory.Count; i++)
        {
            items.Add(ItemBase.GetItem(data.inventory[i]));
        }
    }

    public bool AddItem(Item item)
    {
        if (items.Count < space)
        {
            items.Add(item);
            data.inventory.Add(ItemBase.GetItemId(item));
            return true;
        }
        else return false;
    }

    public void DropItem(Item item)
    {
        Drop(item);
        CmdRemoveItem(items.IndexOf(item));
    }

    [Command]
    void CmdDropItem(int index)
    {
        if (items[index] != null)
        {
            Drop(items[index]);
            RemoveItem(items[index]);
        }
    }

    [Command]
    void CmdRemoveItem(int index)
    {
        if (items[index] != null)
        {
            items.RemoveAt(index);
        }
    }

    // ивент, вызываемый при отработке Callback
    public event SyncList<Item>.SyncListChanged onItemChanged;

    public override void OnStartLocalPlayer()
    {
        // привязка функции обработчика к делегату
        items.Callback += ItemChanged;
    }

    // функция для обработки вызова Callback
    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        // вызов ивента
        onItemChanged(op, itemIndex);
    }

    private void Drop(Item item)
    {
            // выброшенный предмет получает рандомный поворот по оси Y
            PickUpItem pickupItem = Instantiate(item.pickupPrefab, player.character.transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
            pickupItem.item = item;
            NetworkServer.Spawn(pickupItem.gameObject);        
    }

    public void UseItem(Item item)
    {
        CmdUseItem(items.IndexOf(item));
    }

    [Command]
    void CmdUseItem(int index)
    {
        if (items[index] != null)
        {
            items[index].Use(player);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        data.inventory.Remove(ItemBase.GetItemId(item));
    }
}
