require('dotenv').config();
var express = require('express');
var database = require('./databaseconnection.js');
var security = require('./cryptowork.js');

var port = process.env.PORT || 3000;
var app = express();

app.use(database.session({
    store: new database.pgSession({
        pool: database.pool,
        tableName: 'session'
    }),
    secret: process.env.SESSION_SECRET,
    resave: false,
    saveUninitialized: false,
    cookie: { maxAge: 30 * 24 * 60 * 60 * 1000 }, // 30 days
    unset: 'destroy',
    genid: (req) => {
        return security.generateSessionKey();
    }
}));

app.use(express.urlencoded({ extended: true }));
app.use(express.json());


app.get('/', function (req, res) {
    res.send({ test: 'testing', hello: 'klayton', and: 'aleks', aswellas: 'bridget', andlastbutnotleast: 'yara' });
});

app.post('/register', function (req, res) {;
    var username = String(req.body.username);
    var password = String(req.body.password);

    let promise = database.userExists(username);
    promise.then(
        function (value) {
            if (value.result == 0) {
                let pwPromise = security.hashAndSaltPassword(password);
                pwPromise.then(
                    function (saltandhash) {
                        try {
                            database.registerUser(username, saltandhash.passwordHash, saltandhash.salt);
                            res.send({ "success": 1 });
                        }
                        catch {
                            res.send({ "success": 0, "error": "Failed to register the new user." });
                        }
                    }
                );
            }
            else {
                res.send({ "success": 0, "error": "User already exists." });
            }

        },
        function (err) {
            res.send({ "success": 0, "error": "Failure checking the database for existing users." });
        }
    );
});

app.post('/login', function (req, res) {
    var username = String(req.body.username);
    var password = String(req.body.password);

    let promise = database.userExists(username);
    promise.then(
        function (value) {
            if (value.result == 1) {
                var compare = value.password;
                var salt = value.salt;

                let verifyPromise = security.verifyPassword(compare, salt, password);
                verifyPromise.then(
                    function (result) {

                        req.session.user = {
                            name: username
                        };

                        res.send({ "success": 1, "session": req.session });

                    },
                    function (fail) {
                        res.send({ "success": 0, "error": "Incorrect password." });
                    }
                );
            }
            else {
                res.send({ "success": 0, "error": "User does not exist." });
            }

        },
        function (err) {
            res.send({ "success": 0, "error": "Failure checking the database for existing users." });
        }
    );
});

app.get('/getOSandRegistryLocations', function (req, res) {
    let promise = database.getOSandRegistryLocations();
    promise.then(
        function (results) {
            console.log(results);
            if(Object.keys(results).length > 0)
                res.send({ "success": 1, "json": results });
            else
                res.send({ "success": 0, "error": "No files to fetch" });
        },
        function (err) {
            res.send({ "success": 0, "error": "Failed to get any OS versions and corresponding shellbag registry locations" });

        }
    );
});

app.get('/getGUIDs', function (req, res) {
    database.getGUIDs(function (guids) {
        if (Object.keys(guids).length > 0)
            res.send({ "success": 1, "json": guids});
        else
            res.send({ "success": 0, "error":"Failed to get any GUIDs" });
    });
});

app.post('/addGUID', function (req, res) {
    var guid = String(req.body.guid);
    var name = String(req.body.name);

    let promise = database.GUIDDoesNotExist(guid);
    promise.then(
        function (value) {
            let addPromise = database.addGUID(guid, name);
            addPromise.then(
                function (value) {
                    res.send({ "success": 1 });
                },
                function (err) {
                    res.send({ "success": 0, "error": "Failed to add new GUID." });
                }
            );
        },
        function (err) {
            res.send({ "success": 0, "error": "GUID exists in the database already."});
        }
    );
});

app.get('/logout', function (req, res) {
    req.session.destroy(function (err) {
        if (err) {
            console.log(err);
        }
        else {
            res.redirect('/');
        }
    });
});

app.listen(port, function () {
    console.log('Example app listening...');
});