using CNSSInv.Models;
using SQLite;

namespace CNSSInv.Services;
  public class UserDatabaseController
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
                if (!database.TableMappings.Any(m => m.MappedType.Name == typeof(UserLogin).Name))
                {
                    await database.CreateTablesAsync(CreateFlags.None, typeof(UserLogin)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public UserDatabaseController()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        public Task<UserLogin> GetUser()
        {
            lock (locker)
            {
                if (database.Table<UserLogin>().CountAsync().Result == 0)
                    return null;
                else
                    return database.Table<UserLogin>().FirstAsync();
            }
        }

        public int GetCountUserByIDSearch(string keyword)
        {
            lock (locker)
            {
                return database.Table<UserLogin>().Where(i => i.Name_User.ToLower().Contains(keyword.ToLower())).CountAsync().Result;

            }
        }

        public int GetCountUserByID(string id)
        {
            lock (locker)
            {
                return database.Table<UserLogin>().Where(i => i.Name_User == id).CountAsync().Result;

            }
        }

        public Task<List<UserLogin>> GetAllUsers()
        {
            return database.Table<UserLogin>().OrderBy(i=>i.Name_User).ToListAsync();
        }

        public Task<UserLogin> GetUserById(string id)
        {

            lock (locker)
            {
                if (database.Table<UserLogin>().Where(i => i.Name_User == id).CountAsync().Result == 0)
                    return null;
                else
                    return database.Table<UserLogin>()
                            .Where(i => i.Name_User == id)
                            .FirstAsync();
            }
        }

        public Task<UserLogin> Login(string id, string password)
        {

            lock (locker)
            {
                if (database.Table<UserLogin>().Where(i => i.Name_User == id && i.Password == password).CountAsync().Result == 0)
                    return null;
                else
                    return database.Table<UserLogin>()
                            .Where(i => i.Name_User == id)
                            .FirstAsync();
            }
        }

        public Task<int> UpdateUserQuery(UserLogin user, string oldusername)
        {
            lock (locker)
            {
                string QueryUpdate = "update UserLogin set Name_User = ? , Password = ? where Name_User = ?";
                return database.ExecuteAsync(QueryUpdate, user.Name_User, user.Password, oldusername);
            }
        }

        public Task<int> SaveUser(UserLogin user, string olduser)
        {
            lock (locker)
            {
                if (GetCountUserByID(olduser) > 0)
                {
                    return UpdateUserQuery(user, olduser);
                }
                else
                    return database.InsertAsync(user);
            }
        }

        public Task<int> UpdateUser(UserLogin user)
        {
            lock (locker)
            {
                return database.UpdateAsync(user);
            }
        }

        public Task<int> DeleteUser(string NameUser)
        {
            lock (locker)
            {
                return database.DeleteAsync<UserLogin>(NameUser);
            }
        }
  }
