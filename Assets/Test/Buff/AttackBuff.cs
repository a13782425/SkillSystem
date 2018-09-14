using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSSkill;
using UnityEngine;

public class AttackBuff : BuffBase
{
    public AttackBuff(string args) : base(args)
    {
    }

    public override long NeedComponent
    {
        get
        {
            return SkillComponentIds.ALL_COMPONENT;
        }
    }

    public override void Execute(IPlayerData playerData)
    {
        Debug.LogError("Attack");
    }
}

