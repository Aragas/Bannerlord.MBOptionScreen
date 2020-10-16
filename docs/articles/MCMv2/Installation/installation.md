* You need to install the NuGet package ``Bannerlord.MBOptionScreen``.  
* Add the submodule in your [SubModule](https://github.com/Aragas/Bannerlord.MBOptionScreen/wiki/Editing-SubModule.xml) definition
* Add an implementation of [SettingsBase](https://github.com/Aragas/Bannerlord.MBOptionScreen/wiki/Settings-Definition-Example).  
  
Please note that you should not use ``Settings.Instance`` in ``OnSubModuleLoad()`` as the setting might still not be loaded depending on mod loading order.  
  
No additional steps are required.  
The Module will find all existing SettingsBase implementation and show them in the settings menu. Old and current implementations based on ModLib are supported, but the settings will reset due to different handling of configuration files.  
You don't need to add the .xml files. They are embedded into the module itself and will be injected into the game automatically.