
function openlinkFunc(linkUrl)
{
    document.body.addEventListener("touchend", function () {
        document.body.removeEventListener("touchend", arguments.callee);
        window.open(linkUrl);
    });
}

function open2DLinkFunc(linkUrl)
{
    //window.parent.location.href = linkUrl;
    window.parent.location.replace(linkUrl);
}