require('dotenv').config();
const { Pool, Client } = require('pg'),
    session = require('express-session'),
    pgSession = require('connect-pg-simple')(session);

const pool = new Pool({
    connectionString: process.env.DATABASE_URL,
    ssl:true
});
pool.connect();


function userExists(username, callback) {
    pool.query('SELECT * FROM logininfo WHERE username=$1;', [username], (err, res) => {
        if (err) {
            console.log(err);
            throw err;
        }
        
        if (res.rowCount > 0) {
            callback ({
                result: 1,
                password: res.rows[0].password,
                salt: res.rows[0].salt
            });
            return;
        }

        callback({result:0});
    });
}

function registerUser(username, password, salt) {
    pool.query('INSERT INTO logininfo(username, password, salt) values($1, $2, $3);', [username, password, salt], (err, res) => {
        if (err) {
            throw err;
        }
    });
}

function getGUIDs(callback) {
    pool.query('SELECT guid, name FROM guids;', (err, res) => {
        if (err) {
            callback({});
            throw err;
        }

        callback(JSON.stringify(res.rows));
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
    getGUIDs,
    addGUID,
    GUIDDoesNotExist
}

