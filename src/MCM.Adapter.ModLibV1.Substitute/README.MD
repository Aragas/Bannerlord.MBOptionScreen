Because mods implementing ModLib do not include ModLib, in case of user not installing ModLib, a fallback mechanism should exist that should still emulate ModLibs save/loading behaviour.
While this is not the best implementation, it should still be sufficient for now.
We need to keep everything made public here, because if modders decide to use some extensions, their lack will cause exceptions