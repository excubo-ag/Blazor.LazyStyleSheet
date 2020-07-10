using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Excubo.Blazor.Tests_LazyStyleSheet")]
namespace Excubo.Blazor.LazyStyleSheet
{
    public class Stylesheet : ComponentBase
    {
        [Parameter] public string Src { get; set; }
        [Inject] IJSRuntime js { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Src != null)
            {
                var condition = $"document.head.querySelector(`[src='{Src}']`) == null";
                var action = $"let s = document.createElement('link'); s.setAttribute('rel', 'stylesheet'); s.setAttribute('href', '{Src}'); document.head.appendChild(s);";
                await js.InvokeVoidAsync("eval", $"if ({condition}) {{ {action} }}");
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
