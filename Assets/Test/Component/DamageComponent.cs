using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : TSSkill.ISkillComponent
{

    public long ComponentId
    {
        get
        {
            return SkillComponentIds.Damage;
        }
    }
}
