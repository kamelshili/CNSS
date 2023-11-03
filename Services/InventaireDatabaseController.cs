using CNSSInv.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace CNSSInv.Services;

    class InventaireDatabaseController
    {
        static object locker = new object();
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
                if (!database.TableMappings.Any(m => m.MappedType.Name == typeof(Models.Inventaire).Name))
                {
                    await database.CreateTablesAsync(CreateFlags.None, typeof(Models.Inventaire)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public InventaireDatabaseController()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        public Task<Models.Inventaire> GetInventaire()
        {
            lock (locker)
            {
                if (database.Table<Models.Inventaire>().CountAsync().Result == 0)
                    return null;
                else
                    return database.Table<Models.Inventaire>().FirstAsync();
            }
        }
        public Task<List<Models.Inventaire>> GetAllInventaires()
        {
            return database.Table<Models.Inventaire>().ToListAsync();
        }
     

        public Task<List<Models.Inventaire>> GetNumImmoInventaires(string codeimmo)
        {
            string AllCodeImmoInventaire = ("select distinct NumImmo from Inventaire");
            return database.QueryAsync<Models.Inventaire>(AllCodeImmoInventaire, codeimmo);
        }
        public Task<Models.Inventaire> GetInventaireById(string id)
        {

            lock (locker)
            {
                if (database.Table<Models.Inventaire>().Where(i => i.NumImmo == id).CountAsync().Result == 0)
                    return null;
                else
                    return database.Table<Models.Inventaire>()
                            .Where(i => i.NumImmo == id)
                            .FirstAsync();
            }
        }
        public Task<Models.Inventaire> GetInventaireByNSerie(string id)
        {

            lock (locker)
            {
                if (database.Table<Models.Inventaire>().Where(i => i.Num_Serie == id).CountAsync().Result == 0)
                    return null;
                else
                    return database.Table<Models.Inventaire>()
                            .Where(i => i.Num_Serie == id)
                            .FirstAsync();
            }
        }

        public Task<List<Models.Inventaire>> GetInventaireByEmplCodeUB(string empl, string code, string codedirection)
        {
            string AllInventaireByUniteBudgetaire = ("select distinct * from Inventaire Where EMPL_TH = ? and Code_UB_TH = ? and Code_Direction_TH = ? and IsRead = ?");
            return database.QueryAsync<Models.Inventaire>(AllInventaireByUniteBudgetaire, empl, code, codedirection, 0);
        }

        public Task<List<Models.Inventaire>> GetInventaireByEmplZone(string numimmo)
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().Where(i => i.NumImmo.ToLower().Contains(numimmo.ToLower())).ToListAsync();


            }
        }

        public int GetCountInventaireByNumImmo(string numimmo)
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().Where(i => i.NumImmo.ToLower().Contains(numimmo.ToLower())).CountAsync().Result;
            }
        }
        public int GetCountInventaire(string id)
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().Where(i => i.NumImmo == id).CountAsync().Result;

            }
        }

        public int GetCountInventaireByNserie(string id)
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().Where(i => i.Num_Serie == id).CountAsync().Result;

            }
        }

        public int GetCountInventaireByNserieIsRead(string id)
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().Where(i => i.Num_Serie == id && i.IsRead == true).CountAsync().Result;

            }
        }
        public int GetCountAllInventaire()
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().CountAsync().Result;

            }
        }
        public int GetCountInventaireByRead(string id)
        {

            lock (locker)
            {
                return database.Table<Models.Inventaire>().Where(i => i.NumImmo == id && i.IsRead == true).CountAsync().Result;

            }
        }

        public async void GetInventaireByRead(string id)
        {
            Models.Inventaire lst = null;
            lock (locker)
            {
                 lst = database.Table<Models.Inventaire>().Where(i => i.NumImmo == id).FirstAsync().Result;


            }
            await Application.Current.MainPage.DisplayAlert("Update?", lst.Code_Nature_TH+" "+ lst.IsRead.ToString(),"ok");

        }

        public Task<int> SaveInventaire(Models.Inventaire Inventaire)
        {
            lock (locker)
            {

                if(Inventaire.NumImmo.Length > 3)
                {

                }
                if (GetInventaireById(Inventaire.NumImmo) != null)
                {
                    return database.UpdateAsync(Inventaire);
                }
                else
                    return database.InsertAsync(Inventaire);
            }
        }
        public Task<int> UpdateInventaire(Models.Inventaire Inventaire)
        {
            lock (locker)
            {

                return database.UpdateAsync(Inventaire);

            }
        }
        public Task<int> DeleteInventaire(string code)
        {
            lock (locker)
            {
                return database.DeleteAsync<Models.Inventaire>(code);
            }
        }
    }

