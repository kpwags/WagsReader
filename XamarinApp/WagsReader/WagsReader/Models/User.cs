using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WagsReader.Interfaces;
using Xamarin.Forms;
using static System.Diagnostics.Debug;

namespace WagsReader.Models
{
    [Table("User")]
    public class User : BaseModel
    {
        [PrimaryKey]
        [JsonProperty("userId")]
        public string UserId { get; set; }

        public AccountType Type { get; set; }

        public string AccountName { get; set; }

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("userProfileId")]
        public string UserProfileId { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("isBloggerUser")]
        public bool IsBloggerUser { get; set; }

        [JsonProperty("signupTimeSec")]
        public long SignupTimeSec { get; set; }

        [JsonProperty("isMultiLoginEnabled")]
        public bool IsMultiLoginEnabled { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public UserToken Token { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Folder> Folders { get; set; }

        public static async Task<List<User>> GetUsersAsync(bool recursive = false)
        {
            return await Task.Run(() =>
            {
                if (_db == null)
                {
                    _db = DependencyService.Get<IDatabaseConnection>().DBConnection();
                    _db.Execute("PRAGMA foreign_keys = ON");
                }

                if (recursive)
                {
                    return _db.GetAllWithChildren<User>(recursive: true).OrderBy(u => u.Username).ToList();
                }

                return _db.Table<User>().OrderBy(u => u.Username).ToList();
            });
        }

        public static async Task<User> GetUserAsync(string userId, bool recursive = true)
        {
            return await Task.Run(() =>
            {
                User user = _db.Table<User>().Where(u => u.UserId == userId).FirstOrDefault();

                if (user != null)
                {
                    if (recursive)
                    {
                        user = _db.FindWithChildren<User>(pk: userId, recursive: true);
                    }
                }

                return user;
            });
        }

        public static async Task<User> Save(User item)
        {
            try
            {
                if (_db.Find<User>(u => u.UserId == item.UserId) == null)
                {
                    _db.RunInTransaction(() =>
                    {
                        _db.InsertWithChildren(item, recursive: true);
                    });

                    return await GetUserAsync(item.UserId, true);
                }
                else
                {
                    // user already exists, update
                    var existingUser = await GetUserAsync(item.UserId, true);
                    _db.RunInTransaction(() =>
                    {
                        _db.Update(item);
                    });

                    return await GetUserAsync(item.UserId, true);
                }
            }
            catch (SQLiteException ex)
            {
                WriteLine($"SQLite Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                WriteLine($"Exception: {ex.Message}");
                return null;
            }
        } 
    }
}
