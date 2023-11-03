using CNSSInv.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using System.Linq;

namespace CNSSInv.Services;

public class UniteBudgetaireDatabaseController
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
            if (!database.TableMappings.Any(m => m.MappedType.Name == typeof(UniteBudgetaire).Name))
            {
                await database.CreateTablesAsync(CreateFlags.None, typeof(UniteBudgetaire)).ConfigureAwait(false);
            }
            initialized = true;
        }
    }

    public UniteBudgetaireDatabaseController()
    {
        InitializeAsync().SafeFireAndForget(false);
    }

    public Task<UniteBudgetaire> GetUser()
    {
        lock (locker)
        {
            if (database.Table<UniteBudgetaire>().CountAsync().Result == 0)
                return null;
            else
                return database.Table<UniteBudgetaire>().FirstAsync();
        }
    }
    public Task<List<UniteBudgetaire>> GetAllUniteBudgetaires()
    {
        return database.Table<UniteBudgetaire>().ToListAsync();
    }
   
    public Task<List<UniteBudgetaire>> GetZoneUniteBudgetaires()
    {
        string AllZoneUniteBudgetaire = "select distinct ZONE from UniteBudgetaire";
        return database.QueryAsync<UniteBudgetaire>(AllZoneUniteBudgetaire);
    }

    public Task<List<UniteBudgetaire>> GetCodeBatimentUniteBudgetaires()
    {
        string AllZoneUniteBudgetaire = "select distinct EMPL,SITE from UniteBudgetaire ORDER BY EMPL ASC";
        return database.QueryAsync<UniteBudgetaire>(AllZoneUniteBudgetaire);
    }
    public Task<List<UniteBudgetaire>> GetCodeLocaleUniteBudgetaires()
    {
        string AllZoneUniteBudgetaire = "select distinct Code_Local from UniteBudgetaire where Code_Local <> 'VIDE' ORDER BY Code_Local ASC";
        return database.QueryAsync<UniteBudgetaire>(AllZoneUniteBudgetaire);
    }
    public Task<List<UniteBudgetaire>> GetSiteUniteBudgetaires(string zone)
    {
        string AllSiteUniteBudgetaire = ("select distinct SITE from UniteBudgetaire Where ZONE = ?");
        return database.QueryAsync<UniteBudgetaire>(AllSiteUniteBudgetaire, zone);
    }
    public Task<List<UniteBudgetaire>> GetEmpUniteBudgetaires(string site)
    {
        string AllEmplUniteBudgetaire = ("select distinct EMPL from UniteBudgetaire Where SITE = ?");
        return database.QueryAsync<UniteBudgetaire>(AllEmplUniteBudgetaire, site);
    }
    public Task<List<UniteBudgetaire>> GetCodeUniteBudgetaires(string emp, string codedirection, string site, string zone)
    {
        string AllCodeUniteBudgetaire = ("select distinct Code_UB from UniteBudgetaire Where EMPL = ? and Code_Direction = ? and SITE = ? and ZONE = ? ORDER BY Code_UB ASC");
        return database.QueryAsync<UniteBudgetaire>(AllCodeUniteBudgetaire, emp, codedirection, site, zone);
    }

    public Task<List<UniteBudgetaire>> GetCodeDirectionUniteBudgetaires(string emp, string lib_bat)
    {
        string AllCodeUniteBudgetaire = ("select distinct Code_Direction,ZONE from UniteBudgetaire Where EMPL = ? and SITE = ? ORDER BY Code_Direction ASC");
        return database.QueryAsync<UniteBudgetaire>(AllCodeUniteBudgetaire, emp, lib_bat);
    }

    public Task<List<UniteBudgetaire>> GetLibUniteBudgetaires(string code, string empl, string codedirection, string zone, string site)
    {
        string AllLibUniteBudgetaire = ("select distinct Lib_UB from UniteBudgetaire Where Code_UB = ? and EMPL = ? and Code_Direction = ? and ZONE = ? and SITE = ?");
        return database.QueryAsync<UniteBudgetaire>(AllLibUniteBudgetaire, code, empl, codedirection, zone, site);
    }
    public Task<List<UniteBudgetaire>> GetLibBATIMENTUniteBudgetairesByEmpl(string empl)
    {
        string AllLibUniteBudgetaire = ("select distinct SITE from UniteBudgetaire Where EMPL = ?");
        return database.QueryAsync<UniteBudgetaire>(AllLibUniteBudgetaire, empl);
    }

    public Task<List<UniteBudgetaire>> GetUniteBudgetairesByAll(string id, string empl, string codedirection, string site, string zone, string lib_ub)
    {
        string UniteBudgetaireByAll = ("SELECT * FROM UniteBudgetaire WHERE EMPL = ?  and SITE = ?  and Code_Direction = ? and ZONE = ? and Code_UB = ?");
        return database.QueryAsync<UniteBudgetaire>(UniteBudgetaireByAll, empl, site, codedirection, zone, id);
    }

    public Task<List<UniteBudgetaire>> GetLibDirectionUniteBudgetairesByEmplDirection(string empl, string codedirection)
    {
        string AllLibUniteBudgetaire = ("select distinct ZONE from UniteBudgetaire Where EMPL = ? and Code_Direction = ? ");
        return database.QueryAsync<UniteBudgetaire>(AllLibUniteBudgetaire, empl, codedirection);
    }

    public Task<List<UniteBudgetaire>> GetLibDirectionUniteBudgetairesByEmpl(string codedirection, string empl, string SITE)
    {
        string AllLibUniteBudgetaire = ("select distinct ZONE from UniteBudgetaire Where Code_Direction = ? and EMPL = ? and SITE = ?");
        return database.QueryAsync<UniteBudgetaire>(AllLibUniteBudgetaire, codedirection, empl, SITE);
    }

    public int GetCountUnitBudgetaireByZone(string zone)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.ZONE.ToLower().Contains(zone.ToLower())).CountAsync().Result;
        }
    }

    public Task<List<UniteBudgetaire>> GetUniteBudgetaireByZone(string zone)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.ZONE.ToLower().Contains(zone.ToLower())).ToListAsync();
        }
    }

    public int GetCountSitetaireByZone(string site)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.SITE.ToLower().Contains(site.ToLower())).CountAsync().Result;
        }
    }
    public Task<List<UniteBudgetaire>> GetUniteBudgetaireBySite(string site)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.SITE.ToLower().Contains(site.ToLower())).ToListAsync();
        }
    }

    public int GetCountUnitBudgetaireBySite(string site)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.SITE.ToLower().Contains(site.ToLower())).CountAsync().Result;


        }
    }
    public Task<List<UniteBudgetaire>> GetUniteBudgetaireByEmpl(string empl)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.EMPL.ToLower().Contains(empl.ToLower())).ToListAsync();


        }
    }
    public int GetCountUnitBudgetaireByEmpl(string empl)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.EMPL.ToLower().Contains(empl.ToLower())).CountAsync().Result;


        }
    }

    public int GetCountUnitBudgetaireByCodeDirection(string codedirection)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.Code_Direction.ToLower().Contains(codedirection.ToLower())).CountAsync().Result;


        }
    }

    public int GetCountUnitBudgetaireByCodeLocale(string codelocale)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.Code_Local.ToLower().Contains(codelocale.ToLower())).CountAsync().Result;


        }
    }

    public Task<List<UniteBudgetaire>> GetUniteBudgetaireByCodeUB(string codeub)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.Code_UB.ToLower().Contains(codeub.ToLower())).ToListAsync();


        }
    }

    public int GetCountUnitBudgetaireByCodeUB(string codeub)
    {

        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.Code_UB.ToLower().Contains(codeub.ToLower())).CountAsync().Result;


        }
    }

    public Task<UniteBudgetaire> GetUniteBudgetaireById(string id)
    {

        lock (locker)
        {
            if (database.Table<UniteBudgetaire>().Where(i => i.Code_Local == id).CountAsync().Result == 0)
                return null;
            else
                return database.Table<UniteBudgetaire>()
                        .Where(i => i.Code_Local == id)
                        .FirstAsync();
        }
    }
    public Task<UniteBudgetaire> GetUniteBudgetaireByAll(string id, string empl, string codedirection, string site, string zone, string lib_ub)
    {

        lock (locker)
        {
            if (database.Table<UniteBudgetaire>().Where(i => i.Code_UB == id && i.EMPL == empl && i.Code_Direction == codedirection && i.SITE == site && i.Lib_UB == lib_ub).CountAsync().Result == 0)
                return null;
            else
                return database.Table<UniteBudgetaire>()
                        .Where(i => i.Code_UB == id && i.EMPL == empl && i.Code_Direction == codedirection && i.SITE == site && i.Lib_UB == lib_ub)
                        .FirstAsync();
        }
    }

    public Task<UniteBudgetaire> GetUniteBudgetaireByCodeLocale(string id)
    {

        lock (locker)
        {
            if (database.Table<UniteBudgetaire>().Where(i => i.Code_Local == id).CountAsync().Result == 0)
                return null;
            else
                return database.Table<UniteBudgetaire>()
                        .Where(i => i.Code_Local == id)
                        .FirstAsync();
        }
    }
    public int GetCountUniteBudgetaire()
    {
        lock (locker)
        {
            return database.Table<UniteBudgetaire>().CountAsync().Result;

        }
    }

    public int GetCountUniteBudgetaireByID(string id)
    {
        lock (locker)
        {
            return database.Table<UniteBudgetaire>().Where(i => i.Code_Local == id).CountAsync().Result;

        }
    }

    public Task<int> SaveUniteBudgetaire(UniteBudgetaire uniteBudgetaire)
    {
        lock (locker)
        {
            if (GetUniteBudgetaireById(uniteBudgetaire.Code_Local) != null)
            {
                return database.UpdateAsync(uniteBudgetaire);
            }
            else
                return database.InsertAsync(uniteBudgetaire);
        }
    }

    public Task<int> UpdateUniteBudgetaire(UniteBudgetaire uniteBudgetaire)
    {
        lock (locker)
        {
            return database.UpdateAsync(uniteBudgetaire);
        }
    }

    public Task<int> DeleteUniteBudgetaire(string NameUser)
    {
        lock (locker)
        {
            return database.DeleteAsync<UniteBudgetaire>(NameUser);
        }
    }
}
