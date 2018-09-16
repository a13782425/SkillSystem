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
        public float Value { get; set; }

        protected SkillData _currentSkillData = null;
        private Dictionary<string, FieldInfo> _fieldInfoDic = null;


        public BaseSkillComponent Clone()
        {
            BaseSkillComponent baseSkillComponent = Activator.CreateInstance(this.GetType()) as BaseSkillComponent;
            if (baseSkillComponent !=null)
            {
                baseSkillComponent.Value = this.Value;
            }
            return baseSkillComponent;
        }




        private BaseSkillComponent() { }

        public BaseSkillComponent(SkillData skillData)
        {
            Refresh(skillData);
        }
        /// <summary>
        /// 刷新组件
        /// </summary>
        /// <param name="skillData"></param>
        public void Refresh(SkillData skillData)
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
