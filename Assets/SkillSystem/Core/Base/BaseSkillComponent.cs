using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TSSkill
{
    public abstract class BaseSkillComponent : ISkillComponent
    {
        public abstract Int64 ComponentId { get; }
        protected ISkillData _currentSkillData = null;
        private Dictionary<string, FieldInfo> _fieldInfoDic = null;

        private BaseSkillComponent() { }

        public BaseSkillComponent(ISkillData skillData)
        {
            Refresh(skillData);
        }
        /// <summary>
        /// 刷新组件
        /// </summary>
        /// <param name="skillData"></param>
        public void Refresh(ISkillData skillData)
        {
            _currentSkillData = skillData;
            if (_fieldInfoDic == null)
            {
                _fieldInfoDic = new Dictionary<string, FieldInfo>(SkillSystem.Instance.GetComponentField(this.GetType()));
            }
            if (_fieldInfoDic.Count > 0)
            {
                foreach (KeyValuePair<string, FieldInfo> item in _fieldInfoDic)
                {
                    item.Value.SetValue(this, SkillSystem.Instance.GetSkillDataValue(skillData, item.Key));
                }
            }
        }
    }
}
