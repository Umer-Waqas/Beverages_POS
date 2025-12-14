using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Restaurant_MS_Core.Entities
{
    public class Appsettings
    {
        public int Id { get; set; }
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
    }

    public static class AppSettingKeys
    {
        public static string SystemIdentifier = "SystemIdentifier";
        public static string SystemActivationStatus = "SystemActivationStatus";
        public static string RememberUserCredentials = "RememberUserCredentials";
        public static string RememberUserName = "RememberUserName";
        public static string RememberUserPassword = "RememberUserPassword";
    }
}