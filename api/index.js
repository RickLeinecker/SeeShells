var express = require('express');

var port = process.env.PORT || 3000;

var app = express();

app.get('/', function (req, res) {
    res.send({ test: 'testing', hello: 'klayton', and: 'aleks', aswellas: 'bridget', andlastbutnotleast: 'yara' });
});

app.listen(port, function () {
    console.log('Example app listening...');
});