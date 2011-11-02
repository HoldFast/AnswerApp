<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2></h2>
    <p>
        The Answer App is a web tool to help students find the assistance they need in order to excel in their academic pursuits.  The Answer App provides answers to text book problems on demand for students to use as reference when studying for exams or furthering their understanding of course material through practice problems.  
    </p>
    <p align="center">
        <img align="middle" src="http://web.me.com/hold_fast/Hold_Fast_Consultants/Home_files/shapeimage_2.png" />
    </p>
    <p>
        Please <%: Html.ActionLink("Register", "Register") %> if you don't already have an account.
    </p>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.") %>
        <table>
            <tr valign="bottom" align=left>
                <td>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.UserName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.TextBoxFor(m => m.UserName) %>
                        <%: Html.ValidationMessageFor(m => m.UserName) %>
                    </div>
                </td>
                <td>
                    <div class="editor-label">
                        <%: Html.LabelFor(m => m.Password) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.PasswordFor(m => m.Password) %>
                        <%: Html.ValidationMessageFor(m => m.Password) %>
                    </div>
                </td>
                <td>
                    <input type="submit" value="Log On" />
                </td>
                <td>
                    <div class="editor-label">
                        <%: Html.CheckBoxFor(m => m.RememberMe) %>
                        <%: Html.LabelFor(m => m.RememberMe) %>
                    </div>
                </td>
            </tr>
        </table>
    <% } %>
</asp:Content>
