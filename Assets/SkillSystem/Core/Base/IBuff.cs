using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSSkill
{
    public interface IBuff
    {
        Int64 NeedComponent { get; }

        void Execute(IPlayerData playerData);
    }

    public abstract class BuffBase : IBuff
    {
        public abstract long NeedComponent { get; }

        private float _startTime = 0;
        public float StartTime { get { return _startTime; } protected set { _startTime = value; } }

        private bool _isExecuted = false;
        /// <summary>
        /// 是否已经执行完毕
        /// </summary>
        public bool IsExecuted { get { return _isExecuted; } protected set { _isExecuted = value; } }

        public abstract void Execute(IPlayerData playerData);

        private BuffBase() { }

        public BuffBase (string args)
        {

        }

    }
}
