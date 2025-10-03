using System.Diagnostics;
using System.Management;
using PrinterManager.PrinterData;

namespace PrinterManager;

internal static class PrinterHelper {
    /// <summary>
    /// Retrieves a list of printers installed on the system, including their details and associated drivers.
    /// </summary>
    /// <remarks>This method queries the system for all installed printers and gathers information such as the
    /// printer's display name, port name, driver name, and whether it is the default printer. It also associates each
    /// printer with its corresponding driver details, if available.</remarks>
    /// <returns>A list of <see cref="PrinterInfo"/> objects, where each object contains information about an installed printer.
    /// The list will be empty if no printers are found.</returns>
    internal static List<PrinterInfo> GetPrinters() {
        var printers = new List<PrinterInfo>();
        var drivers = GetInstalledPrinterDrivers();
        using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
        foreach (var printer in searcher.Get()) {
            string SafeGet(string propName) {
                try {
                    var prop = printer.Properties[propName];
                    return prop != null && prop.Value != null ? prop.Value.ToString() : string.Empty;
                }
                catch (ManagementException) {
                    return string.Empty;
                }
            }

            var driverName = SafeGet("DriverName");
            var driver = drivers.FirstOrDefault(d => d.Name == driverName);

            printers.Add(new PrinterInfo {
                IsDefault = bool.TryParse(SafeGet("Default"), out var isDef) && isDef,
                PortName = SafeGet("PortName"),
                DisplayName = SafeGet("Name"), // Correct property for printer display name
                DriverName = driverName,
                DriverDetails = driver
            });
        }
        return printers;
    }

    /// <summary>
    /// Retrieves a list of installed printer drivers on the system.
    /// </summary>
    /// <remarks>This method queries the system for installed printer drivers using the Win32_PrinterDriver
    /// WMI class. Each driver is represented as a <see cref="PrinterDriver"/> object containing details such as the
    /// driver name, version, environment, and associated files.</remarks>
    /// <returns>A list of <see cref="PrinterDriver"/> objects, where each object contains information about an installed printer
    /// driver. The list will be empty if no printer drivers are found.</returns>
    internal static List<PrinterDriver> GetInstalledPrinterDrivers() {
        var drivers = new List<PrinterDriver>();
        using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PrinterDriver");
        foreach (var obj in searcher.Get()) {
            string SafeGet(string propName) {
                try {
                    var prop = obj.Properties[propName];
                    return prop != null && prop.Value != null ? prop.Value.ToString() : string.Empty;
                }
                catch (ManagementException) {
                    // Property is not available for this driver
                    return string.Empty;
                }
            }

            drivers.Add(new PrinterDriver {
                Name = SafeGet("Name"),
                Version = SafeGet("DriverVersion"),
                Environment = SafeGet("Environment"),
                InfName = SafeGet("InfName"),
                DriverPath = SafeGet("DriverPath"),
                DataFile = SafeGet("DataFile"),
                ConfigFile = SafeGet("ConfigFile")
            });
        }
        return drivers;
    }

    /// <summary>
    /// Renames a printer on the system from the specified old name to the new name.
    /// </summary>
    /// <remarks>This method uses the Windows Management Instrumentation (WMI) to locate and rename the
    /// printer.  It returns <see langword="false"/> if the printer with the specified <paramref name="oldName"/>  is
    /// not found, if the renaming operation fails, or if an exception occurs during the process.</remarks>
    /// <param name="oldName">The current name of the printer to be renamed. This value cannot be null or empty.</param>
    /// <param name="newName">The new name to assign to the printer. This value cannot be null or empty.</param>
    /// <returns><see langword="true"/> if the printer was successfully renamed; otherwise, <see langword="false"/>.</returns>
    public static bool RenamePrinter(string oldName, string newName) {
        try {
            string query = $"SELECT * FROM Win32_Printer WHERE Name = '{oldName}'";
            using var searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject printer in searcher.Get().Cast<ManagementObject>()) {
                var result = printer.InvokeMethod("Rename", [ newName ]);
                return (uint)result == 0; // 0 = success
            }
        }
        catch {
            return false;
        }

        return false;
    }

    /// <summary>
    /// Installs a new printer on the system using the specified display name, port, and driver information.
    /// </summary>
    /// <remarks>This method requires administrative privileges to execute successfully. It uses the Windows
    /// PrintUIEntry utility  to install the printer and optionally sets it as the default printer if <paramref
    /// name="setDefault"/> is <see langword="true"/>.</remarks>
    /// <param name="displayName">The display name of the printer to be installed. This name will appear in the system's list of printers.</param>
    /// <param name="portName">The name of the port to which the printer is connected. For example, "COM1" or "IP_192.168.1.100".</param>
    /// <param name="driverInfo">An object containing the printer driver information, including the driver name and path.</param>
    /// <param name="setDefault">A value indicating whether the newly installed printer should be set as the default printer. <see
    /// langword="true"/> to set the printer as the default; otherwise, <see langword="false"/>.</param>
    internal static void AddPrinter(string displayName, string portName, PrinterDriver driverInfo, bool setDefault = false) {
        string args = $"/if /b \"{displayName}\" /f \"{driverInfo.DriverPath}\" /r \"{portName}\" /m \"{driverInfo.Name}\"";

        var psi = new ProcessStartInfo("rundll32.exe", $"printui.dll,PrintUIEntry {args}") {
            Verb = "runas", // Run as admin
            CreateNoWindow = true,
            UseShellExecute = true
        };

        Process.Start(psi)?.WaitForExit();

        if (setDefault) {
            NativeCalls.SetDefaultPrinter(displayName);
        }
    }

    /// <summary>
    /// Deletes a printer associated with the specified port name.
    /// </summary>
    /// <remarks>This method removes the printer by invoking the system's print management utility. 
    /// Administrator privileges are required to execute this operation.</remarks>
    /// <param name="portName">The name of the port associated with the printer to be deleted. This value cannot be <see langword="null"/> or
    /// empty.</param>
    /// <exception cref="ArgumentException">Thrown if no printer is found with the specified <paramref name="portName"/>.</exception>
    internal static void DeletePrinter(string portName) {
        var printers = GetPrinters();
        var printer = printers.FirstOrDefault(p => p.PortName == portName) ?? throw new ArgumentException("Printer not found.", nameof(portName));
        string args = $"/dl /n \"{printer.DisplayName}\" /p \"{portName}\"";
        var psi = new ProcessStartInfo("rundll32.exe", $"printui.dll,PrintUIEntry {args}") {
            Verb = "runas", // Run as admin
            CreateNoWindow = true,
            UseShellExecute = true
        };

        Process.Start(psi)?.WaitForExit();

        MessageBox.Show($"{printer.DisplayName} on Port {printer.PortName} has been removed.");
    }
}