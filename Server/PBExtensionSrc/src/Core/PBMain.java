package Core;

import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.extensions.SFSExtension;

import Handlers.*;

public class PBMain extends SFSExtension{
	
	public static List<RoomData> rooms;

	@Override
	public void init() {
		trace("Initializing Extension");
		
		addRequestHandler("Sync", RoomSync.class); //send sync data to all users in room
		addRequestHandler("ChangeHost", HostChange.class); //change host of lobby
		addRequestHandler("LeaveRoom", LeaveRoom.class); //player leave room
		addRequestHandler("ReqData", ReqData.class); //request room data from host
		addRequestHandler("SendData", SendData.class); //send room data to user
		addRequestHandler("SetHost", SetHost.class); //set the room host
		addRequestHandler("CreateRoom", CreateRoom.class); //set the room host
		addRequestHandler("RemoveRoom", CreateRoom.class); //set the room host
		
		
		trace("Handlers added\nExtension Ready");
		
	}

}
