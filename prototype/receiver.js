const express = require('express');
const app = express();

app.get('/:char', function(req, res) {
	process.stdout.write(req.params.char);
	res.header('Access-Control-Allow-Origin', '*');
	res.send('Received: ' + req.params.char);
});

app.listen(9001, () => console.log('Listening on port 9001'));
