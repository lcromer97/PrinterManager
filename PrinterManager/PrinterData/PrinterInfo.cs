namespace PrinterManager.PrinterData;

internal class PrinterInfo {
    public bool IsDefault { get; set; }
    public string? PortName { get; set; } // key entry
    public string? DisplayName { get; set; }
    public string? DriverName { get; set; }
    public PrinterDriver? DriverDetails { get; set; }
}