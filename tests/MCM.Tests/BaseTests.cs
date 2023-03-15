extern alias UI;
extern alias v5;

using NSubstitute;

using NUnit.Framework;

using System.IO;

using v5::BUTR.DependencyInjection;

using v5::MCM.Abstractions.FluentBuilder;
using v5::MCM.Abstractions.GameFeatures;
using v5::MCM.Abstractions.Properties;
using v5::MCM.Implementation;
using v5::MCM.Implementation.FluentBuilder;
using v5::MCM.LightInject;

namespace MCM.Tests
{
    public class BaseTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fileSystemProvider = Substitute.For<IFileSystemProvider>();
            fileSystemProvider.GetOrCreateFile(Arg.Any<GameDirectory>(), Arg.Any<string>()).Returns(x =>
            {
                var directory = x.ArgAt<GameDirectory>(0);
                var fileName = x.ArgAt<string>(1);
                using var _ = File.Create(Path.Combine(directory.Path, fileName));
                return new GameFile(directory, fileName);
            });
            fileSystemProvider.GetFile(Arg.Any<GameDirectory>(), Arg.Any<string>()).Returns(x =>
            {
                var directory = x.ArgAt<GameDirectory>(0);
                var fileName = x.ArgAt<string>(1);
                return new GameFile(directory, fileName);
            });
            fileSystemProvider.ReadData(Arg.Any<GameFile>()).Returns(x =>
            {
                var file = x.ArgAt<GameFile>(0);
                return File.ReadAllBytes(Path.Combine(file.Owner.Path, file.Name));
            });
            fileSystemProvider.WhenForAnyArgs(x => x.WriteData(Arg.Any<GameFile>(), Arg.Any<byte[]>())).Do(x =>
            {
                var file = x.ArgAt<GameFile>(0);
                var data = x.ArgAt<byte[]>(1);
                File.WriteAllBytes(Path.Combine(file.Owner.Path, file.Name), data);
            });
            
            var services = new ServiceContainer();
            services.Register<IFileSystemProvider>(_ => fileSystemProvider);
            services.Register<ISettingsBuilderFactory, DefaultSettingsBuilderFactory>();
            services.Register<ISettingsPropertyDiscoverer, AttributeSettingsPropertyDiscoverer>();
            services.Register<ISettingsPropertyDiscoverer, FluentSettingsPropertyDiscoverer>();
            GenericServiceProvider.GlobalServiceProvider = new LightInjectGenericServiceProvider(services);
        }
    }
}