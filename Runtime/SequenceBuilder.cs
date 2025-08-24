using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NSequence
{
    public class SequenceBuilder
    {
        private readonly List<IProcess> _processes = new();

        /// <summary>
        /// プロセスを追加
        /// </summary>
        public SequenceBuilder Add(IProcess process)
        {
            _processes.Add(process ?? throw new ArgumentNullException(nameof(process)));
            return this;
        }

        /// <summary>
        /// 遅延処理を追加
        /// </summary>
        public SequenceBuilder Delay(float seconds)
        {
            _processes.Add(new DelayProcess(Math.Max(0, seconds)));
            return this;
        }

        /// <summary>
        /// 条件付き処理を追加
        /// </summary>
        public SequenceBuilder If(Func<bool> condition, IProcess ifTrue, IProcess ifFalse = null)
        {
            _processes.Add(new ConditionalProcess(condition, ifTrue, ifFalse));
            return this;
        }

        /// <summary>
        /// アクション処理を追加
        /// </summary>
        public SequenceBuilder Do(Func<CancellationToken,UniTask> action)
        {
            _processes.Add(new ActionProcess(action));
            return this;
        }

        /// <summary>
        /// シーケンスを構築
        /// </summary>
        public Sequence Build()
        {
            var sequence = new Sequence(new List<IProcess>(_processes));
            _processes.Clear();

            return sequence;
        }
    }
}
