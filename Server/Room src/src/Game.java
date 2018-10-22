
import com.smartfoxserver.v2.entities.User;

public class Game {
	
	private String gameName = "Default";
	
	public void start() {
	}
	
	public void end(int endCode){
		
		
	}
	
	public void lostUser(User user){
		end(1);
	}

	public String getGameName() {
		return gameName;
	}

	public void setGameName(String gameName) {
		this.gameName = gameName;
	}

}
