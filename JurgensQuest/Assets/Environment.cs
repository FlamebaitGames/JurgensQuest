using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {
    public Craen120 helicopter;

    public void Reset()
    {
        BroadcastMessage("OnReset");
    }
}
