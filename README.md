# Bannerlord.MBOptionScreen

A fork of ``ModLib`` created by ``mipen`` that only focuses on providing a library for creating an unified Mod Option screen for mods.
The main idea for this fork is to introduce a versioning system.  
``Bannerlord.MBOptionScreen`` guarantees that if for example, one mod uses an old version of ``Bannerlord.MBOptionScreen`` and another mod uses a newer version of ``Bannerlord.MBOptionScreen``, conflict between them will be avoided and only the newer version will be used.  
The actual UI implementations are tied to the game version, so if ``Bannerlord`` will do a lot of small tweaks in the UI in the next updates, it will still work across all of them, at the cost of bloating the code with implementations copies or using harmony.  
