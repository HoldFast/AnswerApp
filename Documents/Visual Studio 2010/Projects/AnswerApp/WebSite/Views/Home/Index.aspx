<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p>
            Welcome back <%: ViewBag.Username %>
    </p>

    <form id="Form1" runat="server">    
        <asp:ScriptManager ID="ScriptManager" 
                           runat="server" 
                           EnablePartialRendering="true" />

        <table width="600px">
            <tr>
                <td width="100px">
                    Textbook: </td><td width="500px"><asp:DropDownList ID="TextbooksList" runat="server" width="500px"/>
                </td>
            </tr>
            <tr>
                <td>
                    Unit: </td><td width="500px"><asp:DropDownList ID="UnitsList" runat="server" width="500px"/>
                </td>
            </tr>
                <td>
                    Chapter: </td><td width="500px"><asp:DropDownList ID="ChaptersList" runat="server" width="500px" />
                </td>
            <tr>
                <td>
                    Section: </td><td width="500px"><asp:DropDownList ID="SectionsList" runat="server" width="500px"/>
                </td>
            </tr>
            <tr>
                <td>
                    Page: </td><td width="500px"><asp:DropDownList ID="PagesList" runat="server" width="500px"/> 
                </td>
            </tr>
            <tr>
                <td>
                    Question: </td><td><asp:DropDownList ID="QuestionsList" runat="server"  width="500px"/>
                </td>
            </tr>
        </table>

        <asp:DropDownList ID="GetQuestions" runat="server" hidden="true"/>
            
        <input type="submit" value="Select" />
            
        <ajaxToolkit:CascadingDropDown ID="TextbooksByUserInput" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetTextbooks" 
            TargetControlID="TextbooksList" 
            Category="Textbook" 
            PromptText="Select Textbook" />

        <ajaxToolkit:CascadingDropDown ID="UnitsByTextbook" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetUnits" 
            TargetControlID="UnitsList" 
            ParentControlID="TextbooksList"
            Category="Unit" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="ChaptersByTextbook" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetChapters" 
            TargetControlID="ChaptersList" 
            ParentControlID="TextbooksList" 
            Category="Chapter" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SectionsByTextbok" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetSections" 
            TargetControlID="SectionsList" 
            ParentControlID="TextbooksList" 
            Category="Section" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="PagesByTextbook" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetPages" 
            TargetControlID="PagesList" 
            ParentControlID="TextbooksList" 
            Category="Page" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="QuestionsByTextbook" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetQuestions" 
            TargetControlID="QuestionsList" 
            ParentControlID="TextbooksList" 
            Category="Question" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="ChaptersByUnit" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetChapters" 
            TargetControlID="ChaptersList" 
            ParentControlID="UnitsList" 
            Category="Chapter" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SectionsByUnit" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetSections" 
            TargetControlID="SectionsList" 
            ParentControlID="UnitsList" 
            Category="Section" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="PagesByUnit" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetPages" 
            TargetControlID="PagesList" 
            ParentControlID="UnitsList" 
            Category="Page" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="QuestionsByUnit" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetQuestions" 
            TargetControlID="QuestionsList" 
            ParentControlID="UnitsList" 
            Category="Question" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SectionsByChapter" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetSections" 
            TargetControlID="SectionsList" 
            ParentControlID="ChaptersList" 
            Category="Section" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="PagesByChapter" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetPages" 
            TargetControlID="PagesList" 
            ParentControlID="ChaptersList" 
            Category="Page" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="QuestionsByChapter" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetQuestions" 
            TargetControlID="QuestionsList" 
            ParentControlID="ChaptersList" 
            Category="Question" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="PagesBySection" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetPages" 
            TargetControlID="PagesList" 
            ParentControlID="SectionsList" 
            Category="Page" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="QuestionsBySection" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetQuestions" 
            TargetControlID="QuestionsList" 
            ParentControlID="SectionsList" 
            Category="Question" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="QuestionsByPage" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="GetQuestions" 
            TargetControlID="QuestionsList" 
            ParentControlID="PagesList" 
            Category="Question" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SubmitByQuestion" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="SetQuestions" 
            TargetControlID="GetQuestions" 
            ParentControlID="QuestionsList" 
            Category="Question" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SubmitByPage" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="SetQuestions" 
            TargetControlID="GetQuestions" 
            ParentControlID="PagesList" 
            Category="Page" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SubmitBySection" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="SetQuestions" 
            TargetControlID="GetQuestions" 
            ParentControlID="SectionsList" 
            Category="Section" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SubmitByChapter" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="SetQuestions" 
            TargetControlID="GetQuestions" 
            ParentControlID="ChaptersList" 
            Category="Chapter" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SubmitByUnit" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="SetQuestions" 
            TargetControlID="GetQuestions" 
            ParentControlID="UnitsList" 
            Category="Unit" 
            PromptValue="1" />

        <ajaxToolkit:CascadingDropDown ID="SubmitByTextbook" 
            runat="server" 
            ServicePath="~/SelectionWebService.asmx" 
            ServiceMethod="SetQuestions" 
            TargetControlID="GetQuestions" 
            ParentControlID="TextbooksList" 
            Category="Textbook" 
            PromptValue="1" />

    </form>

</asp:Content>
