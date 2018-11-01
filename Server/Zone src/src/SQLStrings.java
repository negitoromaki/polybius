
public class SQLStrings {

	//messages
	public String publicMSG = "SELECT message, sender FROM users.msgs WHERE level=? AND levelname=? ORDER BY time DESC LIMIT ?";
	public String privateMSG = "SELECT message, sender, reciever FROM users.msgs WHERE level=? AND (sender=? OR reciever=?)  ORDER BY time DESC LIMIT ?";
	public String sendMSG = "INSERT INTO users.msgs (sender, reciever, time, message, level, levelname) VALUES (?, ?, ?, ?, ?, ?)";
	public String getFriends = "SELECT friends FROM users.userdata WHERE username =?";
	public String updateFriend = "UPDATE users.userdata SET friends = ? WHERE username = ?";
	public String getUserFromID = "SELECT username FROM users.userdata WHERE id =?";
	public String getIDFromUser = "SELECT id FROM users.userdata WHERE username =?";
	public String getUsers = "SELECT * username id FROM users.userdata";
	public String login = "SELECT * FROM users.userdata WHERE username =? AND password=?";
	public String goOnline = "UPDATE users.userdata SET isonline = ? WHERE username=?";
	public String logout = "UPDATE users.userdata SET isonline = 0 WHERE username=?";
	public String clearMSG = "DELETE FROM users.msgs WHERE sender = ? AND reciever=?" ;


}
