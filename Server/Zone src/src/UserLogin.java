
import java.sql.SQLException;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSZone;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;




public class UserLogin extends BaseClientRequestHandler{
	
	
	public UserLogin() {
		// TODO Auto-generated constructor stub
	}
	
	@Override
	public void handleClientRequest(User user, ISFSObject obj) {
		
		//get request type
		
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		
		
		String username = obj.getUtfString("username");
		String pass = obj.getUtfString("password");
		
		trace(user + " : " + username + " : " + pass);
			
		login(user, username, pass, db);
		
			
			
		
		
		
		
	}

	public String login(User user, String username, String password, IDBManager db) {
		// TODO Auto-generated method stub
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.login; 
		ISFSObject ret = new SFSObject();
		try{
			
			ISFSArray resp = db.executeQuery(sql, new Object[] {username,password});
			
			
			//trace("login: " + resp.getSFSObject(0).getInt("isonline"));
			if(resp.size() >0){
				//is user online?
				if(resp.getSFSObject(0).getInt("isonline") == null || resp.getSFSObject(0).getInt("isonline") == 1){
					//trace("here");
					throw new IsOnlineException();
				}else{
					//login user
					//set as online
					//trace("here");
					sql = sqls.goOnline;
					Object[] params = {new Integer(1),username};
					db.executeUpdate(sql, params);
					
					if(user != null){
						SFSZone zone = (SFSZone) getParentExtension().getParentZone();
						user.setName(username);
						zone.getRoomByName("Lobby").addUser(user);
					}
					
					//trace(resp.getSFSObject(0).getUtfString("username") + " is logging in!");
				}
				
			}else{
				
			}
		}catch (SQLException e){
			//trace("error: sql" );
			ret.putUtfString("result", "failed");
			ret.putUtfString("message", "error: sql");
			if(user != null)
				send("UserLogin", ret, user);
			return "Login failed! Reason: " + e.getMessage();
		}catch (Exception e2){
			if(e2.getMessage() != null){
				//trace("error: " + e2.getMessage());
				ret.putUtfString("result", "failed");
				ret.putUtfString("message", "" + e2.getMessage());
				if(user != null)
					send("UserLogin", ret, user);
				return "Login failed! Reason: " + e2.getMessage();
			}
			return "error";
		}
		
		ret.putUtfString("result", "success");
		ret.putUtfString("message", "User logged in!");
		if(user != null)
			send("UserLogin", ret, user);
		return "Login success!";
		
	}

}
