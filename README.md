# Bannerlord.MBOptionScreen

Check the [wiki](https://github.com/Aragas/Bannerlord.MBOptionScreen/wiki) for more info!  

A fork of ``ModLib`` created by ``mipen`` that only focuses on providing a library for creating an unified Mod Option screen for mods.
The main idea for this fork is to introduce a versioning system.  
``Bannerlord.MBOptionScreen`` guarantees that if for example, one mod uses an old version of ``Bannerlord.MBOptionScreen`` (the game version in this case might be e1.0.20, and the mod e1.0.10 - the game updates didn't break the mod, but it might have broken the UI, so MBOptionScreen won't work on the newer versions) and another mod uses a newer version of ``Bannerlord.MBOptionScreen``, conflict between them will be avoided and the newer version will be used (so the old mod will be able to work even tho it references an older MBOptionScreen, but this does require that at least one mod installed is using MBOptionScreen that is compatible with the game version).  
The actual UI implementations are tied to the game version, so if ``Bannerlord`` will do a lot of small tweaks in the UI in the next updates, it will still work across all of them.  
