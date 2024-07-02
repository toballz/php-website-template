<?php /*  
    required for all pages using(this page)
        $headTitle = "";
        $headDescription = "";
        $headKeywords = "";
    not required
        $headCanonical = "";
*/?>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="robots" content="index, follow">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<!-- e -->
<title><?php if(isset($headTitle)){echo $headTitle;}else{$a="@@@^^no title%";echo $a;die($a);}?></title>
<base href="<?php echo site::url("domain");?>">
<link rel="shortcut icon" href="/favicon.ico?<?php echo $rs;?>">
<link rel="icon" type="image/x-icon" href="/favicon.ico?<?php echo $rs;?>">
<?php if(isset($headCanonical)){echo $headCanonical;}else{?><link rel="canonical" href="<?php echo site::url("domain").explode("?",site::url("uri"))[0];?>"/><?php }?>
<meta name="keywords" content="<?php if(isset($headKeywords)){echo $headKeywords;}else{$a="@@@^^no Keywords%";echo $a;die($a);}?>">
<!-- 1 -->
<meta name="robots" content="all">
<meta name="revisit-after" content="1 days">
<!--
<meta content="dark" name="color-scheme">
<meta http-equiv="refresh" content="660">
-->
<meta name="theme-color" content="#151d3d">
<meta name="description" content="<?php if(isset($headDescription)){echo $headDescription;}else{$a="@@@^^no description%";echo $a;die($a);}?>">
<!-- a -->
<script type="text/javascript" src="static/cj/jq.js?<?php echo $rs;?>"></script>
<link rel="stylesheet" href="static/cj/css.css?<?php echo $rs;?>">
 <!-- t -->
<style type="text/css"> </style>
<script>;</script> 
