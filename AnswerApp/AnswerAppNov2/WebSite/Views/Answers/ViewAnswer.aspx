<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.SelectModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ViewAnswer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ViewAnswer</h2>
    
    <embed src="/Answers/GetPdf/<%="" + Page.User.Identity.Name + "_" + ViewData["FileName"]%>" width="100%" height="1470px">&nbsp;
    <%if (Request.IsAuthenticated){%>
    <br /><a href="/Answers/GetPdf/<%="" + Page.User.Identity.Name + "_" + ViewData["FileName"]%>"><%=ViewData["FileName"]%></a> </embed>
    <%}%>
    <br /><%: Html.ActionLink("Select another answer to view", "Select/" + Model.Textbook, "Answers")%>

    <form id="Form1" runat="server">
        <%= Html.Hidden("Textbook", Model.Textbook) %>
        <%= Html.Hidden("Textbook", Model.CorrectAnswer) %>
        <%= Html.Hidden("Textbook", Model.PracticeProblemAnswer) %>
        <br /><img src="<%=ViewData["FileNameExtensionless"]%>_Practice Problem.png" alt="<%=ViewData["FileNameExtensionless"]%>_Practice Problem.png" /><br />
        <%if (ViewData["RenderAnswer"].Equals("true"))
          { %>
        <%if(Model.CorrectAnswer.Equals(Model.PracticeProblemAnswer))
        { %>
            You answered correctly<br />
        <%}
        else
        { %>
            You answered incorrectly.  The correct Answer is <%=Model.CorrectAnswer %><br />
        <% } %>
            <%= Html.RadioButton("PracticeProblemAnswer", "A", Model.PracticeProblemAnswer.Equals("A"))%> A <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "B", Model.PracticeProblemAnswer.Equals("A"))%> B <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "C", Model.PracticeProblemAnswer.Equals("A"))%> C <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "D", Model.PracticeProblemAnswer.Equals("A"))%> D <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "E", Model.PracticeProblemAnswer.Equals("A"))%> E None of the Above <br />
        <%}
          else
          { %>
            <%= Html.RadioButton("PracticeProblemAnswer", "A", false)%> A <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "B", false)%> B <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "C", false)%> C <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "D", false)%> D <br />
            <%= Html.RadioButton("PracticeProblemAnswer", "E", false)%> E None of the Above <br />
        <%} %>
            <br /><br />
            <input type="submit" value="Submit" />
        
    </form>


</asp:Content>
