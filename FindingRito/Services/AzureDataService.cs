using FindingRito.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindingRito.Services
{
    public class AzureDataService
    {
        public MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable<Member> memberTable;

        public async Task Initialize()
        {
            MobileService = new MobileServiceClient("https://findingrito.azurewebsites.net");
            const string path = "Member.db";

            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Member>();

            await MobileService.SyncContext.InitializeAsync(store);
            memberTable = MobileService.GetSyncTable<Member>();
        }

        public async Task<List<Member>> GetMembers()
        {
            await SyncMember();
            return await memberTable.ToListAsync();
        }

        public async Task<List<Member>> GetMemberByUserNameAndPassword(string userName, string password)
        {
            await SyncMember();
            return await memberTable.Where(m => m.UserName == userName && m.Password == password).ToListAsync();
        }

        public async Task AddMember(string userName, string password, string email)
        {
            var member = new Member
            {
                UserName = userName,
                Password = password,
                Email = email,
                CreatedUtcTime = DateTime.UtcNow.ToString()
            };

            await memberTable.InsertAsync(member);
            await SyncMember();
        }

        public async Task SyncMember()
        {
            await memberTable.PullAsync("Member", memberTable.CreateQuery());
            await MobileService.SyncContext.PushAsync();
        }
    }
}
