var JSONhelper = require('./JSONhelper.js');

require('dotenv').config();
const { Pool, Client } = require('pg'),
    session = require('express-session'),
    pgSession = require('connect-pg-simple')(session);

const pool = new Pool({
    connectionString: process.env.DATABASE_URL,
    ssl:true
});
pool.connect();


function userExists(username) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT * FROM logininfo WHERE username=$1;', [username], (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            if (res.rowCount > 0) {
                resolve({
                    result: 1,
                    password: res.rows[0].password,
                    salt: res.rows[0].salt
                });
                return;
            }

            resolve({ result: 0 });
        });
    });
}

function keysIDExists(mainkeysid) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT * from mainshellkeys WHERE mainkeysid=$1;', [mainkeysid], (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            if (res.rowCount > 0) {
                resolve(true);
                return;
            }

            resolve(false);
        });
    });
}

function makeNewKeyGroup() {
    return new Promise(async function (resolve, reject) {
        pool.query('INSERT INTO mainshellkeys DEFAULT VALUES returning mainkeysid;', (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            resolve(res.rows[0].mainkeysid);
        });

    });
}

function addKeys(keysArray) {
    return new Promise(function (resolve, reject) {
        let newKeyGroup = makeNewKeyGroup();
        newKeyGroup.then(
            async function (keygroupID) {
                for (let key of keysArray) {
                    await pool.query('INSERT INTO keys(location, mainkeysid) values($1, $2);', [String(key), keygroupID], (err, res) => {
                        if (err) {
                            reject(err);
                            return;
                        }
                    });
                }

                resolve(keygroupID);
            },
            function (err) {
                reject(err);
            }
        )
    });
    
}

function registerUser(username, password, salt) {
    pool.query('INSERT INTO logininfo(username, password, salt) values($1, $2, $3);', [username, password, salt], (err, res) => {
        if (err) {
            throw err;
        }
    });
}

function addOS(num, name, keysid) {
    return new Promise(function (resolve, reject) {
        pool.query('INSERT INTO osversion(osnum, osname, mainkeysid) values($1, $2, $3);', [num, name, keysid], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "Success" });
        });
    });
}

function getOSandRegistryLocations() {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT osversion.osname, keys.location FROM osversion INNER JOIN mainshellkeys ON osversion.mainkeysid = mainshellkeys.mainkeysid INNER JOIN keys ON keys.mainkeysid = mainshellkeys.mainkeysid ORDER BY osversion.osid ASC;', (err, res) => {
            if (err) {
                reject({});
                return;
            }

            let promise = JSONhelper.buildOSFileJSON(res.rows);
            promise.then(
                function (value) {
                    resolve(value);
                },
                function (err) {
                    reject({});
                }
            )
        });
    });
}

function getRegistryLocations() {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT mainshellkeys.mainkeysid, keys.location FROM keys INNER JOIN mainshellkeys ON keys.mainkeysid = mainshellkeys.mainkeysid ORDER BY mainshellkeys.mainkeysid ASC;', (err, res) => {
            if (err) {
                reject({});
                return;
            }

            let promise = JSONhelper.buildFileLocationJSON(res.rows);
            promise.then(
                function (value) {
                    resolve(value);
                },
                function (err) {
                    reject({});
                }
            )
        });
    });
}

function getGUIDs(callback) {
    pool.query('SELECT guid, name FROM guids;', (err, res) => {
        if (err) {
            callback({});
        }

        callback(res.rows);
    });
}

function addGUID(guid, name) {
    return new Promise(function (resolve, reject) {
        pool.query('INSERT INTO guids(guid, name) values($1, $2);', [guid, name], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "Success" });
        });
    });
}

function GUIDDoesNotExist(guid) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT name FROM guids WHERE guid=$1;', [guid], (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            if (res.rowCount > 0) {
                reject({
                    result: res.rowCount
                });
                return;
            }

            resolve({ result: "Not exisiting GUID" });
        });
    });
}

module.exports = {
    pool,
    session,
    pgSession,
    userExists,
    keysIDExists,
    addKeys,
    registerUser,
    addOS,
    getOSandRegistryLocations,
    getRegistryLocations,
    getGUIDs,
    addGUID,
    GUIDDoesNotExist
}

