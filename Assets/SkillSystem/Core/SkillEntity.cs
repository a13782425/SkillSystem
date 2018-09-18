using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace TSSkill
{
    public class SkillEntity
    {
        private int _id = -1;
        /// <summary>
        /// 技能ID
        /// </summary>
        public int R_Id { get { return _id; } }

        private string _name = null;

        /// <summary>
        /// 技能名称（可选）
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        private Dictionary<long, BaseSkillComponent> _componentDic = new Dictionary<long, BaseSkillComponent>();

        private List<ISkillTrigger> _skillTriggerList = new List<ISkillTrigger>();
        private XmlNode _skillXml;


        #region 构造函数
        private SkillEntity() { }

        /// <summary>
        /// 技能实体
        /// /*
        /// <Level Value="2">
		///	    <AddBuff buffSkillId = "2" buffLevel="2"/>
		///	    <AttackBuff buffSkillId = "2" buffLevel="2"/>
		/// </Level>
        /// */
        /// </summary>
        /// <param name="id">技能ID</param>
        /// <param name="data">技能参数</param>
        public SkillEntity(int id, XmlNode skillXml)
        {
            this._id = id;
            this._skillXml = skillXml;
            _skillTriggerList = new List<ISkillTrigger>();
            XmlNodeList nodeList = _skillXml.ChildNodes;
            foreach (XmlNode item in nodeList)
            {
                if (item is XmlElement)
                {
                    string triggerName = item.Name;
                    ISkillTrigger skillTrigger= SkillSystem.Instance.GetTrigger(triggerName);
                    if (skillTrigger == null)
                    {
                        Debug.LogError(triggerName + "没有找到类型！！！");
                        continue;
                    }
                    
                    _skillTriggerList.Add(skillTrigger);
                }
            }


        }

        /*
Skill(1000)
{
	SkillType:10
	MaxLevel:2
	Levels:[
		[
			Level : 1,
			属性 : value,
			Trigger:
			[
				AddBuff(buffSkillId:1,buffLevel:1),
				AddBuff(buffSkillId:2,buffLevel:2),
			]
		],
		[
			Level : 2,
			属性 : value,
			Trigger:
			[
				AddBuff(buffSkillId:1,buffLevel:1),
				AddBuff(buffSkillId:2,buffLevel:2),
				AddBuff(buffSkillId:3,buffLevel:3),
			]
		],
	]
}
         */



        #endregion


    }
}


