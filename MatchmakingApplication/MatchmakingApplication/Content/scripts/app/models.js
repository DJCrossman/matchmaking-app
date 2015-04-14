//define Player model
var Player = Backbone.Model.extend();

//define Player collection
var Players = Backbone.Collection.extend({
    url: "/players/200"
});

//define Host model
var Host = Backbone.Model.extend();

//define Host collection
var Hosts = Backbone.Collection.extend({
    initialize: function (options) {
        options || (options = {});
        this.matches = options.matches;
    },
    url: function() {
        return "/hosts/" + this.matches;
    }
});