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
    <label for="file">Filename:</label>
    <input type="file" name="file" id="file" />
 
    <input type="submit" name="submit" value="Submit" />
<ul>
<% foreach (AnswerApp.Models.ViewDataUploadFilesResult v in this.ViewData.Model)  { %>
       <%=String.Format("<li>Uploaded: {0} totalling {1} bytes.</li> {2}",v.Name,v.Length, this.ViewData.Model.ToString()) %>
<%   } %>   

<%=ViewBag.RetrievedAnswer %>
</ul>
</form>


</asp:Content>
