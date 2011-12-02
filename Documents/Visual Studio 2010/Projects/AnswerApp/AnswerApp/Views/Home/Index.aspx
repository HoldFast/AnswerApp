<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="asm" runat="server" />
        <p>
            Welcome back <%: ViewBag.Username %>
        </p>
        <fieldset>
            <div> 
                Textbook: <asp:DropDownList ID="TextbooksList" runat="server"/><br /> 
                Unit: <asp:DropDownList ID="UnitsList" runat="server"/><br /> 
                Chapter: <asp:DropDownList ID="ChaptersList" runat="server"/><br /> 
                Section: <asp:DropDownList ID="SectionsList" runat="server"/><br /> 
                Page: <asp:DropDownList ID="PagesList" runat="server"/><br /> 
                Question: <asp:DropDownList ID="QuestionsList" runat="server" /><br /> 
                <asp:DropDownList ID="GetQuestions" runat="server" hidden="true"/>
                <input type="submit" value="Select" />
            </div>

            <ajaxToolkit:CascadingDropDown ID="ccd1" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="GetTextbooks" 
                TargetControlID="TextbooksList" 
                Category="Textbook" 
                PromptText="Select Textbook" />

            <ajaxToolkit:CascadingDropDown ID="CascadingDropDown1" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="GetUnits" 
                TargetControlID="UnitsList" 
                ParentControlID="TextbooksList"
                Category="Unit" 
                PromptText="Select Unit" />

            <ajaxToolkit:CascadingDropDown ID="ccd2" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="GetChapters" 
                TargetControlID="ChaptersList" 
                ParentControlID="UnitsList" 
                Category="Chapter" 
                PromptText="Select Chapter" />

            <ajaxToolkit:CascadingDropDown ID="ccd3" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="GetSections" 
                TargetControlID="SectionsList" 
                ParentControlID="ChaptersList" 
                Category="Section" 
                PromptText="Select Section" />

            <ajaxToolkit:CascadingDropDown ID="ccd4" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="GetPages" 
                TargetControlID="PagesList" 
                ParentControlID="SectionsList" 
                Category="Page" 
                PromptText="Select Page" />

            <ajaxToolkit:CascadingDropDown ID="ccd5" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="GetQuestions" 
                TargetControlID="QuestionsList" 
                ParentControlID="PagesList" 
                Category="Question" 
                PromptText="Select Question" />

            <ajaxToolkit:CascadingDropDown ID="ccd6" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="SetQuestions" 
                TargetControlID="GetQuestions" 
                ParentControlID="QuestionsList" 
                Category="Final" 
                PromptText="Select" />
        </fieldset>

        <!--table width="200px">
            <tr>
                <%: Html.ActionLink("Mathematics 10", "Select/Mathematics 10", "Answers")%><br />
            </tr>
            <tr>
                <%: Html.ActionLink("Science 10", "ResourceUnavailable", "Home")%><br />
            </tr>

            
        </table-->
    </form>

</asp:Content>
