
public class User {

	//needed vars name, password, id, status, currentgame,friends, stats, notifications
	
	private String name = "";
	private String password = "";
	private int id = -1;
	private boolean isOnline = false;
	private int currentGame = -1;
	private String[] friends = {"you have no friends"};
	private String[] stats = {"you suck!"};
	private String[] notifications = {"you are alone"};
	
	
	
	public User() {
		// TODO Auto-generated constructor stub
	}



	public String[] getNotifications() {
		return notifications;
	}
	public void setNotifications(String[] notifications) {
		this.notifications = notifications;
	}
	public String[] getStats() {
		return stats;
	}
	public void setStats(String[] stats) {
		this.stats = stats;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getPassword() {
		return password;
	}
	public void setPassword(String password) {
		this.password = password;
	}
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public boolean isOnline() {
		return isOnline;
	}
	public void setOnline(boolean isOnline) {
		this.isOnline = isOnline;
	}
	public int getCurrentGame() {
		return currentGame;
	}
	public void setCurrentGame(int currentGame) {
		this.currentGame = currentGame;
	}
	public String[] getFriends() {
		return friends;
	}
	public void setFriends(String[] friends) {
		this.friends = friends;
	}

}
