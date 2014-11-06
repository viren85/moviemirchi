var accessToken;

var GetFBLoginDialog = function () {
    var control =  "<div class=\"modal fade\" id=\"Login\" tabindex=\"-2\" role=\"dialog\" aria-labelledby=\"myModalLabel2\" aria-haspopup=\"false\">" +
                        "<div class=\"modal-dialog\" id=\"popup\">" +
                            "<div class=\"modal-content\">" +
                                "<div class=\"modal-header\" id=\"popup-header\">" +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">" +
                                        "&times;" +
                                    "</button>" +
                                    "<h3 class=\"panel-title\">" +
                                        "Sign in" +
                                    "</h3>" +
                                "</div>" +
                                "<div class=\"modal-body\">" +
                                    "<div class=\"Record\">" +
                                        "<div class=\"Column2\">" +
                                            "<div class=\"alert alert-danger\" id=\"loginError\" style=\"display: none\"></div>" +
                                         "</div>" +
                                    "</div>" +
                                    "<div id=\"fb-root\"></div>" +
                                    "<div class=\"Record\" style=\"margin-top: 20px\">" +
                                        "<fb:login-button show-faces=\"false\" size=\"large\" max-rows=\"1\" width=\"500\">" +
                                            "Login with Facebook" +
                                        "</fb:login-button>" +
                                    "</div>" +
                                    "<div class=\"Record\">"+
                                        "<div class=\"popup-or\">" +
                                            "Or" +
                                        "</div>" +
                                    "</div>" +
                                    "<form method=\"post\">" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"text\" id=\"username\" placeholder=\"Email Address\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"password\" id=\"loginPassword\" placeholder=\"Password\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\" style=\"text-align: left; margin-left: 15px;\">" +
                                            "<div class=\"Column2\">" +
                                                "<input id=\"remember_me2\" class=\"remember_me\" type=\"checkbox\" value=\"true\" name=\"remember_me\" />" +
                                                "<span>Remember me</span>" +
                                                "<span style=\"float: right; margin-top: 5px; margin-right: 30px; display: none;\">" +
                                                    "<a href=\"#\" class=\"forgot_password\">Forgot Password</a>" +
                                                "</span>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<div id=\"popup-auth\" class=\"btn btn-primary\" onclick=\"AuthenticateUser();\">Login</div>" +
                                             "</div>" +
                                        "</div>" +
                                    "</form>" +
                               "</div>" +
                               "<div class=\"modal-footer\" id=\"popup-footer\">" +
                                     "Don't have an account? <a data-toggle=\"modal\" href=\"#SignUp\" role=\"presentation\" data-dismiss=\"modal\">Sign Up</a>" +
                               "</div>"  +
                         "</div>" +
                     "</div>" +
                 "</div>";

    $('body').append(control);
}

window.fbAsyncInit = function () {
    FB.init({
        appId: fbAppId,
        status: true, 
        cookie: true, 
        xfbml: true  
    });

    FB.Event.subscribe('auth.authResponseChange', function (response) {
        if (response.status === 'connected') {
            var uid = response.authResponse.userID;
            var accessToken = response.authResponse.accessToken;
            
            GetFacebookUserFields(response);
        } else {
            GetFacebookUserFields(response);
        }
    });

    FB.login(function (response) {
        if (response.authResponse) {
            accessToken = "";
            accessToken = response.authResponse.accessToken;

            $(window).colorbox.close();

            if (response.status === 'connected') {
                loginStatus = true;
                USER_ID = response.id;
                $("#hfUserId").attr("value", USER_ID);
                FB.api('/me', function (response) {
                    userFields.UserId = response.id;
                    userFields.UserType = "facebook";
                    userFields.FirstName = response.first_name;
                    userFields.LastName = response.last_name;
                    userFields.Email = response.email;
                    userFields.Mobile = "";
                    userFields.DateOfBirth = response.birthday;
                    userFields.Gender = response.gender;
                    userFields.City = "";
                    userFields.Profile_Pic_Http = "http://graph.facebook.com/" + response.id + "/picture?type=large";
                    userFields.Profile_Pic_Https = "https://graph.facebook.com/" + response.id + "/picture?type=large";
                    userFields.Country = accessToken; // since we do not want to make major changes, posting access token in country field
                    connectUser();
                });
            }
        }
    }, { scope: 'email,user_about_me,user_birthday,user_website,publish_actions' });
};

function GetFacebookUserFields(response) {
    
    /*if (userFields.UserId == "")
        return;

    accessToken = "";*/
    accessToken = response.authResponse.accessToken;

    FB.api('/me', function (response) {
        userFields.UserId = response.id;
        userFields.UserType = "facebook";
        userFields.FirstName = response.first_name;
        userFields.LastName = response.last_name;
        userFields.Email = response.email;
        userFields.Mobile = "";
        userFields.DateOfBirth = response.birthday;
        userFields.Gender = response.gender;
        userFields.City = "";
        userFields.Profile_Pic_Http = "http://graph.facebook.com/" + response.id + "/picture?type=large";
        userFields.Profile_Pic_Https = "https://graph.facebook.com/" + response.id + "/picture?type=large";
        userFields.Country = accessToken; 
        connectUser();
    });
}

(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));
