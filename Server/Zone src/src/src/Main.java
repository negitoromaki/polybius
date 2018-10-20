
import com.smartfoxserver.v2.extensions.SFSExtension;

import java.sql.SQLException;

import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.bitswarm.sessions.Session;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSUser;
import com.smartfoxserver.v2.entities.User;

public class Main extends SFSExtension{


	@Override
	public void init() {
		//add event handlers here
		
		addRequestHandler("UserLogin", UserLogin.class);
		addRequestHandler("CreateUser", CreateUser.class);
		
		
		
		
		trace("307 Extension loaded!");
		trace("Let the games begin!");
		
		//tester();
		
		
	}
	

	
	public void destroy(){
		super.destroy();
	}

}
