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

function registerUser(username, password, salt) {
    pool.query('INSERT INTO logininfo(username, password, salt) values($1, $2, $3);', [username, password, salt], (err, res) => {
        if (err) {
            throw err;
        }
    });
}

function addOS(num, name, keysid) {
    return new Promise(function (resolve, reject) {
        pool.query('INSERT INTO osversions(osnum, osname, mainkeysid) values($1, $2, $3);', [num, name, keysid], (err, res) => {
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
    registerUser,
    addOS,
    getOSandRegistryLocations,
    getGUIDs,
    addGUID,
    GUIDDoesNotExist
}

