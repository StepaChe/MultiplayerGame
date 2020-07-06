using UnityEngine;
using UnityEngine.Networking;

public class PickUpItem : Interactable
{
    public Item item;

    // время, через которое предмет будет уничтожен
    public float lifetime = 60;

    private void Update()
    {
        if (isServer)
        {
                // уничтожение предмета по окончании времени его жизни
                lifetime -= Time.deltaTime;
                if (lifetime <= 0) Destroy(gameObject);
        }
    }
    public override bool Interact(GameObject user)
    {
        return PickUp(user);
    }

    public bool PickUp(GameObject user)
    {
        Character character = user.GetComponent<Character>();
        if (character != null && character.player.inventory.AddItem(item))
        {
            Destroy(gameObject);
            return true;
        }
        else return false;
    }
}
