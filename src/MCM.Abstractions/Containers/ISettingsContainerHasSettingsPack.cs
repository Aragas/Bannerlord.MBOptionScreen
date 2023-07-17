using MCM.Abstractions.Base;

using System.Collections.Generic;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsContainerHasSettingsPack
    {
        IEnumerable<SettingSnapshot> SaveAvailableSnapshots();
        IEnumerable<BaseSettings> LoadAvailableSnapshots(IEnumerable<SettingSnapshot> snapshots);
    }
}