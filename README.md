<p align="center">
    <a href="https://www.nuget.org/packages/Bannerlord.MCM" alt="NuGet Bannerlord.MCM">
        <img src="https://img.shields.io/nuget/vpre/Bannerlord.MCM.svg?label=Bannerlord.MCM&colorB=blue" /></a>
    <a href="https://www.nuget.org/packages/Bannerlord.MCM.Integrated" alt="NuGet Bannerlord.MCM.Integrated">
        <img src="https://img.shields.io/nuget/vpre/Bannerlord.MCM.Integrated.svg?label=Bannerlord.MCM.Integrated&colorB=blue" /></a>
</p>

AKA MBOptionScreen Standalone.  
  
MCM is a Mod Options screen library designed to let modders use its API for defining the options.  
It can also display settings from other API's like ModLib, pre 1.3 and post 1.3, MBOv1/MCMv2, by using the compatibility layer modules.  
  
MCM supports two setting types - Global and PerCharacter. Global are shared across characters and saves, PerCharacter are shared across saves with the same character!  
  
It provides 5 types of options:
* Bool
* Int Slider / Textbox
* Float Slider / Textbox 
* Textbox
* Dropdown  
  
The settings can be defined at compile time by using the Attribute API and at runtime by using the Fluent Builder.  
  
See the [Wiki](https://github.com/Aragas/Bannerlord.MBOptionScreen/wiki/MCMv3) for more details!  
  
Use NuGet packets [Bannerlord.MCM](https://www.nuget.org/packages/Bannerlord.MCM/) and [Bannerlord.MCM.Integrated](https://www.nuget.org/packages/Bannerlord.MCM.Integrated/)!  
