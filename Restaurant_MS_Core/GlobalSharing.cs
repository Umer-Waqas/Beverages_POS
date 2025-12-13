
namespace Restaurant_MS_Core
{
    public static class GlobalSharing
    {
        public enum Patient_SearchBy { Name = 1, MRNo = 2, Email = 3, Phone = 4, DateOfBirth = 5, Address = 6, ReferredBy = 7 }
        
        public static long UserId = 0;
        
        public static string Salt = "@#$sjaFDs?!djsRT";
    }
}