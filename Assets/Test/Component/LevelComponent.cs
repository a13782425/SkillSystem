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

    [Dependency]
    public int Level;

    public LevelComponent(ISkillData skillData) : base(skillData)
    {
    }
}


