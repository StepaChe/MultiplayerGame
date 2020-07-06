using UnityEngine;

public class ChestInteractive : Interactable
{
    [SerializeField] private Animator animator;
    private bool key = false;

    public override bool Interact(GameObject user)
    {
       // Character character = user.GetComponent<Character>();
      //  if (character != null)
        //{
            Open();
            //return true;
       // }
        return false;
    }

    void FixedUpdate()
    {
        if (!key)
        {
            animator.SetBool("Key", key);
        }
        else
        {
            animator.SetBool("Key", key);
        }
    }

    private void Open()
    {
        key = true;
        //animator.SetBool("Key", true);
        Invoke("Close", 3.0f);
    }

    private void Close()
    {
        key = false;
        //animator.SetBool("Key", false);
    }
}
