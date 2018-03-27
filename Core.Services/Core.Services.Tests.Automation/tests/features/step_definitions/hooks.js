var reporter = require('cucumber-html-reporter');

var cucumber_html_option = {
        theme: 'bootstrap',
        jsonFile: 'tests/report/cucumber_report.json',
        output: 'tests/report/cucumber_report.html',
        reportSuiteAsScenarios: true,
        launchReport: true,
        name:"CoreService Automation Tests Results",
        metadata: {
            "App Version":"0.3.2",
            "Test Environment": "STAGING",
            "Browser": "Chrome  54.0.2840.98",
            "Platform": "Windows 10",
            "Parallel": "Scenarios",
            "Executed": "Remote"
        }
    };


var {defineSupportCode} = require('cucumber');

defineSupportCode(function({After, Before, registerHandler}) {
 Before(function (scenario, callback) {        
        this.scenario = scenario;
        callback();
    });

    Before({tags: '@testPassing'}, function (scenario, callback)   
    {   
        callback();
    });

    registerHandler('AfterFeatures', function (features,callback) {
         reporter.generate(cucumber_html_option);
         setTimeout(function(){callback(); },1000);     
     });

    
});