using CNSSInv.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using System.Linq;

namespace CNSSInv.Services;

 public class NatureDatabaseController
 {
    static readonly object locker = new object();
    static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
    {
        //return new SQLiteAsyncConnection(Constants.DatabasePath().Result, Constants.Flags);
        return new SQLiteAsyncConnection(Constants.DatabasePath(), Constants.Flags);

    });
    static SQLiteAsyncConnection database => lazyInitializer.Value;
    static bool initialized = false;

    async Task InitializeAsync()
    {
        if (!initialized)
        {
            if (!database.TableMappings.Any(m => m.MappedType.Name == typeof(Nature).Name))
            {
                await database.CreateTablesAsync(CreateFlags.None, typeof(Nature)).ConfigureAwait(false);
            }
            initialized = true;
        }
    }

    public NatureDatabaseController()
    {
        InitializeAsync().SafeFireAndForget(false);
    }

    public Task<Nature> GetUser()
    {
        lock (locker)
        {
            if (database.Table<Nature>().CountAsync().Result == 0)
                return null;
            else
                return database.Table<Nature>().FirstAsync();
        }
    }
    public Task<List<Nature>> GetAllNatures()
    {
        return database.Table<Nature>().ToListAsync();
    }


    public Task<Nature> GetNatureById(string id)
    {

        lock (locker)
        {
            if (database.Table<Nature>().Where(i => i.Code_Nature == id).CountAsync().Result == 0)
                return null;
            else
                return database.Table<Nature>()
                        .Where(i => i.Code_Nature == id)
                        .FirstAsync();
        }
    }


    public Task<Nature> GetNatureByIdLibNature(string id)
    {

        lock (locker)
        {
            if (database.Table<Nature>().Where(i => i.Lib_Nature == id).CountAsync().Result == 0)
                return null;
            else
                return database.Table<Nature>()
                        .Where(i => i.Lib_Nature == id)
                        .FirstAsync();
        }
    }
    public int GetCountNatureByID(string id)
    {

        lock (locker)
        {
            return database.Table<Nature>().Where(i => i.Code_Nature == id).CountAsync().Result;

        }
    }

    public int GetCountNatureByIDLibNat(string id)
    {

        lock (locker)
        {
            return database.Table<Nature>().Where(i => i.Lib_Nature == id).CountAsync().Result;

        }
    }

    //public int GetCountNatureByLibNat(string libnat)
    //{

    //    lock (locker)
    //    {
    //        return database.Table<Nature>().Where(i => i.Lib_Nature == libnat).CountAsync().Result;

    //    }
    //}

    public Task<List<Nature>> GetNatureByLibNat(string libnat)
    {

        lock (locker)
        {
            return database.Table<Nature>().Where(i => i.Lib_Nature.ToLower().Contains(libnat.ToLower())).ToListAsync();


        }
    }

    public int GetCountNatureByLibNat(string libnat)
    {

        lock (locker)
        {
            return database.Table<Nature>().Where(i => i.Lib_Nature.ToLower().Contains(libnat.ToLower())).CountAsync().Result;


        }
    }
    public int GetCountNature()
    {

        lock (locker)
        {
            return database.Table<Nature>().CountAsync().Result;

        }
    }


    public Task<int> SaveNature(Nature Nature)
    {
        lock (locker)
        {
            if (GetNatureById(Nature.Code_Nature) != null)
            {
                return database.UpdateAsync(Nature);
            }
            else
                return database.InsertAsync(Nature);
        }
    }
    public Task<int> UpdateNature(Nature Nature)
    {
        lock (locker)
        {

            return database.UpdateAsync(Nature);

        }
    }
    public Task<int> DeleteNature(string NameUser)
    {
        lock (locker)
        {
            return database.DeleteAsync<Nature>(NameUser);
        }
    }

 }
