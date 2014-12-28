using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Sqlite3DatabaseHandle = System.IntPtr;
using Sqlite3Statement = System.IntPtr;

namespace Fresh.Windows.Helpers
{
    public class SQLitePlatformW81 : ISQLitePlatform
    {
        public SQLitePlatformW81()
        {
            var api = new SQLiteApi();

            // api.SetDirectory(/*temp directory type*/2, Windows.Storage.ApplicationData.Current.TemporaryFolder.Path); 

            SQLiteApi = api;
            VolatileService = new VolatileService();
            ReflectionService = new ReflectionService();
            StopwatchFactory = new StopwatchFactory();

        }

        public ISQLiteApi SQLiteApi { get; private set; }
        public IStopwatchFactory StopwatchFactory { get; private set; }
        public IReflectionService ReflectionService { get; private set; }
        public IVolatileService VolatileService { get; private set; }
    }

    class SQLiteApi : ISQLiteApi
    {
        [BestFitMapping(false, ThrowOnUnmappableChar = true)]
        static class NativeMethods
        {
            [DllImport("sqlite3", EntryPoint = "sqlite3_open", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern Result Open([MarshalAs(UnmanagedType.LPStr)] string filename, out IntPtr db);

            [DllImport("sqlite3", EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern Result Open([MarshalAs(UnmanagedType.LPStr)] string filename, out IntPtr db, int flags, IntPtr zvfs);

            [DllImport("sqlite3", EntryPoint = "sqlite3_open_v2", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Open(byte[] filename, out IntPtr db, int flags, IntPtr zvfs);

            [DllImport("sqlite3", EntryPoint = "sqlite3_open16", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern Result Open16([MarshalAs(UnmanagedType.LPWStr)] string filename, out IntPtr db);

            [DllImport("sqlite3", EntryPoint = "sqlite3_enable_load_extension", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result EnableLoadExtension(IntPtr db, int onoff);

            [DllImport("sqlite3", EntryPoint = "sqlite3_close", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Close(IntPtr db);

            [DllImport("sqlite3", EntryPoint = "sqlite3_initialize", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Initialize();

            [DllImport("sqlite3", EntryPoint = "sqlite3_shutdown", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Shutdown();

            [DllImport("sqlite3", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Config(ConfigOption option);

            [DllImport("sqlite3", EntryPoint = "sqlite3_win32_set_directory", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            public static extern int SetDirectory(uint directoryType, string directoryPath);

            [DllImport("sqlite3", EntryPoint = "sqlite3_busy_timeout", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result BusyTimeout(IntPtr db, int milliseconds);

            [DllImport("sqlite3", EntryPoint = "sqlite3_changes", CallingConvention = CallingConvention.Cdecl)]
            public static extern int Changes(IntPtr db);

            [DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern Result Prepare2(IntPtr db, [MarshalAs(UnmanagedType.LPStr)] string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);

            [DllImport("sqlite3", EntryPoint = "sqlite3_prepare_v2", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Prepare2(IntPtr db, byte[] queryBytes, int numBytes, out IntPtr stmt, IntPtr pzTail);

            public static IntPtr Prepare2(IntPtr db, string query)
            {
                IntPtr stmt;
                byte[] queryBytes = Encoding.UTF8.GetBytes(query);
                var r = Prepare2(db, queryBytes, queryBytes.Length, out stmt, IntPtr.Zero);
                if (r != Result.OK)
                {
                    throw SQLiteException.New(r, GetErrmsg(db));
                }
                return stmt;
            }

            [DllImport("sqlite3", EntryPoint = "sqlite3_step", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Step(IntPtr stmt);

            [DllImport("sqlite3", EntryPoint = "sqlite3_reset", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Reset(IntPtr stmt);

            [DllImport("sqlite3", EntryPoint = "sqlite3_finalize", CallingConvention = CallingConvention.Cdecl)]
            public static extern Result Finalize(IntPtr stmt);

            [DllImport("sqlite3", EntryPoint = "sqlite3_last_insert_rowid", CallingConvention = CallingConvention.Cdecl)]
            public static extern long LastInsertRowid(IntPtr db);

            [DllImport("sqlite3", EntryPoint = "sqlite3_errmsg16", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Errmsg(IntPtr db);

            public static string GetErrmsg(IntPtr db)
            {
                return Marshal.PtrToStringUni(Errmsg(db));
            }

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_parameter_index", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            public static extern int BindParameterIndex(IntPtr stmt, [MarshalAs(UnmanagedType.LPStr)] string name);

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_null", CallingConvention = CallingConvention.Cdecl)]
            public static extern int BindNull(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
            public static extern int BindInt(IntPtr stmt, int index, int val);

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
            public static extern int BindInt64(IntPtr stmt, int index, long val);

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_double", CallingConvention = CallingConvention.Cdecl)]
            public static extern int BindDouble(IntPtr stmt, int index, double val);

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_text16", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, BestFitMapping = true, ThrowOnUnmappableChar = true)]
            public static extern int BindText(IntPtr stmt, int index, [MarshalAs(UnmanagedType.LPWStr)] string val, int n, IntPtr free);

            [DllImport("sqlite3", EntryPoint = "sqlite3_bind_blob", CallingConvention = CallingConvention.Cdecl)]
            public static extern int BindBlob(IntPtr stmt, int index, byte[] val, int n, IntPtr free);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_count", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ColumnCount(IntPtr stmt);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_name", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ColumnName(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_name16", CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr ColumnName16Internal(IntPtr stmt, int index);
            public static string ColumnName16(IntPtr stmt, int index)
            {
                return Marshal.PtrToStringUni(ColumnName16Internal(stmt, index));
            }

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern ColType ColumnType(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_int", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ColumnInt(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_int64", CallingConvention = CallingConvention.Cdecl)]
            public static extern long ColumnInt64(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_double", CallingConvention = CallingConvention.Cdecl)]
            public static extern double ColumnDouble(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_text", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ColumnText(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_text16", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ColumnText16(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_blob", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ColumnBlob(IntPtr stmt, int index);

            [DllImport("sqlite3", EntryPoint = "sqlite3_column_bytes", CallingConvention = CallingConvention.Cdecl)]
            public static extern int ColumnBytes(IntPtr stmt, int index);

            public static string ColumnString(IntPtr stmt, int index)
            {
                return Marshal.PtrToStringUni(NativeMethods.ColumnText16(stmt, index));
            }

            public static byte[] ColumnByteArray(IntPtr stmt, int index)
            {
                int length = ColumnBytes(stmt, index);
                var result = new byte[length];
                if (length > 0)
                    Marshal.Copy(ColumnBlob(stmt, index), result, 0, length);
                return result;
            }

            [DllImport("sqlite3", EntryPoint = "sqlite3_extended_errcode", CallingConvention = CallingConvention.Cdecl)]
            public static extern ExtendedResult ExtendedErrCode(IntPtr db);

            [DllImport("sqlite3", EntryPoint = "sqlite3_libversion_number", CallingConvention = CallingConvention.Cdecl)]
            public static extern int LibVersionNumber();
        }

        public Result Open(byte[] filename, out IDbHandle db, int flags, IntPtr zVfs)
        {
            string dbFileName = Encoding.UTF8.GetString(filename, 0, filename.Length - 1);
            Sqlite3DatabaseHandle internalDbHandle;
            var ret = (Result)NativeMethods.Open(dbFileName, out internalDbHandle, flags, zVfs);
            db = new DbHandle(internalDbHandle);
            return ret;
        }

        public ExtendedResult ExtendedErrCode(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return NativeMethods.ExtendedErrCode(dbHandle.InternalDbHandle);
        }

        public int LibVersionNumber()
        {
            return NativeMethods.LibVersionNumber();
        }

        public Result EnableLoadExtension(IDbHandle db, int onoff)
        {
            var dbHandle = (DbHandle)db;
            return (Result)NativeMethods.EnableLoadExtension(dbHandle.InternalDbHandle, onoff);
        }

        public Result Close(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return (Result)NativeMethods.Close(dbHandle.InternalDbHandle);
        }

        public Result Initialize()
        {
            throw new NotSupportedException();
        }
        public Result Shutdown()
        {
            throw new NotSupportedException();
        }

        public Result Config(ConfigOption option)
        {
            throw new NotSupportedException();
        }

        public Result BusyTimeout(IDbHandle db, int milliseconds)
        {
            var dbHandle = (DbHandle)db;
            return (Result)NativeMethods.BusyTimeout(dbHandle.InternalDbHandle, milliseconds);
        }

        public int Changes(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return NativeMethods.Changes(dbHandle.InternalDbHandle);
        }


        public IDbStatement Prepare2(IDbHandle db, string query)
        {
            var dbHandle = (DbHandle)db;
            var stmt = default(Sqlite3Statement);

            int queryByteCount = Encoding.UTF8.GetByteCount(query);
            int r = (int)NativeMethods.Prepare2(dbHandle.InternalDbHandle, query, queryByteCount, out stmt, default(IntPtr));

            if (r != 0)
            {
                throw SQLiteException.New((Result)r, GetErrmsg(db));
            }
            return new DbStatement(stmt);
        }

        public Result Step(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            return (Result)NativeMethods.Step(dbStatement.InternalStmt);
        }

        public Result Reset(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            return (Result)NativeMethods.Reset(dbStatement.InternalStmt);
        }

        public Result Finalize(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            Sqlite3Statement internalStmt = dbStatement.InternalStmt;
            return (Result)NativeMethods.Finalize(internalStmt);
        }

        public long LastInsertRowid(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return NativeMethods.LastInsertRowid(dbHandle.InternalDbHandle);
        }

        public string Errmsg16(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return NativeMethods.GetErrmsg(dbHandle.InternalDbHandle);
        }

        public int BindParameterIndex(IDbStatement stmt, string name)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.BindParameterIndex(dbStatement.InternalStmt, name);
        }

        public int BindNull(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.BindNull(dbStatement.InternalStmt, index);
        }

        public int BindInt(IDbStatement stmt, int index, int val)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.BindInt(dbStatement.InternalStmt, index, val);
        }

        public int BindInt64(IDbStatement stmt, int index, long val)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.BindInt64(dbStatement.InternalStmt, index, val);
        }

        public int BindDouble(IDbStatement stmt, int index, double val)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.BindDouble(dbStatement.InternalStmt, index, val);
        }

        public int BindText16(IDbStatement stmt, int index, string val, int n, IntPtr free)
        {
            var dbStatement = (DbStatement)stmt;
            val = val.Replace("’", "'").Replace("–", "-");
            return NativeMethods.BindText(dbStatement.InternalStmt, index, val, n, free);
        }

        public int BindBlob(IDbStatement stmt, int index, byte[] val, int n, IntPtr free)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.BindBlob(dbStatement.InternalStmt, index, val, n, free);
        }

        public int ColumnCount(IDbStatement stmt)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.ColumnCount(dbStatement.InternalStmt);
        }

        public string ColumnName16(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return Marshal.PtrToStringAnsi(NativeMethods.ColumnName(dbStatement.InternalStmt, index));
        }

        public ColType ColumnType(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return (ColType)NativeMethods.ColumnType(dbStatement.InternalStmt, index);
        }

        public int ColumnInt(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.ColumnInt(dbStatement.InternalStmt, index);
        }

        public long ColumnInt64(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.ColumnInt64(dbStatement.InternalStmt, index);
        }

        public double ColumnDouble(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.ColumnDouble(dbStatement.InternalStmt, index);
        }

        public int ColumnBytes(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return NativeMethods.ColumnBytes(dbStatement.InternalStmt, index);
        }

        public byte[] ColumnByteArray(IDbStatement stmt, int index)
        {
            return ColumnBlob(stmt, index);
        }

        public string ColumnText16(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return Marshal.PtrToStringAnsi(NativeMethods.ColumnText(dbStatement.InternalStmt, index));
        }

        public byte[] ColumnBlob(IDbStatement stmt, int index)
        {
            //var dbStatement = (DbStatement)stmt;
            //return NativeMethods.ColumnBlob(dbStatement.InternalStmt, index);

            throw new NotImplementedException();
        }

        public Result Open(string filename, out IDbHandle db)
        {
            Sqlite3DatabaseHandle internalDbHandle;
            var ret = (Result)NativeMethods.Open(filename, out internalDbHandle);
            db = new DbHandle(internalDbHandle);
            return ret;
        }

        public string GetErrmsg(IDbHandle db)
        {
            var dbHandle = (DbHandle)db;
            return NativeMethods.GetErrmsg(dbHandle.InternalDbHandle);
        }

        public string ColumnString(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return Marshal.PtrToStringAnsi(NativeMethods.ColumnText(dbStatement.InternalStmt, index));
        }

        public string ColumnName(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return Marshal.PtrToStringAnsi(NativeMethods.ColumnName(dbStatement.InternalStmt, index));
        }

        public string ColumnText(IDbStatement stmt, int index)
        {
            var dbStatement = (DbStatement)stmt;
            return Marshal.PtrToStringAnsi(NativeMethods.ColumnText(dbStatement.InternalStmt, index));
        }

        private struct DbHandle : IDbHandle
        {
            public DbHandle(Sqlite3DatabaseHandle internalDbHandle)
                : this()
            {
                InternalDbHandle = internalDbHandle;
            }

            public Sqlite3DatabaseHandle InternalDbHandle { get; set; }

            public bool Equals(IDbHandle other)
            {
                return other is DbHandle && InternalDbHandle == ((DbHandle)other).InternalDbHandle;
            }
        }

        private struct DbStatement : IDbStatement
        {
            public DbStatement(Sqlite3Statement internalStmt)
                : this()
            {
                InternalStmt = internalStmt;
            }

            internal Sqlite3Statement InternalStmt { get; set; }

            public bool Equals(IDbStatement other)
            {
                return (other is DbStatement) && ((DbStatement)other).InternalStmt == InternalStmt;
            }
        }
    }

    class VolatileService : IVolatileService
    {
        public void Write(ref int transactionDepth, int depth)
        {
            Volatile.Write(ref transactionDepth, depth);
        }
    }

    class ReflectionService : IReflectionService
    {
        public IEnumerable<PropertyInfo> GetPublicInstanceProperties(Type mappedType)
        {
            if (mappedType == null)
            {
                throw new ArgumentNullException("mappedType");
            }
            return from p in mappedType.GetRuntimeProperties()
                   where
                       ((p.GetMethod != null && p.GetMethod.IsPublic) || (p.SetMethod != null && p.SetMethod.IsPublic) ||
                        (p.GetMethod != null && p.GetMethod.IsStatic) || (p.SetMethod != null && p.SetMethod.IsStatic))
                   select p;
        }

        public object GetMemberValue(object obj, Expression expr, MemberInfo member)
        {
            if (member is PropertyInfo)
            {
                var m = (PropertyInfo)member;
                return m.GetValue(obj, null);
            }
            else if (member is FieldInfo)
            {
                var m = (FieldInfo)member;
                return m.GetValue(obj);
            }
            throw new NotSupportedException("MemberExpr: " + member.DeclaringType);
        }
    }

    class StopwatchFactory : IStopwatchFactory
    {
        public IStopwatch Create()
        {
            return new StopwatchWP8();
        }

        private class StopwatchWP8 : IStopwatch
        {
            private readonly Stopwatch _stopWatch;

            public StopwatchWP8()
            {
                _stopWatch = new Stopwatch();
            }

            public void Stop()
            {
                _stopWatch.Stop();
            }

            public void Reset()
            {
                _stopWatch.Reset();
            }

            public void Start()
            {
                _stopWatch.Start();
            }

            public long ElapsedMilliseconds
            {
                get { return _stopWatch.ElapsedMilliseconds; }
            }
        }
    }
}
