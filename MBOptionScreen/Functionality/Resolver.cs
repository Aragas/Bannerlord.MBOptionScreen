using MBOptionScreen.Utils;

namespace MBOptionScreen.Functionality
{
    public static class Resolver
    {
        private static IIngameMenuScreenHandler? _ingameMenuScreenHandler;
        public static IIngameMenuScreenHandler IngameMenuScreenHandler => _ingameMenuScreenHandler
            ??= DI.GetImplementation<IIngameMenuScreenHandler, IngameMenuScreenHandlerWrapper>(ApplicationVersionUtils.GameVersion());

        private static IGameMenuScreenHandler? _gameMenuScreenHandler;
        public static IGameMenuScreenHandler GameMenuScreenHandler => _gameMenuScreenHandler
            ??= DI.GetImplementation<IGameMenuScreenHandler, GameMenuScreenHandlerWrapper>(ApplicationVersionUtils.GameVersion());

        private static IModLibScreenOverrider? _modLibScreenOverrider;
        public static IModLibScreenOverrider ModLibScreenOverrider => _modLibScreenOverrider
            ??= DI.GetImplementation<IModLibScreenOverrider, ModLibScreenOverriderWrapper>(ApplicationVersionUtils.GameVersion());
    }
}