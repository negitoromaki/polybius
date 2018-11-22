package Handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import Core.PBMain;

public class ReqData extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub

		ISFSObject req = new SFSObject();
		req.putUtfString("user", arg0.getName());
		String roomName = arg1.getUtfString("roomName");
		if(roomName.equals("Lobby")){
			return;
		}
		for(int i = 0; i< PBMain.rooms.size();i++){
			if(PBMain.rooms.get(i).getName().equals(roomName)){
				send("ReqData", req, PBMain.rooms.get(i).getHost());
				break;
			}
		}
		
	}

}
