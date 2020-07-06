using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Unit))]
public class UnitDrop : NetworkBehaviour
{
    [SerializeField] DropItem[] dropItems = new DropItem[0];
    public float dropRadius = 1;
    Vector3 _drop;

    [System.Serializable]
    struct DropItem
    {
        public Item item;
        [Range(0, 100f)]
        public float rate;
    }

    private void Drop()
    {
        for (int i = 0; i < dropItems.Length; i++)
        {
            _drop = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(dropRadius, 0, 0) + transform.position;

            if (Random.Range(0, 100f) <= dropItems[i].rate)
            {
                PickUpItem pickupItem = Instantiate(dropItems[i].item.pickupPrefab, _drop, Quaternion.Euler(0, Random.Range(0, 360f), 0));
                pickupItem.item = dropItems[i].item;
                NetworkServer.Spawn(pickupItem.gameObject);
            }
        }
    }
    public override void OnStartServer()
    {
        GetComponent<Unit>().EventOnDie += Drop;
    }
}
