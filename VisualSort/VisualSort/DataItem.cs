using System;
using System.Diagnostics;

namespace VisualSort
{
    [DebuggerDisplay("Value = {Value}")]
    public class DataItem : NotifyObject, IComparable<DataItem>
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value, () => Value); }
        }

        public DataItem(int value)
        {
            Value = value;
        }

        public int CompareTo(DataItem other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return "Value: " + Value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as DataItem;

            return other == null ? false : other.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
