using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSSkill;
using UnityEngine;

public class AttackBuff : SkillTriggerBase
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
    [Config("BuffSkillId","技能ID")]
    private int buffSkillId = 1000;
    [Config("BuffLevel", "Buff等级")]
    private int buffLevel = 1;

    public override void Execute(IPlayerData playerData)
    {
        Debug.LogError("Attack");
    }
}

