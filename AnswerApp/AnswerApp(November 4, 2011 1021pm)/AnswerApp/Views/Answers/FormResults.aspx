<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    FormResults
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>FormResults</h2>

    <p>
    Your favorite color: <b><%= Html.Encode(ViewData["PracticeProblemAnswer"]) %></b>
    </p>

</asp:Content>
