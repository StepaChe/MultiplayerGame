using UnityEngine.Networking;

public class SyncListItem : SyncList<Item>
{

    // запись данных перед отправкой
    protected override void SerializeItem(NetworkWriter writer, Item item)
    {
        writer.Write(ItemBase.GetItemId(item));
    }

    // считывание данных при получении
    protected override Item DeserializeItem(NetworkReader reader)
    {
        return ItemBase.GetItem(reader.ReadInt32());
    }
}
