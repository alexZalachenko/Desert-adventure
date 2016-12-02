using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool c_hasBeenUsed = false;

    public void RestartGameFromCheckpoint(Transform p_playerTransform)
    {
        //restore game and player
        p_playerTransform.position = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (!c_hasBeenUsed && p_collider.gameObject.tag == "Player")
        {
            CheckpointManager.ActiveCheckpoint = this;
            c_hasBeenUsed = true;
        }
    }


}