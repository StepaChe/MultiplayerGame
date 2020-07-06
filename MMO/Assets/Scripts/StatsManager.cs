using UnityEngine;
using UnityEngine.Networking;

public class StatsManager : NetworkBehaviour
{
    [SyncVar] public int damage, armor, moveSpeed;
    [SyncVar] public int level, statPoints;
    [SyncVar] public float exp, nextLevelExp;

    // сущность игрока
    public Player player;

    [Command]
    public void CmdUpgradeStat(int stat)
    {
        if (player.progress.RemoveStatPoint())
        {
            switch (stat)
            {
                case (int)StatType.Damage:
                    player.character.stats.damage.baseValue++;
                    break;
                case (int)StatType.Armor:
                    player.character.stats.armor.baseValue++;
                    break;
                case (int)StatType.MoveSpeed:
                    player.character.stats.moveSpeed.baseValue++;
                    break;
            }
        }
    }
}

// типы статов
public enum StatType { Damage, Armor, MoveSpeed }