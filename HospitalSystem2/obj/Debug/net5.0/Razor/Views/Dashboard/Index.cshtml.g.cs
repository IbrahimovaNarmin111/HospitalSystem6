#pragma checksum "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e22de7c64a48505213f75281c89dadbf89c63415"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_Index), @"mvc.1.0.view", @"/Views/Dashboard/Index.cshtml")]
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
#line 1 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\_ViewImports.cshtml"
using HospitalSystem2;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\_ViewImports.cshtml"
using HospitalSystem2.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
using HospitalSystem2.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e22de7c64a48505213f75281c89dadbf89c63415", @"/Views/Dashboard/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ead0005717a99c4be1ea52c182c15d6922f1b166", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Dashboard_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DashboardVM>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
  
    List<Profit> todayMonthProfits = Model.Profits.Where(x => x.CreatedTime.Month == DateTime.Now.Month).ToList();
    List<Cost> nowMonthCosts = Model.Costs.Where(x => x.CreatedTime.Month == DateTime.Now.Month).ToList();
    List<Profit> todayProfits = Model.Profits.Where(x => x.CreatedTime.Day == DateTime.Now.Day).ToList();
    double total = 0;
    double totalProfits = 0;
    double totalMonth = 0;
    foreach (var item in todayMonthProfits)
    {
        total += item.Amount;
    }
    foreach (var item in nowMonthCosts)
    {
        totalMonth += item.Amount;
    }
    foreach (var item in todayProfits)
    {
        totalProfits += item.Amount;
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("<div ");
            WriteLiteral(" class=\"card\">\r\n    <div style=\"margin-inline: auto;margin-bottom:20px\">\r\n        <h3>Kassa</h3>\r\n    </div>\r\n");
#nullable restore
#line 27 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
     if (Model != null)
    {


#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"row\">\r\n            <div class=\"column\">\r\n\r\n\r\n                <p><strong>Son dəyişmə vaxtı:   </strong>");
#nullable restore
#line 34 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                                    Write(Model.Total.LastModifiedTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                <p style=\"min-inline-size: max-content;\"><strong>Son dəyişmə səbəbi:   </strong>");
#nullable restore
#line 35 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                                                                           Write(Model.Total.LastModifiedDescription);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                <p><strong>Qeyd edən şəxs:   </strong>");
#nullable restore
#line 36 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                                 Write(Model.Total.LastModifiedBy);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                <p><strong>Miqdar:   </strong>");
#nullable restore
#line 37 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                         Write(Model.Total.LastModifiedAmount);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Azn</p>\r\n\r\n            </div>\r\n\r\n            <div class=\"column\">\r\n\r\n                <div class=\"total\" style=\"display: block;margin-left:150px;width:max-content\">\r\n                    <p style=\"margin:5px;font-size:15px;\">Ümumi balans</p>\r\n");
            WriteLiteral("                    <h5 style=\"color: white;margin:5px;font-size:30px;display:contents\"><br>");
#nullable restore
#line 46 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                                                                       Write(Model.Total.TotalCash);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Azn</h5>\r\n                    <i style=\"margin-left:17px;color:white !important\" class=\"fa fa-chart-bar fa-3x text-primary\"></i>\r\n                </div>\r\n            </div>\r\n\r\n\r\n        </div>\r\n");
#nullable restore
#line 53 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
</div>
<div class=""container-fluid pt-4 px-4"" style=""    display: flex;
    justify-content: center;"">
    <div class=""row g-4"">
        <div class=""col-sm-6 col-xl-3"" style=""width:max-content"">
            <div class=""bg-light rounded d-flex align-items-center justify-content-between p-4"">
                <i class=""fa fa-chart-line fa-3x"" style=""color:#009CFF !important""></i>
                <div class=""ms-3"">
                    <p class=""mb-2"">Son 30 gün gəlir</p>
                    <h6 class=""mb-0"">");
#nullable restore
#line 64 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                Write(total);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" Azn</h6>
                </div>
            </div>
        </div>
        <div class=""col-sm-6 col-xl-3"" style=""width:max-content;"">
            <div class=""bg-light rounded d-flex align-items-center justify-content-between p-4"">
                <i class=""fa fa-chart-bar fa-3x"" style=""color:#009CFF !important""></i>
                <div class=""ms-3"">
                    <p class=""mb-2"">Son 30 gün xərc</p>
                    <h6 class=""mb-0"">");
#nullable restore
#line 73 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                Write(totalMonth);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" Azn</h6>
                </div>
            </div>
        </div>
        <div class=""col-sm-6 col-xl-3"" style=""width:max-content;"">
            <div class=""bg-light rounded d-flex align-items-center justify-content-between p-4"">
                <i class=""fa fa-chart-bar fa-3x"" style=""color:#009CFF !important""></i>
                <div class=""ms-3"">
                    <p class=""mb-2"">Son 1 gün gəlir</p>
                    <h6 class=""mb-0"">");
#nullable restore
#line 82 "C:\Users\hp\OneDrive\Desktop\HospitalSystem2\HospitalSystem2\Views\Dashboard\Index.cshtml"
                                Write(totalProfits);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" Azn</h6>
                </div>
            </div>
        </div>
    </div>
</div>
<style>
    .card {
        background-color: #f2f2f2;
        border-radius: 5px;
        padding: 50px;
        width: 300px;
        box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
        margin: auto;
        margin-top: 25px;
        min-inline-size: max-content;
    }

    .column {
        float: left;
        width: 50%;
        padding: 10px;
    }

    /* Clear floats after the columns */
    .row:after {
        content: """";
        display: table;
        clear: both;
    }

    .total {
        background-color: #009CFF;
        color: #fff;
        border-radius: 5px;
        padding: 5px;
        margin-top: 10px;
        display: flex;
        /*/justify-content: space-between;/*/
    }

</style>

");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DashboardVM> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
