using UnityEngine;

[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item
{

    public EquipmentSlotType equipSlot;

    public int damageModifier;
    public int armorModifier;
    public int speedModifier;
    public GameObject prefab; // внешность снаряжения

    public override void Use(Player player)
    {
        player.inventory.RemoveItem(this);
        EquipmentItem oldItem = player.equipment.EquipItem(this);
        if (oldItem != null) player.inventory.AddItem(oldItem);
        base.Use(player);
        //player.equipment.EquipHolder(equipSlot, prefab); // Это здесь пока не нужно... переделываю
    }

    // вызывается при экипировке предмета
    public virtual void Equip(Player player)
    {
        if (player != null)
        {
            UnitStats stats = player.character.stats;
            stats.damage.AddModifier(damageModifier);
            stats.armor.AddModifier(armorModifier);
            stats.moveSpeed.AddModifier(speedModifier);
        }
    }

    // вызывается при снятии предмета
    public virtual void Unequip(Player player)
    {
        if (player != null)
        {
            UnitStats stats = player.character.stats;
            stats.damage.RemoveModifier(damageModifier);
            stats.armor.RemoveModifier(armorModifier);
            stats.moveSpeed.RemoveModifier(speedModifier);
        }
    }
}

public enum EquipmentSlotType { Head, Chest, Legs, RighHand, LeftHand }
