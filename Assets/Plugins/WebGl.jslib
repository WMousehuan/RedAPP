mergeInto(LibraryManager.library, {
    clickSelectFileBtn:function (name) 
    {
    clickSelectFileBtn(UTF8ToString(name));
    },
    receiveMessageFromUnity:function (content) 
    {
    receiveMessageFromUnity(UTF8ToString(content));
    },

    AboutUsDataToJs: function (str) 
    {
        receivedMessage(UTF8ToString(str));
    }
    ,
    AboutUsToJsNoParam:function()
    {
        openWindow();
    }
    ,

    OpenLink: function (linkUrl) 
    {
        openlinkFunc(UTF8ToString(linkUrl));
    }
    ,

    closeAllViewsUnity:function()
    {
        closeAllView();
    }
    ,
    ClickPlayVideoUnity:function(mes){
        clickPlayVideo(UTF8ToString(mes));
    }
    ,

    Open2DLink: function (linkUrl) 
    {
        open2DLinkFunc(UTF8ToString(linkUrl));
    }
    ,
});