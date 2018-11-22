package Handlers;

import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import Core.PBMain;

public class LeaveRoom extends BaseClientRequestHandler{
	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		String roomName = arg1.getUtfString("roomName");
		List<User> us = getParentExtension().getParentZone().getRoomByName(roomName).getUserList();
		ISFSObject ret = new SFSObject();
		
		for(int i = 0; i<us.size();i++){
			if(!us.get(i).getName().equals(arg0.getName())){
				
				send("LeftRoom",arg1,us.get(i));
				
			}
		}
		ret.putUtfString("result", "success");
		send("LeaveRoom",ret,arg0);
		
	}
}


