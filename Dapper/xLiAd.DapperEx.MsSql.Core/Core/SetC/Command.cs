﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using xLiAd.DapperEx.MsSql.Core.Core.Interfaces;
using xLiAd.DapperEx.MsSql.Core.Model;

namespace xLiAd.DapperEx.MsSql.Core.Core.SetC
{
    /// <summary>
    /// 指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : ICommand<T>, IInsert<T>
    {
        public event DapperExExceptionHandler ErrorHappened;
        protected readonly SqlProvider<T> SqlProvider;
        protected readonly IDbConnection DbCon;
        private readonly IDbTransaction _dbTransaction;
        protected DataBaseContext<T> SetContext { get; set; }
        /// <summary>
        /// 是否抛出错误，如果不抛，可以用 event 实现日志功能
        /// </summary>
        public bool Throws { get; }
        /// <summary>
        /// 刚刚执行过的SQL语句（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public string SqlString { get; private set; }
        /// <summary>
        /// 刚刚执行过的语句使用的参数（注：由于单例模式时会发生线程问题，本属性只作为调试用，不应该在程序里引用。）
        /// </summary>
        public DynamicParameters Params { get; private set; }
        private void SetSql()
        {
            if (SqlProvider.SqlString != null)
                this.SqlString = SqlProvider.SqlString;
            if (SqlProvider.Params != null)
                this.Params = SqlProvider.Params;
        }
        /// <summary>
        /// 新建立命令器
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="sqlProvider">SQL转换器</param>
        /// <param name="dbTransaction">事务</param>
        /// <param name="throws">是否抛出错误</param>
        protected Command(IDbConnection conn, SqlProvider<T> sqlProvider, IDbTransaction dbTransaction = null, bool throws = true)
        {
            SqlProvider = sqlProvider;
            DbCon = conn;
            _dbTransaction = dbTransaction;
            Throws = throws;
        }

        public int Update(T entity)
        {
            SqlProvider.FormatUpdate(entity);
            SetSql();
            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public int Delete<TKey>(TKey id)
        {
            SqlProvider.FormatDelete(id);
            SetSql();
            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Update(Expression<Func<T, T>> updateExpression)
        {
            SqlProvider.FormatUpdate(updateExpression);
            SetSql();
            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public int Update(T model, params Expression<Func<T, object>>[] updateExpression)
        {
            SqlProvider.FormatUpdateZhanglei(model, updateExpression);
            SetSql();
            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }
        public int Update<TKey>(Expression<Func<T, TKey>> expression, TKey value)
        {
            SqlProvider.FormatUpdateZhanglei(expression, value);
            SetSql();
            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        public int Delete()
        {
            SqlProvider.FormatDelete();
            SetSql();
            return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        }

        //public int Insert(T entity)
        //{
        //    SqlProvider.FormatInsert(entity);

        //    return Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
        //}

        public int Insert(T entity)
        {
            SqlProvider.FormatInsert(entity, out var isHaveIdentity, out var property);
            SetSql();
            if (isHaveIdentity == System.ComponentModel.DataAnnotations.IdentityTypeEnum.Int)
            {
                var id = DbCon.ExecuteScalar<int>(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
                return id;
            }
            else if(isHaveIdentity == System.ComponentModel.DataAnnotations.IdentityTypeEnum.Guid)
            {
                var gi = DbCon.ExecuteScalar<Guid>(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
                property.SetValue(entity, gi);
                return 1;
            }
            else
            {
                var r = Exec(SqlProvider.SqlString, SqlProvider.Params, _dbTransaction);
                return r;
            }
        }
        public int Insert(IEnumerable<T> entitys)
        {
            if (entitys.Count() < 1)
                return 0;
            SqlProvider.FormatInsert(entitys.First(), out var isHaveIdentity,out var _, true);
            SetSql();
            var rst = DbCon.Execute(SqlProvider.SqlString, entitys, _dbTransaction);
            return rst;
        }

        private int Exec(string sqlString, DynamicParameters param, IDbTransaction dbTransaction)
        {
            try
            {
                return DbCon.Execute(sqlString, param, dbTransaction);
            }
            catch(Exception e)
            {
                CallEvent(sqlString, param, e.Message);
                if (Throws)
                    throw new Exception($"{e.Message} sql:{sqlString} params:{param}", e);
                else
                    return 0;
            }
        }
        private void CallEvent(string sqlString, DynamicParameters param, string message)
        {
            try
            {
                var args = new DapperExEventArgs(sqlString, param, message);
                ErrorHappened?.Invoke(this, args);
            }
            catch { }
        }
    }
}
