var app = angular.module("VotrApp", []);

app.controller("PollCtrl", function () {

    var self = this;

    self.showPopup = function () {
        window.alert();
    }
    self.Options = [];

    self.AddOption = function (e) {
        // Get value from input field
        var input_val = $("#firstoption").val();
        var incrementer = 1;
        self.Options.push(input_val);
        $("#firstoption").val("");
        // Then append it onto the Options object
        e.preventDefault();

    }
});