const { Pool, Client } = require('pg');

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

function getGUIDS() {
    pool.query('SELECT guid, name FROM guids;', (err, res) => {
        if (err) throw err;
        for (let row of res.rows) {
            console.log(JSON.stringify(row));
        }
    });
}

module.exports = {
    userExists,
    registerUser
}

