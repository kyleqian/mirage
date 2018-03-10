const express = require('express');
const app = express();

app.get('/:char', function(req, res) {
	process.stdout.write(req.params.char);
	res.send('Received: ' + req.params.char);
});

app.listen(process.env.PORT);
