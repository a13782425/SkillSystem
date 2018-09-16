using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSSkill;

public class LevelComponent : BaseSkillComponent
{
    public override long ComponentId
    {
        get
        {
            return SkillComponentIds.Level;
        }
    }

    public LevelComponent(SkillData skillData) : base(skillData)
    {
    }
}


