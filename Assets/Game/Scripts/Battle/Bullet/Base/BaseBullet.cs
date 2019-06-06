using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{

    public int Damage { get; set; }
    public GameObject HitEff;

    public virtual void Update()
    {
        BulletMove();
    }

    public abstract void InitBullet();

    public abstract void BulletMove();
    public abstract void Hit();
}
