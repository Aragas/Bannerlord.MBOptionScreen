using System.Collections.Generic;

using TaleWorlds.Library;

namespace MBOptionScreen.ExtensionMethods
{
    public static class MBBindingListExtensions
    {
        public static void AddRange<T>(this MBBindingList<T> bindingList, List<T> listToAdd)
        {
            if (listToAdd.Count == 1)
                bindingList.Add(listToAdd[0]);
            else if (listToAdd.Count > 0)
            {
                for (var i = listToAdd.Count - 1; i >= 0; i--)
                {
                    bindingList.Add(listToAdd[i]);
                }
            }
        }
    }
}