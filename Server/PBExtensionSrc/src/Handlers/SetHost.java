package Handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import Core.PBMain;

public class SetHost extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String roomName = arg1.getUtfString("roomName");
		for(int i = 0;i< PBMain.rooms.size();i++){
			if(PBMain.rooms.get(i).getName().equals(roomName)){
				ISFSObject ret = new SFSObject();
				PBMain.rooms.get(i).setHost(arg0);
				ret.putUtfString("result", "success");
				send("SetHost", ret, arg0);
			}
		}
		
	}

}
