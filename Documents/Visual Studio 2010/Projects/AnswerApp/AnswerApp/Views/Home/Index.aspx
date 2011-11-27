<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
        <p>
            Welcome back <%: ViewBag.Username %>
        </p>
        <table width="200px">
            <tr>
                <%: Html.ActionLink("Mathematics 10", "Select/Mathematics 10", "Answers")%><br />
            </tr>
            <tr>
                <%: Html.ActionLink("Science 10", "ResourceUnavailable", "Home")%><br />
            </tr>

            <asp:ScriptManager ID="asm" runat="server" />

        </table>
    </form>

</asp:Content>
