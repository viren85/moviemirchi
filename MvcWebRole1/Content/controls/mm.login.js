function AuthenticateUser() {
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

        if (isValid) {
            var hflogin = { "UserName": username, "Email": username, "Password": loginPassword };

            $("#popup-auth").removeAttr("onclick").html("Login&nbsp;<img src=\"images/Loading.gif\" width=\"10px\" height=\"10px\" />");

            $.ajax({
                url: WEB_BASE_URL + '/Login/UserLogin',
                data: hflogin,
                type: 'Post',
                dataType: 'text',
                success: ShowSuccessMessageLogin,
                error: function (xhr, status, error) {
                    console.log(error);
                    $("#popup-auth").attr("onclick", "AuthenticateUser();").html("Login");
                }
            });
        }
    } catch (ex) {
        $("#loginError").html("There some error please try again.");
        $("#loginError").show();
        $("#popup-auth").attr("onclick", "AuthenticateUser();").html("Login");
    }
}

function ShowSuccessMessageLogin(result) {

    if (result == "OK") {
        $('#Login').modal('hide');
        $('#SignUp').modal('hide');
        $('#SignUp-Input').modal('hide');
        ClearLoginformData();
    } else if (result == "MISSING") {
        $("#loginError").html("Username and Password are mandatory.");
        $("#loginError").show();
        ClearLoginformData();
    } else if (result == "ERROR") {
        $("#loginError").html("Error occurred.");
        $("#loginError").show();
    }
    else if (result == "INVALID") {
        $("#loginError").html("Invalid username or password.");
        $("#loginError").show();
    }

    $("#popup-auth").attr("onclick", "AuthenticateUser();").html("Login");
    $("#LoginLink").hide();
    $("#LogoutLink").show();
}

function ClearLoginformData() {
    $("#username").val("");
    $("#loginPassword").val("");
    $("#loginError").hide().html("");
}

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
                "UserId": email,
                "Email": email,
                "Password": pwd,
                "Mobile": confirmPassword // mobile no use as confirm password
            };
            
            $("#btnRegister").removeAttr("onclick").html("Register&nbsp;<img src=\"images/Loading.gif\" width=\"10px\" height=\"10px\" />");

            $.ajax({
                url: BASE_URL + '/api/UserSignup/Register',
                data: user,
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
    $("#btnRegister").attr("onclick", "RegisterUser();").html("Register");

    if (result == "OK") {
        $("#successStatusR").html("You are successfully register. Please login to access your account.");
        $("#successStatusR").show();
        ClearformData();
    } else if (result == "ERROR") {
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
    $("#firstName").val("");
    $("#lastName").val("");
    $("#emailAddress").val("");
    $("#userPassword").val("");
    $("#confirmPassword").val("");
    $("#registerError").html().hide();
}

function DoLogout() {
    $("#LogoutLink").removeAttr("onclick").html("Sign Out&nbsp;<img src=\"images/Loading.gif\" width=\"10px\" height=\"10px\" />");
    $.ajax({
        url: WEB_BASE_URL + '/Login/Logout',
        type: 'get',
        success: ShowSuccessMessageLogout,
        error: function (xhr, status, error) {

        }
    });
}

function ShowSuccessMessageLogout() {
    $("#LoginLink").show();
    $("#LogoutLink").attr("onclick", "DoLogout();").html("Sign Out").hide();
    ClearLoginformData();
}