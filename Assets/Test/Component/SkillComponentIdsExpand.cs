using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class SkillComponentIds
{
    /// <summary>
    /// 等级
    /// </summary>
    public const Int64 Move = (1L << 0) | LOW_FLAG;
    /// <summary>
    /// 等级
    /// </summary>
    public const Int64 Damage = (1L << 1) | LOW_FLAG;
    /// <summary>
    /// 等级
    /// </summary>
    public const Int64 Level = (1L << 2) | LOW_FLAG;
}

