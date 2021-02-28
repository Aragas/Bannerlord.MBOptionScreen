﻿using HarmonyLib;

using MCM.Utils;

namespace MCM.Abstractions.Common
{
    public sealed class SelectedIndexWrapper : IWrapper
    {
        private delegate int GetSelectedIndexDelegate();
        private delegate void SetSelectedIndexDelegate(int value);

        private readonly GetSelectedIndexDelegate _getSelectedIndexDelegate;
        private readonly SetSelectedIndexDelegate? _setSelectedIndexDelegate;

        /// <inheritdoc/>
        public object Object { get; }

        public int SelectedIndex
        {
            get => _getSelectedIndexDelegate.Invoke();
            set => _setSelectedIndexDelegate?.Invoke(value);
        }

        public SelectedIndexWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            var selectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));
            _getSelectedIndexDelegate = AccessTools3.GetDelegate<GetSelectedIndexDelegate>(@object, selectedIndexProperty.GetGetMethod())!;
            _setSelectedIndexDelegate = AccessTools3.GetDelegate<SetSelectedIndexDelegate>(@object, selectedIndexProperty.GetSetMethod());
        }
    }
}