using Bunit.Mocking.JSInterop;
using Excubo.Blazor.LazyStyleSheet;
using NUnit.Framework;
using System.Linq;

namespace Excubo.Blazor.Tests_LazyStyleSheet
{
    public class LazyLoadTest : Bunit.TestContext
    {
        [Test]
        public void Test()
        {
            var js = Services.AddMockJsRuntime();
            Bunit.IRenderedComponent<Stylesheet>? cut;
            Assert.DoesNotThrow(() => cut = RenderComponent<Stylesheet>((name: "Src", value: "hello.css")));
            Assert.IsTrue(js.VerifyInvoke("eval").Arguments.Single().ToString().Contains("hello.css"));
        }
    }
}
