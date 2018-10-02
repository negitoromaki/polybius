
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSZone;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
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
		
		
		String username = obj.getUtfString("user");
		String pass = obj.getUtfString("password");
			
		login(user, username, pass, db);
		
			
			
		
		
		
		
	}

	public String login(User user, String username, String password, IDBManager db) {
		// TODO Auto-generated method stub
		
		String sql = "SELECT * FROM users.userdata WHERE username ='" + username + "' AND password = '" + password + "'"; 
		ISFSObject ret = new SFSObject();
		try{
			ISFSArray resp = db.executeQuery(sql, new Object[] {});
			if(resp.size() >0){
				//is user online?
				if(resp.getSFSObject(0).getInt("isonline") != 0){
					throw new IsOnlineException();
				}else{
					//login user
					//set as online
					sql = "UPDATE users.userdata SET isonline = ? WHERE username='" + username + "'";
					Integer[] params = {new Integer(1)};
					db.executeUpdate(sql, params);
					
					SFSZone zone = (SFSZone) getParentExtension().getParentZone();
					zone.getRoomByName("Lobby").addUser(user);
					
					trace(resp.getUtfString(0) + " is logging in!");
				}
				ISFSObject message = new SFSObject();
				message.putUtfString("message", "Login success!");
				send("UserLogin", message, user);
				trace("Login success!");
			}else{
				
			}
		}catch (Exception e){
			ret.putUtfString("result", "failed");
			ret.putUtfString("message", e.getMessage());
			send("UserLogin", ret, user);
			return "Login failed! Reason: " + e.getMessage();
		}
		
		ret.putUtfString("result", "success");
		ret.putUtfString("message", "User logged in!");
		send("UserLogin", ret, user);
		return "Login success!";
		
	}

}
