var JSONhelper = require('./helpers/JSONhelper.js');

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
        pool.query('SELECT * FROM logininfo WHERE username=$1', [username], (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            if (res.rowCount > 0) {
                resolve({
                    result: 1,
                });
                return;
            }

            resolve({ result: 0 });
        });
    });
}

function userExistsAndIsApproved(username) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT * FROM logininfo WHERE username=$1 AND approved=TRUE;', [username], (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            if (res.rowCount > 0) {
                resolve({
                    result: 1,
                    id: res.rows[0].id,
                    username: res.rows[0].username,
                    password: res.rows[0].password,
                    salt: res.rows[0].salt
                });
                return;
            }

            resolve({ result: 0 });
        });
    });
}

function getSession(sid) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT * FROM session WHERE sid=$1;', [sid], (err, res) => {
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

function destroySession(sid) {
    return new Promise(function (resolve, reject) {
        pool.query('DELETE FROM session WHERE sid=$1;', [sid], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "success" });
        });
    });
}

function getUnapprovedUsers() {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT id, username FROM logininfo WHERE approved=FALSE;', (err, res) => {
            if (err) {
                reject({});
                return;
            }

            resolve(res.rows);
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
    pool.query('INSERT INTO logininfo(username, password, salt, approved) values($1, $2, $3, FALSE);', [username, password, salt], (err, res) => {
        if (err) {
            throw err;
        }
    });
}

function approveUser(userID) {
    return new Promise(function (resolve, reject) {
        pool.query('UPDATE logininfo SET approved=TRUE WHERE id=$1;', [userID], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "success" });
        });
    });
}

function rejectUser(userID) {
    return new Promise(function (resolve, reject) {
        pool.query('DELETE FROM logininfo WHERE id=$1;', [userID], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "success" });
        });
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

function addScript(identifier, encodedscript) {
    return new Promise(function (resolve, reject) {
        pool.query('INSERT INTO scripts(typeidentifier, script) values($1, $2);', [identifier, encodedscript], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "Success" });
        });
    });
}

function updateExistingScript(identifier, encodedscript) {
    return new Promise(function (resolve, reject) {
        pool.query('UPDATE scripts SET script=$1 WHERE typeidentifier=$2;', [encodedscript, identifier], (err, res) => {
            if (err) {
                reject(err);
            }

            resolve({ "message": "Success" });
        });
    });
}

function updateScript(identifier, encodedscript) {
    return new Promise(function (resolve, reject) {
        let promise = scriptForIdentifierDoesNotExist(identifier);
        promise.then(
            function () {
                let addPromise = database.addScript(identifier, encodedscript);
                addPromise.then(
                    function (value) {
                        resolve({ "success": 1 });
                    },
                    function (err) {
                        reject({ "success": 0, "error": "Failed to add new script." });
                    }
                );
            },
            function (err) {
                if (err.result >= 1) {
                    let update = updateExistingScript(identifier, encodedscript);
                    update.then(
                        function () {
                            resolve({ "success": 1 });
                        },
                        function () {
                            reject({ "success": 0, "error": "Error in updating an existing script." });
                        }
                    );
                }
                else {
                    reject({ "success": 0, "error": err.message });
                }
            }
        );
    });
}

function scriptForIdentifierDoesNotExist(identifier) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT id FROM scripts WHERE typeidentifier=$1;', [identifier], (err, res) => {
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

            resolve({ result: "No script exists for this shell item identifier." });
        });
    });
}

// this will return the scripts, but they will be base64 encoded
// the website will need to decode it
function getScripts(callback) {
    pool.query('SELECT typeidentifier, script FROM scripts;', (err, res) => {
        if (err) {
            callback({});
        }

        callback(res.rows);
    });
}

function getScript(identifier) {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT id, script FROM scripts WHERE typeidentifier=$1;', [identifier], (err, res) => {
            if (err) {
                reject(err);
                return;
            }

            if (res.rowCount > 0) {
                resolve({
                    success: 1,
                    script: res.rows[0].script
                });
                
            }
            else {
                resolve({ "success": 0, result: "No script exists for this shell item identifier." });               
            }

            return;
        });
    });
}

function getHelpInformation() {
    return new Promise(function (resolve, reject) {
        pool.query('SELECT * FROM helpinfo;', (err, res) => {
            if (err) {
                reject({});
                return;
            }

            resolve(res.rows);
        });
    });
}

function updateHelpInformation(title, description) {
    return new Promise(function (resolve, reject) {
        pool.query('UPDATE helpinfo SET title=$1, description=$2;', [title, description], (err, res) => {
            if (err) {
                reject({});
                return;
            }

            resolve({message:"success"});
        });
    });
}

module.exports = {
    pool,
    session,
    pgSession,
    userExists,
    userExistsAndIsApproved,
    getSession,
    destroySession,
    getUnapprovedUsers,
    keysIDExists,
    addKeys,
    registerUser,
    approveUser,
    rejectUser,
    addOS,
    getOSandRegistryLocations,
    getRegistryLocations,
    getGUIDs,
    addGUID,
    GUIDDoesNotExist,
    updateScript,
    scriptForIdentifierDoesNotExist,
    getScripts,
    getScript,
    getHelpInformation,
    updateHelpInformation
}

