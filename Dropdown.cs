using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen
{
    public class Dropdown
    {
        protected IList Values;
        protected int SelectedIndex;

        public virtual IList GetValues() => Values;
        public virtual object GetSelectedValue() => Values[SelectedIndex];
        public virtual int GetSelectedIndex() => SelectedIndex;
    }

    public class Dropdown<T> : Dropdown
    {
        public static Dropdown<T> Empty => new Dropdown<T>(Array.Empty<T>(), 0);

        public Dropdown(IEnumerable<T> values, int selectedIndex)
        {
            Values = values.ToList();
            SelectedIndex = selectedIndex;

            if (SelectedIndex != 0 && SelectedIndex >= Values.Count)
                throw new Exception();
        }

        public virtual IEnumerable<T> GetEnumerable()
        {
            foreach (var @object in Values)
                yield return (T)@object;
        }
        public virtual IList<T> GetValues() => GetEnumerable().ToList();
        public new virtual T GetSelectedValue() => (T)Values[SelectedIndex];
        public new virtual void SelectValue(T @obj)
        {
            SelectedIndex = Values.IndexOf(@obj);
        }
    }
}