using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;

namespace  .Client.ValidatorBase.LocalStorage
{
    /// <summary>
    /// the local storage features is to support offline mode
    /// </summary>
    public class SqliteHelper
    {
        private const string DbPath = "LocalStorage.db";
        private const int OperationSuccess = 1;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SqliteHelper));

        public static bool TryDropTable<T>()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                var success = db.DropTable<T>();
                if (success != OperationSuccess)
                {
                    Log.Error("[Sqlite] " + "Drop table failed , Type name :'" + typeof(T).Name + "'");
                }

                return success == OperationSuccess;
            }
        }

        public static bool TryCreateTable<T>()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                var table = db.GetTableInfo(typeof(T).Name);
                if (table != null && table.Count > 0)
                {
                    return false;
                }
                db.CreateTable<T>();
                return true;
            }
        }

        public static T Add<T>(T record)
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                var success = db.Insert(record);
                if (success != OperationSuccess)
                {
                    Log.Error("[Sqlite] Add record failed. type :" + typeof(T).Name);
                }
            }
            return record;
        }

        public static T DeleteById<T>(Guid id) where T : LocalStorageBaseModel, new()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                var record = db.Table<T>().FirstOrDefault(x => x.Id == id);
                if (record != null)
                {
                    var success = db.Delete(record);
                    if (success != OperationSuccess)
                    {
                        Log.Error("[Sqlite] Delete record failed, id :" + id);
                        return null;
                    }
                    return record;
                }
                else
                {
                    Log.Error("[Sqlite] trying to delete record doesnt exists, id :" + id);
                    return null;
                }
            }
        }

        public static T UpdateById<T>(Guid id, T updating) where T : LocalStorageBaseModel, new()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                var singleRecord = db.Table<T>().FirstOrDefault(x => x.Id == id);
                if (singleRecord != null)
                {
                    singleRecord = updating;
                    singleRecord.Id = id; //restore the id

                    var success = db.Update(singleRecord);
                    if (success != OperationSuccess)
                    {
                        Log.Error("[Sqlite] Updating failed, id :" + id);
                        return null;
                    }
                    return updating;
                }
                else
                {
                    Log.Error("[Sqlite] trying to update record doesnt exists, id :" + id);
                    return null;
                }
            }
        }

        public static T UpdateBy<T>(Expression<Func<T, bool>> where, T updating) where T : LocalStorageBaseModel, new()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                var records = db.Table<T>().Where(where).ToList();
                if (records.Count > 0)
                {
                    for (int i = 0; i < records.Count; i++)
                    {
                        var id = records[i].Id;
                        records[i] = updating;
                        records[i].Id = id; // restore id
                    }
                    var success = db.UpdateAll(records);
                    if (success != OperationSuccess)
                    {
                        Log.Error("[Sqlite] updating failed, type :" + typeof(T));
                        return null;
                    }
                    return updating;
                }
                return null;
            }
        }

        public static IEnumerable<T> All<T>() where T : new()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                return db.Table<T>().ToList();
            }
        }

        public static IEnumerable<T> GetBy<T>(Expression<Func<T, bool>> prediction) where T : new()
        {
            using (var db = new SQLite.SQLiteConnection(DbPath))
            {
                return db.Table<T>().Where(prediction).ToList();
            }
        }
    }
}
