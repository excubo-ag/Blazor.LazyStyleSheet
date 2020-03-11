using Excubo.Blazor.LazyStyleSheet;
using NUnit.Framework;

namespace Excubo.Blazor.Tests_LazyStyleSheet
{
    public class StyleSheetServiceTests
    {
        [Test]
        public void Add()
        {
            var style_sheet_service = new StyleSheetService();
            Assert.AreEqual(0, style_sheet_service.RequiredStyleSheets.Count);
            Assert.DoesNotThrow(() => style_sheet_service.Add("test"));
            Assert.AreEqual(1, style_sheet_service.RequiredStyleSheets.Count);
            Assert.AreEqual("test", style_sheet_service.RequiredStyleSheets[0]);
            Assert.DoesNotThrow(() => style_sheet_service.Add("test"));
            Assert.AreEqual(1, style_sheet_service.RequiredStyleSheets.Count);
            Assert.AreEqual("test", style_sheet_service.RequiredStyleSheets[0]);
        }
    }
}