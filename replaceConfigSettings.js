var fs = require('fs')
var deploymentTargetFolder = process.argv[2]
var dataCacheKey = process.argv[3]
var environment = process.argv[4]

console.log("Running replaceConfigSettings.js")

console.log("deploymentTargetFolder: " + deploymentTargetFolder)

var configFilePath = deploymentTargetFolder.replace(/\\/g, "/");
configFilePath = configFilePath + "/web.config"
console.log("configFilePath: " + configFilePath)

fs.readFile(configFilePath, 'utf8', function (err,data) {
  if (err) {
    return console.log(err);
  }
  var result = data.replace(/dataCacheClientKey/g, dataCacheKey);
  
  if (environment == 'staging')  {
	result = result.replace(/"AFCacheSessionState"/g, '"AFCacheSessionStateStaging"');  
  }

  fs.writeFile(configFilePath, result, 'utf8', function (err) {
     if (err) return console.log(err);
  });
});