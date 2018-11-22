package Handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import Core.PBMain;

public class RemoveRoom extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String roomName = arg1.getUtfString("roomName");
		ISFSObject o = new SFSObject();
		for(int i = 0; i<PBMain.rooms.size();i++){
			if(PBMain.rooms.get(i).getName().equals(roomName)){
				PBMain.rooms.remove(i);
				o.putUtfString("result", "success");
				send("RemoveRoom",o,arg0);
				return;
				
			}
		}
		o.putUtfString("result", "fail");
		send("RemoveRoom",o,arg0);
	}

}
