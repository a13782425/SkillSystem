using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static partial class SkillComponentIds
{

    #region 标签位

    /// <summary>
    /// 低位标签
    /// </summary>
    private const Int64 LOW_FLAG = 0L;
    /// <summary>
    /// 高位标签
    /// </summary>
    private const Int64 HIGH_FLAG = 1L << 63;

    #endregion

    #region 系统
    /// <summary>
    /// 需要所有组件
    /// </summary>
    public const Int64 ALL_COMPONENT = Int64.MaxValue;
    /// <summary>
    /// 不需要任何组件
    /// </summary>
    public const Int64 NONE_COMPONENT = Int64.MaxValue;

    #endregion
}

public static partial class SkillTriggerIds
{

    #region 标签位

    /// <summary>
    /// 低位标签
    /// </summary>
    private const Int64 LOW_FLAG = 0L;
    /// <summary>
    /// 高位标签
    /// </summary>
    private const Int64 HIGH_FLAG = 1L << 63;

    #endregion
}

[Serializable]
public partial class SkillData
{
    public int MaxLevel;

    //public int Level;
}