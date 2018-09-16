using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : TSSkill.ISkillComponent
{
    public long ComponentId
    {
        get
        {
            return SkillComponentIds.Move;
        }
    }

    public float Value
    {
        get;
        set;
    }
}
