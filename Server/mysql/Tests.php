<?php
$servername="localhost";
$username="root";
$password="root";
$dbname="users";

$mysql = new mysqli($servername, $username, $password, $dbname);

$mysql->select_db($dbname);
for($g=0;$g<=10000; $g++){
	$sql = "INSERT INTO userdata (username , password, email, isonline, currentgame, friends, statistics, notification)
	VALUES ('Greg', 'PWD', 'Real@email.com', 1, 3, 'John, Greg, Billy', '1 2 3 4 5 6', 'Invite here!')";
	if($mysql->query($sql)==TRUE){
		echo "New record created";
	}else{
		echo "Error1";
	}
}


//$sqls = "INSERT INTO userdata (username , password, email, isonline, currentgame, friends, statistics, notification)
//VALUES ('G')";





/*$data = "SELECT * FROM userdata";

$result= $mysql->query($data);
echo $result;
if($mysql->query($sqls)==TRUE){
	echo "New record created";
}else{
	echo "Error2";
}*/

$mysql->close();
?>
