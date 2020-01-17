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

    database.userExists(username, function (exists) {
        var success = false;
        var message = "Unexpected error.";

        if (exists.result == 0) {
            security.hashAndSaltPassword(password, function (saltandhash) {
                database.registerUser(username, saltandhash.passwordHash, saltandhash.salt, (err) => {
                    if (err) {
                        message = 'Failed to register the new user.';
                    }
                });
                success = true;
            });
        }
        else {
            message = 'User already exists.';
        }

        if (success) {
            res.send({ "success": 1 });
        }
        else {
            res.send({ "success": 0, "error": message });
        }
    });
});

app.post('/login', function (req, res) {
    var username = String(req.body.username);
    var password = String(req.body.password);

    database.userExists(username, function (exists) {
        var success = false;
        var message = "Unexpected error.";

        if (exists.result == 1) {
            var compare = exists.password;
            var salt = exists.salt;

            security.verifyPassword(compare, salt, password, function (result) {

                if (result == 1) {
                    req.session.user = {
                        name: username
                    };

                    success = true;
                }
                else {
                    message = 'Incorrect password.';
                }
            });
        }
        else {
            message = 'User does not exist.';
        }

        if (success) {
            res.send({ "success": 1, "session": req.session });
        }
        else {
            res.send({ "success": 0, "error": message });
        }
    });
});

app.get('/getGUIDs', function (req, res) {
    database.getGUIDs(function (guids) {
        res.send(guids);
    });
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