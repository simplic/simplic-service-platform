using Moq;
using Simplic.ServicePlatform.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Simplic.ServicePlatform.Test
{
    public class ServiceDefinitionServiceTest
    {
        [Fact]
        public async Task GetInstances_ServiceName_ServiceNotFoundException()
        {
            var repository = new Mock<IServiceDefinitionRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(default(ServiceDefinition));

            var definitionService = new Mock<IModuleDefinitionService>().Object;

            var service = new ServiceDefinitionService(repository.Object, definitionService, new ServiceSession());

            await Assert.ThrowsAsync<ServiceConfigurationNotFoundException>(async () => await service.GetInstances("Some random name"));
        }

        [Fact]
        public async Task GetInstances_WrongModule_ModuleNotFoundException()
        {
            var repository = new Mock<IServiceDefinitionRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new ServiceDefinition
            {
                Modules = new List<ServiceModule>
                {
                    new ServiceModule { Name = "TEST" }
                }
            });

            var definitionService = new Mock<IModuleDefinitionService>();
            definitionService.Setup(x => x.GetAll()).ReturnsAsync(new List<ModuleDefinition>() { });

            var service = new ServiceDefinitionService(repository.Object, definitionService.Object, new ServiceSession());

            await Assert.ThrowsAsync<ModuleNotFoundException>(async () => await service.GetInstances("Some random name"));
        }

        [Fact]
        public async Task GetInstances_DisableAutoStart_CanNotStartRequiredModule()
        {
            var repository = new Mock<IServiceDefinitionRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new ServiceDefinition
            {
                Modules = new List<ServiceModule>
                {
                    new ServiceModule { Name = "module-2" }
                }
            });

            var definitionService = new Mock<IModuleDefinitionService>();
            definitionService.Setup(x => x.GetAll()).ReturnsAsync(new List<ModuleDefinition>()
            {
                new ModuleDefinition { Name = "module-1", EnableAutoStart = false },
                new ModuleDefinition { Name = "module-2", Requires = new[] { "module-1" } }
            });

            var service = new ServiceDefinitionService(repository.Object, definitionService.Object, new ServiceSession());

            await Assert.ThrowsAsync<CanNotStartRequiredModuleException>(async () => await service.GetInstances("Some random name"));
        }

        [Fact]
        public async Task GetInstances_EnableAutoStart_ModulesLoaded()
        {
            var repository = new Mock<IServiceDefinitionRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new ServiceDefinition
            {
                Modules = new List<ServiceModule>
                {
                    new ServiceModule { Name = "module-2" }
                }
            });

            var definitionService = new Mock<IModuleDefinitionService>();
            definitionService.Setup(x => x.GetAll()).ReturnsAsync(new List<ModuleDefinition>()
            {
                new ModuleDefinition { Name = "module-1", EnableAutoStart = true },
                new ModuleDefinition { Name = "module-2", Requires = new[] { "module-1" } }
            });

            var service = new ServiceDefinitionService(repository.Object, definitionService.Object, new ServiceSession());

            var session = await service.GetInstances("Some random name");

            Assert.Contains("module-1", session.Modules.Select(x => x.Name).ToList());
            Assert.Contains("module-2", session.Modules.Select(x => x.Name).ToList());
        }

        [Fact]
        public async Task GetInstances_Configuration_ConfigurationComposed()
        {
            var repository = new Mock<IServiceDefinitionRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new ServiceDefinition
            {
                Modules = new List<ServiceModule>
                {
                    new ServiceModule { Name = "module-1" },
                    new ServiceModule 
                    {
                        Name = "module-2",
                        Configuration = new List<ServiceModuleConfiguration>
                        { 
                            new ServiceModuleConfiguration { Name = "C-2", Value = "actual-c2" }
                        }
                    },
                    new ServiceModule 
                    {
                        Name = "module-3",
                        Configuration = new List<ServiceModuleConfiguration>
                        {
                            new ServiceModuleConfiguration { Name = "C-3", Value = "actual-c3" }
                        } 
                    }
                }
            });

            var definitionService = new Mock<IModuleDefinitionService>();
            definitionService.Setup(x => x.GetAll()).ReturnsAsync(new List<ModuleDefinition>()
            {
                new ModuleDefinition 
                {
                    Name = "module-1",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition>
                    { 
                        new ModuleConfigurationDefinition { Name = "C-1", Default = "default-c1" }
                    }
                },

                new ModuleDefinition 
                {
                    Name = "module-2",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition>
                    {
                        new ModuleConfigurationDefinition { Name = "C-2", Default = "default-c2" }
                    }
                },

                new ModuleDefinition
                {
                    Name = "module-3"
                }
            });

            var service = new ServiceDefinitionService(repository.Object, definitionService.Object, new ServiceSession());

            var session = await service.GetInstances("Some random name");

            var module1 = session.Modules.FirstOrDefault(x => x.Name == "module-1");
            Assert.Equal("C-1", module1.Configuration[0].Name);
            Assert.Equal("default-c1", module1.Configuration[0].Value);
            Assert.Equal(1, module1.Configuration.Count);

            var module2 = session.Modules.FirstOrDefault(x => x.Name == "module-2");
            Assert.Equal("C-2", module2.Configuration[0].Name);
            Assert.Equal("actual-c2", module2.Configuration[0].Value);
            Assert.Equal(1, module2.Configuration.Count);

            var module3 = session.Modules.FirstOrDefault(x => x.Name == "module-3");
            Assert.Equal("C-3", module3.Configuration[0].Name);
            Assert.Equal("actual-c3", module3.Configuration[0].Value);
            Assert.Equal(1, module3.Configuration.Count);
        }
    }
}
