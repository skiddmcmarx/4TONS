using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObject : PoolObject
{

    public Transform trans;
    public string ObjectName;
    public bool alive;
    public float lifeTime;
    public float lifeTimer;
    public int damageType;

    //this function is called when the spellobjects are first created. Spell object finds all information about itself that it will need to work, aside from player info.
    public virtual void initialize()
    {
        trans = this.transform;

    }

    public override void loadObject()
    {
        tag = "Player1";
        gameObject.layer = 12;

    }
    //This function is called when a spell is cast and the object is initially brought into the world.
    public override void ResetObject()
    {
        gameObject.SetActive(true);
        lifeTimer = 0;
        alive = true;
    }
    public virtual void Update()
    {
        if (alive)
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= lifeTime)
            {
                print("lifetime exceeded. object destroyed");
                Destroy();
            }
        }
    }
    //this function is called when a spell object is destroyed.
    public override void TerminateObjectFunctions()
    {
        base.TerminateObjectFunctions();
        alive = false;
    }
}
