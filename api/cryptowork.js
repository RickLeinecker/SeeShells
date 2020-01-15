var crypto = require('crypto');

function getSalt(callback) {
    var result = crypto.randomBytes(16).toString('hex').slice(0, 32)
    callback(result);
};

function hashAndSaltPassword(password, callback) {
    getSalt(function (salt) {
        var hash = crypto.createHmac('sha512', salt);
        hash.update(password);
        var value = hash.digest('hex');
        callback({
            salt: salt,
            passwordHash: value
        });
    });
};

function verifyPassword(hash, salt, enteredPassword, callback) {
    var hashedSalt = crypto.createHmac('sha512', salt);
    hashedSalt.update(enteredPassword);
    var value = hashedSalt.digest('hex');

    if (value == hash)
        callback(1);
    else
        callback(0);
};

function generateSessionKey() {
    return crypto.randomBytes(16).toString('base64');
}

module.exports = {
    hashAndSaltPassword,
    verifyPassword,
    generateSessionKey
}
