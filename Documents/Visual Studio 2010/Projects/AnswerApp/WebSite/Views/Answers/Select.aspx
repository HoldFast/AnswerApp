<%@ Page Title="Select" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.SelectModel>" %>

<script runat="server">

    protected void QuestionsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        // Get selected values
        string Unit = UnitsList.SelectedItem.Text;
        string Chapter = ChaptersList.SelectedItem.Text;
        string Section = SectionsList.SelectedItem.Text;
        string Page = PagesList.SelectedItem.Text;
        string Question = QuestionsList.SelectedItem.Text;

        // Output result string based on which values are specified
        if (string.IsNullOrEmpty(Unit))
        {
            Label1.Text = "Please select a unit.";
        }
        else if (string.IsNullOrEmpty(Chapter))
        {
            Label1.Text = "Please select a chapter.";
        }
        else if (string.IsNullOrEmpty(Section))
        {
            Label1.Text = "Please select a section.";
        }
        else
        {
            Label1.Text = string.Format("You have chosen a {0} {1} {2}. Nice car!", Unit, Chapter, Section);
        }
        
    }
</script>
<asp:Content ID="SelectTitle" ContentPlaceHolderID="TitleContent" runat="server">
    
</asp:Content>

<asp:Content ID="SelectContent" ContentPlaceHolderID="MainContent" runat="server">


    <h2><%: Model.Textbook %></h2>
            
    <form id="form1" runat="server">

    <asp:ScriptManager ID="asm" runat="server" />
        <%if (Request.IsAuthenticated){%>
        <% using (Html.BeginForm()) { %>
            <div>
                <fieldset>
                    <legend>Selection a Question</legend>
                    <table>
                            <%= Html.Hidden("Textbook", Model.Textbook) %>
                            <%= Html.Hidden("PracticeProblemAnswer", Model.PracticeProblemAnswer)%>
                        <tr>
                            <td>
                                <label for="Unit">Unit:</label>
                            </td>
                            <td>
                                <%= Html.DropDownList("Unit", ViewBag.UnitDropDownList as SelectList)%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="Chapter">Chapter:</label>
                            </td>
                            <td>
                                <%= Html.DropDownList("Chapter", ViewBag.ChapterDropDownList as SelectList)%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="Section">Section:</label>
                            </td>
                            <td>
                                <%= Html.DropDownList("Section", ViewBag.SectionDropDownList as SelectList)%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="Page">Page:</label>
                            </td>
                            <td>
                                <%= Html.DropDownList("Page", ViewBag.PageDropDownList as SelectList)%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="Question">Question:</label>
                            </td>
                            <td>
                                <%= Html.DropDownList("Question", ViewBag.QuestionDropDownList as SelectList)%>
                            </td>
                        </tr>
                    </table>
                <input type="submit" value="Select" />
                </fieldset>
            </div>
        <% } %>
    <% } %>
    </form>
</asp:Content>
