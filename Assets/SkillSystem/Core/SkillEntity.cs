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

        #region 构造函数
        
        /// <summary>
        /// 技能实体
        /// </summary>
        /// <param name="id">技能ID</param>
        public SkillEntity(int id) : this(id, null) { }
        /// <summary>
        /// 技能实体
        /// </summary>
        /// <param name="id">技能ID</param>
        /// <param name="name">技能名称</param>
        public SkillEntity(int id, string name) { _id = id; _name = name; }

        #endregion
        public List<ISkillTrigger> m_SkillTriggers = new List<ISkillTrigger>();



    }
}


