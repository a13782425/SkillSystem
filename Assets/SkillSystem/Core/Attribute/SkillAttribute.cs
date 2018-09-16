using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSSkill
{

    /// <summary>
    /// 依赖，需要某个值注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DependencyAttribute : Attribute
    {
        private string _dependName = null;
        public string R_DependName
        {
            get
            {
                return _dependName;
            }
        }
        /// <summary>
        /// 此构造使用字段自带名称查找依赖
        /// </summary>
        public DependencyAttribute() : this(null) { }
        /// <summary>
        /// 此构造输入的依赖名称
        /// </summary>
        /// <param name="dependName"></param>
        public DependencyAttribute(string dependName) { _dependName = dependName; }

    }
    /// <summary>
    /// 注入，提供给依赖使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class InjectAttribute : Attribute
    {
        private string _injectName = null;
        public string R_InjectName
        {
            get
            {
                return _injectName;
            }
        }
        /// <summary>
        /// 此构造使用字段自带名称进行注入
        /// </summary>
        public InjectAttribute() : this(null) { }
        /// <summary>
        /// 此构造改变注入名称，在依赖时请注意
        /// </summary>
        /// <param name="injectName"></param>
        public InjectAttribute(string injectName) { _injectName = injectName; }
    }

    /// <summary>
    /// 需要配置的字段名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ConfigAttribute : Attribute
    {
        private string _configName = null;
        public string R_ConfigName { get { return _configName; } }

        private string _editorShowName = null;
        public string R_EditorShowName { get { return _editorShowName; } }

        /// <summary>
        /// 此属性为Buff 需要读表的字段添加
        /// </summary>
        public ConfigAttribute() : this(null) { }
        /// <summary>
        /// 此属性为Buff 需要读表的字段添加
        /// </summary>
        /// <param name="configName"></param>
        public ConfigAttribute(string configName) : this(configName, null) { }

        /// <summary>
        /// 此属性为Buff 需要读表的字段添加
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="editorShowName"></param>
        public ConfigAttribute(string configName, string editorShowName) { _configName = configName; _editorShowName = editorShowName; }
    }

}
