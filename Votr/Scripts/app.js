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

app.controller("DetailsCtrl", function ($http) {

    var self = this;

    self.vote = {
        poll: $('#vote-poll').val(),
        user: $('#vote-user').val(),
        option: ''
    };

    self.CastVote = function (e) {
        alert(JSON.stringify(this.vote));
        e.preventDefault();
        /*
        $http({
            method: 'POST',
            url: '/api/Votes',

        });
        */
    }

});