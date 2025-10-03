namespace PrinterManager;

using System;
using System.Windows.Forms;

public class InputDialog : Form {
    private TextBox inputBox;
    private Button okButton;
    private Button cancelButton;

    private CheckBox checkBox;
    private TextBox textBox1;
    private TextBox textBox2;
    private ComboBox comboBox;
    private Button randomizedNameButton;
    private Button loadInfFileButton;

    public string InputText => inputBox.Text;

    public bool CheckboxValue => checkBox.Checked;
    public string TextField1 => textBox1.Text;
    public string TextField2 => textBox2.Text;
    public string? DropdownValue => comboBox.SelectedItem?.ToString();

    public InputDialog(string title, string prompt) {
        Text = title;
        Width = 300;
        Height = 150;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        var label = new Label { Left = 10, Top = 10, Text = prompt, Width = 260 };
        inputBox = new TextBox { Left = 10, Top = 35, Width = 260 };

        okButton = new Button { Text = "Submit", Left = 110, Width = 75, Top = 70, DialogResult = DialogResult.OK };
        cancelButton = new Button { Text = "Cancel", Left = 195, Width = 75, Top = 70, DialogResult = DialogResult.Cancel };

        AcceptButton = okButton;
        CancelButton = cancelButton;

        Controls.Add(label);
        Controls.Add(inputBox);
        Controls.Add(okButton);
        Controls.Add(cancelButton);
    }

    public InputDialog(string title, string[] dropdownOptions) {
        Text = title;
        Width = 470;
        Height = 225;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        var label1 = new Label { Text = "Set to Default", Left = 10, Top = 15, Width = 100 };
        checkBox = new CheckBox { Left = 120, Top = 10, Width = 150 };

        var label2 = new Label { Text = "Port Name", Left = 10, Top = 45, Width = 100 };
        textBox1 = new TextBox { Left = 120, Top = 40, Width = 200 };

        var label3 = new Label { Text = "Display Name", Left = 10, Top = 75, Width = 100 };
        textBox2 = new TextBox { Left = 120, Top = 70, Width = 200 };

        // Random name button
        randomizedNameButton = new Button { Text = "Random", Left = 330, Top = 40, Width = 75 };
        randomizedNameButton.Click += (s, e) => {
            textBox1.Text = "WSD-" + Guid.NewGuid().ToString();
        };

        var label4 = new Label { Text = "Printer Driver", Left = 10, Top = 105, Width = 100 };
        comboBox = new ComboBox { Left = 120, Top = 100, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        comboBox.Items.AddRange(dropdownOptions);
        if (comboBox.Items.Count > 0)
            comboBox.SelectedIndex = 0; // default select first

        // Load .inf Button
        loadInfFileButton = new Button { Text = "Load .inf", Left = 330, Top = 100, Width = 100 };
        loadInfFileButton.Click += (s, e) => {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "INF Files (*.inf)|*.inf|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK) {
                string fileName = Path.GetFileName(ofd.FileName);
                // Add to dropdown if not already present
                if (!comboBox.Items.Contains(fileName))
                    comboBox.Items.Add(fileName);

                comboBox.SelectedItem = fileName; // auto-select
            }
        };

        okButton = new Button { Text = "Submit", Left = 270, Width = 75, Top = 160, DialogResult = DialogResult.OK };
        cancelButton = new Button { Text = "Cancel", Left = 355, Width = 75, Top = 160, DialogResult = DialogResult.Cancel };

        AcceptButton = okButton;
        CancelButton = cancelButton;

        Controls.Add(label1);
        Controls.Add(checkBox);
        Controls.Add(label2);
        Controls.Add(textBox1);
        Controls.Add(label3);
        Controls.Add(textBox2);
        Controls.Add(randomizedNameButton); // add the new button
        Controls.Add(label4);
        Controls.Add(comboBox);
        Controls.Add(loadInfFileButton);
        Controls.Add(okButton);
        Controls.Add(cancelButton);
    }

    public static string? Show(string title, string prompt) {
        using var dialog = new InputDialog(title, prompt);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.InputText : null;
    }

    public static (bool? checkbox, string text1, string text2, string dropdown)? Show(string title, string[] dropdownOptions) {
        using var dialog = new InputDialog(title, dropdownOptions);
        if (dialog.ShowDialog() == DialogResult.OK) {
            return (dialog.CheckboxValue, dialog.TextField1, dialog.TextField2, dialog.DropdownValue)!;
        }
        else {
            (bool? checkbox, string text1, string text2, string dropdown) CanceledResult = (null, null, null, null)!;
            return CanceledResult; // user canceled
        }
    }
}
