<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AnswerApp.Models.ViewDataUploadFilesResult>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ViewAnswer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Upload</h2>

    <img alt="<%=ViewData["FileName"] %>" 
    longdesc="This is a test image for demonstration.  Normally the selected answer would be displayed here." 
    src="/Answers/GetPdf" style="width: 436px; height: 232px" />

    <embed src="/Answers/GetPdf" width="100%" height="1470px">&nbsp;
    
    <form action="/Answers/Upload" method="post" enctype="multipart/form-data">

    <label for="file1">Answer:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
    <input type="file" name="file1" id="file1" />

    <br />

    <label for="file2">Practice Problem:</label>
    <input type="file" name="file2" id="file2" />

    <br />
    <label for="file3">Batch:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
    <input type="file" name="file3" id="file3" />

    <br />

    <%= Html.RadioButton("PracticeProblemAnswer", "A", false)%> A <br />
    <%= Html.RadioButton("PracticeProblemAnswer", "B", false)%> B <br />
    <%= Html.RadioButton("PracticeProblemAnswer", "C", false)%> C <br />
    <%= Html.RadioButton("PracticeProblemAnswer", "D", false)%> D <br />
    <%= Html.RadioButton("PracticeProblemAnswer", "E", false)%> E None of the Above <br />

    <input type="submit" name="submit" value="Submit" />
<ul>
<% foreach (AnswerApp.Models.ViewDataUploadFilesResult v in this.ViewData.Model)  { %>
       <%=String.Format("<li>Uploaded: {0} totalling {1} bytes.</li>",v.Name,v.Length) %>
<%   } %>   

<%//=ViewBag.RetrievedAnswer %>
<%//=ViewData["Test"] %>
</ul>
</form>


</asp:Content>
