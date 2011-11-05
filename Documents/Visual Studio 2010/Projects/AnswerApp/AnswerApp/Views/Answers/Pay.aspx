<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.SelectModel>" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Pay Us
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Purchase New Solution</h2>
    <p>
        You don't currently have access to this solution.  Would you like to purchase this solution?
        <br /><%//: Model.FileName %>
        <br />
        <form id="form1" runat="server">
        <input type="submit" value="Yes, I want to view this solution" />
        </form>
        <%//: Html.ActionLink("Yes, I want to view this solution", "ViewAnswer/" + Model.Textbook + "_" + Model.Unit + "_" + Model.Chapter + "_" + Model.Section + "_" + Model.Page + "_" + Model.Question, "Answers")%><br />
        <br /><%: Html.ActionLink("No, take me back to the selection page", "Select", "Answers")%><br />
    </p>
</asp:Content>
