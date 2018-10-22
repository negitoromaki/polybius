

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class UTest extends BaseClientRequestHandler {
	
	GameSettings gs;
	public UTest(GameSettings gs){
		this.gs = gs;
	}
	
	public String startTest(){
		String URes = "";
		URes = URes + "testGSExists: " + (testGSExists() == true? "true":"false") + "\n";
		URes = URes + "testSetGame: " + (testSetGame("Pong") == "pass"? "pass":"fail") + "\n";
		URes = URes + "testCurrentGame: " + (testCurrentGameName().equals("Pong")? "pass":"fail") + "\n";
		
		URes += "\n\n=====End Tests=====\n";
		return URes;
	}
	
	private boolean testGSExists(){
		return gs != null? true : false;
	}
	
	private String testSetGame(String gameName){
		
		return gs.setGameName(gameName);
	}
	
	private String testCurrentGameName(){
		
		return gs.getCurrentGame() != null? gs.getGameName(): "fail";
	}
	

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		
		
	}

}
