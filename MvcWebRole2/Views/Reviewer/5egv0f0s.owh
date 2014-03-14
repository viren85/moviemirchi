﻿@{
    ViewBag.Title = "Affilation";
    Layout = "~/Views/Shared/_AdminLayout.cshtml";
}

<div class="panel panel-default">
    <div class="panel-heading">
        <h3 class="panel-title">Affilation</h3>
    </div>
    <div class="panel-body" id="affilationinfo">

        <div id="successStatus" style="display:none" class="alert alert-success"></div>
        <div id="affilationError" class="alert alert-danger" style="display:none;"></div>

        @using (Html.BeginForm())
        {
            <div class="row">
                <div class="col-sm-12">
                    <div id="affilation">

                        <input type="hidden" id="hfAffilation" value="1" style="display:none;" />
                        <div id="affilation_well_0" class="col-sm-12">

                            <input type="text" name="affilationName" id="affilationName_0" class="reviewInput" placeholder="Affilation Name" /><br />
                            @*<input type="button" class="btn btn-success" name="Add More" value="Add More" id="addMore" onclick="AddAffilationControl();" />*@
                            <input type="text" name="websiteName" id="websiteName_0" class="reviewInput" placeholder="Website Name" /><br />
                            <input type="text" name="websiteLink" id="websiteLink_0" class="reviewInput" placeholder="Website Link" /><br />
                            <input type="text" name="logoLink" id="logoLink_0" class="reviewInput" placeholder="Logo" /><br />
                            <input type="text" name="country" id="country_0" class="reviewInput" placeholder="Country" />

                        </div>
                    </div>
                </div>
            </div>
            <div>

                <input type="submit" name="Create" value="Create" id="create" class="btn btn-primary" onclick="insertAffilaion(); return false;" />
            </div>

            @Html.Hidden("hfAffilations", "")
        }
    </div>
</div>


<script type="text/javascript">


    /*
    $(document).ready(function () {
        $('form').on('submit', function (e) {
            e.preventDefault();
            var x = this;
            insertAffilaion();
            x.submit();
        });
    });

    function insertAffilaion() {
        //var isValid = true;

        var affilationName_0 = $("#affilationName_0").val();
        var websiteName_0 = $("#websiteName_0").val();
        var websiteLink_0 = $("#websiteLink_0").val();
        var logoLink_0 = $("#logoLink_0").val();
        var country_0 = $("#country_0").val();


        validate();


        var affilation = {
            "AffilationName": affilationName_0,
            "WebsiteName": websiteName_0,
            "WebsiteLink": websiteLink_0,
            "LogoLink": logoLink_0,
            "Country": country_0
        };

        alert(JSON.stringify(affilation));
        $("#hfAffilations").val(JSON.stringify(affilation));
        alert(JSON.stringify(affilation));
    }

    function validate() {


        if (affilationName_0 == "") {

            $("#affilationError").html("Please provide Affilation Name.");
            $("#affilationError").show();
            $("#affilationName_0").focus();

            return false;
        }

        if (websiteName_0 == "") {
            $("#affilationError").html("Please provide Website Name.");
            $("#affilationError").show();
            $("#websiteName_0").focus();
            return false;
        }

        if (websiteLink_0 == "") {
            $("#affilationError").html("Please provide Website Link.");
            $("#affilationError").show();
            $("#websiteLink_0").focus();
            return false;
        }

        if (logoLink_0 == "") {
            $("#affilationError").html("Please provide Logo Link.");
            $("#affilationError").show();
            $("#logoLink_0").focus();
            return false;

        }

        if (country_0 == "") {
            $("#affilationError").html("Please provide Country.");
            $("#affilationError").show();
            $("#country_0").focus();
            return false;

        }

        return false;
    }
    */


    function insertAffilaion() {
        var isValid = true;
        try {
            var affilationName_0 = $("#affilationName_0").val();
            var websiteName_0 = $("#websiteName_0").val();
            var websiteLink_0 = $("#websiteLink_0").val();
            var logoLink_0 = $("#logoLink_0").val();
            var country_0 = $("#country_0").val();

            if (affilationName_0 == "") {

                $("#affilationError").html("Please provide Affilation Name.");
                $("#affilationError").show();
                $("#affilationName_0").focus();
                isValid = false;
            }

            if (websiteName_0 == "") {
                $("#affilationError").html("Please provide Website Name.");
                $("#affilationError").show();
                $("#websiteName_0").focus();
                isValid = false;
            }

            if (websiteLink_0 == "") {
                $("#affilationError").html("Please provide Website Link.");
                $("#affilationError").show();
                $("#websiteLink_0").focus();
                isValid = false;
            }

            if (logoLink_0 == "") {
                $("#affilationError").html("Please provide Logo Link.");
                $("#affilationError").show();
                $("#logoLink_0").focus();
                isValid = false;

            }

            if (country_0 == "") {
                $("#affilationError").html("Please provide Country.");
                $("#affilationError").show();
                $("#country_0").focus();
                isValid = false;
            }


            if (isValid) {
                var affilation = {
                    "AffilationName": affilationName_0,
                    "WebsiteName": websiteName_0,
                    "WebsiteLink": websiteLink_0,
                    "LogoLink": logoLink_0,
                    "Country": country_0
                };

                alert(JSON.stringify(affilation));
                $("#hfAffilations").val(JSON.stringify(affilation));
                alert(JSON.stringify(affilation));

                $.ajax({
                    url: '@Url.Action("Affilation", "Admin")',
                    data: { "hfAffilations": JSON.stringify(affilation) },
                    type: 'POST',
                    dataType: 'json',
                    success: ShowSuccessMessage,
                    error: function (xhr, status, error) {
                    }
                });
            }
        } catch (e) {
            $("#affilationError").html("There some error please try again.");
            $("#affilationError").show();
        }



    }
    function ShowSuccessMessage(result) {
        if (result.Status == "Ok") {
            $("#successStatus").html("Affiiation details saved successfully");
            $("#successStatus").show();
            ClearformData();
           // $('cform')[0].reset();


            // ClearMoviesControl();
        } else if (result.Status == "Error") {
            $("#successStatus").removeAttr("class");
            $("#successStatus").attr("class", "alert alert-danger");
            $("#successStatus").html("Unable to save Affilation details. Please try again after some time.");
            $("#successStatus").show();
        }
    }


    function ClearformData() {
        $("#affilationName_0").val("");
        $("#websiteName_0").val("");
        $("#websiteLink_0").val("");
        $("#logoLink_0").val("");
        $("#country_0").val("");
        $("#affilationError").hide();
    }
        /*
        var counter = 0;

        $("#affilationError").html("");
        $("#affilationError").hide();

        if ($("#hfAffilation").val() != "") {
            counter = $("#hfAffilation").val();
        }

        for (var p = 0; p < counter; p++) {
            if ($("#affilation_well_0" + p) != undefined) {
                if (p == 0) {
                    $("#affilationName_0" + p).val("");
                    $("#websiteName_0" + p).val("");
                    $("#websiteLink_0" + p).val("");
                    $("#logoLink_0" + p).val("");
                    $("#country_0" + p).val("");
                } else {
                    $("#affilation_well_0" + p).remove();
                }
            }
        }
    }



    /*
    function clearValues() {
        if (affilationName_0 != "") {

            $("#affilationError").html(" ");
            $("#affilationError").show();
            $("#affilationName_0").focus();
        }

        if (websiteName_0 != "") {
            $("#affilationError").html("Please provide Website Name.");
            $("#affilationError").show();
            $("#websiteName_0").focus();

        }

        if (websiteLink_0 != "") {
            $("#affilationError").html("Please provide Website Link.");
            $("#affilationError").show();
            $("#websiteLink_0").focus();
        }

        if (logoLink_0 != "") {
            $("#affilationError").html("Please provide Logo Link.");
            $("#affilationError").show();
            $("#logoLink_0").focus();


        }

        if (country_0 != "") {
            $("#affilationError").html("Please provide Country.");
            $("#affilationError").show();
            $("#country_0").focus();

        }
    }*/

</script>