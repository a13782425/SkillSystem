using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Reflection;

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
        private Type _injectType = null;
        private Type _dependencyType = null;
        private Type _configType = null;

        #endregion

        #region 缓存数据

        private Dictionary<string, FieldInfo> _skillFieldInfoDic = null;

        private Dictionary<string, Dictionary<string, FieldInfo>> _skillComponentFieldInfoCacheDic = null;

        private Dictionary<string, Dictionary<string, FieldInfo>> _skillBuffFieldInfoCacheDic = null;

        private Dictionary<int, SkillEntity> _firstGenerationCacheDic = null;

        private Dictionary<int, Stack<SkillEntity>> _skillEntityCacheDic = null;

        private Dictionary<int, string> _skillStringCacheDic = null;

        private string _cacheSkillString = null;

        #endregion

        #region 私有变量

        private bool _isInit = false;
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

            //_skillStringCacheDic = SkillParser.ParserSkillIds(_cacheSkillString);
            foreach (KeyValuePair<int, string> item in _skillStringCacheDic)
            {
                SkillEntity skillEntity = new SkillEntity(item.Key, item.Value);

                Debug.LogError("SkillId:" + item.Key + "====SkillCode:" + item.Value);
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


        //    skill(1000) //技能1
        //    {
        //    FaceToTarget(0)
        //    PlayAnimation(1, Skill_1)
        //    Bullet(1.3, Bullet, 10)
        //    PlayEffect(0, Explode8, 3)
        //    AddBuff(1, 1, 1, 30%)
        //    Attack()
        //    }
        public bool CreateSkillEntity(int skillId)
        {
            //if (string.IsNullOrEmpty(skillConfig))
            //    return false;
            //string[] lines = skillConfig.Split('\n');

            //SkillEntity skillEntity = new SkillEntity(10);

            return true;
        }


        #endregion



        /// <summary>
        /// 构造函数
        /// </summary>
        private SkillSystem()
        {
            _skillDataType = typeof(SkillData);
            _iskillComponentType = typeof(ISkillComponent);
            _injectType = typeof(InjectAttribute);
            _dependencyType = typeof(DependencyAttribute);
            _configType = typeof(ConfigAttribute);
            _skillComponentFieldInfoCacheDic = new Dictionary<string, Dictionary<string, FieldInfo>>();
            _skillBuffFieldInfoCacheDic = new Dictionary<string, Dictionary<string, FieldInfo>>();
            _skillFieldInfoDic = new Dictionary<string, FieldInfo>();
            _skillStringCacheDic = new Dictionary<int, string>();
            _firstGenerationCacheDic = new Dictionary<int, SkillEntity>();
            _skillEntityCacheDic = new Dictionary<int, Stack<SkillEntity>>();
            FieldInfo[] fieldInfos = _skillDataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo item in fieldInfos)
            {
                object[] attrs = item.GetCustomAttributes(_injectType, false);

                if (attrs.Length > 0)
                {
                    InjectAttribute injectAttribute = attrs[0] as InjectAttribute;
                    if (injectAttribute != null)
                    {
                        string fieldName = item.Name;
                        if (!string.IsNullOrEmpty(injectAttribute.R_InjectName))
                        {
                            fieldName = injectAttribute.R_InjectName;
                        }
                        this._skillFieldInfoDic.Add(fieldName, item);
                    }
                }
            }

        }
    }
}
