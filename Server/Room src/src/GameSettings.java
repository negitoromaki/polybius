import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GameSettings extends BaseClientRequestHandler{
	private String gameName = "";
	private int maxPlayers = 4;
	private int numPlayers = 0;
	private int numSpectators = 0;
	private boolean gameInProgress = false;
	private Game currentGame;
	
	
	
	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		String cmd = arg1.getUtfString("cmd");
		
		
	}
	

	public String getGameName() {
		return gameName;
	}

	public String setGameName(String gameName) {
		this.gameName = gameName;
		
		try {
			Class c = Class.forName(gameName);
			Game g = (Game)c.newInstance();
			return setCurrentGame(g) == true? "pass": "fail";
			
		} catch (ClassNotFoundException | InstantiationException | IllegalAccessException e) {
			// TODO Auto-generated catch block
			return "fail: " + e.getMessage();
		}
		
		
		
		
	}

	public int getMaxPlayers() {
		return maxPlayers;
	}

	public void setMaxPlayers(int maxPlayers) {
		this.maxPlayers = maxPlayers;
	}

	public int getNumPlayers() {
		return numPlayers;
	}

	public void setNumPlayers(int numPlayers) {
		this.numPlayers = numPlayers;
	}

	public int getNumSpectators() {
		return numSpectators;
	}

	public void setNumSpectators(int numSpectators) {
		this.numSpectators = numSpectators;
	}

	public boolean isGameInProgress() {
		return gameInProgress;
	}
 
	public void setGameInProgress(boolean gameInProgress) {
		this.gameInProgress = gameInProgress;
	}

	public Game getCurrentGame() {
		return currentGame;
	}

	public boolean setCurrentGame(Game game) {
		if(game == null)
			return false;
		this.currentGame = game;
		return true;
	}

	

}
