var GetFBSignupDialog = function () {
    var control = "<div class=\"modal fade\" id=\"SignUp\" tabindex=\"-2\" role=\"dialog\" aria-labelledby=\"myModalLabel2\" aria-haspopup=\"false\">" +
                        "<div class=\"modal-dialog\" id=\"popup\">" +
                            "<div class=\"modal-content\">" +
                                "<div class=\"modal-header\" id=\"popup-header\">" +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">" +
                                        "&times;" +
                                    "</button>" +
                                    "<h3 class=\"panel-title\">" +
                                        "Sign up" +
                                    "</h3>" +
                                "</div>" +
                                "<div class=\"modal-body\">" +
                                    "<div class=\"Record\">" +
                                        "<div class=\"ErrorContainer\" id=\"successStatus\" style=\"display: none\">" +
                                        "</div>" +
                                    "</div>" +
                                    "<div id=\"fb-root\"></div>" +
                                    "<div class=\"Record\" style=\"margin-top: 20px\">" +
                                        "<fb:login-button show-faces=\"false\" size=\"large\" max-rows=\"1\" width=\"500\">" +
                                            "Sign Up with Facebook" +
                                        "</fb:login-button>" +
                                    "</div>" +
                                    "<div class=\"Record\">" +
                                        "<div class=\"popup-or\">" +
                                            "Or" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"Record\">" +
                                        "<div class=\"Column2\">" +
                                            "<a data-toggle=\"modal\" href=\"#SignUp-Input\" role=\"presentation\" data-dismiss=\"modal\">Sign up with Email</a>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"modal-footer\" id=\"popup-footer\">" +
                                    "Already Movie Mirchi member? <a data-toggle=\"modal\" href=\"#Login\" role=\"presentation\" data-dismiss=\"modal\">Log in</a>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>";

    $('body').append(control);
}

var GetEmailSignupDialog = function () {
    var control = "<div class=\"modal fade\" id=\"SignUp-Input\" tabindex=\"-2\" role=\"dialog\" aria-labelledby=\"myModalLabel2\" aria-haspopup=\"false\">" +
                        "<div class=\"modal-dialog\" id=\"popup\">" +
                            "<div class=\"modal-content\">" +
                                "<div class=\"modal-header\" id=\"popup-header\">" +
                                    "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">" +
                                        "&times;" +
                                    "</button>" +
                                    "<h3 class=\"panel-title\">" +
                                        "Sign up" +
                                    "</h3>" +
                                "</div>" +
                                "<div class=\"modal-body\">" +
                                    "<div class=\"Record\">" +
                                        "<div class=\"Column2\">" +
                                            "<div class=\"alert alert-danger\" id=\"registerError\" style=\"display: none\"></div>" +
                                            "<div class=\"alert alert-success\" id=\"successStatusR\" style=\"display: none\"></div>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div id=\"fb-root\"></div>" +
                                        "<div class=\"Record\" style=\"margin-top: 20px\">" +
                                            "<fb:login-button show-faces=\"false\" size=\"large\" max-rows=\"1\" width=\"500\">" +
                                                "Sign up with Facebook" +
                                            "</fb:login-button>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"popup-or\">" +
                                                "Or" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"text\" id=\"firstName\" placeholder=\"First Name\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"text\" id=\"lastName\" placeholder=\"Last Name\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"text\" id=\"emailAddress\" placeholder=\"Email Address\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"password\" id=\"userPassword\" placeholder=\"Password\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<input type=\"password\" id=\"confirmPassword\" placeholder=\"Confirm Password\" class=\"popup-input\" />" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\" style=\"text-align: left; margin-left: 15px;\">" +
                                            "<div class=\"Column2\">" +
                                                "<input id=\"remember_me2\" class=\"remember_me\" type=\"checkbox\" value=\"true\" name=\"remember_me\" />" +
                                                "<span>" +
                                                    "Tell me about Movie Mirchi" +
                                                "</span>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"Record\">" +
                                            "<div class=\"Column2\">" +
                                                "<div class=\"btn btn-primary\" id=\"btnRegister\" onclick=\"RegisterUser();\">Register</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"modal-footer\" id=\"popup-footer\" style=\"margin-top: 10px\">" +
                                        "Already Movie Mirchi member? <a data-toggle=\"modal\" href=\"#Login\" role=\"presentation\" data-dismiss=\"modal\">Log in</a>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                    "</div>";

    $('body').append(control);
}