var express = require('express');
var database = require('./databaseconnection.js');
var security = require('./cryptowork.js');

var port = process.env.PORT || 3000;
var app = express();

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


app.listen(port, function () {
    console.log('Example app listening...');
});