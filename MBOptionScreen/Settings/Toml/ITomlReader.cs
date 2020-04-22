using JetBrains.Annotations;
using Tomlyn.Syntax;

namespace CommunityPatch {

  public interface ITomlReader {

    KeyValueSyntax GetConfig([NotNull] string key);

    KeyValueSyntax GetOrCreateConfig([NotNull] string key);

    void DeleteConfig([NotNull] string key);

    void Set<T>(string key, T value);

    T Get<T>(string key);

  }

}