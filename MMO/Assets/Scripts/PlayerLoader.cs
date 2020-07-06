using UnityEngine;
using UnityEngine.Networking;

public class PlayerLoader : NetworkBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] Player player;

    public override void OnStartAuthority()
    {
        CmdCreatePlayer();
    }

    [Command]
    public void CmdCreatePlayer()
    {
        Character character = CreateCharacter();
        player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), isLocalPlayer);
        controller.SetCharacter(character, isLocalPlayer);
    }

    [TargetRpc]
    void TargetLinkCharacter(NetworkConnection target, NetworkIdentity unit)
    {
        Character character = unit.GetComponent<Character>();
        player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
        controller.SetCharacter(character, true);
    }

    public Character CreateCharacter()
    {
        // создаём персонажа по хешу из пользовательский данных
        UserAccount acc = AccountManager.GetAccount(connectionToClient);
        GameObject unitPrefab = NetworkManager.singleton.spawnPrefabs.Find(x => x.GetComponent<NetworkIdentity>().assetId.Equals(acc.data.characterHash));
        GameObject unit = Instantiate(unitPrefab, acc.data.posCharacter, Quaternion.identity);
        // указываем объект игрока для определения видимости персонажа
        Character character = unit.GetComponent<Character>();
        character.player = player;
        // реплицируем персонажа
        NetworkServer.Spawn(unit);
        // настраиваем персонажа на клиенте, которому он принадлежит
        TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
        return character;
    }

    private void OnDestroy()
    {
        if (isServer && player.character != null)
        {
            UserAccount acc = AccountManager.GetAccount(connectionToClient);
            acc.data.posCharacter = player.character.transform.position;
            Destroy(player.character.gameObject);
            NetworkManager.singleton.StartCoroutine(acc.Quit());
        }
    }

    public override bool OnCheckObserver(NetworkConnection connection)
    {
        return false;
    }
}
