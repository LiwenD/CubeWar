using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    float CDTime { get; set; }

    float CDTimer { get; set; }

    void Init();

    void Update();

    void Release();

    bool Check();

    void ExcuteSkill();
}
