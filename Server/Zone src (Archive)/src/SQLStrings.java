
public class SQLStrings {
	
	//messages
	public String publicMSG = "SELECT message, sender FROM users.msgs WHERE level=? AND levelname=? ORDER BY time DESC LIMIT ?";
	public String privateMSG = "SELECT message, sender, reciever FROM users.msgs WHERE level=? AND (sender=? AND reciever=?)  ORDER BY time DESC LIMIT ?";
	public String sendMSG = "INSERT INTO users.msgs (sender, reciever, time, message, level, levelname) VALUES (?, ?, ?, ?, ?, ?)";
	public String getFriends = "SELECT friends FROM users.userdata WHERE username =?";
	public String updateFriend = "UPDATE users.userdata SET friends = ? WHERE username = ?";
	public String getUserFromID = "SELECT username FROM users.userdata WHERE id =?";
	public String getIDFromUser = "SELECT id FROM users.userdata WHERE username =?";
	public String getUsers = "SELECT username, id, private FROM users.userdata";
	public String login = "SELECT * FROM users.userdata WHERE username =? AND password=?";
	public String goOnline = "UPDATE users.userdata SET isonline = ? WHERE username=?";
	public String logout = "UPDATE users.userdata SET isonline = ? WHERE username=?";
	public String setPrivate = "UPDATE users.userdata SET private = ? WHERE username=?";
	public String addRoom = "INSERT INTO users.rooms (roomName, gameType, latcord, longcord) VALUES (?, ?, ?, ?)";
	public String getRooms = "SELECT roomName, roomID, gameType, latcord, longcord FROM users.rooms";
	public String removeRoom = "DELETE FROM users.rooms WHERE roomName = ?";
	public String clearMSG = "DELETE FROM users.msgs WHERE sender = ? AND reciever=?" ;
	

}
