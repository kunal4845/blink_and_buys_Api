#pragma checksum "D:\blink_and_buys_Api\BlinkAndBuys\Templates\ResetPassword.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ee7ca0d6f0f5dfd3d694be00bafc5a802b76309c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Templates_ResetPassword), @"mvc.1.0.view", @"/Templates/ResetPassword.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Templates/ResetPassword.cshtml", typeof(AspNetCore.Templates_ResetPassword))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ee7ca0d6f0f5dfd3d694be00bafc5a802b76309c", @"/Templates/ResetPassword.cshtml")]
    public class Templates_ResetPassword : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("background-color: #e9ecef;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 25, true);
            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n");
            EndContext();
            BeginContext(25, 2373, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "43b98800b8cc4ba6a65b2f5445a6adba", async() => {
                BeginContext(31, 339, true);
                WriteLiteral(@"

    <meta charset=""utf-8"">
    <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
    <title>Password Reset</title>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <style type=""text/css"">
  /**
   * Google webfonts. Recommended to include the .woff version for cross-client compatibility.
   */
");
                EndContext();
                BeginContext(1014, 1377, true);
                WriteLiteral(@"
  /**
   * Avoid browser level font resizing.
   * 1. Windows Mobile
   * 2. iOS / OSX
   */
  body,
  table,
  td,
  a {
    -ms-text-size-adjust: 100%; /* 1 */
    -webkit-text-size-adjust: 100%; /* 2 */
  }

  /**
   * Remove extra space added to tables and cells in Outlook.
   */
  table,
  td {
    mso-table-rspace: 0pt;
    mso-table-lspace: 0pt;
  }

  /**
   * Better fluid images in Internet Explorer.
   */
  img {
    -ms-interpolation-mode: bicubic;
  }

  /**
   * Remove blue links for iOS devices.
   */
  a[x-apple-data-detectors] {
    font-family: inherit !important;
    font-size: inherit !important;
    font-weight: inherit !important;
    line-height: inherit !important;
    color: inherit !important;
    text-decoration: none !important;
  }

  /**
   * Fix centering issues in Android 4.4.
   */
  div[style*=""margin: 16px 0;""] {
    margin: 0 !important;
  }

  body {
    width: 100% !important;
    height: 100% !important;
    padding: 0 ");
                WriteLiteral(@"!important;
    margin: 0 !important;
  }

  /**
   * Collapse table borders to avoid space between cells.
   */
  table {
    border-collapse: collapse !important;
  }

  a {
    color: #1a82e2;
  }

  img {
    height: auto;
    line-height: 100%;
    text-decoration: none;
    border: 0;
    outline: none;
  }
    </style>

");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2398, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(2400, 8260, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "b7941e6db4ef4e65977de2946ce4e0c1", async() => {
                BeginContext(2441, 8212, true);
                WriteLiteral(@"

    <!-- start preheader -->
    <div class=""preheader"" style=""display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;"">
        Tap the button below to reset your customer account password
    </div>
    <!-- end preheader -->
    <!-- start body -->
    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
        <!-- start logo -->
        <tr>
            <td align=""center"" bgcolor=""#e9ecef"">
                <!--[if (gte mso 9)|(IE)]>
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                <tr>
                <td align=""center"" valign=""top"" width=""600"">
                <![endif]-->
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                    <tr>
                        <td align=""center"" valign=""top"" style=""padding: 36px 24px;"">
                            <a href=""javascript:;"" target=""_bl");
                WriteLiteral(@"ank"" style=""display: inline-block;"">
                                <img src=""../Resources//Images//"" alt=""Logo"" border=""0"" width=""48"" style=""display: block; width: 48px; max-width: 48px; min-width: 48px;"">
                            </a>
                        </td>
                    </tr>
                </table>
                <!--[if (gte mso 9)|(IE)]>
                </td>
                </tr>
                </table>
                <![endif]-->
            </td>
        </tr>
        <!-- end logo -->
        <!-- start hero -->
        <tr>
            <td align=""center"" bgcolor=""#e9ecef"">
                <!--[if (gte mso 9)|(IE)]>
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                <tr>
                <td align=""center"" valign=""top"" width=""600"">
                <![endif]-->
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                    <tr>
         ");
                WriteLiteral(@"               <td align=""left"" bgcolor=""#ffffff"" style=""padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;"">
                            <h1 style=""margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;"">Reset Your Password</h1>
                        </td>
                    </tr>
                </table>
                <!--[if (gte mso 9)|(IE)]>
                </td>
                </tr>
                </table>
                <![endif]-->
            </td>
        </tr>
        <!-- end hero -->
        <!-- start copy block -->
        <tr>
            <td align=""center"" bgcolor=""#e9ecef"">
                <!--[if (gte mso 9)|(IE)]>
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                <tr>
                <td align=""center"" valign=""top"" width=""600"">
                <![endif]-->
                <table border=""0"" cellpadding=""0"" ce");
                WriteLiteral(@"llspacing=""0"" width=""100%"" style=""max-width: 600px;"">

                    <!-- start copy -->
                    <tr>
                        <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
                            <p style=""margin: 0;"">Tap the button below to reset your customer account password. If you didn't request a new password, you can safely delete this email.</p>
                        </td>
                    </tr>
                    <!-- end copy -->
                    <!-- start button -->
                    <tr>
                        <td align=""left"" bgcolor=""#ffffff"">
                            <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tr>
                                    <td align=""center"" bgcolor=""#ffffff"" style=""padding: 12px;"">
                                        <table border=""0"" cellpadding=""0""");
                WriteLiteral(@" cellspacing=""0"">
                                            <tr>
                                                <td align=""center"" bgcolor=""#1a82e2"" style=""border-radius: 6px;"">
                                                    <a href={{Url}} target=""_blank"" style=""display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;"">Reset Password</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- end button -->
                    <!-- start copy -->
                    <tr>
                        <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source San");
                WriteLiteral(@"s Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;"">
                            <p style=""margin: 0;"">If that doesn't work, copy and paste the following link in your browser:</p>
                            <p style=""margin: 0;""><a href={{Url}} target=""_blank"">{{Url}}</a></p>
                        </td>
                    </tr>
                    <!-- end copy -->
                    <!-- start copy -->
                    <tr>
                        <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf"">
                            <p style=""margin: 0;"">Cheers,<br> Blink & Buys</p>
                        </td>
                    </tr>
                    <!-- end copy -->

                </table>
                <!--[if (gte mso 9)|(IE)]>
                </td>
                </tr>
                </table>
                <![e");
                WriteLiteral(@"ndif]-->
            </td>
        </tr>
        <!-- end copy block -->
        <!-- start footer -->
        <tr>
            <td align=""center"" bgcolor=""#e9ecef"" style=""padding: 24px;"">
                <!--[if (gte mso 9)|(IE)]>
                <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                <tr>
                <td align=""center"" valign=""top"" width=""600"">
                <![endif]-->
                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">

                    <!-- start permission -->
                    <tr>
                        <td align=""center"" bgcolor=""#e9ecef"" style=""padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;"">
                            <p style=""margin: 0;"">You received this email because we received a request for Reset Password for your account. If you didn't request Reset Password you can safel");
                WriteLiteral(@"y delete this email.</p>
                        </td>
                    </tr>
                    <!-- end permission -->
                    <!-- start unsubscribe -->
                    <tr>
                        <td align=""center"" bgcolor=""#e9ecef"" style=""padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;"">
                            <p style=""margin: 0;"">To stop receiving these emails, you can <a href=""javascript:;"" target=""_blank"">Unsubscribe</a> at any time.</p>
                            <p style=""margin: 0;"">1234 S. Broadway St. City, State 12345</p>
                        </td>
                    </tr>
                    <!-- end unsubscribe -->

                </table>
                <!--[if (gte mso 9)|(IE)]>
                </td>
                </tr>
                </table>
                <![endif]-->
            </td>
        </tr>
        <!-- end footer -->

    </table>
    <");
                WriteLiteral("!-- end body -->\r\n\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(10660, 9, true);
            WriteLiteral("\r\n</html>");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
