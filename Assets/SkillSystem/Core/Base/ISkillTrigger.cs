using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSSkill
{
    public interface ISkillTrigger
    {
        void Init(string args);
        void Reset();
        ISkillTrigger Clone();
        bool Execute(object data, float curTime);
        float GetStartTime();
        bool IsExecuted();

        string GetTypeName();
    }
}

