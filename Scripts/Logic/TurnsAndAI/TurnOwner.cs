using UnityEngine;
using System.Collections;

public abstract class TurnOwner : MonoBehaviour {

    protected Player PlayerOwner;

    void Awake()
    {
        PlayerOwner = GetComponent<Player>();
    }

    public virtual void OnTurnStart()
    {
        PlayerOwner.OnTurnStart();
    }

}
