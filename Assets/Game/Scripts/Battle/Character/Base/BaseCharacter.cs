using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{

    public Animator mAnimator;

    public CharacterData mData;
    public BaseWeapon mBaseWeapon;
    protected FSMSystem mAIFSM;
    protected ISkill mSkill;

    public abstract void InitCharacter();
}
