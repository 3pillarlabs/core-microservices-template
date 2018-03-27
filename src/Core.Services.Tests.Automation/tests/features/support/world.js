'use strict';

var request = require('request')

var config = require('../step_definitions/config')

var World = function World(callback) {
  var self = this
  this.defaultHeaders = {'User-Agent': 'request','Content-Type': 'application/json; charset=utf-8'};
  this.lastResponse = null

  this.get = function(path, headers, callback) {
    var uri = this.uri(path)
    console.log(uri);
    request.get({url: uri, headers: this.addDefaultHeaders(headers)},
    function (error, response) {
      console.log("response "+  response);
        if (error || !('' + response.statusCode).match(/^2\d\d$/)) {         
          callback('Error on GET request to ' + uri +
            ': ' + (error!=null?error.message: response.body))
            return 
        }
                
        self.lastResponse = response
        callback()
     })
  }

  this.post = function(path, requestBody, headers, callback) {
    var uri = this.uri(path)
     //console.log(uri);    
    request({url: uri, body: requestBody, method: 'POST',headers: this.addDefaultHeaders(headers)}
      ,function(error, response) {
          //console.log("response "+  response.body);
          if (error || !('' + response.statusCode).match(/^2\d\d$/)) {
            return callback('Error on POST request to ' + uri + ': ' +
            (error!=null?error.message: response.body))
        }
      self.lastResponse = response
      callback(null, self.lastResponse.headers.location)
    })
  }

  this.put = function(path, requestBody, headers, callback) {
    var uri = this.uri(path)
    request({url: uri, body: requestBody, method: 'PUT',
        headers: this.addDefaultHeaders(headers)},
        function(error, response) {
          if (error) {
            return callback('Error on PUT request to ' + uri + ': ' +
                error.message)
          }
          self.lastResponse = response
          callback(null, self.lastResponse.headers.locations)
    })
  }

  this.delete = function(path, headers, callback) {
    var uri = this.uri(path)
    request({ url: uri, method: 'DELETE',
        headers: this.addDefaultHeaders(headers)},
        function(error, response) {
      if (error) {
          return callback('Error on DELETE request to ' + uri + ': ' +
            error.message)
      }
      self.lastResponse = response
      callback()
    })
  }

  this.options = function(path, headers, callback) {
    var uri = this.uri(path)
    request({'uri': uri, method: 'OPTIONS',
          headers: this.addDefaultHeaders(headers)
        },
        function(error, response) {
      if (error) {
          return callback('Error on OPTIONS request to ' + uri +
            ': ' + error.message)
      }
      self.lastResponse = response
      callback()
    })
  }

  this.rootPath = function() {
    return '/'
  }

  this.uri = function(path) {
    return config.BASE_URL + path
  }
  
  this.addDefaultHeaders = function(reqHeaders){
    reqHeaders= reqHeaders || [];
    for(var h in this.defaultHeaders){
      reqHeaders[h] = this.defaultHeaders[h]
    }
    return reqHeaders;
  }
}

exports.World = World
