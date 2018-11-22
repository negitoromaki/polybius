package Core;

import com.smartfoxserver.v2.entities.User;

public class RoomData {
	
	private String name = "";
	private User host;
	
	
	public RoomData(String roomName){
		this.name = roomName;
	}
	
	
	
	
	public String getName(){
		return this.name;		
	}
	public void setName(String name){
		this.name = name;
	}



	public User getHost() {
		return host;
	}



	public void setHost(User host) {
		this.host = host;
	}

}
