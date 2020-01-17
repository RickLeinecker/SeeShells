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

module.exports = {
    pool,
    session,
    pgSession,
    userExists,
    registerUser,
    getGUIDs
}

