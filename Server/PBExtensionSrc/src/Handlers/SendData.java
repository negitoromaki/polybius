package Handlers;

import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class SendData extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String name = arg1.getUtfString("user");
		String roomName = arg1.getUtfString("roomName");
		List<User> us = getParentExtension().getParentZone().getRoomByName(roomName).getUserList();
		
		for(int i = 0; i< us.size();i++){
			if(us.get(i).getName().equals(name)){
				ISFSObject ret = arg1.getSFSObject("data");
				send("ReqDataRes", ret, us.get(i));
				break;
			}
		}
		
	}

}
