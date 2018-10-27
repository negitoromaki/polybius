import java.util.ArrayList;

import com.smartfoxserver.v2.api.CreateRoomSettings;
import com.smartfoxserver.v2.config.ZoneSettings.RoomSettings;
import com.smartfoxserver.v2.entities.SFSRoomRemoveMode;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSCreateRoomException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class Lobby extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String cmd = arg1.getUtfString("cmd"); //host or join
		String roomName = arg1.getUtfString("roomname");
		String initGame = arg1.getUtfString("game"); //Pong
		
		if(cmd == "host"){
			hostLobby(arg0, roomName, initGame);
			
		}
		
	}
	
	
	public boolean hostLobby(User user, String roomName, String initGame){
		CreateRoomSettings r = new CreateRoomSettings();
		r.setName(roomName);
		r.setAutoRemoveMode(SFSRoomRemoveMode.WHEN_EMPTY);
		CreateRoomSettings.RoomExtensionSettings re = new CreateRoomSettings.RoomExtensionSettings("roomExtension", "RoomExtension");
		r.setExtension(re);
		ArrayList<RoomVariable> rvl = new ArrayList<RoomVariable>();
		RoomVariable ig = new SFSRoomVariable("initGame", initGame);
		rvl.add(ig);
		r.setRoomVariables(rvl);
		
		
		
		try {
			getApi().createRoom(getParentExtension().getParentZone(), r, user);
		} catch (SFSCreateRoomException e) {
			// TODO Auto-generated catch block
			trace(e.getMessage());
		}
		
		
		return false;
	}
	

}
