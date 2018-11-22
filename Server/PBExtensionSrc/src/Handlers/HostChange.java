package Handlers;

import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import Core.PBMain;

public class HostChange extends BaseClientRequestHandler{
	
	//Used for when host leaves lobby
	//sfsobject migrates all room data to new host

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String roomName = arg1.getUtfString("roomName");
		List<User> us = getParentExtension().getParentZone().getRoomByName(roomName).getUserList();
		ISFSObject ret = new SFSObject();
		if(us.size() < 2){
			ret.putUtfString("result", "last"); //current host is last user in lobby
			send("ChangeHost", ret,arg0);
		}
		for(int i = 0; i<us.size();i++){
			for(int j = 0; j < PBMain.rooms.size();j++){
				if(PBMain.rooms.get(j).getName().equals(roomName) && !us.get(i).getName().equals(PBMain.rooms.get(j).getHost().getName())){
					PBMain.rooms.get(j).setHost(us.get(i));
					ret.putUtfString("result", "success"); 
					send("ChangeHost", ret,arg0);
					send("BecameHost",ret,us.get(i));
					break;
				}
			
			}
		}
		
	}

}
