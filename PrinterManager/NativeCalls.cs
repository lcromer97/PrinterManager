using System.Runtime.InteropServices;

namespace PrinterManager;

public class NativeCalls {
    /// <summary>
    /// Sets the specified printer as the default printer for the current user.
    /// </summary>
    /// <remarks>This method is a wrapper for the Windows API function in "winspool.drv".  If the operation
    /// fails, use <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/>  to retrieve the error code
    /// for more information.</remarks>
    /// <param name="Name">The name of the printer to set as the default. This must match the name of an installed printer.</param>
    /// <returns><see langword="true"/> if the operation succeeds; otherwise, <see langword="false"/>.</returns>
    [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool SetDefaultPrinter(string Name);
}