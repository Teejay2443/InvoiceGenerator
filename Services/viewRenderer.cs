using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;
namespace InvoiceGenerator.Services
{


    public class ViewRenderer : IViewRenderer
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderer(ICompositeViewEngine viewEngine,
                            IServiceProvider serviceProvider,
                            ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderViewToStringAsync(ControllerContext context, string viewName, object model)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(context, viewName, false);
                if (!viewResult.Success)
                {
                    throw new InvalidOperationException($"Unable to find view '{viewName}'.");
                }

                var viewContext = new ViewContext(
                    context,
                    viewResult.View,
                    new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(context.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }

    public interface IViewRenderer
    {
        Task<string> RenderViewToStringAsync(ControllerContext context, string viewName, object model);
    }

}
