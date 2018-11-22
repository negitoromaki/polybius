import java.sql.SQLException;
import java.util.ArrayList;

import com.smartfoxserver.v2.api.CreateRoomSettings;
import com.smartfoxserver.v2.config.ZoneSettings.RoomSettings;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.SFSRoomRemoveMode;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSCreateRoomException;
import com.smartfoxserver.v2.exceptions.SFSJoinRoomException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class Lobby extends BaseClientRequestHandler{

	SQLStrings sqls = new SQLStrings();
	String roomName = "";
	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String cmd = arg1.getUtfString("cmd"); //host or join
		roomName = arg1.getUtfString("roomname");
		String initGame = arg1.getUtfString("game"); //Pong
		
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		if(cmd == "host"){
			int gameID = arg1.getInt("gameID");
			float lat = arg1.getFloat("latcord");
			float longc = arg1.getFloat("longcord");
			
			hostLobby(arg0, roomName, gameID, lat, longc, initGame,db);
			
		}else if(cmd.equals("join")){
			joinLobby(arg0,roomName);
		}else if(cmd.equals("getRooms")){
			getRooms(arg0,db);
		}
		
	}
	
	
	public boolean hostLobby(User user, String roomName, int gameID, float lat, float longc, String initGame, IDBManager db){
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
			if(user != null)
				getApi().createRoom(getParentExtension().getParentZone(), r, user);
			
			ISFSObject ret = new SFSObject();
			ret.putUtfString("result", "success");
			ret.putUtfString("cmd", "host");
			ret.putUtfString("message", "user joined lobby");
			
			//db that
			
			String sql = sqls.addRoom;
			db.executeInsert(sql, new Object[] {roomName, gameID, lat, longc});
			
			if(user != null)
				send("Lobby", ret,user);
			return true;
		} catch (SFSCreateRoomException | SQLException e) {
			// TODO Auto-generated catch block
			ISFSObject ret = new SFSObject();
			ret.putUtfString("result", "fail");
			ret.putUtfString("cmd", "join");
			ret.putUtfString("message", "user did not join lobby");
			if(user != null){
				send("Lobby", ret,user);
				trace(e.getMessage());
			}
			
		}
		
		
		return false;
	}
	public boolean joinLobby(User user, String roomName){
		Room r = getParentExtension().getParentZone().getRoomByName("roomName");
		
		try {
			getApi().joinRoom(user, r);
			
			ISFSObject u = new SFSObject();
			u.putUtfString("user", user.getName());
			send("UserJoined", u, r.getUserList());
			
			ISFSObject ret = new SFSObject();
			ret.putUtfString("result", "success");
			ret.putUtfString("message", "user did joined lobby");
			if(user != null)
				send("Lobby", ret,user);
			return true;
		} catch (SFSJoinRoomException e) {
			// TODO Auto-generated catch block
			ISFSObject ret = new SFSObject();
			ret.putUtfString("result", "fail");
			ret.putUtfString("message", "user did not join lobby");
			send("Lobby", ret,user);
			e.printStackTrace();
		}
		
		return false;
	}
	
	public void getRooms(User user, IDBManager db){
		
		ISFSArray rooms = new SFSArray();
		String sql = sqls.getRooms;
		try {
			rooms = db.executeQuery(sql, null);
			SFSObject o = new SFSObject();
			o.putUtfString("cmd", "getRooms");
			o.putSFSArray("roomdata", rooms);
			
			if(user != null)
				send("Lobby", o,user);
			return;
			
			
			
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
			
			
			
		
		
		SFSObject o = new SFSObject();
		o.putUtfString("cmd", "getRooms");
		//o.putSFSArray("roomdata", rooms);
		
		
	}
	

}
