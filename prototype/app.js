(function(window, document) {

    /*
     * Borrowed from https://stackoverflow.com/questions/20194722/can-you-get-a-users-local-lan-ip-address-via-javascript
     */
    window.RTCPeerConnection = window.RTCPeerConnection || window.mozRTCPeerConnection || window.webkitRTCPeerConnection; //compatibility for Firefox and chrome
    var pc = new RTCPeerConnection({iceServers:[]}), noop = function(){};
    pc.createDataChannel(''); //create a bogus data channel
    pc.createOffer(pc.setLocalDescription.bind(pc), noop); // create offer and set local description
    pc.onicecandidate = function(ice) {
        if (ice && ice.candidate && ice.candidate.candidate) {
            localIp = /([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/.exec(ice.candidate.candidate)[1];
            pc.onicecandidate = noop;
        }
    };

    var localIp;
    function waitForElement() {
        if (typeof localIp === "undefined") {
            setTimeout(waitForElement, 250);
        }
    }
    waitForElement();

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

    function recognitionCallback(data, err) {
        if (err) throw err;
        else result.innerText = data;

        var lowerCase = data.filter(word => [' '].includes(word) || word.toUpperCase() != word);
        var char = lowerCase.length === 0 ? data[0] : lowerCase[0];

        var xhr = new XMLHttpRequest();
        if (typeof localIp !== "undefined")
            xhr.open("GET", "http://" + localIp + ":9001/" + char);
        else
            xhr.open("GET", "http://" + "10.34.164.54" + ":9001/" + char);
        xhr.send();
    }

    //Set callback function
    canvas.setCallBack(recognitionCallback);

    //Set line width shown on the canvasvas element (default: 3)
    canvas.setLineWidth(15);

    //Set options
    canvas.setOptions({
        language: "en",
        numOfReturn: 3
    });

})(window, document);
