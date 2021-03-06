#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Text;

using NHibernate;
using ClearCanvas.Enterprise.Core;

namespace ClearCanvas.Enterprise.Hibernate
{
    /// <summary>
    /// NHibernate implemenation of <see cref="IReadContext"/>.
    /// </summary>
    public class ReadContext : PersistenceContext, IReadContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sessionFactory"></param>
        internal ReadContext(PersistentStore pstore)
            : base(pstore)
        {
        }

        #region Protected overrides

        protected override ISession CreateSession()
        {
            ISession session = this.PersistentStore.SessionFactory.OpenSession();

            // never write changes to the database from a read context
            session.FlushMode = FlushMode.Never;
            return session;
        }

        protected override void SynchStateCore()
        {
            // do nothing
        }

        internal override bool ReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Commits the transaction (does not flush anything to the database)
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // commit the transaction - nothing will be flushed to the DB
                    CommitTransaction();
                }
                catch (Exception e)
                {
                    HandleHibernateException(e, SR.ExceptionCloseContext);
                }
            }

            base.Dispose(disposing);
        }

        protected override EntityLoadFlags DefaultEntityLoadFlags
        {
            get { return EntityLoadFlags.None; }
        }

        protected override void LockCore(DomainObject obj, DirtyState dirtyState)
        {
            if (dirtyState != DirtyState.Clean)
                throw new InvalidOperationException();

            this.Session.Lock(obj, LockMode.None);
        }

        #endregion
    }
}
