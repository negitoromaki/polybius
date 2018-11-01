import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.db.IDBManager;

public class UserLogout extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User arg0, ISFSObject obj) {
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		String username = obj.getUtfString("username");
		trace("user ditching the fun: " + username );
		String res = logOut(arg0,username,db);
		SFSObject ret = new SFSObject();
		ret.putUtfString("result", (res.equals("success")?"success":"fail"));
		ret.putUtfString("message", res);
		
	}
	
	public String logOut(User user, String userName,IDBManager db){
		
		try{
			SQLStrings sqls = new SQLStrings();
			String sql = sqls.logout;
			Object[] params = {new Integer(0), userName};
			db.executeUpdate(sql, params);
			if(user != null){
				ISFSObject ret = new SFSObject();
				getParentExtension().getParentZone().removeUser(user);
				ret.putUtfString("result", "success");
				ret.putUtfString("message", "User logged out");
				send("UserLogout", ret, user);
			
			}
			return "success";
			
		}catch(Exception e){
			if(user != null){
				ISFSObject ret = new SFSObject();
				ret.putUtfString("result", "fail");
				ret.putUtfString("message", e.getMessage());
				send("UserLogout", ret, user);
				trace(e.getMessage());
			
			}
			return e.getMessage();
		}
		
		
	}

}
