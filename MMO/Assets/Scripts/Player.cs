using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(StatsManager), typeof(NetworkIdentity), typeof (PlayerProgress))]
public class Player : MonoBehaviour
{
    [SerializeField] Character _character;
    [SerializeField] Inventory _inventory;
    [SerializeField] Equipment _equipment;
    [SerializeField] StatsManager _statsManager;
    [SerializeField] PlayerProgress _progress;

    public Character character { get { return _character; } }
    public Inventory inventory { get { return _inventory; } }
    public Equipment equipment { get { return _equipment; } }
    public PlayerProgress progress { get { return _progress; } }

    public NetworkConnection conn
    {
        get
        {
            if (_conn == null)
                _conn = GetComponent<NetworkIdentity>().connectionToClient;
            return _conn;
        }
    }

    NetworkConnection _conn;

    public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
    {
        _statsManager = GetComponent<StatsManager>();
        _progress = GetComponent<PlayerProgress>();
        _character = character;
        _inventory = inventory;
        _equipment = equipment;
        _character.player = this;
        _inventory.player = this;
        _equipment.player = this;
        _statsManager.player = this;

        if (GetComponent<NetworkIdentity>().isServer)
        {
            UserAccount account = AccountManager.GetAccount(GetComponent<NetworkIdentity>().connectionToClient);
            _character.stats.Load(account.data);
            _progress.Load(account.data);
            _inventory.Load(account.data);
            _equipment.Load(account.data);
            // передача менеджера статам игрока
            _character.stats.manager = _statsManager;
            // присваиваем прогрессу менеджер статов
            _progress.manager = _statsManager;
        }

        if (isLocalPlayer)
        {
            InventoryUI.instance.SetInventory(_inventory);
            EquipmentUI.instance.SetEquipment(_equipment);
            StatsUI.instance.SetManager(_statsManager);
        }
    }
}