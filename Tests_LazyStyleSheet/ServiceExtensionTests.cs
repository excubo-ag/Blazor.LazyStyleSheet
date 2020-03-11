using Excubo.Blazor.LazyStyleSheet;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Excubo.Blazor.Tests_LazyStyleSheet
{
    public class ServiceExtensionTests
    {
        [Test]
        public void Equal()
        {
            var services = new Mock<IServiceCollection>()
                .Object;
            Assert.AreEqual(services, services.AddStyleSheetLazyLoading());
        }
    }
}