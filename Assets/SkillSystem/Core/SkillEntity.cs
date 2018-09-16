using System.Collections;
using System.Collections.Generic;
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


        public SkillEntity Clone()
        {
            SkillEntity entity = new SkillEntity();
            entity._id = this.R_Id;
            entity.Name = this.Name;
            foreach (KeyValuePair<long, BaseSkillComponent> item in this._componentDic)
            {
                entity._componentDic.Add(item.Key, item.Value.Clone());
            }

            return entity;
        }



        #region 构造函数
        public SkillEntity() { }
        /// <summary>
        /// 技能实体
        /// </summary>
        /// <param name="id">技能ID</param>
        public SkillEntity(int id) : this(id, null) { }
        /// <summary>
        /// 技能实体
        /// </summary>
        /// <param name="id">技能ID</param>
        /// <param name="data">技能参数</param>
        public SkillEntity(int id, string data)
        {
            _id = id;

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
        public SkillEntity(string data)
        {

        }


        #endregion


    }
}


