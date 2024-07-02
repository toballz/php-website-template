<?php session_name(md5("newsesh();"));session_start();
class db{
    const servername = "localhost"; 
    const username = "root";//u723978224_wwwwwwwww
    const password = "";//wwwwwwwwwww
    const dbname = "continuefrom";//u723978224_eeeeeeeeee
    // Create a connection
    static function conn(){
        $rr= new mysqli(self::servername, self::username, self::password, self::dbname);
        if($rr->connect_error){
            die("Connection err: ");
        }else{return $rr;}
    }
    static function stmt($stm){
        return mysqli_query(self::conn(), $stm);
    }
}
const __dir=__dir__;
//donot delete edit as needed
include 'aapp/e39e22bb8b4e0f238180959c70eaf4c7.php';
?>