#pragma checksum "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7973db4587877d6fa7b767b81c247d5387d47273"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_OrderItem_Submit), @"mvc.1.0.view", @"/Views/OrderItem/Submit.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\_ViewImports.cshtml"
using StoreApp.WebApp;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\_ViewImports.cshtml"
using StoreApp.WebApp.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7973db4587877d6fa7b767b81c247d5387d47273", @"/Views/OrderItem/Submit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c8a01c39c5b68c58f4450ee717299453051b35f3", @"/Views/_ViewImports.cshtml")]
    public class Views_OrderItem_Submit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<StoreApp.WebApp.OrderItemViewModel>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
  
    ViewData["Title"] = "Submit";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h4>order complete!</h4>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 12 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
           Write(Html.DisplayNameFor(model => model.Product.ProductName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 15 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
           Write(Html.DisplayNameFor(model => model.Product.ProductPrice));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 18 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
           Write(Html.DisplayNameFor(model => model.Quantity));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 23 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
#nullable restore
#line 26 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
           Write(Html.DisplayFor(modelItem => item.Product.ProductName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 29 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
           Write(Html.DisplayFor(modelItem => item.Product.ProductPrice));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 32 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
           Write(Html.DisplayFor(modelItem => item.Quantity));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 35 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n<h6>total</h6>\r\n<p>");
#nullable restore
#line 39 "C:\Users\dariu\042020-dotnet-uta\dariusVallejo-repo1\Projects\StoreApp\StoreApp.WebApp\Views\OrderItem\Submit.cshtml"
Write(ViewData["Total"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7973db4587877d6fa7b767b81c247d5387d472737273", async() => {
                WriteLiteral("\r\n        <i class=\"material-icons\">subdirectory_arrow_left</i>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<StoreApp.WebApp.OrderItemViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
