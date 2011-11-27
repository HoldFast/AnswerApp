<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.SelectModel>" %>

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
                        <!--tr>
                            <td>
                                <label for="Textbook">Textbook:</label>
                            </td>
                            <td-->
                                <%//= Html.DropDownList("Textbook", ViewBag.TextbookDropDownList as SelectList)%>
                                <%= Html.Hidden("Textbook", Model.Textbook) %>
                                <%= Html.Hidden("PracticeProblemAnswer", Model.PracticeProblemAnswer)%>
                            <!--/td>
                        </tr-->
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

            <div> 
                Vendor: <asp:DropDownList ID="VendorsList" runat="server"/><br /> 
                Contacts: <asp:DropDownList ID="ContactsList" runat="server"/><br /> 
            </div>

            <ajaxToolkit:CascadingDropDown ID="ccd1" runat="server" ServicePath="~/SelectionWebService.asmx" ServiceMethod="GetVendors" TargetControlID="VendorsList" Category="Vendor" PromptText="Select Vendor" />

            <ajaxToolkit:CascadingDropDown ID="ccd2" runat="server" ServicePath="~/SelectionWebService.asmx" ServiceMethod="GetContactsForVendor" TargetControlID="ContactsList" ParentControlID="VendorsList" Category="Contact" PromptText="Select Contact" />

        <% } %>
    <% } %>
    </form>
</asp:Content>
