using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSSkill;
using UnityEngine;

public class AttackBuff : SkillTriggerBase
{
    //public AttackBuff(string args) : base(args)
    //{
    //}

    public override long SkillTriggerId
    {
        get
        {
            return SkillTriggerIds.Damage;
        }
    }
    [Config("Attack", "伤害")]
    private int attack = 1000;


    public override void Execute(IPlayerData playerData)
    {
        Debug.LogError("造成了" + attack + "伤害");
        Debug.LogError("Attack");
    }
}

