using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSSkill
{
    public interface ISkillComponent
    {
        Int64 ComponentId { get; }

        Single Value { get; set; }
    }
}

