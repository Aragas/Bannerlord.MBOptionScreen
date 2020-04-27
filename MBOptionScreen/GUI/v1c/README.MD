So the game has an issue when a multiple Widget derived classes have the same name
I can't patch WidgetInfo.CollectWidgetTypes() because GauntletUISubModule will be always loaded, so there is no workayound for it
The only way right now to avoid widget collisions is to increment the while GUI implementation for each new version.