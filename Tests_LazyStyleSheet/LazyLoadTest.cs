using Bunit;
using Bunit.JSInterop;
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
            IRenderedComponent<Stylesheet>? cut = null;
            JSInterop.SetupVoid("eval", (i) => true);
            Assert.DoesNotThrow(() => cut = RenderComponent<Stylesheet>((name: "Src", value: "hello.css")));
            CollectionAssert.IsNotEmpty(JSInterop.Invocations);
            var verification = JSInterop.VerifyInvoke("eval");
            Assert.IsNotNull(verification);
            CollectionAssert.IsNotEmpty(verification.Arguments);
            Assert.IsTrue(verification.Arguments.First().ToString()!.Contains("hello.css"));
        }
    }
}