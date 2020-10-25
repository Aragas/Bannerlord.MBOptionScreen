For the options to work, you should include the additional SubModule that ``Bannerlord.MBOptionScreen`` provides
```xml
    <SubModule>
      <Name value="MBOptionScreen" />
      <DLLName value="MBOptionScreen.v2.0.10.dll" />
      <SubModuleClassType value="MBOptionScreen.MBOptionScreenSubModuleV2010" />
      <Tags>
        <Tag key="DedicatedServerType" value="none" />
        <Tag key="IsNoRenderModeElement" value="false" />
      </Tags>
    </SubModule>
```
* You need to reference a specific assembly version. For example, if you are integrating v2.0.4, be sure that the .dll name is ``MBOptionScreen.v2.0.4.dll``.  
The reason for this is that the game might have multiple mods with integrated MBOptionScreen. If the library name is the same one (MBOptionScreen.dll), only one version would be used. It could be a lower version, which will break mods depending on a higher version. By specifying the library version, every ``MBOptionScreen`` library will be loaded. While this will take a little more more resources, we can guarantee that ``MBOptionScreen`` will always find the latest interface implementations that are compatible with the game version.

In conclusion, let's say you are making a mod Aragas.MercenaryContract.

And you would like to use MBOptionScreen as a dependency.

Instead of your old ways with ModLib(when you add a DependedModule).

You should add another tag of `SubModule` inside the `SubModules` tag.

So basically your `SubModules tag` is supposed to hold two `SubModule tag` inside of itself.

And they should look like this:
```xml
<SubModules>
    <SubModule>
      <Name value="Aragas.MercenaryContract" />
      <DLLName value="Aragas.MercenaryContract.dll" />
      <SubModuleClassType value="Aragas.MountAndBlade.MercenaryContractSubModule" />
      <Tags>
        <Tag key="DedicatedServerType" value="none" />
        <Tag key="IsNoRenderModeElement" value="false" />
      </Tags>
    </SubModule>
    <SubModule>
      <Name value="MBOptionScreen" />
      <DLLName value="MBOptionScreen.v2.0.4.dll" />
      <SubModuleClassType value="MBOptionScreen.MBOptionScreenSubModule" />
      <Tags>
        <Tag key="DedicatedServerType" value="none" />
        <Tag key="IsNoRenderModeElement" value="false" />
      </Tags>
    </SubModule>
  </SubModules>
```

Remember after you compile your mod, there is supposed to be an `MBOptionScreen.v%VERSION%.dll` inside the `Win64_Shipping_Client` folder of your module.

In this way, the Bannerlord launcher should load your module with MBOptionScreen without any problems.