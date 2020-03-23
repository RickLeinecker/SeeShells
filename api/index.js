require('dotenv').config();
var express = require('express');
var database = require('./databaseconnection.js');
var security = require('./cryptowork.js');
const { check, validationResult } = require('express-validator');
var flash = require("connect-flash");

var port = process.env.PORT || 3000;
var app = express();

app.use(function (req, res, next) {
    res.setHeader('Access-Control-Allow-Origin', '*'); //https://rickleinecker.github.io
    res.setHeader('Access-Control-Allow-Headers', 'Origin, X-Requested-With, X-Auth-Token, Content-Type, Accept');
    res.setHeader('Access-Control-Allow-Methods', 'POST, GET');
    res.setHeader('Access-Control-Allow-Credentials', true);
    next();
});

app.use(database.session({
    store: new database.pgSession({
        pool: database.pool,
        tableName: 'session'
    }),
    secret: process.env.SESSION_SECRET,
    resave: false,
    saveUninitialized: false,
    cookie: { maxAge: 30 * 60 * 1000, secure:true }, // 30 minutes
    unset: 'destroy',
    genid: (req) => {
        return security.generateSessionKey();
    }
}));

app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(require('sanitize').middleware);
app.use(flash());

app.get('/', function (req, res) {
    res.send({ success: 1 });
});
app.get('/notauthenticated', function (req, res) {
    res.send({ message: 'You must log in to perform this action.' });
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
    var username = (req.body.username);
    var password = (req.body.password);

    let promise = database.userExistsAndIsApproved(username);
    promise.then(
        function (user) {
            if (user.result == 1) {
                var compare = user.password;
                var salt = user.salt;

                let verifyPromise = security.verifyPassword(compare, salt, password);
                verifyPromise.then(
                    function (result) {
                        req.session.user = user.username;
                        res.send({ "success": 1, "session": req.sessionID });
                    },
                    function (fail) {
                        res.send({ "success": 0, "message":  'Incorrect password.' });
                    }
                );
            }
            else {
                res.send({ "success": 0, "message": 'Incorrect username.' });
            }
        },
        function (err) {
            return done(err);
        }

    );
});

app.get('/getNewUsers', function (req, res) {
    let promise = database.getUnapprovedUsers();
    promise.then(
        function (results) {
            res.send({ "success": 1, "json": results });
        },
        function (err) {
            res.send({ "success": 0, "error": "Failed to get any existing new users." });

        }
    );
});

app.post('/approveUser', function (req, res) {
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
                var id = (req.body.userID);
                let promise = database.approveUser(id);
                promise.then(
                    function () {
                        res.send({ "success": 1 });
                    },
                    function () {
                        res.send({ "success": 0, "error": "Failed to approve the user." });
                    }
                );
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )
});

app.post('/rejectUser', function (req, res) {
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
                var id = (req.body.userID);
                let promise = database.rejectUser(id);
                promise.then(
                    function () {
                        res.send({ "success": 1 });
                    },
                    function () {
                        res.send({ "success": 0, "error": "Failed to delete the user." });
                    }
                );
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )
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
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
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
                        res.send({ "success": 0, "error": "GUID exists in the database already." });
                    }
                );
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )
});

app.post('/addOS', function (req, res) {
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
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
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )      
});

app.post('/addOSWithFileLocations', function (req, res) {
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
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
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )
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
app.post('/updateScript', [
    check('identifier').isNumeric().trim().escape(),
    check('script').isBase64().trim(),
], function (req, res) {

        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return res.status(422).json({ errors: errors.array() });
        }

        let promise = database.getSession(req.header('x-auth-token'));
        promise.then(
            function (result) {
                if (result == true) {
                    var identifier = String(req.body.identifier);
                    var encodedscript = String(req.body.script);

                    let promise = database.updateScript(identifier, encodedscript);
                    promise.then(
                        function (value) {
                            res.send({ "success": 1 });
                        },
                        function (err) {
                            res.send({ "success": 0, "error": err.message });
                        }
                    );
                }
                else {
                    res.redirect('/notauthenticated');
                }
            },
            function (err) {
                res.redirect('/notauthenticated');
            }
        )

});

app.get('/getScripts',  function (req, res) {

    database.getScripts(function (results) {
        if (Object.keys(results).length > 0)
            res.send({ "success": 1, "json": results });
        else
            res.send({ "success": 0, "error": "Failed to get any scripts" });
    });

});

app.get('/getScript', function (req, res) {

    var identifier = req.query.identifier;

    let promise = database.getScript(identifier);
    promise.then(
        function (results) {
            if (results.success == 1) {
                res.send({ "success": 1, "script": results.script });
            }
            else {
                res.send({ "success": 0, "error": "No script exists for this identifier." });
            }
        },
        function () {
            res.send({ "success": 0, "error": "Failed to get script information." });
        }
    );

});


app.get('/getHelpInformation', function (req, res) {
    let promise = database.getHelpInformation();
    promise.then(
        function (results) {
            res.send({ "success": 1, "json":results });
        },
        function () {
            res.send({ "success": 0, "error": "Failed to get the help information." });
        }
    );
});

app.post('/changeHelpInformation', function (req, res) {
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
                var title = (req.body.title);
                var description = (req.body.description);
                let promise = database.updateHelpInformation(title, description);
                promise.then(
                    function () {
                        res.send({ "success": 1 });
                    },
                    function () {
                        res.send({ "success": 0, "error": "Failed to update the help information." });
                    }
                );
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )
});


app.get('/logout', function (req, res) {
    let promise = database.getSession(req.header('x-auth-token'));
    promise.then(
        function (result) {
            if (result == true) {
                let promise = database.destroySession(req.header('x-auth-token'));
                promise.then(
                    function () {
                        res.send({ "success": 1 });
                    },
                    function () {
                        res.send({ "success": 0, "error": "Failed to destroy session." });
                    }
                );
            }
            else {
                res.redirect('/notauthenticated');
            }
        },
        function (err) {
            res.redirect('/notauthenticated');
        }
    )
});


app.listen(port, function () {
    console.log('Example app listening...');
});
