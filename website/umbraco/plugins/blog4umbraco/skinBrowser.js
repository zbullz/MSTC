var currentContainer = "";
var oldLoadingCaption = "";

function chooseSkin(id) {
    if (currentContainer != "") {
        var skinHtml = jQuery("#" + id).html();
        jQuery("#skinContainer_" + currentContainer).html("<li>" + skinHtml + "</li>");
        jQuery("#skinContainer_" + currentContainer + " li img").prependTo("#skinContainer_" + currentContainer + ">li");
        jQuery("#skinContainer_" + currentContainer + " a").remove();
        jQuery("#" + currentContainer).attr("value", id);
    } else {
        alert('No container defined!');
    }
}

function loadSkins(id, packageAlias) {
    currentContainer = id;
    // add loading message
    oldLoadingCaption = jQuery("#themeLoadLink_" + id).html();
    jQuery("#themeLoadLink_" + id).html("Loading skins (please wait...)");
    jQuery("#skinContainer_" + id).html("");
    jQuery("#skinContainer_" + id).load("plugins/blog4umbraco/skinBrowser.aspx?package=" + packageAlias, null, function() {
        jQuery("#themeLoadLink_" + id).html(oldLoadingCaption);
    });
}