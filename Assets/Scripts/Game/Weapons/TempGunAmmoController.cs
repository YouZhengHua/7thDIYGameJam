using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using UnityEngine;

public class TempGunAmmoController : MonoBehaviour, IAmmoController
{
    public Transform SetPlayerTransform { set => throw new System.NotImplementedException(); }
    public IEndUIController SetEndUI { set => throw new System.NotImplementedException(); }

    public bool IsActive => true;

    public int AmmoGroup { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void HitEmeny()
    {
        //nothing to do
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
