using System.Globalization;

namespace RoadRashBikeSpecEditor;

public sealed class MainForm : Form
{
    private readonly ComboBox bikes = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 330 };
    private readonly DataGridView grid = new()
    {
        Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
        RowHeadersVisible = false, AutoGenerateColumns = false, EditMode = DataGridViewEditMode.EditOnEnter
    };
    private readonly ToolStripStatusLabel status = new("Open BIKESPEC.RSC to begin.");
    private readonly Button open = new() { Text = "Open..." };
    private readonly Button save = new() { Text = "Save" };
    private readonly Button saveAs = new() { Text = "Save As..." };
    private readonly Button restoreBike = new() { Text = "Restore selected bike" };
    private readonly Button restoreAll = new() { Text = "Restore ALL defaults" };
    private readonly Button restoreOriginal = new() { Text = "Restore original backup" };
    private readonly Button restoreBackup = new() { Text = "Restore backup..." };
    private readonly Button revert = new() { Text = "Revert unsaved" };
    private readonly Label fileName = new() { AutoSize = true, Padding = new Padding(6, 7, 0, 0), Text = "No file loaded" };

    private string? path;
    private byte[]? sourceBytes;
    private byte[]? baselineBytes;
    private int[][]? values;
    private int[][]? baselineValues;
    private int currentBike;
    private bool loading;
    private bool dirty;

    public MainForm()
    {
        Text = "Road Rash BIKESPEC.RSC Editor";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1120, 720);
        MinimumSize = new Size(900, 560);
        BuildUi();
        WireEvents();
        foreach (var b in FactoryDefaults.Bikes) bikes.Items.Add(b);
        bikes.SelectedIndex = 0;
        EnableEditor(false);
    }

    private void BuildUi()
    {
        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = true, Padding = new Padding(8) };
        bar.Controls.AddRange(new Control[]
        {
            open, save, saveAs,
            new Label { Text = "Bike:", AutoSize = true, Padding = new Padding(8, 7, 0, 0) }, bikes,
            restoreBike, restoreAll, restoreOriginal, restoreBackup, revert, fileName
        });

        AddColumn("Offset", "Offset", 75, true);
        AddColumn("Parameter", "Parameter", 235, true);
        AddColumn("Group", "Group", 135, true);
        AddColumn("Factory", "Factory default", 110, true);
        AddColumn("Range", "Factory range", 145, true);
        AddColumn("Value", "Value", 130, false);

        var strip = new StatusStrip();
        strip.Items.Add(status);
        Controls.Add(grid);
        Controls.Add(bar);
        Controls.Add(strip);
    }

    private void AddColumn(string name, string header, int width, bool readOnly) =>
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = header, Width = width, ReadOnly = readOnly });

    private void WireEvents()
    {
        open.Click += (_, _) => OpenFile();
        save.Click += (_, _) => SaveFile();
        saveAs.Click += (_, _) => SaveFileAs();
        restoreBike.Click += (_, _) => RestoreSelectedBike();
        restoreAll.Click += (_, _) => RestoreAllDefaults();
        restoreOriginal.Click += (_, _) => RestoreOriginal();
        restoreBackup.Click += (_, _) => RestoreFromDialog();
        revert.Click += (_, _) => RevertUnsaved();
        bikes.SelectedIndexChanged += (_, _) => SwitchBike();
        grid.CellValidating += ValidateCell;
        grid.CellEndEdit += (_, e) => { if (!loading && e.ColumnIndex == grid.Columns["Value"].Index) CommitCell(e.RowIndex); };
        FormClosing += (_, e) => { if (dirty && MessageBox.Show(this, "Close without saving changes?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) e.Cancel = true; };
    }

    private void OpenFile()
    {
        if (!CanDiscard()) return;
        using var d = new OpenFileDialog { Title = "Open BIKESPEC.RSC", Filter = "BIKESPEC.RSC|BIKESPEC.RSC;*.rsc|RSC files|*.rsc|All files|*.*" };
        if (d.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var bytes = BikeSpecFile.LoadBytes(d.FileName);
            var backup = BikeSpecFile.EnsureOriginalBackup(d.FileName);
            path = d.FileName;
            LoadState(bytes, false);
            fileName.Text = Path.GetFileName(path);
            EnableEditor(true);
            status.Text = $"Loaded. Original backup: {Path.GetFileName(backup)}";
        }
        catch (Exception ex) { Error("Could not open BIKESPEC.RSC", ex); }
    }

    private void LoadState(byte[] bytes, bool asUnsavedChange)
    {
        sourceBytes = (byte[])bytes.Clone();
        values = BikeSpecFile.ReadValues(bytes);
        if (!asUnsavedChange)
        {
            baselineBytes = (byte[])bytes.Clone();
            baselineValues = BikeSpecFile.CloneValues(values);
            dirty = false;
        }
        else dirty = true;
        currentBike = Math.Max(0, bikes.SelectedIndex);
        Render();
        UpdateTitle();
    }

    private void SwitchBike()
    {
        if (loading || values is null || bikes.SelectedIndex < 0) return;
        if (!CommitGrid()) { loading = true; bikes.SelectedIndex = currentBike; loading = false; return; }
        currentBike = bikes.SelectedIndex;
        Render();
        status.Text = $"Editing {FactoryDefaults.Bikes[currentBike]}";
    }

    private void Render()
    {
        if (values is null) return;
        loading = true;
        grid.Rows.Clear();
        for (var f = 0; f < BikeSpecFile.FieldCount; f++)
        {
            var def = FactoryDefaults.Fields[f];
            var factory = FactoryDefaults.GetDefault(currentBike, f);
            var (min, max) = FactoryDefaults.GetFactoryRange(f);
            var value = values[currentBike][f];
            var r = grid.Rows.Add($"0x{def.Offset:X3}", def.Name, def.Group, factory,
                min == max ? min.ToString(CultureInfo.InvariantCulture) : $"{min} .. {max}", value);
            StyleCell(r, f, value, factory, min, max);
        }
        loading = false;
    }

    private void ValidateCell(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (loading || e.ColumnIndex != grid.Columns["Value"].Index) return;
        if (!int.TryParse(Convert.ToString(e.FormattedValue, CultureInfo.InvariantCulture), out _))
        {
            e.Cancel = true;
            MessageBox.Show(this, "Value must be a signed 32-bit integer.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void CommitCell(int row)
    {
        if (values is null || row < 0 || !TryValue(row, out var value)) return;
        values[currentBike][row] = value;
        var factory = FactoryDefaults.GetDefault(currentBike, row);
        var (min, max) = FactoryDefaults.GetFactoryRange(row);
        StyleCell(row, row, value, factory, min, max);
        MarkDirty();
    }

    private bool CommitGrid()
    {
        if (values is null) return true;
        grid.EndEdit();
        for (var f = 0; f < BikeSpecFile.FieldCount; f++)
        {
            if (!TryValue(f, out var value)) { MessageBox.Show(this, $"Invalid value in field #{f}.", "Invalid value"); return false; }
            values[currentBike][f] = value;
        }
        return true;
    }

    private bool TryValue(int row, out int value) => int.TryParse(Convert.ToString(grid.Rows[row].Cells["Value"].Value, CultureInfo.InvariantCulture), out value);

    private void RestoreSelectedBike()
    {
        if (values is null) return;
        if (MessageBox.Show(this, $"Restore all 89 values for {FactoryDefaults.Bikes[currentBike]} to embedded defaults?\n\nNothing is written until Save.",
            "Restore selected bike", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        values[currentBike] = FactoryDefaults.GetBikeDefaults(currentBike);
        Render(); MarkDirty(); status.Text = "Selected bike restored to embedded factory/reference defaults.";
    }

    private void RestoreAllDefaults()
    {
        if (values is null) return;
        if (MessageBox.Show(this, "Restore all 15 motorcycles to embedded factory/reference defaults?\n\nNothing is written until Save.",
            "Restore all defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        values = FactoryDefaults.CreateAllDefaults();
        Render(); MarkDirty(); status.Text = "All 15 bikes restored to embedded defaults in memory.";
    }

    private void RestoreOriginal()
    {
        if (path is null) return;
        var backup = BikeSpecFile.GetOriginalBackupPath(path);
        if (!File.Exists(backup)) { MessageBox.Show(this, $"Original backup not found:\n{backup}", "Backup not found"); return; }
        RestoreBackupFile(backup, "original backup");
    }

    private void RestoreFromDialog()
    {
        if (path is null) return;
        using var d = new OpenFileDialog
        {
            Title = "Restore BIKESPEC backup", InitialDirectory = Path.GetDirectoryName(path),
            Filter = "BIKESPEC backups|*.bak;*.rsc|All files|*.*",
            FileName = File.Exists(BikeSpecFile.GetLastSaveBackupPath(path)) ? Path.GetFileName(BikeSpecFile.GetLastSaveBackupPath(path)) : ""
        };
        if (d.ShowDialog(this) == DialogResult.OK) RestoreBackupFile(d.FileName, Path.GetFileName(d.FileName));
    }

    private void RestoreBackupFile(string backupPath, string label)
    {
        try
        {
            var bytes = BikeSpecFile.LoadBytes(backupPath);
            LoadState(bytes, true);
            status.Text = $"Loaded {label} into memory. Click Save to write it to BIKESPEC.RSC.";
        }
        catch (Exception ex) { Error("Could not restore backup", ex); }
    }

    private void RevertUnsaved()
    {
        if (!dirty || baselineBytes is null || baselineValues is null) return;
        if (MessageBox.Show(this, "Discard all unsaved edits?", "Revert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        sourceBytes = (byte[])baselineBytes.Clone();
        values = BikeSpecFile.CloneValues(baselineValues);
        dirty = false; Render(); UpdateTitle(); status.Text = "Unsaved edits discarded.";
    }

    private void SaveFile()
    {
        if (path is null || sourceBytes is null || values is null || !CommitGrid() || !ConfirmExtreme()) return;
        try
        {
            BikeSpecFile.CreateLastSaveBackup(path);
            var output = BikeSpecFile.WriteValues(sourceBytes, values);
            File.WriteAllBytes(path, output);
            AcceptSaved(output);
            status.Text = $"Saved. Previous file: {Path.GetFileName(BikeSpecFile.GetLastSaveBackupPath(path))}";
        }
        catch (Exception ex) { Error("Could not save BIKESPEC.RSC", ex); }
    }

    private void SaveFileAs()
    {
        if (sourceBytes is null || values is null || !CommitGrid() || !ConfirmExtreme()) return;
        using var d = new SaveFileDialog { Title = "Save BIKESPEC.RSC As", Filter = "RSC files|*.rsc|All files|*.*", FileName = "BIKESPEC.RSC", InitialDirectory = path is null ? null : Path.GetDirectoryName(path) };
        if (d.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var target = d.FileName;
            if (File.Exists(target))
            {
                if (!File.Exists(BikeSpecFile.GetOriginalBackupPath(target))) File.Copy(target, BikeSpecFile.GetOriginalBackupPath(target));
                File.Copy(target, BikeSpecFile.GetLastSaveBackupPath(target), true);
            }
            else File.WriteAllBytes(BikeSpecFile.GetOriginalBackupPath(target), sourceBytes);
            var output = BikeSpecFile.WriteValues(sourceBytes, values);
            File.WriteAllBytes(target, output);
            path = target; fileName.Text = Path.GetFileName(path); AcceptSaved(output);
            status.Text = $"Saved as {Path.GetFileName(path)}";
        }
        catch (Exception ex) { Error("Could not save BIKESPEC.RSC", ex); }
    }

    private void AcceptSaved(byte[] output)
    {
        sourceBytes = (byte[])output.Clone(); baselineBytes = (byte[])output.Clone();
        baselineValues = BikeSpecFile.CloneValues(values!); dirty = false; Render(); UpdateTitle();
    }

    private bool ConfirmExtreme()
    {
        if (values is null) return true;
        var list = new List<string>();
        for (var b = 0; b < BikeSpecFile.BikeCount && list.Count < 12; b++)
            for (var f = 0; f < BikeSpecFile.FieldCount && list.Count < 12; f++)
                if (Extreme(f, values[b][f])) list.Add($"{FactoryDefaults.Bikes[b].Id:00} {FactoryDefaults.Bikes[b].Name}: {FactoryDefaults.Fields[f].Name} = {values[b][f]}");
        if (list.Count == 0) return true;
        var text = "Some values are far outside the range used by the 15 factory/reference bikes. The game may become unstable or unplayable.\n\n" +
                   string.Join(Environment.NewLine, list) + "\n\nSave anyway?";
        return MessageBox.Show(this, text, "Extreme values detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
    }

    private static bool Extreme(int field, int value)
    {
        var (min, max) = FactoryDefaults.GetFactoryRange(field);
        var maxAbs = Math.Max(Math.Abs((long)min), Math.Abs((long)max));
        return Math.Abs((long)value) > Math.Max(1000L, maxAbs * 10L);
    }

    private void StyleCell(int row, int field, int value, int factory, int min, int max)
    {
        var c = grid.Rows[row].Cells["Value"];
        c.Style.ForeColor = Color.Black;
        c.Style.BackColor = Extreme(field, value) ? Color.MistyRose : value < min || value > max ? Color.LightSalmon : value != factory ? Color.LightGoldenrodYellow : Color.White;
    }

    private void MarkDirty() { dirty = true; UpdateTitle(); }
    private void UpdateTitle() { Text = "Road Rash BIKESPEC.RSC Editor" + (dirty ? " *" : ""); revert.Enabled = values is not null && dirty; }
    private bool CanDiscard() => !dirty || MessageBox.Show(this, "Discard unsaved changes?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
    private void EnableEditor(bool enabled) { foreach (var c in new Control[] { save, saveAs, bikes, restoreBike, restoreAll, restoreOriginal, restoreBackup }) c.Enabled = enabled; revert.Enabled = enabled && dirty; }
    private void Error(string title, Exception ex) => MessageBox.Show(this, ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
}
