import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class Users2 extends BaseClientRequestHandler{

	
	
	@Override
	public void handleClientRequest(com.smartfoxserver.v2.entities.User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String cmd = arg1.getUtfString("cmd"); //getUsers 
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		trace("here");
		if(cmd.equals("getUsers")){
			SFSObject ret = new SFSObject();
			ret.putUtfString("cmd", "getUsers");
			ret.putSFSArray("users", getUsers(db));
			send("Users",ret,arg0);
		}else if(cmd.equals("setPrivate")){
			setPrivate(arg0, arg1.getUtfString("username"),arg1.getBool("private"),db);
		}
		
		
		
	}
	

	public int getUserID(String username, IDBManager db){
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.getIDFromUser;
		try{
			ISFSArray res = db.executeQuery(sql, new Object[] {username});
			ISFSObject o = res.getSFSObject(0);
			
			int id = o.getInt("id");
			return id;
			
			
		}catch (Exception e){
			return -1;
		}
		
		
		
		
	}
	
	
	
	public SFSArray getUsers(IDBManager db){
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.getUsers;
		try{
			ISFSArray res = db.executeQuery(sql, null);
			return (SFSArray)res;
			
			
			
		}catch (Exception e){
			try{
				trace("Error getting users: " + e.getMessage());
			}catch(Exception e2){
				
			}
			return null;
		}
		
		
	}
	
	public boolean setPrivate(User user, String userName, boolean p,IDBManager db){
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.setPrivate;
		
		try{
			ISFSArray res = db.executeQuery(sql, new Object[] {p,userName});
			SFSObject o = new SFSObject();
			o.putUtfString("result", "success");
			o.putUtfString("message", "privacy set");
			if(user != null){
				send("Users", o, user);
			}
			return true;
			
			
		}catch (Exception e){
			try{
				trace("Error getting users: " + e.getMessage());
				SFSObject o = new SFSObject();
				o.putUtfString("result", "fail");
				o.putUtfString("message", "privacy not set");
				if(user != null){
					send("Users", o, user);
				}
			}catch(Exception e2){
				
			}
			return false;
		}
	}

	



}
