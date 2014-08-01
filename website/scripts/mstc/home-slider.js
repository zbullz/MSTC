$(document).ready(function(){
    
	//Homepage Slider
    var options = {
        nextButton: false,
        prevButton: false,
        pagination: true,
        animateStartingFrameIn: true,
        autoPlay: true,
        autoPlayDelay: 3000,
        preloader: true
    };

    if ($("#sequence").length > 0) {
        var mySequence = $("#sequence").sequence(options).data("sequence");
    }
});