using CheckMate.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CheckMate.Data
{
    public class DatabaseContext
    {
        private const string DbName = "MyDatabase.db3";

        // Specifies for different platforms the File System path
        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection _Database => (_connection ??= new SQLiteAsyncConnection(DbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));

        private async Task CreatTableIfNonexistent<TTable>() where TTable : class, new()
        {
            await _Database.CreateTableAsync<TTable>();
        }
        private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable: class, new()
        {
            await CreatTableIfNonexistent<TTable>();
            return _Database.Table<TTable>();
        }
        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }
        public async Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.Where(predicate).ToListAsync();
        }
        public async Task<TTable> GetTaskByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreatTableIfNonexistent<TTable>();
            return await _Database.GetAsync<TTable>(primaryKey);
        }
        public async Task<bool> AddTaskAsync<TTable>(TTable task) where TTable : class, new()
        {
            await CreatTableIfNonexistent<TTable>();
            return await _Database.InsertAsync(task) > 0;
        }
        public async Task<bool> UpdateTaskAsync<TTable>(TTable task) where TTable : class, new()
        {
            await CreatTableIfNonexistent<TTable>();
            return await _Database.UpdateAsync(task) > 0;
        }
        public async Task<bool> DeleteTaskAsync<TTable>(TTable task) where TTable : class, new()
        {
            await CreatTableIfNonexistent<TTable>();
            return await _Database.DeleteAsync(task) > 0;
        }
        public async Task<bool> DeleteTaskByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreatTableIfNonexistent<TTable>();
            return await _Database.DeleteAsync<TTable>(primaryKey) > 0;
        }
    }
}
