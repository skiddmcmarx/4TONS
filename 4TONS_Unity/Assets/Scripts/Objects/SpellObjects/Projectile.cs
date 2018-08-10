using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Projectile : PoolObject
{
    public float dmg;
    public float manaDmg;
    public float moveSpeed;
    private float defaultMoveSpeed;
    public AnimationCurve moveSpeedCurve;
    public Rigidbody2D rb;
    public bool isPhysical = false;
    public float dragPower;
    public bool leavesDebris = true;
    public GameObject explosion;
    public Transform explosionPool;
    

    public override void initialize()
    {
        base.initialize();
        if (leavesDebris)
            explosionPool = PoolManager.instance.CreatePool(explosion, 1);
        defaultMoveSpeed = moveSpeed;
    }
    public override void ResetObject()
    {
        base.ResetObject();
        moveSpeed = defaultMoveSpeed;
        if (isPhysical)
        {
            rb.AddRelativeForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
        }
    }

    public virtual void FixedUpdate()
    {
        if (isPhysical)
        {

            //rb.AddRelativeForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);
            rb.drag = dragPower / (lifeTimer - Time.time);
        }
        else
        {
            float currentSpeed = moveSpeedCurve.Evaluate(lifeTimer / lifeTime);
            currentSpeed *= moveSpeed;
            transform.Translate(Vector2.right * currentSpeed);
        }
    }
    public void ExplodeProjectile(Vector3 pos)
    {
        if (leavesDebris)
            PoolManager.instance.ReuseObject(explosion, pos, Quaternion.identity);
    }
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        //when this object hits something, Explode into dust.
        ContactPoint2D[] contax = new ContactPoint2D[2];
        other.GetContacts(contax);
        ExplodeProjectile(contax[0].point);
        Destroy();
        string otherName = other.gameObject.name;
       /* if (DamageManager.damageableObjects.ContainsKey(otherName))
        {
            print("found object " + otherName + " in dictionary. Applying damage.");
            DamageManager.damageableObjects[otherName].ApplyDamage(dmg, damageType);
        }*/




