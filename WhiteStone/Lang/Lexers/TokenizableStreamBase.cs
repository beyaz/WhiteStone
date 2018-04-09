using System;
using System.Collections.Generic;

namespace Lang.Lexers
{
    /// <summary>
    ///     The tokenizable stream base
    /// </summary>
    public class TokenizableStreamBase<T> where T : class
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenizableStreamBase{T}" /> class.
        /// </summary>
        public TokenizableStreamBase(Func<List<T>> extractor)
        {
            Index = 0;

            Items = extractor();

            SnapshotIndexes = new Stack<int>();
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the current.
        /// </summary>
        public virtual T Current
        {
            get
            {
                if (EOF(0))
                {
                    return null;
                }

                return Items[Index];
            }
        }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        protected int Index { get; set; }

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        List<T> Items { get; set; }

        /// <summary>
        ///     Gets or sets the snapshot indexes.
        /// </summary>
        Stack<int> SnapshotIndexes { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Commits the snapshot.
        /// </summary>
        public void CommitSnapshot()
        {
            SnapshotIndexes.Pop();
        }

        /// <summary>
        ///     Consumes this instance.
        /// </summary>
        public void Consume()
        {
            Index++;
        }

        /// <summary>
        ///     Ends this instance.
        /// </summary>
        public bool End()
        {
            return EOF(0);
        }

        /// <summary>
        ///     Peeks the specified lookahead.
        /// </summary>
        public virtual T Peek(int lookahead)
        {
            if (EOF(lookahead))
            {
                return null;
            }

            return Items[Index + lookahead];
        }

        /// <summary>
        ///     Rollbacks the snapshot.
        /// </summary>
        public void RollbackSnapshot()
        {
            Index = SnapshotIndexes.Pop();
        }

        /// <summary>
        ///     Takes the snapshot.
        /// </summary>
        public void TakeSnapshot()
        {
            SnapshotIndexes.Push(Index);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     EOFs the specified lookahead.
        /// </summary>
        bool EOF(int lookahead)
        {
            if (Index + lookahead >= Items.Count)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}