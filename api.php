<?php include("coff.php");

// $genCors = bin2hex(random_bytes(32)) . "." . time();
$namer = new stdClass();
$namer->action = "action";
$namer->getLogin = "guest-login"; 
$namer->token = "cors_token";
$namer->getSignup = "2bu4tywnr7";
$namer->userProfile = "nw95yiuw6iu";
$namer->e="wme68-uyd9-89n8ar-fy-fh-j";

class h {
    static function encodeStr($v) {
        return str_rot13(base64_encode($v));
    }
    static function decodeStr($v) {
        return base64_decode(str_rot13($v));
    }
    static function passwordHash($v) {
        $sam="gyuf\u1240,@.";
        return md5(base64_encode($v.$sam));
    }
    static function   generateAlphanumeric($length) {
        $characters = '0123456789abc1def2gh3ijkl4mno5pqrs6tuvw7xy8z';
        $randomString = substr(str_shuffle(str_repeat($characters, 5)), 8, $length); // Repeat to ensure enough characters
        return $randomString;
    }
}

// Default response
$response = [
    "code" => 100,
    "message" => "Nothing to see",
];
$good2go = false;

// Read and decode the JSON input
$phpInput = file_get_contents('php://input');
$pI = json_decode($phpInput, true); 

// Check if the request method is POST
if ($_SERVER["REQUEST_METHOD"] === "POST") { 
    if (empty($pI[$namer->token]) || strlen($pI[$namer->token]) < 30 || empty($pI[$namer->action])) {
        $response['code'] = 400;
        $response['message'] = "Invalid or missing CORS token!";
    } else { 
        $good2go = true;
        $response['code'] = 409;
        $response['message'] = "Nothing to process...";
    }
} else {
    // If the request method is not POST, return a 404 code
    $response['code'] = 404;
    $response['message'] = "Request not permitted!";
}

if ($good2go) {
    // Check for the 'action' key and process it if it exists
    if ($pI[$namer->action] === $namer->getLogin) {
        $email = isset($pI['login_email']) ? trim(db::real_escape_string(h::decodeStr($pI['login_email']))) : $namer->e;
        $password = isset($pI['login_password']) ? h::passwordHash(h::decodeStr($pI['login_password'])) : rand();
 
        $user_stmt=db::stmt("SELECT * FROM `users` WHERE `user_email` = '$email' AND `user_password` = '$password'");

        if(mysqli_num_rows($user_stmt) == 1)
        {
            $user_fetch_assoc=mysqli_fetch_assoc($user_stmt);
            $response['code'] = 200;
            $response['message']=1;
            $response["user_id"] = $user_fetch_assoc['user_id'] ;     
        }else{
            $response['code'] = 404;
            $response['message'] =  "Wrong username or password.";   
        }
    }
    else if($pI[$namer->action] === $namer->getSignup){
        $uid= h::generateAlphanumeric(20);
        $email = isset($pI['email']) ? trim(db::real_escape_string(h::decodeStr($pI['email']))) : $namer->e;
        $password = isset($pI['password']) ? h::passwordHash(h::decodeStr($pI['password'])) : rand();
        $fullname = isset($pI['fullname']) ? trim(db::real_escape_string(h::decodeStr($pI['fullname']))) : $namer->e; 

        if(db::stmt("INSERT INTO `users` 
                (`user_id`, `user_fullname`, `user_email`, `user_password`, `user_premium`, `user_active`, `user_datecreated`) 
                    VALUES 
                ('$uid', ".(empty($fullname)?"NULL":"'$fullname'").", '$email', '$password', '0', '1', current_timestamp());"))
        { 
            $response['code'] = 200;
            $response['message'] = "ok";
            $response['user_id']= $uid;     
        }else{
            $response['code'] = 404;
            $response['message'] = "Account not created.";   
        }
    
    }else if($pI[$namer->action] === $namer->userProfile){ 
        $get_userId = isset($pI['user_id']) ? trim(db::real_escape_string(h::decodeStr($pI['user_id']))) :$namer->e; 
        $userGetInfo=db::stmt("SELECT * FROM `users` WHERE `user_id`='$get_userId';");
 
        if(mysqli_num_rows($userGetInfo) == 1)
        { 
            $userGetInfo_fetchAssoc=mysqli_fetch_assoc($userGetInfo);
            $response['code'] = 200;
            $response['message'] = "ok";
            $response['user_info']= $userGetInfo_fetchAssoc;     
        }else{
            $response['code'] = 404;
            $response['message'] = "No user found.";   
        }
    }











    //else
    //else
    else {
        $response['code'] = 400;
        $response['message'] =  " Invalid action! " ;
    }
}

// Set the response content type and output the JSON response
header("Content-Type: application/json");
echo json_encode($response);
