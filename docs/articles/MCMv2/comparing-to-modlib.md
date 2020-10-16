## Modularity  
One of the main features of ``Bannerlord.MBOptionScreen`` is modularity or 'versioning'.  
Every component was abstracted, so that they could be easily replaced if the game updates.  
#### What does it mean for modders?  
It doesn't matter that you haven't updated ``Bannerlord.MBOptionScreen`` to the latest version. As long as any loaded Module in the game uses a newer version of it, the implementations inside it will be used globally.  
Each new implementation of a component will be written with backwards compatibility, so if some really old mod was installed together with a fresh one, ``Bannerlord.MBOptionScreen`` will not break and will provide options for both mods.  
#### What does it mean for users?  
If some mod was broken because of an old version of ``Bannerlord.MBOptionScreen``, installing a newer version of it as a standalone module will fix it without the need of replacing anything in the original mod.  
  
## Idempotency 
``Bannerlord.MBOptionScreen`` ensures that it will be initialized only once, no matter how much modules reference it or includes it within them. And only the latest implementations that are compatible with the game version will be used.  

## Easy to use  
* ``Bannerlord.MBOptionScreen`` does not require anything from the modder except of the implementation of a ``SettingsBase`` class.  
It will discover every implementation of a ``SettingsBase`` itself, via reflection and initiate it.  
* You also don't need to include any additional .xml files like brushes and prefabs. They are embed into the assembly and will inject themselves into the game during the initialization phase.

## In-game option  
The mod add an entry to the in-game escape menu to access the ``Mod Options`` menu inside the campaign!

## Backwards-compatibility
``Bannerlord.MBOptionScreen`` supports settings from ``ModLib``.  
By default, the original ``ModLib`` menu option is replaced with ``Bannerlord.MBOptionScreen``. This can be disabled in options.  
It also provides a stub ModLib.dll file to make ``Bannerlord.MBOptionScreen`` work even if ``ModLib`` is not present.

## Localization
Supports TW's way of localizing text.