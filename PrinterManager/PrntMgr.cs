using System.Data;
using System.Diagnostics;
using PrinterManager.PrinterData;

namespace PrinterManager;

public partial class PrinterManagerApp : Form {
    public PrinterManagerApp() {
        InitializeComponent();
        printerDataGrid.AllowUserToAddRows = false;    // no empty new row
        printerDataGrid.AllowUserToDeleteRows = false; // prevent deletion by keyboard
        printerDataGrid.ReadOnly = true;               // prevent editing
        printerDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        printerDataGrid.MultiSelect = false;
        PrinterDataGrid = printerDataGrid;
        LoadPrinterTableData();
    }

    private static DataGridView? PrinterDataGrid;

    /// <summary>
    /// Populates the printer data grid with information about available printers.
    /// </summary>
    /// <remarks>This method clears any existing data in the printer data grid and repopulates it with columns
    /// and rows representing the available printers. Each row corresponds to a printer, including details such as
    /// whether it is the default printer, its port, display name, and driver name. Printers with invalid or unsupported
    /// port names (e.g., "nul:", "PORTPROMPT:", or empty ports) are excluded.</remarks>
    private static void LoadPrinterTableData() {
        if (PrinterDataGrid is null)
            return;

        // Clear existing data
        if (PrinterDataGrid.Rows is not null) {
            PrinterDataGrid.Rows.Clear();
            PrinterDataGrid.Columns.Clear();
        }

        PrinterDataGrid.Columns.Add(new DataGridViewCheckBoxColumn {
            Name = "IsDefault",
            HeaderText = "Is Default",
            DataPropertyName = "IsDefault",
            Width = 62,
            ReadOnly = true
        });
        PrinterDataGrid.Columns.Add(new DataGridViewTextBoxColumn {
            Name = "Port",
            HeaderText = "Port*",
            DataPropertyName = "Port",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = true
        });
        PrinterDataGrid.Columns.Add(new DataGridViewTextBoxColumn {
            Name = "DisplayName",
            HeaderText = "Display Name",
            DataPropertyName = "DisplayName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = true
        });
        PrinterDataGrid.Columns.Add(new DataGridViewTextBoxColumn {
            Name = "DriverName",
            HeaderText = "Driver Name",
            DataPropertyName = "DriverName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = true
        });

        var printers = PrinterHelper.GetPrinters();
        foreach (var printer in printers) {
            if (printer.PortName is "nul:" ||
                printer.PortName is "PORTPROMPT:" ||
                string.IsNullOrWhiteSpace(printer.PortName))
                continue;

            var indexRow = PrinterDataGrid.Rows.Add(printer.IsDefault, printer.PortName, printer.DisplayName, printer.DriverName);
            PrinterDataGrid.Rows[indexRow].Tag = printer; // Store the PrinterInfo object in the row's Tag property for easy access later
        }
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { return; }

    /// <summary>
    /// Handles the click event for the "Set Default" button, setting the selected printer as the default system
    /// printer.
    /// </summary>
    /// <remarks>This method retrieves the currently selected printer from the data grid and attempts to set
    /// it as the default printer. If no printer is selected, a message box prompts the user to select one. If an error
    /// occurs while setting the default printer, an error message is displayed to the user.</remarks>
    /// <param name="sender">The source of the event, typically the "Set Default" button.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void setDefaultBtn_Click(object sender, EventArgs e) {
        var selectedRow = printerDataGrid.CurrentRow;

        if (selectedRow?.Tag is not PrinterInfo printerInfo) {
            MessageBox.Show("Please select a printer first.");
            return;
        }

        var printers = PrinterHelper.GetPrinters();
        var selectedPrinter = printers.FirstOrDefault(p => p.PortName == printerInfo.PortName);
        try {
            NativeCalls.SetDefaultPrinter(selectedPrinter?.DisplayName ?? string.Empty);
            LoadPrinterTableData();
        }
        catch (Exception ex) {
            MessageBox.Show($"Failed to set default printer:\n{ex.Message}");
        }
    }

    /// <summary>
    /// Handles the click event for the "Add Printer" button, allowing the user to add a new printer by providing
    /// necessary details.
    /// </summary>
    /// <remarks>This method displays a dialog box for the user to input printer details, including the port
    /// name, display name, and driver selection. If the input is invalid or incomplete, the operation is aborted, and
    /// an error message is displayed. The method ensures that the port name is unique among existing printers before
    /// adding the new printer.</remarks>
    /// <param name="sender">The source of the event, typically the "Add Printer" button.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void addPrinterBtn_Click(object sender, EventArgs e) {
        var result = InputDialog.Show("Add Printer", [..PrinterHelper.GetInstalledPrinterDrivers().Select(d => d.Name ?? "Unknown Driver")]);
        if (result is null) {
            MessageBox.Show("Invalid or no data was entered, aborting.", "Add Printer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var checkbox = result.Value.checkbox;
        var portname = result.Value.text1;
        var displayname = result.Value.text2;
        var dropdown = result.Value.dropdown;

        if (string.IsNullOrWhiteSpace(portname) || string.IsNullOrWhiteSpace(displayname) || string.IsNullOrWhiteSpace(dropdown)) {
            MessageBox.Show("Invalid or no data was entered, aborting.", "Add Printer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var driverInfo = PrinterHelper.GetInstalledPrinterDrivers().FirstOrDefault(d => d.Name == dropdown);
        var currentprinters = PrinterHelper.GetPrinters();
        if (portname == currentprinters.FirstOrDefault(p => p.PortName == portname)?.PortName) {
            MessageBox.Show("A printer with that port name already exists, aborting.", "Add Printer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        PrinterHelper.AddPrinter(displayname, portname, driverInfo!, checkbox ?? false);
        LoadPrinterTableData();
    }

    /// <summary>
    /// Handles the click event for the "Remove Printer" button. Prompts the user to confirm the removal of the selected
    /// printer and deletes the printer if confirmed.
    /// </summary>
    /// <remarks>If no printer is selected, a message box is displayed to inform the user.  If a printer is
    /// selected, the user is prompted with a confirmation dialog.  The printer is removed only if the user confirms the
    /// action and the printer's port name is valid.</remarks>
    /// <param name="sender">The source of the event, typically the "Remove Printer" button.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private void removePrinterBtn_Click(object sender, EventArgs e) {
        var selectedRow = printerDataGrid.CurrentRow;

        if (selectedRow?.Tag is not PrinterInfo printerInfo) {
            MessageBox.Show("Please select a printer first.");
            return;
        }

        var result = MessageBox.Show($"Are you sure you want to remove '{printerInfo.DisplayName}' from this computer?",
                "Remove Printer",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

        if (result == DialogResult.Yes && !string.IsNullOrWhiteSpace(printerInfo.PortName)) {
            PrinterHelper.DeletePrinter(printerInfo.PortName);
            LoadPrinterTableData();
        }
        else return;
    }

    /// <summary>
    /// Handles the click event for the "Rename Printer" button, allowing the user to rename the selected printer.
    /// </summary>
    /// <remarks>This method prompts the user to enter a new name for the currently selected printer in the
    /// data grid. If no printer is selected, a message box is displayed to inform the user. The method ensures that the
    /// new name is not empty and does not conflict with the name of an existing printer. If the new name is valid, the
    /// printer's name is updated using the <see cref="PrinterHelper.RenamePrinter"/> method.</remarks>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void renamePrinterBtn_Click(object sender, EventArgs e) {
        var selectedRow = printerDataGrid.CurrentRow;

        if (selectedRow?.Tag is not PrinterInfo printerInfo) {
            MessageBox.Show("Please select a printer first.");
            return;
        }

        var newName = InputDialog.Show("Rename Printer", "Enter new printer name:");

        if (string.IsNullOrWhiteSpace(printerInfo.PortName) || string.IsNullOrWhiteSpace(newName))
            return;

        var currentprinters = PrinterHelper.GetPrinters();
        if (newName == currentprinters.FirstOrDefault(p => p.DisplayName == newName)?.DisplayName) {
            MessageBox.Show("A printer with that display name already exists, aborting.", "Rename Printer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var success = PrinterHelper.RenamePrinter(printerInfo.PortName, newName);
        if (success) {
            MessageBox.Show("Printer renamed successfully.");
            LoadPrinterTableData();
        }
        else {
            MessageBox.Show("Failed to rename printer.", "Rename Printer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Handles the click event for the "Change Port" button, allowing the user to update the port of the selected
    /// printer.
    /// </summary>
    /// <remarks>This method prompts the user to select a printer from the grid and then displays a dialog box
    /// to input a new port name. If no printer is selected, a message box is shown to notify the user.</remarks>
    /// <param name="sender">The source of the event, typically the "Change Port" button.</param>
    /// <param name="e">The event data associated with the click event.</param>
    private void changePortBtn_Click(object sender, EventArgs e) {
        var selectedRow = printerDataGrid.CurrentRow;

        if (selectedRow?.Tag is not PrinterInfo printerInfo) {
            MessageBox.Show("Please select a printer first.");
            return;
        }

        var newPort = InputDialog.Show("Change Printer Port", "Enter new printer port:");
        if (string.IsNullOrWhiteSpace(newPort))
            return;

        var currentprinters = PrinterHelper.GetPrinters();
        if (newPort == currentprinters.FirstOrDefault(p => p.PortName == newPort)?.PortName) {
            MessageBox.Show("A printer with that port already exists, aborting.", "Change Printer Port", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var args = $"/Xs /n \"{printerInfo.DisplayName}\" PortName \"{newPort}\"";

        ProcessStartInfo psi = new ("rundll32.exe", $"printui.dll,PrintUIEntry {args}") {
            Verb = "runas",
            UseShellExecute = true,
            CreateNoWindow = true
        };

        Process.Start(psi)?.WaitForExit();
        LoadPrinterTableData();

        MessageBox.Show($"Port for '{printerInfo.DisplayName}' changed to '{newPort}'.");
    }

    /// <summary>
    /// Handles the click event for the "Open Print Queue" button, opening the print queue for the selected printer.
    /// </summary>
    /// <remarks>This method retrieves the currently selected printer from the data grid and opens its print
    /// queue using the system's print management utility. If no printer is selected, a message box is displayed to
    /// prompt the user to select a printer.</remarks>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void openPrintQueueBtn_Click(object sender, EventArgs e) {
        var selectedRow = printerDataGrid.CurrentRow;

        if (selectedRow?.Tag is not PrinterInfo printerInfo) {
            MessageBox.Show("Please select a printer first.");
            return;
        }

        string args = $"/o /n \"{printerInfo.DisplayName}\"";

        ProcessStartInfo psi = new ("rundll32.exe", $"printui.dll,PrintUIEntry {args}") {
            UseShellExecute = true
        };

        Process.Start(psi);
    }

    /// <summary>
    /// Handles the click event of the refresh button and reloads the printer table data.
    /// </summary>
    /// <param name="sender">The source of the event, typically the refresh button.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void refreshBtn_Click(object sender, EventArgs e) => LoadPrinterTableData();

    /// <summary>
    /// Handles the click event for the Properties button, allowing the user to view or modify the properties of the
    /// selected printer.
    /// </summary>
    /// <remarks>This method retrieves the currently selected printer from the data grid and opens the printer
    /// properties dialog for that printer. If no printer is selected, a message box is displayed to prompt the user to
    /// select a printer.</remarks>
    /// <param name="sender">The source of the event, typically the Properties button.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    private void propertiesButton_Click(object sender, EventArgs e) {
        var selectedRow = printerDataGrid.CurrentRow;

        if (selectedRow?.Tag is not PrinterInfo printerInfo) {
            MessageBox.Show("Please select a printer first.");
            return;
        }

        string args = $"/p /n \"{printerInfo.DisplayName}\"";

        ProcessStartInfo psi = new ("rundll32.exe", $"printui.dll,PrintUIEntry {args}") {
            UseShellExecute = true,
            CreateNoWindow = true
        };

        Process.Start(psi);
    }
}

