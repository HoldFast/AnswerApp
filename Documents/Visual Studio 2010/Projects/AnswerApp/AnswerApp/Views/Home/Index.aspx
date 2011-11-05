<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AnswerApp.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
    .meter-wrap{
    margin: 0 auto 1em auto;
        position: absolute;
    }

    .meter-wrap, .meter-value, .meter-text {
        /* The width and height of your image */
        width: 200px; height: 30px;
    }
                .meter-wrap, .meter-value{
                
                    background: #bdbdbd url(/wp-content/uploads/2009/06/meter-outline.png) top left no-repeat;
                }
            
    .meter-text {
        position: absolute;
        top:0; left:0;

        padding-top: 5px;
                
        color: #fff;
        text-align: center;
        width: 100%;
    }
    </style>

    <form id="form1" runat="server">
    <p>
        Welcome back <%: ViewBag.Username %>
    </p>
    <table width="200px">
        <tr>
            <%: Html.ActionLink("Mathematics 10", "Select/Mathematics 10", "Answers")%><br />
        </tr>
        <tr>
            <%: Html.ActionLink("Science 10", "ResourceUnavailable", "Home")%><br />
        </tr>
        <!--tr align="left">
            <div class="meter-wrap">
                <div class="meter-value" style="background-color: #4DA4F3; width: 40%;">
                    <div class="meter-text">
                        <%: Html.ActionLink("Mathematics 10", "Select/Mathematics 10", "Answers")%><br />
                    </div>
                </div>
            </div>
        </tr-->
    </table>
    </form>
</asp:Content>
