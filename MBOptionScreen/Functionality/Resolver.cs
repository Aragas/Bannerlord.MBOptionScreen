using MBOptionScreen.Utils;

namespace MBOptionScreen.Functionality
{
    public static class Resolver
    {
        private static IIngameMenuScreenHandler? _ingameMenuScreenHandler;
        public static IIngameMenuScreenHandler IngameMenuScreenHandler => _ingameMenuScreenHandler
            ??= ReflectionUtils.GetImplementation<IIngameMenuScreenHandler, IngameMenuScreenHandlerWrapper>(ApplicationVersionParser.GameVersion());

        private static IGameMenuScreenHandler? _gameMenuScreenHandler;
        public static IGameMenuScreenHandler GameMenuScreenHandler => _gameMenuScreenHandler
            ??= ReflectionUtils.GetImplementation<IGameMenuScreenHandler, GameMenuScreenHandlerWrapper>(ApplicationVersionParser.GameVersion());

        private static IModLibScreenOverrider? _modLibScreenOverrider;
        public static IModLibScreenOverrider ModLibScreenOverrider => _modLibScreenOverrider
            ??= ReflectionUtils.GetImplementation<IModLibScreenOverrider, ModLibScreenOverriderWrapper>(ApplicationVersionParser.GameVersion());
    }
}