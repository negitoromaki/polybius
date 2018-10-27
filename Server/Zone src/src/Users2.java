import com.smartfoxserver.v2.db.IDBManager;
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
		if(cmd.equals("getUsers")){
			SFSObject ret = new SFSObject();
			ret.putSFSArray("users", getUsers(db));
			send("Users",ret,arg0);
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
			return null;
		}
		
		
	}

	



}
