using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;

namespace MBOptionScreen.GUI.v1a.GauntletUI
{
    // TODO: Finish the constructor
    public class ModOptionsScreenWrapper : ScreenBase
    {
        private readonly object _object;
        private PropertyInfo MouseVisibleProperty { get; }
        private PropertyInfo IsActiveProperty { get; }
        private PropertyInfo LayersProperty { get; }
        private PropertyInfo DebugInputProperty { get; }
        private PropertyInfo IsPausedProperty { get; }

        private MethodInfo ActivateMethod { get; }
        private MethodInfo ActivateAllLayersMethod { get; }
        private MethodInfo AddLayerMethod { get; }
        private MethodInfo DeactivateMethod { get; }
        private MethodInfo DeactivateAllLayersMethod { get; }
        private MethodInfo DeactivateLayerCategoryMethod { get; }
        private MethodInfo FindLayer1Method { get; }
        private MethodInfo FindLayer2Method { get; }
        private MethodInfo HasLayerMethod { get; }
        private MethodInfo RemoveLayerMethod { get; }
        private MethodInfo SetLayerCategoriesStateMethod { get; }
        private MethodInfo SetLayerCategoriesStateAndDeactivateOthersMethod { get; }
        private MethodInfo SetLayerCategoriesStateAndToggleOthersMethod { get; }
        private MethodInfo UpdateLayoutMethod { get; }


        public virtual bool MouseVisible { get => (bool) MouseVisibleProperty.GetValue(_object); set => MouseVisibleProperty.SetValue(_object, value); }
        public bool IsActive => (bool) IsActiveProperty.GetValue(_object);
        public IReadOnlyList<ScreenLayer> Layers => (IReadOnlyList<ScreenLayer>) LayersProperty.GetValue(_object);
        public IInputContext DebugInput => (IInputContext) DebugInputProperty.GetValue(_object);
        public bool IsPaused => (bool) IsPausedProperty.GetValue(_object);

        public void Activate() => ActivateMethod.Invoke(_object, new object[] { });
        public void ActivateAllLayers() => ActivateAllLayersMethod.Invoke(_object, new object[] { });
        public void AddLayer(ScreenLayer layer) => AddLayerMethod.Invoke(_object, new object[] { layer });
        public void Deactivate() => DeactivateMethod.Invoke(_object, new object[] { });
        public void DeactivateAllLayers() => DeactivateAllLayersMethod.Invoke(_object, new object[] { });
        public void DeactivateLayerCategory(string categoryId) => DeactivateLayerCategoryMethod.Invoke(_object, new object[] { categoryId });
        public T FindLayer<T>() where T : ScreenLayer => (T) FindLayer1Method.Invoke(_object, new object[] { });
        public T FindLayer<T>(string name) where T : ScreenLayer => (T) FindLayer2Method.Invoke(_object, new object[] { name });
        public bool HasLayer(ScreenLayer layer) => (bool) HasLayerMethod.Invoke(_object, new object[] { layer });
        public void RemoveLayer(ScreenLayer layer) => RemoveLayerMethod.Invoke(_object, new object[] { layer });
        public void SetLayerCategoriesState(string[] categoryIds, bool isActive) => SetLayerCategoriesStateMethod.Invoke(_object, new object[] { categoryIds, isActive });
        public void SetLayerCategoriesStateAndDeactivateOthers(string[] categoryIds, bool isActive) => SetLayerCategoriesStateAndDeactivateOthersMethod.Invoke(_object, new object[] { categoryIds, isActive });
        public void SetLayerCategoriesStateAndToggleOthers(string[] categoryIds, bool isActive) => SetLayerCategoriesStateAndToggleOthersMethod.Invoke(_object, new object[] { categoryIds, isActive });
        public void UpdateLayout() => UpdateLayoutMethod.Invoke(_object, new object[] { });
    }
}
