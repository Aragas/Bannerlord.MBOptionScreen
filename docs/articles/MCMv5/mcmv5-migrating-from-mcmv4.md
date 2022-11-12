### Notable changes:
* MCM has moved a lot of namespaces to split it's codebase into 4 parts  
  * `MCM.Common` that contains the most basic MCM classes.  
  * `MCM.Abstractions` that contains core MCM code that is fully independent from the game's abstractions.  
  * `MCM.Implementation` that contains default implementation classes for MCM's abstractions.  
  * `MCM.Bannerlord` contains the code that glues together MCM and Bannerlord.  

  By making most of the MCM's code independent from the game's abstractions we ensure that MCM won't break in a way that we can't fix without breaking our public ABI and API.

* `DropdownDefault<>` was renamed to `Dropdown<>`
* Preset system was changed:
  * `GetAvailablePresets` was renamed to `GetBuiltInPresets`
  * The signature changed. Instead of a dictionary with a lambda - constructor inside a class `MemorySettingsPreset` is used with a similar signature. Check MCMv5 docs for the new usage.

* Implementation SubModule was renamed from `MCM.Implementation.MCMImplementationSubModule` to `MCM.Internal.MCMImplementationSubModule`