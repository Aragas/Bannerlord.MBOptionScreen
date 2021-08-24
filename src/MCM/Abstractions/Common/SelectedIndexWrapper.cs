using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

namespace MCM.Abstractions.Common
{
    public sealed class SelectedIndexWrapper : IWrapper
    {
        private delegate int GetSelectedIndexDelegate();
        private delegate void SetSelectedIndexDelegate(int value);

        private readonly GetSelectedIndexDelegate? _getSelectedIndexDelegate;
        private readonly SetSelectedIndexDelegate? _setSelectedIndexDelegate;

        /// <inheritdoc/>
        public object Object { get; }

        public int SelectedIndex
        {
            get => _getSelectedIndexDelegate?.Invoke() ?? -1;
            set => _setSelectedIndexDelegate?.Invoke(value);
        }

        public SelectedIndexWrapper(object @object)
        {
            Object = @object;
            var type = @object?.GetType();

            _getSelectedIndexDelegate = type is not null
                ? AccessTools2.GetPropertyGetterDelegate<GetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex))
                : null;
            _setSelectedIndexDelegate = type is not null
                ? AccessTools2.GetPropertySetterDelegate<SetSelectedIndexDelegate>(@object, type, nameof(SelectedIndex))
                : null;

            if (_getSelectedIndexDelegate is null || _setSelectedIndexDelegate is null)
            {
                if (GenericServiceProvider.GetService<IBUTRLogger<SelectedIndexWrapper>>() is { } logger)
                {
                    if (@object is null)
                    {
                        logger.LogError(default!, "@object is null!");
                    }
                    if (_getSelectedIndexDelegate is null)
                    {
                        logger.LogError(default!, $"@_getSelectedIndexDelegate is null! Type {type}");
                    }
                    if (_setSelectedIndexDelegate is null)
                    {
                        logger.LogError(default!, $"@_setSelectedIndexDelegate is null! Type {type}");
                    }
                }
            }
        }
    }
}