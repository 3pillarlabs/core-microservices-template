'use strict';

var jsonPath = require('JSONPath').eval,
    url = require('url'),
    request = require('request'),
    config = require('./config'),
    DataObjectParser = require('dataobject-parser'),
    {defineSupportCode} = require('cucumber');    

defineSupportCode(function({Given,When,Then,Before,setWorldConstructor,registerHandler, After}){
    var parameters = {},
        headers={}, 
        befores =0,
        dParser = null;
     
    setWorldConstructor(require('../support/world.js').World);    
    Before(function(scenario, callback) {
     //console.log('Test number: (' + befores + ')');             
      befores++;
      parameters = {};
      headers = {};      
      callback();
      dParser = new DataObjectParser()
    });   

    Given(/^(body|query|path|header) parameter ([^"]*) = "([^"]*)"(?:.*)$/,function(parameterType, name, data, callback) {
        parameters[name] = {
            data : data?data.replace(/"/g,""):""
            ,type : parameterType           
        };        
        callback()            
    })
    
    When(/^I send a "([^"]*)" request on "([^"]*)"(?:.*)$/, 
        function(method,uri,callback){
        send.call(this, method,uri,callback);
        //console.log(requestBody)
    })

    var send = function(method, uri, callback) {       
       var requestBody={};  
       for(var name in parameters) {
            var parameter = parameters[name];
            var data = parameter.data;            
            switch(parameter.type) {
                case "path":
                    uri = uri.replace("{"+ name +"}", data);
                break;
                case "body":                    
                    dParser.set(name, data);
                    //requestBody[name] = data;
                break;
                case "query":                
                    var separator = uri.indexOf('?') !== -1 ? "&" : "?";    
                    uri = uri + separator + name + "=" + data;
                break;
                case "header":
                    headers[name] = parameter.data;                    
                break;
            }
       }
       switch(method.toLowerCase()){
            case "post":                                       
                //console.log(uri);
                this.post(uri, JSON.stringify(dParser.data()),headers, callback)                       
            break;
            case "delete":
                this.delete(uri, headers, callback)
            break;
            case "put":
                this.put(uri,JSON.stringify(dParser.data()), headers, callback)
            break;
            case "get":                        
                this.get(uri,headers, callback);
            break;
        }               
    }

    
    Given(/^I have an authorized key$/,function(callback){
        headers["apikey"] = config.API_KEY
        callback(); 
    })    


    When(/^I GET the service root$/,
        function (callback) {
            this.get('/v1', headers, callback)
        })

    When(/^I GET the version$/,
        function (callback) {
            this.get("version/", headers, callback)
        })

    When(/^I GET the employees list "([^"]*)"$/,
        function (invoiceId, callback) {
            var url = "v1/" ;
            this.get(url,headers, callback)
        })

    When(/^I GET the employees detail "([^"]*)"$/,
        function (id, callback) {
            var url = "v1/" + id;
            this.get(url, headers, callback)
        })
  

    //=== end user =====
    When(/^I GET a non-existing resource$/, function (callback) {
        this.get('/does/not/exist',headers, callback)
    })


     Then(/^the http status should be (\d+)$/, function (status, callback) {
        if (!assertResponse(this.lastResponse, callback)) { return }
        // deliberately using != here (no need to cast integer/string)
        /* jshint -W116 */
        if (this.lastResponse.statusCode != status) {
            /* jshint +W116 */
            callback('The last http response did not have the expected ' +
                'status, expected ' + status + ' but got ' +
                this.lastResponse.statusCode)
        } else {
            callback()
        }
    })
     Then(/^(?:the )?([\w_.$\[\]]+) should equal to null$/, function (key, callback) {
         if (!assertPropertyIsNotNull(this.lastResponse, key, callback)) {
             //callback()
             //return
         }
         callback()
     });
    // Check if a certain property of the response is equal to something
    Then(/^(?:the )?([\w_.$\[\]]+) should equal "([^"]+)"$/,
        function (key, expectedValue, callback) {
            if (!assertPropertyIs(this.lastResponse, key, expectedValue, callback)) {
                //return
            }
            callback()
        })
    Then(/^(?:the )?([\w_.$\[\]]+) should not equal to null$/, function (key,callback) {
        if (!assertPropertyIsNotNull(this.lastResponse, key, callback)) {
            //callback()
            //return
        }
        callback()
    });
    // Check if a substring is contained in a certain property of the response
   Then(/^I should see "([^"]+)" in the (\w+)$/, function (callback)
        {
            callback()
        })
    
    function assertResponse(lastResponse, callback) {
        if (!lastResponse) {
            callback('No request has been made until now.')
            return false
        }
        return true
    }

    function assertBody(lastResponse, callback) {
        if (!assertResponse(lastResponse, callback)) { return false }
        if (!lastResponse.body) {
            callback('The response to the last request had no body.')
            return null
        }
        return lastResponse.body
    }

    function assertValidJson(lastResponse, callback) {
        var body = assertBody(lastResponse, callback)
        if (!body) {
            return null
        }
        try {
            return JSON.parse(body)
        } catch (e) {
            callback(
                'The body of the last response was not valid JSON.')
            return null
        }
    }

    function assertPropertyExists(lastResponse, key, expectedValue,
        callback) {
        var object = assertValidJson(lastResponse, callback)
        if (!object) { return null }
        var property
        if (key.indexOf('$.') !== 0 && key.indexOf('$[') !== 0) {
            // normal property
            property = object[key]
        } else {
            // JSONPath expression
            var matches = jsonPath(object, key)
            if (matches.length === 0) {
                // no match
                callback('The last response did not have the property: ' +
                    key + '\nExpected it to be\n' + expectedValue)
                return null
            } else if (matches.length > 1) {
                // ambigious match
                callback('JSONPath expression ' + key + ' returned more than ' +
                    'one match in object:\n' + JSON.stringify(object))
                return null
            } else {
                // exactly one match, good
                property = matches[0]
            }
        }
        if (property == null) {
            callback('The last response did not have the property ' +
                key + '\nExpected it to be\n' + expectedValue)
            return null
        }
        return property
    }

    function assertPropertyIs(lastResponse, key, expectedValue, callback) {

        var value = assertPropertyExists(lastResponse, key, expectedValue, callback)
        if (value == null) { return false; } // success false
        if (value.toString() !== expectedValue) {

            callback('The last response did not have the expected content in ' +
                'property ' + key + '. ' + 'Got:\n\n' + value + '\n\nExpected:\n\n' +
                expectedValue)

            return false
        }
        return true
    }
    function assertPropertyIsNotNull(lastResponse, key, callback) {

        var value = assertPropertyExists(lastResponse, key, null, callback)
        if (value && value.length) {
            return true
        }
        return false
    }

    function assertPropertyContains(lastResponse, key, expectedValue, callback) {
        var value = assertPropertyExists(lastResponse, key, expectedValue, callback)
        if (!value) { return false }
        if (value.indexOf(expectedValue) === -1) {
            callback('The last response did not have the expected content in ' +
                'property ' + key + '. ' +
                'Got:\n\n' + value + '\n\nExpected it to contain:\n\n' + expectedValue)
            return false
        }
        return true
    }
});