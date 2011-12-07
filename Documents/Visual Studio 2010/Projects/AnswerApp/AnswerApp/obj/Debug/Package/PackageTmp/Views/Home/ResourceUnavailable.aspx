<%@ Page Title="Resource Unavailable" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Resource Unavailable
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Resource Unavailable</h2>

    <p>
        The resource you requested is not yet available.  Please visit again at a later date.  
    </p>
    
<form runat="server">
<asp:ScriptManager ID="ScriptManager" 
                   runat="server" 
                   EnablePartialRendering="true" />
<asp:UpdatePanel ID="UpdatePanel1" 
                 UpdateMode="Conditional"
                 runat="server"
                 ChildrenAsTriggers="true">
    <ContentTemplate>   
        <asp:Button ID="Button1" 
                    Text="Refresh Panel" 
                    runat="server" 
                    hidden="true" />
    </ContentTemplate>
</asp:UpdatePanel>


<asp:Button ID="Button2" 
            Text="Refresh Panel"
            runat="server" />
<asp:UpdatePanel ID="UpdatePanel2" 
                 UpdateMode="Conditional"
                 runat="server"
                 border="0pt">
                 <ContentTemplate>
                 <fieldset style="border: none;height:100%">
                 <%=DateTime.Now.ToString() %>
                <Triggers runat="server">
                    <asp:AsyncPostBackTrigger ControlID="TextbooksList" EventName="SelectedIndexChanged"/>
                </Triggers>
                
                Textbook: <asp:DropDownList ID="TextbooksList" runat="server" /><br /> 
                Unit:     <asp:DropDownList ID="UnitsList" runat="server"/><br /> 
                Chapter:  <asp:DropDownList ID="ChaptersList" runat="server"/><br /> 
                Section:  <asp:DropDownList ID="SectionsList" runat="server"/><br /> 
                Page:     <asp:DropDownList ID="PagesList" runat="server"/><br /> 
                Question: <asp:DropDownList ID="QuestionsList" runat="server" /><br /> 
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
                Category="Final" 
                PromptValue="1" />

            <ajaxToolkit:CascadingDropDown ID="SubmitByPage" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="SetQuestions" 
                TargetControlID="GetQuestions" 
                ParentControlID="PagesList" 
                Category="Final" 
                PromptValue="1" />

            <ajaxToolkit:CascadingDropDown ID="SubmitBySection" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="SetQuestions" 
                TargetControlID="GetQuestions" 
                ParentControlID="SectionsList" 
                Category="Final" 
                PromptValue="1" />

            <ajaxToolkit:CascadingDropDown ID="SubmitByChapter" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="SetQuestions" 
                TargetControlID="GetQuestions" 
                ParentControlID="ChaptersList" 
                Category="Final" 
                PromptValue="1" />

            <ajaxToolkit:CascadingDropDown ID="SubmitByUnit" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="SetQuestions" 
                TargetControlID="GetQuestions" 
                ParentControlID="UnitsList" 
                Category="Final" 
                PromptValue="1" />

            <ajaxToolkit:CascadingDropDown ID="SubmitByTextbook" 
                runat="server" 
                ServicePath="~/SelectionWebService.asmx" 
                ServiceMethod="SetQuestions" 
                TargetControlID="GetQuestions" 
                ParentControlID="TextbooksList" 
                Category="Final" 
                PromptValue="1" />

        </fieldset>
                 </ContentTemplate>
</asp:UpdatePanel>
</form>





</asp:Content>
