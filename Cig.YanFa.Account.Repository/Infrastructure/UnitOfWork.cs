﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContext _context;
        public UnitOfWork(IContext context)
        {
            _context = context;
            _context.BeginTransaction();
        }
        public void SaveChanges()
        {
            if (!_context.IsTransactionStarted)
                throw new InvalidOperationException("Transaction have already been commited or disposed.");
            _context.Commit();
        }
        public void Dispose()
        {
            if (_context.IsTransactionStarted)
                _context.Rollback();
        }
    }
}
