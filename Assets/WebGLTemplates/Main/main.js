var background=document.getElementById("background");
var logo=document.getElementById("logo");
var unity_container=document.getElementById("unity-container");
var unity_canvas=document.getElementById("unity-canvas");
var messageDiv= document.getElementById("message");
var unity_progress_bar_empty=document.getElementById("unity-progress-bar-empty");
var unityInstance = null;
var chatCloseImg=null;
var initSceneName = 'LoadingRedPackage';
var boothId=null;
var url = window.location.toString();
var mainUrl="";
var urlsStage = [];
if(url.includes('?')){
    urlsStage= url.split("?");
    mainUrl=urlsStage[0];
}
else{
    mainUrl= url;
}
var infoDiv = document.getElementById("message");
var urlSearch_Dictionary = {};
if (url.includes('?')) {
   
    var searchsStage = urlsStage[1].split("&")
    searchsStage.forEach(element => {
        var elementStage = element.split('=');
        urlSearch_Dictionary[elementStage[0]] = elementStage[1];
    });
}
var realTargetScene="";
var encryptSuperiorId="";
if("encryptSuperiorId" in urlSearch_Dictionary)
{
    encryptSuperiorId=urlSearch_Dictionary["encryptSuperiorId"];
}
var channelId="";
if("channelId" in urlSearch_Dictionary)
{
    channelId=urlSearch_Dictionary["channelId"];
}
 var xhr = null;
// var haveToken = false;
// var token = "";
// if ("token" in urlSearch_Dictionary) {
//     token = urlSearch_Dictionary["token"].toString();
//     haveToken = true;
// }
// if ("sessionId" in urlSearch_Dictionary) {
//     token = urlSearch_Dictionary["sessionId"].toString();
//     haveToken = true;
// }
// var haveUserId = false;
 var userId = "";
// if ("userId" in urlSearch_Dictionary) {
//     userId = urlSearch_Dictionary["userId"].toString();
//     haveUserId = true;
// }
var lang = "en";
if ("lang" in urlSearch_Dictionary) {
    lang = urlSearch_Dictionary["lang"].toString();
    switch (lang) {
    case "zh-HK":
        infoDiv.textContent = "載入中";
        break;
    case "zh-CN":
        infoDiv.textContent = "载入中";
        break;
    case "en":
        break;
    default:
        lang="en"
        break;
    }
}



var conferenceIndex=null;
if ("conferenceIndex" in urlSearch_Dictionary) {
    conferenceIndex = urlSearch_Dictionary["conferenceIndex"].toString();
}


//////////////////////////////////////////////////////////////////////////////
function CreateUnity(finishAction){

var buildUrl ="https://test-dinosaur-file.s3.ap-east-1.amazonaws.com/webgl/Build/";
//var buildUrl = "Build/";


var canvas = document.querySelector("#unity-canvas");
var loadingBar = document.querySelector("#unity-progress-bar-empty");
var progressBarFull = document.querySelector("#unity-progress-bar-full");

// Shows a temporary message banner/ribbon for a few seconds, or
// a permanent error message on top of the canvas if type=='error'.
// If type=='warning', a yellow highlight color is used.
// Modify or remove this function to customize the visually presented
// way that non-critical warnings and error messages are presented to the
// user.
function unityShowBanner(msg, type) {
function updateBannerVisibility() {
    warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
}
var div = document.createElement('div');
div.innerHTML = msg;
warningBanner.appendChild(div);
if (type == 'error') div.style = 'background: red; padding: 10px;';
else {
    if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
    setTimeout(function () {
        warningBanner.removeChild(div);
        updateBannerVisibility();
    }, 5000);
}
updateBannerVisibility();
}


var loaderUrl = buildUrl + "RedPackage_WebGl.loader.js";
var config = {
dataUrl: buildUrl + "RedPackage_WebGl.data",
frameworkUrl: buildUrl + "RedPackage_WebGl.framework.js",
codeUrl: buildUrl + "RedPackage_WebGl.wasm",
streamingAssetsUrl: "StreamingAssets",
companyName: "ThrillGame",
productName: "TreasureChest",
productVersion: "1.0",
};
if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
// Mobile device style: fill the whole browser client area with the game canvas:
var meta = document.createElement('meta');
meta.name = 'viewport';
meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
document.getElementsByTagName('head')[0].appendChild(meta);
//container.className = "unity-mobile";
}
if (/iPhone|iPad|iPod/i.test(navigator.userAgent)){
config.devicePixelRatio = 2;
}
else if (/Android/i.test(navigator.userAgent)){
config.devicePixelRatio = 2;
}
else{
config.devicePixelRatio = 2;
}

//loadingBar.style.display="block";
var script = document.createElement("script");
script.src = loaderUrl;
script.onload = () => {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
createUnityInstance(document.querySelector("#unity-canvas"), config, (progress) => {
    progressBarFull.style.width = 100 * (progress/0.9) + "%";
    messageDiv.textContent=((100 * (progress/0.9)).toFixed(1) + "%").toString();
}).then((unityInstance_Temp) => {
    background.style.display="none";
    logo.style.display="none";
    loadingBar.style.display = 'none';
    loadingBar.style.zIndex = 0;
    infoDiv.textContent = "";
    infoDiv.style.display = 'none';
    infoDiv.style.zIndex = 0;
    unityInstance = unityInstance_Temp;

    finishAction();
    //unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'InitManager|' + initSceneName+"^"+realTargetScene);
    console.log("++++++++++++++++++" + encryptSuperiorId);
    unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', "InitManager|WebInitData^"+encryptSuperiorId+"^"+channelId);
    history.pushState({ key: 'value' }, 'Title', mainUrl)
    //unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'MainUI|GetWebPlatform^' + navigator.userAgent);
});
}
document.body.appendChild(script);
}

if(userId==""||token=="")
{
    CreateUnity(()=>{
        //unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', "UserInit_Ctrl|InitUserData^" + "" + "^" + "" + "^" + lang);
    });
}
else
{
    CreateUnity(()=>{
        //unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', "UserInit_Ctrl|InitUserData^" + token + "^" + userId + "^" + lang + "^" + JSON.stringify(responseData["data"]));
    });
     
    // if (haveToken) 
    // {
    //     xhr_1 = new XMLHttpRequest();
    //     //http://47.238.167.231:8053/v1/api/userProfile/get
    //     xhr_1.open("GET", baseUrl + "/v1/api/userProfile/get", true);
    //     if (lang != "") {
    //         xhr_1.setRequestHeader("Lang", lang)
    //     } else {
    //         xhr_1.setRequestHeader("Lang", "en")
    //     }
    //     xhr_1.setRequestHeader("User-Id", userId);
    //     xhr_1.setRequestHeader("Session-Id", token);
    //     xhr_1.setRequestHeader("Access-Control-Allow-Origin", "*");
    //     xhr_1.send();
    //     //对比令牌，成功后打开unity界面，失败后返回消息
    //     xhr = new XMLHttpRequest();
    //     //http://8.218.25.224:8049/v1/api/userProfile/get
    //     //8.218.25.224:8054
    //     xhr.open("GET", baseUrl + "/v1/api/user/getProfile?uid=" + userId, true);
    //     if (lang != "") {
    //         xhr.setRequestHeader("Lang", lang)
    //     } else {
    //         xhr.setRequestHeader("Lang", "en")
    //     }
    //     xhr.setRequestHeader("User-Id", userId);
    //     xhr.setRequestHeader("Session-Id", token);
    //     xhr.setRequestHeader("Access-Control-Allow-Origin", "*");
    //     xhr.onreadystatechange = function () {
    //         if (xhr.readyState == 4) {
    //             if (xhr.status == 200) {
    //                 var responseData = JSON.parse(xhr.responseText);
    //                 if (responseData["code"] == 200) {
    //                     CreateUnity(() => {
    //                         if ((responseData["data"]["head_oss_url"] == null || responseData["data"]["head_oss_url"] == "")) {
    //                             initSceneName = 'AvatarCreator';
    //                         }
    //                         unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', "UserInit_Ctrl|InitUserData^" + token + "^" + userId + "^" + lang + "^" + JSON.stringify(responseData["data"]));
    //                     });
    //                 } else ///////////////////////////////令牌识别失败
    //                 {
    //                     infoDiv.textContent = responseData["msg"].toString();
    //                     window.parent.postMessage("tokenCheckFailed", '*');
    //                 }
    //             } else {
    //                 console.log(xhr.readyState + ":" + xhr.status + ":" + xhr.statusText);
    //                 infoDiv.textContent = "";
    //                 window.parent.postMessage("netCheckFailed", '*');
    //             }
    //         } else {
    //             console.log("XMLHttpRequest:readyState=" + xhr.readyState + ":" + xhr.status);
    //         }
    //     }
    // }
    // else {
    //     infoDiv.textContent = "";
    //     window.parent.postMessage("tokenNotFound", '*');
    // }
}
  


 var sourceUnityGameObjectName = "";

function sendMessageToUnity(s) {
    //发送给unity
    //unityInstance.SendMessage('MyGameObject', 'MyFunction', 5);
    unityInstance.SendMessage('AvatarManager', 'SelectFileStart', s);
}

//==============================================================================================
function receiveMessageFromUnity(s) {
    //console.log(s);
    var receiveMessageStages = s.split('^');
    if (receiveMessageStages.length > 1) {
        switch (receiveMessageStages[0]) {
            case "clickSelectFileBtn":
                clickSelectFileBtn(receiveMessageStages[1]);
                break;
            case "selectInputButtonUpdate":
                var viewPos = receiveMessageStages[1];
                var viewPosVector = viewPos.split(',');
                var size = receiveMessageStages[2];
                var sizeVector = size.split(',');
                selectInputButtonUpdate(parseInt(viewPosVector[0]) / 100, parseInt(viewPosVector[1]) / 100, parseInt(sizeVector[0]), parseInt(sizeVector[1]));
                break;
            case "setSelectFileButtonState":
                setSelectFileButtonState(receiveMessageStages[1]);
                break;
            case "toScene":
                window.location.href = receiveMessageStages[1];
                break;
            case "applicationFocus":
                switch (receiveMessageStages[1]) {
                    case "True":
                        unity_canvas.focus();
                        break;
                }
                break;
            case "clipbord":
                ClipboardJS.copy(receiveMessageStages[1]);
                break;
        }
    }
    else {
        switch (s) {
            case "GC":
                break;
            case "Refresh":
                window.location.reload();
                break;
        }
    }
}
//var input  = document.createElement("input");
var input = document.getElementById("fileSelect");

function clickSelectFileBtn(name) {
    sourceUnityGameObjectName = name;
    input.click();
}

function fileImport() {
    //获取读取我文件的File对象
    var selectedFile = null;
    selectedFile = input.files[0];
    var maxByteCount = 1.6 * 1024 * 1024;
    if (/Android|webOS/i.test(navigator.userAgent)) {
        maxByteCount = 6 * 1024 * 1024;
    } else if (/iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        maxByteCount = 6 * 1024 * 1024;
    } else {
        maxByteCount = 6 * 1024 * 1024;
    }

    if (selectedFile.size > maxByteCount) {
        unityInstance.SendMessage(sourceUnityGameObjectName, "WebSelectFileStage", "^" + (maxByteCount / 1024 / 1024));
        input.value = "";
        return;
    }



    console.log(selectedFile);
    new Compressor(selectedFile, {
    quality: 0.7, // 设置压缩质量
    maxWidth: 640, // 设置最大宽度
    maxHeight: 640, // 设置最大高度
    success(result) {
        console.log(result);
        var reader = new FileReader();
        reader.readAsDataURL(result);
        reader.onload = function (e) {
        //console.log(this.result);
        var base64Str = selectedFile.name + "|" + this.result; // e.currentTarget.result.substring(e.currentTarget.result.indexOf(',') + 1);
        arr = [];
        step = 10240 * 4;
        for (var i = 0, l = base64Str.length; i < l; i += step) {
            arr.push(base64Str.slice(i, i + step))
        }
        unityInstance.SendMessage(sourceUnityGameObjectName, "WebSelectFileStage", selectedFile.name);
        for (i = 0; i < arr.length; i++) {
            unityInstance.SendMessage(sourceUnityGameObjectName, "WebSelectFileStage", i + "\\" + arr[i]);
            //sendMessageToUnity(arr[i]);
        }
        unityInstance.SendMessage(sourceUnityGameObjectName, "WebSelectFileStage", "");
        //console.log("==============================");
        e.re;
        selectedFile=null;
    }
    },
    error(err) {
        console.error(err.message);
        selectedFile=null;
    },
    });

}

function loadImageAndSendMessage(path) {
    var img = new Image();
    img.src = path; // 替换为实际图像的路径

    img.onload = () => {
        var canvas = document.createElement('canvas');
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext('2d');
        ctx.drawImage(img, 0, 0, img.width, img.height);

        // 从Canvas获取图像的base64编码数据
        var base64ImageData = canvas.toDataURL('image/jpeg');

        // 将图像数据发送到Unity脚本

        unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'MainUI|SelectFileStart^' + s);
        //unityInstance.SendMessage('AvatarManager', 'SelectFileStart', s);
    };
}
let video = document.querySelector('.video');
let canvas = document.querySelector('canvas');

function openx() {
    let constraints = {
        video: { //这里是摄像头的信息
            height: 500,
            width: 500
        },
        // audio: true,  //是否开启麦克风
    }
    let isok = navigator.mediaDevices.getUserMedia(constraints); //这里主要是用于请求用户打开摄像头的权限
    isok.then(res => { //可以看出是使用promise封装的 那么我们就可以使用then和catch
        video.srcObject = res; //用户允许时 将摄像头对象的画面转移到video上面
        video.play(); //打开video的画面
    }).catch((err) => {
        console.log(err) //拒绝时打印错误信息
    })
}

function pho() {
    canvas.getContext("2d").drawImage(video, 0, 0, 300, 300); //第一个参数为要截取的dom对象，第二个和第三个为xy轴的偏移值    3-4为截取图像的大小
}

function exit() {
    video.srcObject.getTracks()[0].stop(); //这里如果打开了麦克风、getTracks是一个数组，我们同样需要获取下标[1]来关闭摄像头 打开麦克风[0]就是麦克风
}
let resizeTimer;

function resetSize() {
    unity_container.style.top=0;
    var height=document.documentElement.clientHeight;
    var width=document.documentElement.clientWidth;
    canvas.style.height = document.documentElement.clientHeight + "px";
    canvas.style.width = document.documentElement.clientWidth + "px";
    handleResize();

    if(background!=null&&background.style.display!="none")
    {
        if(window.innerWidth/window.innerHeight>background.offsetWidth/background.offsetHeight)
        {
            background.style.width="100%";
            background.style.removeProperty("height");
        }
        else
        {     
            background.style.height="100%";
            background.style.removeProperty("width");           
        }
    }
}
// 监听窗口大小变化事件
window.addEventListener('resize', () => {
    // 在窗口大小变化时更新输出
    resetSize();
});


function onClickFileSelect() {
    input.value = "";
}
var windowWidth;
var windowHeight;
function handleResize() {
    if (unityInstance != null) {
        windowWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
         windowHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
        unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'MainUI|OnWebWindowsSizeReset^' + windowWidth + "," + windowHeight);
    }

}
setInterval(resetSize, 200);


var isSelectFileButtonState = true;
var _viewPosX = 0;
var _viewPosY = 0;
var _sizeX = 0;
var _sizeY = 0;

function selectInputButtonUpdate(viewPosX, viewPosY, sizeX, sizeY) {
    _viewPosX = viewPosX;
    _viewPosY = viewPosY;
    _sizeX = sizeX;
    _sizeY = sizeY;

    if (isSelectFileButtonState) {

        input.style.left = viewPosX + "%";
        input.style.top = viewPosY + "%";
    }
    input.style.width = sizeX + 'px';
    input.style.height = sizeY + 'px';
}

function setSelectFileButtonState(state) {
    switch (state) {
    case "true":
        //visibility: visible
        isSelectFileButtonState = true;
        selectInputButtonUpdate(_viewPosX, _viewPosY, _sizeX, _sizeY)
        break;
    case "false":
        isSelectFileButtonState = false;
        input.style.left = 200 + "%";
        input.style.top = 200 + "%";
        break;
    }
}

function loadBody() {
    if (xhr != null) {
        xhr.send();
    }
}


function finish() {
    // window.history.go(-1);
    // window.close();

}

function showInput() {
    input.style.display = "block";
}

function hideInput() {
    input.style.display = "none";
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//父级监听信息
window.addEventListener('message', function (e) {
    if(e.data.toString().includes("^"))
    {
        var msgStage= e.data.split("^");
     switch(msgStage[0]){
      case "sendMeetingScreanShot":
      var base64Str =  msgStage[1];
        arr = [];
        step = 10240 * 4;
        for (var i = 0, l = base64Str.length; i < l; i += step) {
            arr.push(base64Str.slice(i, i + step))
        }
        unityInstance.SendMessage("LiveStreamManager", "ReciveLiveStreamShotStage","Enter");
        for (i = 0; i < arr.length; i++) {
            unityInstance.SendMessage("LiveStreamManager", "ReciveLiveStreamShotStage",arr[i]);
            //sendMessageToUnity(arr[i]);
        }
        unityInstance.SendMessage("LiveStreamManager", "ReciveLiveStreamShotStage", "Finish");
        break;
        case "isMeetingOpen":
        unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'GameManager|isMeetingOpen^');
        break;
        case "isMeetingClose":
        unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'GameManager|isMeetingClose^');
        break;
     }
    } 
});

function clickBrochures(){

//unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'AboutUsPanel|BrochuresButtonClick^');
document.getElementById("app").style.display = "none";
unityInstance.SendMessage("AboutUsPanel","BrochuresButtonClick");

}
function clickLinks(){

//unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'AboutUsPanel|LinksButtonClick^');
document.getElementById("app").style.display = "none";
unityInstance.SendMessage("AboutUsPanel","LinksButtonClick");

}

function closeWindow(){
    document.getElementById("app").style.display = "none";
    //unityInstance.SendMessage('WebMessage_Ctrl', 'ReciveMessage', 'AboutUsPanel|CloseButtonClick');
    unityInstance.SendMessage("AboutUsPanel","CloseButtonClick");
}

function openWindow(){
    document.getElementById("app").style.display = "block";
}
document.addEventListener("visibilitychange", function() {
    if (document.visibilityState === 'visible') {
        unity_canvas.focus();
    } else {
    }
    });

