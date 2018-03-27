cucumber-html-reporter
======================

Generate Cucumber HTML reports with pie charts
> Available HTML themes: `['bootstrap', 'foundation', 'simple']`


## Preview of HTML Reports

1. [Bootstrap Theme Reports with Pie Chart][3]
2. [Foundation Theme Reports][4]
3. [Simple Theme Reports][5]


## Install

``` bash
npm install
```

## Usage

```
$ cucumberjs tests/features/ -f json:tests/report/cucumber_report.json
```

> Multiple formatter are also supported,

```
$ cucumberjs tests/features/ -f pretty -f json:tests/report/cucumber_report.json
```

> Are you using cucumber with other frameworks or running [cucumber-parallel][6]? Pass relative path of JSON file to the `options` as shown [here][7]


## Options

#### `theme`
Available: `['bootstrap', 'foundation', 'simple']`
Type: `String`

Select the Theme for HTML report.


#### `jsonFile`
Type: `String`

Provide path of the Cucumber JSON format file

#### `jsonDir`
Type: `String`

If you have more than one cucumber JSON files, provide the path of JSON directory. This module will create consolidated report of all Cucumber JSON files.

e.g. `jsonDir: 'tests/reports'` //where _reports_ directory contains valid `*.json` files


N.B.: `jsonFile` takes precedence over `jsonDir`. We recommend to use either `jsonFile` or `jsonDir` option.


#### `output`
Type: `String`

Provide HTML output file path and name


#### `reportSuiteAsScenarios`
Type: `Boolean`
Supported in the Bootstrap theme.

`true`: Reports total number of passed/failed scenarios as HEADER.

`false`: Reports total number of passed/failed features as HEADER.

#### `launchReport`
Type: `Boolean`

Automatically launch HTML report at the end of test suite

`true`: Launch HTML report in the default browser

`false`: Do not launch HTML report at the end of test suite

#### `ignoreBadJsonFile`
Type: `Boolean`

Report any bad json files found during merging json files from directory option.

`true`: ignore any bad json files found and continue with remaining files to merge.

`false`: Default option. Fail report generation if any bad files found during merge.

#### `name`
Type: `String` (optional)

Custom project name. If not passed, module reads the name from projects package.json which is preferable.
 
#### `storeScreenShots`
Type: `Boolean`
Default: `undefined`

`true`: Stores the screenShots to the default directory. It creates a directory 'screehshot' if does not exists.

`false` or `undefined` : Does not store screenShots but attaches screenShots as a step-inline images to HTML report


#### `metadata`
Type: `JSON` (optional)
Default: `undefined`

Print more data to your report, such as _browser info, platform, app info, environments_ etc. Data can be passed as JSON `key-value` pair. Reporter will parse the JSON and will show the _Key-Value_ under `Metadata` section on HTML report. Checkout the below preview HTML Report with Metadata.

Pass the _Key-Value_ pair as per your need, as shown in below example,

```json

 metadata: {
        "App Version":"0.3.2",
        "Test Environment": "STAGING",
        "Browser": "Chrome  54.0.2840.98",
        "Platform": "Windows 10",
        "Parallel": "Scenarios",
        "Executed": "Remote"
      }
      
```