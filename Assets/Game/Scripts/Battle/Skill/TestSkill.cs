using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : ISkill
{
    public float CDTime { get; set; }
    public float CDTimer { get; set; }

    bool b = true;

    public bool Check()
    {
        return b;
    }

    public void ExcuteSkill()
    {
        if (Check())
        {
            Debug.Log("执行Test技能");
        }
        b = false;
    }

    public void Init()
    {
        CDTime = 5;
    }

    public void Release()
    {

    }

    public void Update()
    {
        if (!b)
        {
            CDTimer += Time.deltaTime;
            if (CDTimer >= CDTime)
            {
                CDTimer = 0;
                b = true;
            }
        }
    }
}
