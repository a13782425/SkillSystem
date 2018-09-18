using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Reflection;
using System.Xml;

namespace TSSkill
{
    public class SkillSystem
    {

        #region 静态变量

        private static SkillSystem _instance = null;
        public static SkillSystem Instance { get { if (_instance == null) _instance = new SkillSystem(); return _instance; } }

        #endregion

        #region 缓存的类型

        private Type _skillDataType = null;
        private Type _iskillComponentType = null;
        private Type _iskillTriggerType = null;
        private Type _injectType = null;
        private Type _dependencyType = null;
        private Type _configType = null;

        #endregion

        #region 缓存数据

        private Dictionary<string, Type> _skillTriggerTypeCacheDic = null;

        private Dictionary<string, FieldInfo> _skillFieldInfoDic = null;

        private Dictionary<string, Dictionary<string, FieldInfo>> _skillComponentFieldInfoCacheDic = null;

        private Dictionary<string, Dictionary<string, FieldInfo>> _skillBuffFieldInfoCacheDic = null;

        private Dictionary<int, SkillEntity> _firstGenerationCacheDic = null;

        private Dictionary<int, Stack<SkillEntity>> _skillEntityCacheDic = null;

        private Dictionary<int, string> _skillStringCacheDic = null;
        /// <summary>
        /// Key：技能id   Value：key:等级，value：具体技能表现
        /// </summary>
        private Dictionary<int, Dictionary<int, XmlNode>> _skillXmlCacheDic = null;

        private string _cacheSkillString = null;

        private XmlHelper _skillXmlHelper = null;

        #endregion

        #region 私有变量

        private bool _isInit = false;
        private int skillXml;

        /// <summary>
        /// 是否初始化完毕
        /// </summary>
        public bool R_IsInit { get { return _isInit; } }


        #endregion

        #region 公共方法

        public void Init(string skillData)
        {
            if (string.IsNullOrEmpty(skillData))
            {
                Debug.LogError("技能数据为空");
                return;
            }
            _cacheSkillString = skillData;
            _skillXmlHelper = new XmlHelper(skillData);
            List<XmlNode> nodes = _skillXmlHelper.GetElements("Skill");

            foreach (XmlNode item in nodes)
            {
                Dictionary<int, XmlNode> temp = new Dictionary<int, XmlNode>();
                XmlAttribute itemAttribute = item.Attributes["SkillId"];
                if (itemAttribute == null)
                {
                    throw new Exception("技能等级为空！");
                }
                int skillId = 0;
                if (!int.TryParse(itemAttribute.Value, out skillId))
                {
                    throw new Exception("技能Id必须为数字");
                }
                if (_skillXmlCacheDic.ContainsKey(skillId))
                {
                    Debug.LogError("技能ID：" + skillId + "重复!");
                }
                XmlNodeList nodeList = item.SelectNodes("Levels/Level");
                foreach (XmlNode node in nodeList)
                {
                    XmlAttribute xmlAttribute = node.Attributes["Value"];
                    if (xmlAttribute == null)
                    {
                        throw new Exception("技能等级为空！");
                    }
                    int level = 0;
                    if (int.TryParse(xmlAttribute.Value, out level))
                    {
                        if (!temp.ContainsKey(level))
                        {
                            temp.Add(level, node);
                        }
                        else
                        {
                            Debug.LogError("技能ID：" + skillId + "中有等级重复!");
                        }
                    }
                    else
                    {
                        throw new Exception("技能等级必须为数字");
                    }
                }
                _skillXmlCacheDic.Add(skillId, temp);
            }

            _isInit = true;
        }


        /// <summary>
        /// 获取技能数据中的值
        /// </summary>
        /// <param name="skillData">技能数据</param>
        /// <param name="injectName">依赖名字</param>
        public object GetSkillDataValue(SkillData skillData, string injectName)
        {
            if (string.IsNullOrEmpty(injectName))
            {
                return null;
            }
            if (_skillFieldInfoDic.ContainsKey(injectName))
            {
                FieldInfo fieldInfo = _skillFieldInfoDic[injectName];
                try
                {
                    return fieldInfo.GetValue(skillData);
                }
                catch (Exception ex)
                {
                    Debug.LogError("获取数据失败，获取数据名：" + injectName + "，获取失败原因：" + ex.Message);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取技能数据中的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skillData">技能数据</param>
        /// <param name="injectName">依赖名字</param>
        /// <returns></returns>
        public T GetSkillDataValueAsT<T>(SkillData skillData, string injectName)
        {
            object value = GetSkillDataValue(skillData, injectName);
            if (value != null)
            {
                if (value is T)
                {
                    return (T)value;
                }
                return default(T);
            }
            return default(T);
        }

        /// <summary>
        /// 获取Component字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Dictionary<string, FieldInfo> GetComponentField(Type type)
        {
            Dictionary<string, FieldInfo> tempDic = new Dictionary<string, FieldInfo>();
            if (_iskillComponentType.IsAssignableFrom(type))
            {
                string name = type.Name;
                if (_skillComponentFieldInfoCacheDic.ContainsKey(name))
                {
                    return _skillComponentFieldInfoCacheDic[name];
                }
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo item in fieldInfos)
                {
                    object[] attrs = item.GetCustomAttributes(_dependencyType, false);

                    if (attrs.Length > 0)
                    {
                        DependencyAttribute dependencyAttribute = attrs[0] as DependencyAttribute;
                        if (dependencyAttribute != null)
                        {
                            string fieldName = item.Name;
                            if (!string.IsNullOrEmpty(dependencyAttribute.R_DependName))
                            {
                                fieldName = dependencyAttribute.R_DependName;
                            }
                            tempDic.Add(fieldName, item);
                        }
                    }
                }
                _skillComponentFieldInfoCacheDic.Add(name, tempDic);
            }
            return tempDic;
        }

        /// <summary>
        /// 获取Buff字段
        /// </summary>
        /// <param name="type"></param>
        public Dictionary<string, FieldInfo> GetTriggerField(Type type)
        {
            Dictionary<string, FieldInfo> tempDic = new Dictionary<string, FieldInfo>();
            if (_iskillComponentType.IsAssignableFrom(type))
            {
                string name = type.Name;
                if (_skillBuffFieldInfoCacheDic.ContainsKey(name))
                {
                    return _skillBuffFieldInfoCacheDic[name];
                }
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo item in fieldInfos)
                {
                    object[] attrs = item.GetCustomAttributes(_configType, false);

                    if (attrs.Length > 0)
                    {
                        ConfigAttribute configAttribute = attrs[0] as ConfigAttribute;
                        if (configAttribute != null)
                        {
                            string fieldName = item.Name;
                            if (!string.IsNullOrEmpty(configAttribute.R_ConfigName))
                            {
                                fieldName = configAttribute.R_ConfigName;
                            }
                            tempDic.Add(fieldName, item);
                        }
                    }
                }
                _skillBuffFieldInfoCacheDic.Add(name, tempDic);
            }
            return tempDic;
        }

        /// <summary>
        /// 获取技能触发器 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ISkillTrigger GetTrigger(string name)
        {
            if (_skillTriggerTypeCacheDic.ContainsKey(name))
            {
                return Activator.CreateInstance(_skillTriggerTypeCacheDic[name]) as ISkillTrigger;
            }
            return null;
        }


        /// <summary>
        /// 创建技能实体
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool CreateSkillEntity(int skillId, int level)
        {
            if (!_skillXmlCacheDic.ContainsKey(skillId))
            {
                Debug.LogError("技能Id:" + skillId + "不存在");
                return false;
            }
            if (!_skillXmlCacheDic[skillId].ContainsKey(level))
            {
                Debug.LogError("技能Id:" + skillId + ",等级：" + level + "不存在");
                return false;
            }
            XmlNode skillXml = _skillXmlCacheDic[skillId][level];
            SkillEntity entity = new SkillEntity(skillId, skillXml);

            return true;
        }


        #endregion



        /// <summary>
        /// 构造函数
        /// </summary>
        private SkillSystem()
        {
            //_skillDataType = typeof(SkillData);
            _iskillComponentType = typeof(ISkillComponent);
            _iskillTriggerType = typeof(ISkillTrigger);
            //_injectType = typeof(InjectAttribute);
            //_dependencyType = typeof(DependencyAttribute);
            _configType = typeof(ConfigAttribute);
            //_skillComponentFieldInfoCacheDic = new Dictionary<string, Dictionary<string, FieldInfo>>();
            //_skillBuffFieldInfoCacheDic = new Dictionary<string, Dictionary<string, FieldInfo>>();
            //_skillFieldInfoDic = new Dictionary<string, FieldInfo>();
            //_skillStringCacheDic = new Dictionary<int, string>();
            //_firstGenerationCacheDic = new Dictionary<int, SkillEntity>();
            //_skillEntityCacheDic = new Dictionary<int, Stack<SkillEntity>>();
            _skillTriggerTypeCacheDic = new Dictionary<string, Type>();
            _skillXmlCacheDic = new Dictionary<int, Dictionary<int, XmlNode>>();
            Type[] allTypes = _iskillTriggerType.Assembly.GetTypes();
            foreach (Type type in allTypes)
            {
                if (!type.IsAbstract && _iskillTriggerType.IsAssignableFrom(type))
                {
                    if (!_skillTriggerTypeCacheDic.ContainsKey(type.Name))
                    {
                        _skillTriggerTypeCacheDic.Add(type.Name, type);
                    }
                }
            }





            //_skillDataType = typeof(SkillData);
            //_iskillComponentType = typeof(ISkillComponent);
            //_injectType = typeof(InjectAttribute);
            //_dependencyType = typeof(DependencyAttribute);
            //_configType = typeof(ConfigAttribute);
            //_skillComponentFieldInfoCacheDic = new Dictionary<string, Dictionary<string, FieldInfo>>();
            //_skillBuffFieldInfoCacheDic = new Dictionary<string, Dictionary<string, FieldInfo>>();
            //_skillFieldInfoDic = new Dictionary<string, FieldInfo>();
            //_skillStringCacheDic = new Dictionary<int, string>();
            //_firstGenerationCacheDic = new Dictionary<int, SkillEntity>();
            //_skillEntityCacheDic = new Dictionary<int, Stack<SkillEntity>>();
            //_skillXmlCacheDic = new Dictionary<int, Dictionary<int, XmlNode>>();
            //FieldInfo[] fieldInfos = _skillDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //foreach (FieldInfo item in fieldInfos)
            //{
            //    object[] attrs = item.GetCustomAttributes(_injectType, false);

            //    if (attrs.Length > 0)
            //    {
            //        InjectAttribute injectAttribute = attrs[0] as InjectAttribute;
            //        if (injectAttribute != null)
            //        {
            //            string fieldName = item.Name;
            //            if (!string.IsNullOrEmpty(injectAttribute.R_InjectName))
            //            {
            //                fieldName = injectAttribute.R_InjectName;
            //            }
            //            this._skillFieldInfoDic.Add(fieldName, item);
            //        }
            //    }
            //}

        }
    }
}
