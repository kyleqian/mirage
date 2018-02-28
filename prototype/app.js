(function(window, document) {

    var canvasElem = document.getElementById("canvas");
    var canvas = new window.handwriting.Canvas(canvasElem);
    var result = document.getElementById("result");

    function resize() {
        canvasElem.setAttribute("width", window.innerWidth);
        canvasElem.setAttribute("height", window.innerHeight - 50);
        canvas.width = canvasElem.getAttribute("width");
        canvas.height = canvasElem.getAttribute("height");
    }

    resize();

    window.addEventListener("resize", function() {
        resize();
        canvas.width = canvasElem.getAttribute("width");
        canvas.height = canvasElem.getAttribute("height");
    });

    window.recognize = function() {
        //recognize captured trace
        canvas.recognize();
    };

    window.erase = function() {
        //Clear canvas, captured trace, and stored steps
        canvas.erase();
        result.innerText = "";
        result.innerHtml = "&nbsp;";
    };

    //Set callback function
    canvas.setCallBack(function(data, err) {
        if (err) throw err;
        else result.innerText = data;

        var lowerCase = data.filter(word => word.toUpperCase() != word);
        var char = lowerCase.length === 0 ? data[0] : lowerCase[0];

        var xhr = new XMLHttpRequest();
        xhr.open("GET", "http://10.1.10.190:9001/" + char);
        xhr.send();
    });

    //Set line width shown on the canvasvas element (default: 3)
    canvas.setLineWidth(15);

    //Set options
    canvas.setOptions({
        language: "en",
        numOfReturn: 3
    });

})(window, document);
