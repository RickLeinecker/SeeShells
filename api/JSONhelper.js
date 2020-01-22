function getOSIndexinArray(array, key) {
    return new Promise(function (resolve, reject) {
        for (var i in array) {
            var item = array[i];
            if (item.os == key) {
                resolve(i);
                return;
            }
        }
        reject(-1);
    });
}

function buildOSFileJSON(databaseResults) {
    return new Promise(async function (resolve, reject) {
        var result = [];
        var os = "";

        for (var i in databaseResults) {
            var osfile = databaseResults[i];
            if (osfile.osname == os) {
                var value = await getOSIndexinArray(result, osfile.osname);
                result[value].files.push(osfile.location);
            }
            else {
                result.push({ "os": osfile.osname, "files": [osfile.location] });
                os = osfile.osname;
            }
        }

        resolve(result);
    });
}

module.exports = {
    buildOSFileJSON
}