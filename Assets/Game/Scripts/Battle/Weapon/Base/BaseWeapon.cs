using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotMode
{
    None=0,
    SingleShot,//单发
    TripleShot,//三连发
    AutoShot,  //自动
    Max,
}

public abstract class BaseWeapon{

    /// <summary>
    /// 每次射击消耗的MP
    /// </summary>
    private int mExpenseMP;

    private ShotMode mMode = ShotMode.None;
    private GameObject mBullet;

    private GameObject mWeaponGo;
    private GameObject mMuzzleEff;
    private Transform mMuzzlePos;

    public BaseWeapon(int expenseMP, ShotMode mode, GameObject bullet,GameObject weaponGo, GameObject muzzleEff, Transform muzzlePos)
    {
        mExpenseMP = expenseMP;
        mMode = mode;
        mBullet = bullet;
        mWeaponGo = weaponGo;
        mMuzzleEff = muzzleEff;
        mMuzzlePos = muzzlePos;
    }

    public abstract void Fire();

}
