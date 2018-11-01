
import com.smartfoxserver.v2.extensions.SFSExtension;

import java.sql.SQLException;

import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.bitswarm.sessions.Session;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSRoom;
import com.smartfoxserver.v2.entities.SFSUser;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;

public class Main extends SFSExtension{


	@SuppressWarnings("unused")
	@Override
	public void init() {
		//add event handlers here
		
		
		IDBManager db = getParentZone().getDBManager();
		addRequestHandler("UserLogin", UserLogin.class);
		addRequestHandler("CreateUser", CreateUser.class);
		addRequestHandler("Logout", UserLogout.class);
		addRequestHandler("Messages", Messaging.class);
		addRequestHandler("FriendList", FriendList.class);
		addRequestHandler("Users", Users2.class);
		addRequestHandler("UserLogout", UserLogout.class);
		addRequestHandler("Lobby", Lobby.class);
		addEventHandler(SFSEventType.ROOM_REMOVED,CleanUp.class);
		
		
		
		
		trace("307 Extension loaded!");
		trace("Let the games begin!");
		
		UTest2 ut = new UTest2();
		if(ut == null)
			trace("Test File Not Found");
		else
			trace("\n=====Begin Test=====\n\n" + ut.startTest(db));
		
		
		
		
		
		//tester();
		
		
	}
	

	
	public void destroy(){
		super.destroy();
	}

}
