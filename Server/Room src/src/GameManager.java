import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GameManager extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		String cmd = arg1.getUtfString("cmd");
		
		if(cmd.equals("changegame")){
			String gameNameNew = arg1.getUtfString("gamename");
			setGameName(gameNameNew);
		}else if(cmd.equals("start")){
			MainRoom.gs.getCurrentGame().start();
		}else if(cmd.equals("getScores")){
			int[] i = MainRoom.gs.getCurrentGame().getScores();
			ISFSObject o = new SFSObject();
			o.putInt("p1Score", i[0]);
			o.putInt("p2Score", i[1]);
			send("Game", o, arg0);
		}else if(cmd.equals("input")){
			send("Input", arg1, getParentExtension().getParentRoom().getUserList());
			
		}else if(cmd.equals("updateScore")){
			int pID = arg1.getInt("pID");
			int amount = arg1.getInt("amount");
			MainRoom.gs.getCurrentGame().updateScore(pID, amount);
			//return updated score
			int[] i = MainRoom.gs.getCurrentGame().getScores();
			ISFSObject o = new SFSObject();
			o.putInt("p1Score", i[0]);
			o.putInt("p2Score", i[1]);
			send("Game", o, arg0);
			
		}else if(cmd.equals("endGame")){
			MainRoom.gs.getCurrentGame().end(1);
		}
		
		
	}
	

	public String getGameName() {
		return MainRoom.gs.getGameName();
	}

	public String setGameName(String gameName) {
		MainRoom.gs.setGameName(gameName);;
		
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
		return MainRoom.gs.getMaxPlayers();
	}

	public void setMaxPlayers(int maxPlayers) {
		MainRoom.gs.setMaxPlayers(maxPlayers);;
	}

	public int getNumPlayers() {
		return MainRoom.gs.getNumPlayers();
	}

	public void setNumPlayers(int numPlayers) {
		MainRoom.gs.setNumPlayers(numPlayers);;
	}

	public int getNumSpectators() {
		return MainRoom.gs.getNumSpectators();
	}

	public void setNumSpectators(int numSpectators) {
		MainRoom.gs.setNumSpectators(numSpectators);;
	}

	public boolean isGameInProgress() {
		return MainRoom.gs.isGameInProgress();
	}
 
	public void setGameInProgress(boolean gameInProgress) {
		MainRoom.gs.setGameInProgress(gameInProgress);;
	}

	public Game getCurrentGame() {
		return MainRoom.gs.getCurrentGame();
	}

	public boolean setCurrentGame(Game game) {
		if(game == null)
			return false;
		MainRoom.gs.setCurrentGame(game);;
		return true;
	}


}
