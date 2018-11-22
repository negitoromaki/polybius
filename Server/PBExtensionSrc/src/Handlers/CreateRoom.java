package Handlers;

import com.smartfoxserver.v2.api.CreateRoomSettings;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSCreateRoomException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import Core.PBMain;
import Core.RoomData;

public class CreateRoom extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String roomName = arg1.getUtfString("roomName");
		CreateRoomSettings settings = new CreateRoomSettings();
		settings.setName(roomName);
		ISFSObject o = new SFSObject();
		try {
			Room r = getApi().createRoom(getParentExtension().getParentZone(), settings, arg0);
			RoomData rd = new RoomData(roomName);
			rd.setHost(arg0);
			PBMain.rooms.add(rd);
			o.putUtfString("result", "success");
			send("CreateRoom",o,arg0);
		} catch (SFSCreateRoomException e) {
			// TODO Auto-generated catch block
			o.putUtfString("result", "fail");
			send("CreateRoom",o,arg0);
		}
		
	}

}
