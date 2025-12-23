namespace PrinterManager;

using System;
using System.Windows.Forms;

public class InputDialog : Form {
    private TextBox inputBox;
    private Button okButton;
    private Button cancelButton;

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

    public static string? Show(string title, string prompt) {
        using var dialog = new InputDialog(title, prompt);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.inputBox.Text : null;
    }
}
