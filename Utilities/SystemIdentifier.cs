using System.Management;
using System.Security.Cryptography;
using System.Text;

public class SystemIdentifier
{
    public static string GetSystemFingerprint()
    {
        var fingerprintBuilder = new StringBuilder();

        // Get multiple hardware IDs
        fingerprintBuilder.AppendLine(GetProcessorId());
        fingerprintBuilder.AppendLine(GetMotherboardId());
        fingerprintBuilder.AppendLine(GetDiskDriveId());
        fingerprintBuilder.AppendLine(GetBiosId());
        fingerprintBuilder.AppendLine(GetMacAddress());

        // Hash the combined string
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(fingerprintBuilder.ToString()));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    private static string GetProcessorId()
    {
        try
        {
            using (var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["ProcessorId"]?.ToString() ?? "UNKNOWN";
                }
            }
        }
        catch { return "ERROR"; }
        return "UNKNOWN";
    }

    private static string GetMotherboardId()
    {
        try
        {
            using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString() ?? "UNKNOWN";
                }
            }
        }
        catch { return "ERROR"; }
        return "UNKNOWN";
    }

    private static string GetDiskDriveId()
    {
        try
        {
            using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive WHERE Index = 0"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString()?.Trim() ?? "UNKNOWN";
                }
            }
        }
        catch { return "ERROR"; }
        return "UNKNOWN";
    }

    private static string GetBiosId()
    {
        try
        {
            using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString() ?? "UNKNOWN";
                }
            }
        }
        catch { return "ERROR"; }
        return "UNKNOWN";
    }

    private static string GetMacAddress()
    {
        try
        {
            using (var searcher = new ManagementObjectSearcher("SELECT MACAddress FROM Win32_NetworkAdapter WHERE PhysicalAdapter = TRUE"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    var mac = obj["MACAddress"]?.ToString();
                    if (!string.IsNullOrEmpty(mac))
                        return mac;
                }
            }
        }
        catch { return "ERROR"; }
        return "UNKNOWN";
    }
}