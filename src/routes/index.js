var express = require('express');
var router = express.Router();
var request = require('request');
//request.debug = true;
var config = require('../config');

/* GET home page. */
router.get("/meetup/2/:path", function(req, response) {
    console.log(req.query);
    var path = "https://api.meetup.com/2/" + req.params.path + "?key=" + config.meetup.apiKey;
    Object.keys(req.query).forEach(function(p){
        path += "&" + p + "=" + req.query[p];
    });
    console.log(path);
    request(path).pipe(response);
});
module.exports = router;
