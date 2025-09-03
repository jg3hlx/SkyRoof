using System.ComponentModel;
using System.Globalization;

namespace SkyRoof
{
  internal class RadioModelConverter : StringConverter
  {
    private string[]? RadioModels;

    public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
    {
      return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
    {
      ListRadioModels();
      return new StandardValuesCollection(RadioModels);
    }

    protected void ListRadioModels()
    {
      RadioModels ??= CatControlEngine.BuildRadioCapabilitiesList().Select(e => e.model).ToArray();
    }

    // to object
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
      return value as string;
    }

    // to string
    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
      ListRadioModels();

      if (RadioModels!.Contains(value as string))
        return (string)value!;
      else
        return "<please select>";
    }
  }
}
