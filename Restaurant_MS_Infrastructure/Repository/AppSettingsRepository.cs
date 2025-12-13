

using System.Runtime.CompilerServices;
using Utilities;

namespace Restaurant_MS_Infrastructure.Repository
{
    public class AppSettingsRepository : Repository<Appsettings>
    {
        AppDbContext cxt;
        public AppSettingsRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public bool SaveNewAppSetting(string key, string value)
        {
            string encKey = CryptopEngine.Encrypt(key, GlobalSharing.Salt);
            string encVal = CryptopEngine.Encrypt(value, GlobalSharing.Salt);

            Appsettings setting = new Appsettings();
            setting.Key = encKey;
            setting.Value = encVal;

            cxt.AppSettings.Add(setting);
            return true;
        }

        public bool UpdateAppSetting(int Id, string key, string value)
        {
            //string encKey = CryptopEngine.Encrypt(key, GlobalSharing.Salt);
            string encVal = CryptopEngine.Encrypt(value, GlobalSharing.Salt);

            Appsettings? setting = cxt.AppSettings.FirstOrDefault(s => s.Id == Id);
            if (setting != null)
            {
                setting.Value = encVal;
                cxt.AppSettings.Update(setting);
                cxt.SaveChanges();
            }


            return true;
        }

        public string GetAppSetting(string key)
        {
            string encKey = CryptopEngine.Encrypt(key, GlobalSharing.Salt);
            string? val = cxt.AppSettings
                .Where(s => s.Key == encKey)
                .Select(s => s.Value)
                .FirstOrDefault();

            var res = "";
            if (!string.IsNullOrEmpty(val))
            {
                res = CryptopEngine.Decrypt(val, GlobalSharing.Salt);
            }
            return res;
        }
    }
}