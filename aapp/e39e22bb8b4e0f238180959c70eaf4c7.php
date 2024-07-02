<?php
$rs="0x14";//change to reload css,js,src
class site{
    const name="website example name, LLC";
    //
    static function isSecure(){if((!empty($_SERVER['HTTPS']) && $_SERVER['HTTPS'] !== 'off') || $_SERVER['SERVER_PORT'] == 443){return "https:";}else{return "http:";}}
    /// url(servername|uri|full|domain)
    public static function url($vrvi){
        if ($vrvi=="domain"){
            return self::isSecure()."//".$_SERVER['SERVER_NAME'];
        }else if ($vrvi=="servername"){
            return $_SERVER['SERVER_NAME'];
        }else if($vrvi=="uri"){
            return $_SERVER['REQUEST_URI'];
        }else if($vrvi=="full"){
            return self::isSecure()."//".$_SERVER['SERVER_NAME'].$_SERVER['REQUEST_URI'];
        }else{
            die("url Erreor!!!!");
        }
    }
    //
    const head=__dir."/aapp/head.php";
    const header=__dir."/aapp/header.php";
    const footer=__dir."/aapp/footer.php";

}
class tools{
    static function replaceUriNoChar($gsa){
        //returns onlt a-zA-Z0*9\_\.\ \-
        $ta=preg_replace('/[^a-zA-Z0-9\_\.\ \-]/s','',$gsa);
        return str_replace(" ","-",$ta);
    }
    //obviously change
    const passwordsalt='\u2315c#7\ &8*\u0014 0x28';
}