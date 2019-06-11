using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace General.Core
{
    public class EnginContext
    {
        private static IEngine _engine;
        /// <summary>
        /// 异步，一个一个来，线程安全
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IEngine engine)
        {
            if (_engine == null)
                _engine = engine;
            return _engine;
        }

        /// <summary>
        /// 当前引擎
        /// </summary>
        public static IEngine Current
        {
            get { return _engine; }
        }
    }
}
