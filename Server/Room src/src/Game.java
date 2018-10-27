
import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.api.ISFSApi;
import com.smartfoxserver.v2.api.SFSApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;


public abstract class Game {
	
	private String gameName = "Default";
	
	public abstract void start();
	
	public abstract void updateScore(int pID, int amount);
	public abstract int[] getScores();
	public abstract void end(int endCode);
	public abstract void input(ISFSObject o);
	public ISFSApi api = SmartFoxServer.getInstance().getAPIManager().getSFSApi();
	
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
