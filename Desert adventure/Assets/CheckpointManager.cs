using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private static Checkpoint c_activeCheckpoint = null;
    public static Checkpoint ActiveCheckpoint
    {
        set
        {
            c_activeCheckpoint = value;
        }
        get
        {
            return c_activeCheckpoint;
        }
    }
}