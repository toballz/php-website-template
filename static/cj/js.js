//reload page after gaining back focus
window.addEventListener('focus', function() {
    // location.reload();
 });
//BASE URL    
var bH = document.baseURI;
///page loader change to what ever
loader={
    start: function(){$("body").append('<div id="loaderstartstop" style="position:fixed;top:0;left:0;width:100%;height:100%;z-index:1212;background: rgb(0 0 0 / 69%);display:flex;justify-content: center;align-items:center;">'+
        '<div style="height:95px;width:95px"><svg style="width:100%;height:100%" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid" width="200" height="200" style="shape-rendering: auto; display: block; background: rgb(255, 255, 255);" xmlns:xlink="http://www.w3.org/1999/xlink"><g><rect x="19" y="19" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0s" calcMode="discrete"></animate>'+
        '</rect><rect x="40" y="19" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.125s" calcMode="discrete"></animate>'+
        '</rect><rect x="61" y="19" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.25s" calcMode="discrete"></animate>'+
        '</rect><rect x="19" y="40" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.875s" calcMode="discrete"></animate>'+
        '</rect><rect x="61" y="40" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.375s" calcMode="discrete"></animate>'+
        '</rect><rect x="19" y="61" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.75s" calcMode="discrete"></animate>'+
        '</rect><rect x="40" y="61" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.625s" calcMode="discrete"></animate>'+
        '</rect><rect x="61" y="61" width="20" height="20" fill="#505378">'+
        '<animate attributeName="fill" values="#ffffff;#505378;#505378" keyTimes="0;0.125;1" dur="1s" repeatCount="indefinite" begin="0.5s" calcMode="discrete"></animate>'+
        '</rect><g></g></g> </svg></div>'+
        '</div>');
    },
    stop:function(){
        setTimeout(function(){ $("#loaderstartstop").remove(); },1245);
    } 
}

!(function(){
// Definer ************************************************** */

  //get response from https
    function httpResponse(httpResData, getFunction){
        if(getFunction && (httpResData.code in getFunction) ){
            getFunction[httpResData.code]();
            console.log(getFunction );
            return;
        } 
        if(httpResData.code == 301){
            if(httpResData.message == "reload"){
                location.reload();
            }else{
                window.location.href=httpResData.message;
            }
        }else{
            alert(httpResData.message);
        }  
    }
    //share product
   $(".clicktosharebutton").click(async function(){ await navigator.share({ title: document.title, text: document.title, url: window.location.href,});});

   //img lazyload
   //create a better lazyload on scroll
    $("[data-srcimg]").each(function(){
        var ts=$(this);
        setTimeout(function(){
            ts.attr("src",ts.attr("data-srcimg"));
            ts.attr("data-srcimg","");
        },600);
    });

 // END definer ************************************************** */
 //
 //
 //
 













 

//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//end
}());  
 
