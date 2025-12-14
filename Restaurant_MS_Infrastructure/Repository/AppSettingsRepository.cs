

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

        public bool SaveAppSetting(string key, string value)
        {
            string encKey = CryptopEngine.Encrypt(key, GlobalSharing.Salt);
            string encVal = CryptopEngine.Encrypt(value, GlobalSharing.Salt);

            Appsettings? exSetting = cxt.AppSettings.FirstOrDefault(s => s.Key == encKey);
            if (exSetting != null)
            {

                if (exSetting != null)
                {
                    exSetting.Value = encVal;
                    cxt.AppSettings.Update(exSetting);
                    cxt.SaveChanges();
                }
                return true;
            }

            Appsettings setting = new Appsettings();
            setting.Key = encKey;
            setting.Value = encVal;

            cxt.AppSettings.Add(setting);
            cxt.SaveChanges();
            return true;
        }

        public bool SaveAppSetting(Appsettings settings)
        {
            string encKey = CryptopEngine.Encrypt(settings.Key, GlobalSharing.Salt);
            string encVal = CryptopEngine.Encrypt(settings.Value, GlobalSharing.Salt);

            Appsettings? exSetting = cxt.AppSettings.FirstOrDefault(s => s.Key == encKey);
            if (exSetting != null)
            {
                if (exSetting != null)
                {
                    exSetting.Value = encVal;
                    cxt.AppSettings.Update(exSetting);
                    cxt.SaveChanges();
                }
                return true;
            }

            Appsettings setting = new Appsettings();
            setting.Key = encKey;
            setting.Value = encVal;

            cxt.AppSettings.Add(setting);
            cxt.SaveChanges();
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