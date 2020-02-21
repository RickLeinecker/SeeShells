require('dotenv').config();
var express = require('express');
var database = require('./databaseconnection.js');
var security = require('./cryptowork.js');
const { check, validationResult } = require('express-validator');

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
app.use(require('sanitize').middleware);



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

app.post('/addOS', function (req, res) {
    var num = String(req.body.osnum);
    var name = String(req.body.osname);
    var mainkeysid = String(req.body.mainkeysid);

    let keysExist = database.keysIDExists(mainkeysid);
    keysExist.then(
        function (value) {
            if (value == true) {
                let addPromise = database.addOS(num, name, mainkeysid);
                addPromise.then(
                    function (value) {
                        res.send({ "success": 1 });
                    },
                    function (err) {
                        res.send({ "success": 0, "error": "Failed to add new OS." });
                    }
                );
            }
            else {
                res.send({ "success": 0, "error": "The keys ID selected does not exist." });
            }
        },
        function (err) {
            res.send({ "success": 0, "error": "Error with database connection." });
        }
    );
        
});

app.post('/addOSWithFileLocations', function (req, res) {
    var num = String(req.body.osnum);
    var name = String(req.body.osname);
    var keysArray = req.body.keys;

    let keysAdded = database.addKeys(keysArray);
    keysAdded.then(
        function (mainkeysid) {
            let addPromise = database.addOS(num, name, mainkeysid);
            addPromise.then(
                function (value) {
                    res.send({ "success": 1 });
                },
                function (err) {
                    res.send({ "success": 0, "error": "Failed to add new OS." });
                }
            );

        },
        function (err) {
            res.send({ "success": 0, "error": "Failed to add the new registry key locations." });
        }
    );

});

app.get('/getRegistryLocations', function (req, res) {
    let promise = database.getRegistryLocations();
    promise.then(
        function (results) {
            if (Object.keys(results).length > 0)
                res.send({ "success": 1, "json": results });
            else
                res.send({ "success": 0, "error": "No file locations to fetch" });
        },
        function (err) {
            res.send({ "success": 0, "error": "Failed to get any shellbag registry locations" });

        }
    );
});

// scripts must be sent base 64 encoded to this endpoint
app.post('/addScript', [
    check('identifier').isNumeric().trim().escape(),
    check('script').isBase64().trim(),
], function (req, res) {

    const errors = validationResult(req);
    if (!errors.isEmpty()) {
        return res.status(422).json({ errors: errors.array() });
    }

    var identifier = String(req.body.identifier);
    var encodedscript = String(req.body.script);

    let promise = database.scriptForIdentifierDoesNotExist(identifier);
    promise.then(
        function (value) {
            let addPromise = database.addScript(identifier, encodedscript);
            addPromise.then(
                function (value) {
                    res.send({ "success": 1 });
                },
                function (err) {
                    res.send({ "success": 0, "error": "Failed to add new script." });
                }
            );
        },
        function (err) { 
            if(err.result >= 1)
                res.send({ "success": 0, "error": "Script exists in the database already for this identifier." });
            else
                res.send({ "success": 0, "error": err.message });
        }
    );
});

app.get('/getScripts',  function (req, res) {

    database.getScripts(function (results) {
        if (Object.keys(results).length > 0)
            res.send({ "success": 1, "json": results });
        else
            res.send({ "success": 0, "error": "Failed to get any scripts" });
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