using System.Collections;
using System.ComponentModel;
using Pothosware.SoapySDR;

namespace OrbiNom
{
  //------------------------------------------------------------------------------------------------
  //                                     SdrProperties
  //------------------------------------------------------------------------------------------------
  public class SdrProperties : CollectionBase, ICustomTypeDescriptor
  {
    public void Add(SdrProperty Value)
    {
      List.Add(Value);
    }

    public void Remove(string Name)
    {
      foreach (SdrProperty property in List)
        if (property.Name == Name)
        {
          List.Remove(property);
          return;
        }
    }

    public SdrProperty this[int index]
    {
      get => (SdrProperty)List[index];
      set => List[index] = value;      
    }




    public AttributeCollection GetAttributes()
    {
      return TypeDescriptor.GetAttributes(this, true);
    }

    public string? GetClassName()
    {
      return TypeDescriptor.GetClassName(this, true);
    }

    public string? GetComponentName()
    {
      return TypeDescriptor.GetComponentName(this, true);
    }

    public TypeConverter? GetConverter()
    {
      return TypeDescriptor.GetConverter(this, true);
    }

    public EventDescriptor? GetDefaultEvent()
    {
      return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public PropertyDescriptor? GetDefaultProperty()
    {
      return TypeDescriptor.GetDefaultProperty(this, true);
    }

    public object? GetEditor(Type editorBaseType)
    {
      return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public EventDescriptorCollection GetEvents()
    {
      return TypeDescriptor.GetEvents(this, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[]? attributes)
    {
      return TypeDescriptor.GetEvents(this, attributes, true);
    }

    public PropertyDescriptorCollection GetProperties()
    {
      return TypeDescriptor.GetProperties(this, true);
    }

    public PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
    {
      PropertyDescriptor[] newProps = new PropertyDescriptor[Count];
      for (int i = 0; i < Count; i++)
      {
        SdrProperty prop = (SdrProperty)this[i];
        newProps[i] = new SdrPropertyDescriptor(ref prop, attributes);
      }

      return new PropertyDescriptorCollection(newProps);
    }

    public object? GetPropertyOwner(PropertyDescriptor? pd)
    {
      return this;
    }
  }




  //------------------------------------------------------------------------------------------------
  //                                     SdrProperty
  //------------------------------------------------------------------------------------------------
  public class SdrProperty
  {
    public string Name => ArgInfo.Name;
    public string Value;
    public ArgInfo ArgInfo;

    public SdrProperty(ArgInfo argInfo, string value)
    {
      ArgInfo = argInfo;
      Value = value;
    }
  }




  //------------------------------------------------------------------------------------------------
  //                                SdrPropertyDescriptor
  //------------------------------------------------------------------------------------------------
  public class SdrPropertyDescriptor : PropertyDescriptor
  {
    SdrProperty Property;
    public SdrPropertyDescriptor(ref SdrProperty property, Attribute[] attrs) : base(property.Name, attrs)
    {
      Property = property;
    }

    public override Type ComponentType => null;

    public override bool IsReadOnly => false;

    public override Type PropertyType => GetTypeFromArgInfo();

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override object? GetValue(object? component)
    {
      switch (Property.ArgInfo.ArgType)
      {
        case ArgInfo.Type.Bool: return Property.Value == "true";
        case ArgInfo.Type.Int: return int.Parse(Property.Value);
        case ArgInfo.Type.Float: return float.Parse(Property.Value);
        default: return OptionToName();
      }
    }

    private string OptionToName()
    {
      int index = Property.ArgInfo.Options.IndexOf(Property.Value);
      if (index < 0 || index >= Property.ArgInfo.OptionNames.Count) return Property.Value;
      return Property.ArgInfo.OptionNames[index];
    }

    private string NameToOption(string value)
    {
      int index = Property.ArgInfo.OptionNames.IndexOf(value);
      if (index < 0 || index >= Property.ArgInfo.Options.Count) return value;
      return Property.ArgInfo.Options[index];
    }

    public override string Description => Property.ArgInfo.Description;

    public override void ResetValue(object component)
    {
      throw new NotImplementedException();
    }

    public override void SetValue(object? component, object? value)
    {
      switch (Property.ArgInfo.ArgType)
      {
        case ArgInfo.Type.Bool: 
          Property.Value = value?.ToString()?.ToLower() ?? ""; 
          break;

        case ArgInfo.Type.Int:
          Property.Value = NumericToString((int)value);
          break;
        case ArgInfo.Type.Float:
          Property.Value = NumericToString((float)value);
          break;

        default:
          Property.Value = NameToOption((string)value);
          break;
      }
    }

    private string NumericToString(double value)
    {
      if (Property.ArgInfo.Range.Minimum != 0 || Property.ArgInfo.Range.Maximum != 0)
        value = Math.Max(Property.ArgInfo.Range.Minimum, Math.Min(Property.ArgInfo.Range.Maximum, value));        
      
      return value.ToString();
    }

    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }


    private Type GetTypeFromArgInfo()
    {
      switch (Property.ArgInfo.ArgType)
      {
        case ArgInfo.Type.Bool: return typeof(bool);
        case ArgInfo.Type.Int: return typeof(int);
        case ArgInfo.Type.Float: return typeof(float);
        default: return typeof(string);
      }
    }

    public override TypeConverter Converter => GetTypeConverter(); 

    private TypeConverter GetTypeConverter()
    {
      if (Property.ArgInfo.ArgType == ArgInfo.Type.String && Property.ArgInfo.Options.Count > 0)
        return new OptionConverter(Property.ArgInfo);
      else 
        return base.Converter;
    }
  }




  //------------------------------------------------------------------------------------------------
  //                                   OptionConverter
  //------------------------------------------------------------------------------------------------
  public class OptionConverter : StringConverter
  {
    private readonly ArgInfo argInfo;

    public OptionConverter(ArgInfo argInfo)
    {
      this.argInfo = argInfo;
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) => true;

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) => true;

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
    {
      var options = argInfo.OptionNames.Count > 0 ? argInfo.OptionNames : argInfo.Options;
      var coll = new StandardValuesCollection(options.ToArray());
      return coll;
    }
  }
}
