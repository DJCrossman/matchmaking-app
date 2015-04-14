(function ($) {
    var Workspace = Backbone.Router.extend({

        routes: {
            "network/:query": "network",  // #network/matches
            "team/:query": "team",     // #team/amount
        },

        network: function (query) {
            query = typeof (parseInt(query)) != "number" ? 30 : parseInt(query);
            var network = new NetworkView({
                matches: query
            });
        },

        team: function (query) {
            query = typeof (parseInt(query)) != "number" ? 8 : parseInt(query);
            var team = new TeamView({
                maxPlayers: query
            });
        }

    });

    var HostView = Backbone.View.extend({
        template: $("#hostTemplate").html(),

        render: function () {
            var tmpl = _.template(this.template);
            return $.parseHTML(tmpl(this.model.toJSON()));
        }
    });
    
    var NetworkView = Backbone.View.extend({
        el: $("#network-panel"),

        events: {
            "click #network-icon": "matchPlayers",
            "click #player-icon": "renderPlayers"
        },

        initialize: function () {
            var that = this;
            $("#team-panel").hide();
            this.$el.show();

            this.collection = new Hosts({
                matches: that.options.matches
            });
            this.update();
        },

        update: function () {
            var that = this;
            this.collection.fetch({
                success: function (data) {
                    that.render();
                }
            });
        },

        render: function () {
            var that = this;
            $("#network").html("");
            _.each(this.collection.models, function (item) {
                var ip = typeof (item.get("IP")[0]) == "undefined" ? "0.0.0.0" : item.get("IP")[0];
                item.set("Address", ip);
                item.set("PlayerCount", item.get("PlayersList").Count);
                that.renderHost(item);
            }, this);
        },

        renderHost: function (item) {
            var hostView = new HostView({
                model: item
            });
            var HostRoster = RosterView.extend({
                child: $("host-roster"),
                initialize: function (options) {
                    options || (options = {});
                    this.collection = new Players(item.get("PlayersList"));
                    this.update();
                }
            });
            $("#network").append(hostView.render());
        },

        matchPlayers: function () {
            var that = this;
            // start match
            $.ajax({
                method: "POST",
                url: "/host/match/"
            }).done(function() {
                that.update();
            });
        },

        renderPlayers: function () {
            //var that = this;
            //_.each(this.collection.models, function (model) {
            //    var id = model.get("Guid");
            //    var players = new Players();
            //    players.url = "/host/match/" + id;
            //    players.fetch({
            //        success: function (data) {
            //            var roster = new HostRoster(data);
            //            if (roster.collection.count > 0) {
            //                $("#" + id).append($("#hostPlayerTemplate"));
            //                roster.render();
            //            }
            //        }
            //    });
            //});
        }
    });

    var TeamView = Backbone.View.extend({
        el: $("#team-panel"),

        initialize: function () {
            $("#network-panel").hide();
            this.$el.show();
        }
    });

    //define individual contact view
    var PlayerView = Backbone.View.extend({
        tagName: "tr",

        className: "player-container",

        template: $("#playerTemplate").html(),

        render: function () {
            var tmpl = _.template(this.template);

            $(this.el).html(tmpl(this.model.toJSON()));
            return this;
        }
    });

    //define master view
    var RosterView = Backbone.View.extend({
        el: $("#roster-panel"),
        child: $("#roster"),

        events: {
            "click #refresh-icon": "update"
        },

        initialize: function (options) {
            options || (options = {});
            this.child = typeof (options.child) == "undefined" ? this.child : options.child;
            this.collection = new Players(options.collection);
            this.update();
        },

        update: function () {
            var that = this;
            this.collection.fetch({
                success: function (data) {
                    that.render();
                }
            });
        },

        render: function () {
            var that = this;
            this.child.html("");
            _.each(this.collection.models, function (item) {

                that.renderPlayer(item);
            }, this);
            return this;
        },

        renderPlayer: function (item) {
            var playerView = new PlayerView({
                model: item
            });
            this.child.append(playerView.render().el);
        }
    });



    //create instance of master view
    var roster = new RosterView();
    var workspace = new Workspace();
    Backbone.history.start();

}(jQuery));