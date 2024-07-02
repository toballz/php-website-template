<?php include_once("conf.php");?>
<?php
header('Content-Type: application/json');
//**//forgot password
//**//contact us

//return arr['code']['message']
if (isset($_POST['api'])) {

   if(isset($_POST['t']) && ($_POST['t']=="create:signup")){
       $uid=rand(99,999999);
       $uname=mysqli_real_escape_string(db::conn(),trim(strtolower($_POST['uname'])));
       $email=mysqli_real_escape_string(db::conn(),trim(strtolower($_POST['email'])));
       $pword=$_POST['pwod'];
       $hashedpassword=md5($pword.tools::passwordsalt);

       if(db::stmt("INSERT INTO `users` (`username`, `password`, `email`, `id`, `datetime`) VALUES ('$uname', '$hashedpassword', '$email', '$uid', current_timestamp());")){
        $Jarr['code']=200;
        $Jarr['message']="User created successfully";
        $Jarr['userid']="{$uid}";

       }else{$Jarr['code']=500; $Jarr['message']="Account not created. Try a different username or email"; }
   }

   if(isset($_POST['t']) && ($_POST['t']=="delete:account")){
       $uid=mysqli_real_escape_string(db::conn(),trim(strtolower($_POST['uid'])));  

       if(db::stmt("UPDATE `users` set `email`=CONCAT( '-deleted-',email), `username`=CONCAT('-deleted-',username), `premuimType`=CONCAT(premuimType,'-deleted-') WHERE `id`='$uid'")){
        $Jarr['code']=200;
        $Jarr['message']="Account deleted";
       } else{$Jarr['code']=500; $Jarr['message']="Could not delete account. Contact Us."; }
   }

   //update email user preference data
   if(isset($_POST['t']) && $_POST['t']=="update:Email"){
       $email=mysqli_real_escape_string(db::conn(),trim($_POST['email']));
       $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname']));

       if(db::stmt("UPDATE `users` set `email`='$email' WHERE `id`='$uname'")){
        $Jarr['code']=200;
        $Jarr['message']="Email updated successfully";
       }else{$Jarr['code']=500; $Jarr['message']="Email was not updated try again or use another email!"."\n"."UPDATE `users` SET `email`='$email' WHERE `id`='$uname';"; }
   }
   if (isset($_POST['t']) && $_POST['t']=="get"){
       $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname']));
       $gtt=db::stmt("SELECT * FROM `usersdatas` WHERE `userfrom` = '$uname' ORDER BY `dateInsertModify` DESC;");
       if($gtt){
         if($gtt->num_rows > 0){
            while ($getInfo=mysqli_fetch_assoc($gtt)){
                $faa=json_decode($getInfo['dataJson']);
                $faa->sha256hash=$getInfo['HashUserfromJsonDataH'];
                $jsonArt[]=$faa;
            }
            $Jarr['code']=200;
            $Jarr['message']=json_encode($jsonArt);
         }else{$Jarr['code']=204; $Jarr['message']="You have no data! Start uploading."; }
       }else{$Jarr['code']=500; $Jarr['message']="There was an Error!"; }
    }else if (isset($_POST['t']) && $_POST['t']=="getinfoexceptdatas"){
        $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname']));
       $gtt=db::stmt("SELECT `username`,`email`,`datetime`,`premiunm`,`id` FROM `users` WHERE `id` = '$uname' LIMIT 1;");
       if($gtt){if($gtt->num_rows > 0){
            while ($getInfo=mysqli_fetch_assoc($gtt)){
                $jsonArt['email']=$getInfo['email'];
                $jsonArt['premiunm']=$getInfo['premiunm'];
                $jsonArt['datetime']=$getInfo['datetime'];
                $jsonArt['id']=$getInfo['id'];
                $jsonArt['username']=$getInfo['username'];
            }
            $Jarr['code']=200;
            $Jarr['message']=json_encode($jsonArt);
        }else{
            $Jarr['code']=500;
            $Jarr['message']="The username '".$uname."' was not found; there was no data!";}
       }else{$Jarr['code']=500; $Jarr['message']="There was an Error!"; }
    }

    if (isset($_POST['t']) && $_POST['t']=="access:login"){
               $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname']));
               $upwod=md5($_POST['pwod'].tools::passwordsalt);

               $gtt=db::stmt("SELECT * FROM `users` WHERE `username`='$uname'  AND `password`='$upwod' LIMIT 1;");
               if($gtt){
                 if($gtt->num_rows > 0){
                    $fetchAssoc=mysqli_fetch_assoc($gtt);
                    $Jarr['code']=200;
                    $Jarr['message']="Logged in.";
                    $Jarr['userid']= "{$fetchAssoc['id']}";
                 }else{$Jarr['code']=204; $Jarr['message']="Wrong Username or Password";}
               }else{$Jarr['code']=500; $Jarr['message']="User wasn't found!";}
        }

        if (isset($_POST['t']) && $_POST['t']=="insert"){
            $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname']));
            $fds= trim(($_POST['jasonData']));
            $jsonArr=mysqli_real_escape_string(db::conn(),$fds);
            $HashIfExist=hash('sha256', $uname.$jsonArr);


            $getIFpremium=db::stmt("SELECT users.premiunm FROM `users` WHERE users.id='$uname';");
            $getLengtghdatas=db::stmt("SELECT `idr` FROM `usersdatas` WHERE `userfrom` = '$uname' ");
        if(($getLengtghdatas->num_rows >=5) && (mysqli_fetch_assoc($getIFpremium)['premiunm']!=1)){
            $Jarr['code']=404;
            $Jarr['message']="You have used up the maximum number of times to upload data. Upgrade to a premium account!";
        }else{
           $gtt=db::stmt("INSERT INTO `usersdatas` (`userfrom`, `dataJson`,`HashUserfromJsonDataH`) VALUES ('$uname', '". ($jsonArr)."','$HashIfExist') ON DUPLICATE KEY UPDATE dateInsertModify=now();");
             
           if($gtt){
                $Jarr['code']=200;
                $Jarr['message']="successfully uploaded.";
           }else{$Jarr['code']=500;$Jarr['message']="There was an error uploading data:"; }
        }

    }

    //delete from userdatas sql
    if (isset($_POST['t']) && $_POST['t']=="deleteData"){
       $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname']));
       $DeleteHash=mysqli_real_escape_string(db::conn(),trim($_POST['deleteHash'])); 
       $gtt=db::stmt("DELETE FROM `usersdatas` WHERE `HashUserfromJsonDataH` = '$DeleteHash'");
       if($gtt){
            $Jarr['code']=200;
            $Jarr['message']="Data successfully Deleted.";
       }else{$Jarr['code']=500;$Jarr['message']="There was an error deleting data:"; }
    }

    //update to premium acc
    if (isset($_POST['t']) && $_POST['t']=="update:Premium"){
       $pretYpe=mysqli_real_escape_string(db::conn(),trim($_POST['pretYpe'])); 
       $uname=mysqli_real_escape_string(db::conn(),trim($_POST['uname'])); 
       $gtt=db::stmt("UPDATE `users` SET `premiunm` = '1', `premuimType`='$pretYpe' WHERE `id` = '$uname';");
       if($gtt){
            $Jarr['code']=200;
            $Jarr['message']="Successfully upgraded to premium.";
       }else{$Jarr['code']=500;$Jarr['message']="There was an error upgrading to premium."; }
    }
    //SENT CONTACT EMAIL to business yahoo email
    if (isset($_POST['t']) && $_POST['t']=="contact:businessEmail"){
       $fname=mysqli_real_escape_string(db::conn(),trim($_POST['fname'])); 
       $vemail=mysqli_real_escape_string(db::conn(),trim($_POST['vemail'])); 
       $bphone=mysqli_real_escape_string(db::conn(),trim($_POST['bphone'])); 
       $rmessage=mysqli_real_escape_string(db::conn(),trim($_POST['rmessage'])); 
       $gtt=mail("inquiry@".site::url("servername").",elisha4b@yahoo.com", "From Fodonn aPP, SHARE:{$vemail}", "FullName: ".$fname."\nEmail: {$vemail}\nPhone: {$bphone}\nMessage: {$rmessage}");
       if($gtt){
            $Jarr['code']=200;
            $Jarr['message']="You successfully messaged us. You will hear from us soon.";
       }else{$Jarr['code']=500;$Jarr['message']="Please try again."; }
    }
    //forgot password
    if (isset($_POST['t']) && $_POST['t']=="forgot:password" && isset($_POST['unameoremail'])){
       $unameoremail=mysqli_real_escape_string(db::conn(),trim($_POST['unameoremail']));
       $gtt=db::stmt("SELECT `id`,`username`,`email` FROM `users` WHERE `username` = '$unameoremail' OR `email`='$unameoremail' LIMIT 1;");
       if($gtt){
            if($gtt->num_rows == 1){
                $fetchAssoci=mysqli_fetch_assoc($gtt);
                $getContentOfResetHtml=file_get_contents("https://fodonn.com/resetpow.php?resetp=".$fetchAssoci['id']);
                
                $mailto = $fetchAssoci['email'];
                $mailsubject = "Fodonn - Share; Reset Your PassWord!";
                $mailheaders  = "From: noreply@fodonn.com\r\n";
                //$mailheaders .= "Reply-To: " . strip_tags($_POST['req-email']) . "\r\n";
                $mailheaders .= "MIME-Version: 1.0\r\n";
                $mailheaders .= "Content-Type: text/html; charset=UTF-8\r\n";
                $mailmessage = $getContentOfResetHtml;

                $mailIsSent=mail($mailto,$mailsubject,$mailmessage,$mailheaders );
                
                if($mailIsSent){
                    $Jarr['code']=200;
                    $Jarr['message']="Your password reset link has been sent to your email. Also check your spam folders.";
               }else{$Jarr['code']=500;$Jarr['message']="There was an error sending you a reset link to your email. Please contact us.";}
            }else{
                $Jarr['code']=404;$Jarr['message']="This username or email was not found.";
            }

       }else{$Jarr['code']=500;$Jarr['message']="Internal error! Please contact us.";}
    }
    if (isset($_POST['t']) && $_POST['t']=="update:tk" && isset($_POST['tk'])){
       $tk=mysqli_real_escape_string(db::conn(),trim($_POST['tk']));
       $gtt=db::stmt("UPDATE `traffic` SET `countt` = countt + 1 WHERE `fromwhichwebsite` = '$tk'");
        //returns null
        if($gtt){$Jarr['code']=200;$Jarr['message']="true";}
        else{$Jarr['code']=404;$Jarr['message']="false";} 
    }
}
//send files
elseif (isset($_FILES['t']) && explode(";;",$_FILES['t']["name"])[0]=="upload:filE") {
    $uploads_dir = './a';
    if(!is_dir($uploads_dir)){mkdir($uploads_dir, 0777, true);}
    $uname=explode(";;",$_FILES['t']["name"])[1];
    $tmp_name = $_FILES["flieContent"]["tmp_name"];
    $FileName = ($_FILES["flieContent"]["name"]);
    $file_nameHasg=rand(99,9999999).".".$FileName;
    if(move_uploaded_file($tmp_name, "$uploads_dir/$file_nameHasg")){
       $gtt=db::stmt("INSERT INTO `usersdatas` (`userfrom`, `dataJson`,`HashUserfromJsonDataH`) VALUES ('$uname', '{\"StrType\":\"File\",\"StrVal\":\"".$file_nameHasg."\"}','".hash('sha256', $file_nameHasg)."') ON DUPLICATE KEY UPDATE dateInsertModify=now();");
        if($gtt){
            $Jarr['code']=200;
            $Jarr['message']="Your file has been uploaded!.";
        }else{
            unlink($uploads_dir."/".$FileName);
            $Jarr['code']=500;$Jarr['message']="Your file was not uploaded!";
       }
    }else{
        unlink($uploads_dir."/".$FileName);
        $Jarr['code']=500;$Jarr['message']="Your file was not uploaded!!";
    }
}else{
    header("LOCATION: /");
}


//eventusakllluty 00000000000000000000
if(isset($Jarr)){echo json_encode($Jarr);}else{echo "{\"code\":500,\"message\":\"Internal Error! No response\"}";}



/*ob_start();
print_r($_FILES);
$textualRepresentation = ob_get_contents();
ob_end_clean();
file_put_contents("a/x.txt", $textualRepresentation);
*/