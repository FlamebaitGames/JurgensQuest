using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public void LoadLevel(string name)
    {
        Application.LoadLevel(name);
    }
}
