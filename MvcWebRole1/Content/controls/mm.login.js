
/*------ Login ----------------------------------------*/
function authenticateUser() {
    var isValid = true;
    try {
        var username = $("#username").val();
        var loginPassword = $("#loginPassword").val();

        if (username == "") {
            $("#loginError").html("Email address require.");
            $("#loginError").show();
            $("#signin_email").focus();
            isValid = false;
            return;
        }

        if (loginPassword == "") {
            $("#loginError").html("Password require.");
            $("#loginError").show();
            $("#signin_password").focus();
            isValid = false;
            return;
        }
        //CallHandler("Login/UserLogin", encodeURI(JSON.stringify(hfLogin)));

        if (isValid) {
            var hflogin = ({ "UserName": username, "Email": username, "Password": loginPassword });
            // $("#hfLogin").val(JSON.stringify(hfLogin));

            $.ajax({
                url: BASE_URL + 'Login/UserLogin',
                data: { "hfLogin": JSON.stringify(hflogin) },
                type: 'Post',
                dataType: 'json',
                success: ShowSuccessMessageLogin,
                error: function (xhr, status, error) {
                }
            });
        }


    } catch (ex) {
        $("#loginError").html("There some error please try again.");
        $("#loginError").show();
    }


}

function ShowSuccessMessageLogin(result) {
    if (result.Status == "Ok") {
        //window.location = BASE_URL;
        $('#popup').modal('hide');
        $('#SignUp').modal('hide');
        $('#SignUp-Input').modal('hide');
    } else if (result.Status == "Require") {
        $("#loginError").html("Username and Password require.");
        $("#loginError").show();
        ClearLoginformData();
    } else if (result.Status == "Error") {
        $("#loginError").html("Login Failed, Try again.");
        $("#loginError").show();
    }
    else if (result.Status == "Invalid") {
        $("#loginError").html("Login Failed. Invalid username or password.");
        $("#loginError").show();
    }
}


function ClearLoginformData() {

    $("#signin_email").val("");
    $("#signin_password").val("");
    $("#loginError").hide("");
    //$("#Login"). data-dismiss("modal");
}


/*Register the user from popup  -------------------*/
function RegisterUser() {
    var isValid = true;
    try {
        var fname = $("#firstName").val();
        var lname = $("#lastName").val();
        var email = $("#emailAddress").val();
        var pwd = $("#userPassword").val();
        var confirmPassword = $("#confirmPassword").val();

        if (fname == "") {
            $("#registerError").html("Please provide First Name.");
            $("#registerError").show();
            $("#firstName").focus();
            isValid = false;
            return;
        }


        if (lname == "") {
            $("#registerError").html("Please provide Last Name.");
            $("#registerError").show();
            $("#lastName").focus();
            isValid = false;
            return;
        }

        if (email == "") {
            $("#registerError").html("Please provide email address.");
            $("#registerError").show();
            $("#emailAddress").focus();
            isValid = false;
            return;
        }

        if (pwd == "") {
            $("#registerError").html("Please provide password.");
            $("#registerError").show();
            $("#userPassword").focus();
            isValid = false;
            return;
        }

        if (confirmPassword == "") {
            $("#registerError").html("Please provide confirm password.");
            $("#registerError").show();
            $("#confirmPassword").focus();
            isValid = false;
            return;
        }

        if (!new Util().IsEmailValid(email)) {
            $("#registerError").html("Please provide valid email address.");
            $("#registerError").show();
            $("#emailAddress").focus();
            isValid = false;
            return;
        }

        if (pwd != confirmPassword) {
            $("#registerError").html("Password and confirm password does not match.");
            $("#registerError").show();
            $("#confirmPassword").focus();
            isValid = false;
            return;
        }

        if (isValid) {
            var user = {
                "FirstName": fname,
                "LastName": lname,
                "UserName": email,
                "Email": email,
                "Password": pwd,
                "Mobile": confirmPassword // mobile no use as confirm password
            };
            //$("#hfAffilations").val(JSON.stringify(user));
            $.ajax({

                url: 'Login/Register',
                data: { "userJson": JSON.stringify(user) },
                type: 'POST',
                dataType: 'json',
                success: ShowSuccessMessage,
                error: function (xhr, status, error) {
                }
            });
        }
    } catch (e) {
        $("#registerError").html("There some error please try again.");
        $("#registerError").show();
    }
}

function ShowSuccessMessage(result) {

    if (result.Status == "Ok") {
        $("#successStatusR").html("You are successfully register. Please login to access your account.");
        $("#successStatusR").show();
        ClearformData();
    } else if (result.Status == "Error") {
        if (result.Message != undefined) {
            $("#registerError").html(result.Message);
        }

        $("#registerError").show();
    }
    else {
        $("#registerError").html("Unable to register. Please try again after some time.");
        $("#registerError").show();
    }


}

function ClearformData() {
    $("#FirstName").val("");
    $("#LastName").val("");
    $("#Email1").val("");
    $("#password2").val("");
    $("#password3").val("");
    $("#registerError").hide("");
}

/* end Login*/