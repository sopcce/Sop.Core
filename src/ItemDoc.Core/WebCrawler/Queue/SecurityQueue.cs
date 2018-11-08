using System.Collections.Generic;
using System.Threading;

namespace ItemDoc.Core.WebCrawler.Queue
{
    /// <summary>
    /// The security queue.
    /// </summary>
    /// <typeparam name="T">
    /// Any type.
    /// </typeparam>
    public abstract class SecurityQueue<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// The inner queue.
        /// </summary>
        protected readonly Queue<T> InnerQueue = new Queue<T>();

        /// <summary>
        /// The sync object.
        /// </summary>
        protected readonly object SyncObject = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityQueue{T}"/> class.
        /// </summary>
        protected SecurityQueue()
        {
            this.AutoResetEvent = new AutoResetEvent(false);
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The before en queue event handler.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public delegate bool BeforeEnQueueEventHandler(T target);

        #endregion

        #region Public Events

        /// <summary>
        /// The before en queue event.
        /// </summary>
        public event BeforeEnQueueEventHandler BeforeEnQueueEvent;

        #endregion

        #region Public Properties

        
        public AutoResetEvent AutoResetEvent { get; }


        public int Count
        {
            get
            {
                lock (this.SyncObject)
                {
                    return this.InnerQueue.Count;
                }
            }
        }

        
        public bool HasValue
        {
            get
            {
                return this.Count != 0;
            }
        }

        #endregion

        #region Public Methods and Operators

        
        public T DeQueue()
        {
            lock (this.SyncObject)
            {
                if (this.InnerQueue.Count > 0)
                {
                    return this.InnerQueue.Dequeue();
                }

                return default(T);
            }
        }

        
        public void EnQueue(T target)
        {
            lock (this.SyncObject)
            {
                if (this.BeforeEnQueueEvent != null)
                {
                    if (this.BeforeEnQueueEvent(target))
                    {
                        this.InnerQueue.Enqueue(target);
                    }
                }
                else
                {
                    this.InnerQueue.Enqueue(target);
                }

                this.AutoResetEvent.Set();
            }
        }

        #endregion
    }
}