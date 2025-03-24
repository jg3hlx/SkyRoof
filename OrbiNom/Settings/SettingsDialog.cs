using System.Text.RegularExpressions;
using VE3NEA;

namespace OrbiNom

{
  internal partial class SettingsDialog : Form
  {
    private readonly Context ctx;
    private readonly List<string> ChangedFields = new();

    internal SettingsDialog()
    {
      InitializeComponent();
    }

    internal SettingsDialog(Context ctx, string section = null)
    {
      InitializeComponent();
      this.ctx = ctx;
      grid.SelectedObject = Utils.DeepClone(ctx.Settings);
      grid.ExpandAllGridItems();
      SelectSection(section);
    }

    private void SelectSection(string section)
    {
      if (section == null) return;

      var gridItem = grid.GetItemByFullName(section);
      grid.ExpandAndSelect(gridItem);
    }

    private void resetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      grid.ResetSelectedProperty();

      string label = PropertyGridEx.GetItemProperty(grid.SelectedGridItem, "HelpKeyword");
      ChangedFields.Add(label);
    }

    private void applyBtn_Click(object sender, EventArgs e)
    {
      ctx.Settings = Utils.DeepClone((Settings)grid.SelectedObject);
      ApplyChangedSettings();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      ctx.Settings = (Settings)grid.SelectedObject;
      ApplyChangedSettings();
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                        validate changes
    //--------------------------------------------------------------------------------------------------------------
    private void grid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      bool canChange = true;
      string label = PropertyGridEx.GetItemProperty(e.ChangedItem, "HelpKeyword");

      switch (label)
      {
        case "OrbiNom.UserSettings.Call":
          canChange = ValidateField(e, Utils.CallsignRegex, "callsign", CharacterCasing.Upper);
          break;

        case "OrbiNom.UserSettings.Square":
          canChange = ValidateField(e, Utils.GridSquare6Regex, "grid square", CharacterCasing.Upper);
          break;

        case "OrbiNom.UserSettings.Altitude":
          ValidateAltitude(e);
          break;

          //        case "OrbiNom.Something.Enabled":
          //          var sett = (Settings)grid.SelectedObject;
          //          changed = !sett.Something.Enabled || (!string.IsNullOrEmpty(sett.User.Call) && !string.IsNullOrEmpty(sett.User.Square));
          //          if (!canChange)
          //          {
          //            e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
          //            MessageBox.Show($"Please enter your callsign and grid square before you enable DX Cluster", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          //          }
          //          break;
      }

      if (canChange) ChangedFields.Add(label);
    }

    private bool ValidateField(PropertyValueChangedEventArgs e, Regex regEx, string fieldName, CharacterCasing casing)
    {
      string newValue = e.ChangedItem.Value.ToString();
      string cleanValue = Utils.SetCasing(newValue.Trim(), casing);

      if (!regEx.IsMatch(cleanValue))
      {
        e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
        MessageBox.Show($"Invalid {fieldName}: \"{newValue}\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }

      if (cleanValue != newValue) e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, cleanValue);
      return true;
    }

    private void ValidateAltitude(PropertyValueChangedEventArgs e)
    {
      int cleanValue = Math.Max(0, Math.Min(8849, (int)e.ChangedItem.Value));
      e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, cleanValue);
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                            apply changes
    //--------------------------------------------------------------------------------------------------------------
    private void ApplyChangedSettings()
    {
      if (ChangedFields.Contains("OrbiNom.UserSettings.Square") || ChangedFields.Contains("OrbiNom.UserSettings.Altitude"))
      {
        ctx.MainForm.SetLocation();
      }

      if (ChangedFields.Exists(s => s.StartsWith("OrbiNom.AudioSettings.")))
      {
        ctx.MainForm.ApplyAudioSettings();
      }


      ChangedFields.Clear();
    }
  }
}
