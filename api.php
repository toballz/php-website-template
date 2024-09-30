<?php include("coff.php");

// $genCors = bin2hex(random_bytes(32)) . "." . time();
$namer = new stdClass();
$namer->action = "action";
$namer->getLogin = "guest-login"; 
$namer->token = "cors_token";
$namer->getSignup = "2bu4tywnr7";

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


        $email = isset($pI['login_email']) ? db::real_escape_string(h::decodeStr($pI['login_email'])) : '';
        $password = isset($pI['login_password']) ? h::passwordHash(h::decodeStr($pI['login_password'])) : rand();
 
        $user_stmt=db::stmt("SELECT * FROM `users` WHERE `user_email` = '$email' AND `user_password` = '$password'");

        if(mysqli_num_rows($user_stmt) == 1)
        {
            $user_fetch_assoc=mysqli_fetch_assoc($user_stmt);
            $response['code'] = 200;
            $response['message'] = array("user_id"=>$user_fetch_assoc['user_id']);     
        }else{
            $response['code'] = 404;
            $response['message'] =  "Incorrect user name or Password.".$password;   
        }
    }
    else if($pI[$namer->action] === $namer->getSignup){
        $email = isset($pI['email']) ? db::real_escape_string(h::decodeStr($pI['email'])) : '';
        $password = isset($pI['password']) ? h::passwordHash(h::decodeStr($pI['password'])) : rand();
        $fullname = isset($pI['fullname']) ? db::real_escape_string(h::decodeStr($pI['fullname'])) : ''; 

        if(db::stmt("INSERT INTO `users` 
                (`user_id`, `user_fullname`, `user_email`, `user_password`, `user_premium`, `user_active`, `user_datecreated`) 
                    VALUES 
                (NULL, ".(empty($fullname)?"NULL":"'$fullname'").", '$email', '$password', '0', '1', current_timestamp());"))
        { 
            $response['code'] = 200;
            $response['message'] = "ok";     
        }else{
            $response['code'] = 404;
            $response['message'] = "Account not created.";   
        }
    
    }
    else {
        $response['code'] = 400;
        $response['message'] =  " Invalid action! " ;
    }
}

// Set the response content type and output the JSON response
header("Content-Type: application/json");
echo json_encode($response);
