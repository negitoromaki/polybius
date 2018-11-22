
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;



public class CreateUser extends BaseClientRequestHandler{

	public CreateUser() {
		// TODO Auto-generated constructor stub
	}

	@Override
	public void handleClientRequest(User user, ISFSObject o) {
		String username = o.getUtfString("username");
		trace("user creating: " + username );
		String password = o.getUtfString("password");
		String email = o.getUtfString("email");
		
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		
		create(user, username,password,email,db);
		
		
		
		
	}
	public String create(User user, String username, String password, String email, IDBManager db){
		
		
		String sql = "SELECT * FROM users.userdata WHERE username='" + username + "'";
		String sql2 = "SELECT * FROM users.userdata WHERE email='" + email + "'";
		ISFSObject ret = new SFSObject();
		//check user or email exist
		try {
			//trace("create");
			ISFSArray resp = db.executeQuery(sql2, new Object[] {});
			if(resp.size() >0){
				//email exist
				throw new GenericException("Email exist");
			}
			resp = db.executeQuery(sql, new Object[]{});
			if(resp.size() > 0){
				throw new GenericException("User exists!");
			}
			
			sql = "INSERT INTO users.userdata (username, password, email, notification) VALUES ('" + username + "', '" + password + "', '" + email + "', 'welcome!');";
			db.executeInsert(sql, null);
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			if(e.getMessage() != null){
				//trace("message: " + e.getMessage());
				ret.putUtfString("result", "failed");
				ret.putUtfString("message", e.getMessage());
				if(user != null)
					send("CreateUser", ret, user);
				return e.getMessage();
			}
			return "error";
		} 
		
		ret.putUtfString("result", "success");
		ret.putUtfString("message", "User created!");
		if(user != null)
			send("CreateUser", ret, user);
		return "success";
		
	}

}
