"use strict";

const https = require('https');
const http = require('http');
const url = require('url');

const apiKey = process.env.MEETUP_API_KEY || "PLEASE SET YOUR API KEY";
const route = '/meetup/';

const server = http.createServer((req, res) => {

  const reqUrl = url.parse(req.url);
  const path = reqUrl.pathname

  if (path.indexOf(route) != 0) {
    res.statusCode = 404;
    res.end();
    return;
  }

  const search = reqUrl.search;
  const meetupPath = path.substr(route.length);
  const meetupUrl = "https://api.meetup.com/" + meetupPath + (search ? search + "&" : "?") + "key=" + apiKey;

  console.log(`Making request to ${meetupUrl}`);

  https
    .get(meetupUrl, apiRes => apiRes.pipe(res))
    .on('error', err => {
      console.error(err);

      res.statusCode = 500;
      res.write('Could not make meetup request');
      res.end();
    }).end();
});

server.listen(process.env.PORT || 3000);