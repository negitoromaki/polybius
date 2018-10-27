import java.sql.Date;
import java.sql.SQLException;
import java.time.LocalTime;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSZone;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
public class Messaging  extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User user, ISFSObject o) {
		
		String level = o.getUtfString("level"); //zone, room, private
		String levelname = o.getUtfString("levelname");//name of room or zone message origionated
		String mode = o.getUtfString("mode"); //get, send
		String mReciever = o.getUtfString("receiver");
		String mSender = o.getUtfString("sender");
		int number = o.getInt("amount");
		String message = o.getUtfString("message");
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		messanger(user,level,levelname,mode,mReciever,mSender,number,message,db);
		
		
		
	}
	
	public ISFSObject messanger(User user, String level, String levelname, String mode, String mReciever, String mSender, int number, String message,IDBManager db){
		SQLStrings sqls = new SQLStrings();
		if(mode.equals("get")){
			//zone messages
			if(level.equals("zone") || level.equals("room")){
				String sql = sqls.publicMSG;
				Object[] obj = {level, levelname, number};
				
				try {
					ISFSArray res = db.executeQuery(sql, obj);
					ISFSObject ret = new SFSObject();
					ret.putSFSArray("messages", res);
					if(user != null)
						send("Messages", ret, user);
					return ret;
					
				} catch (SQLException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					ISFSObject ret = new SFSObject();
					ISFSArray ar = new SFSArray();
					ISFSObject dif = new SFSObject();
					dif.putUtfString("message", e.getMessage());
					dif.putUtfString("sender", "fail");
					ar.addSFSObject(dif);
					ret.putSFSArray("messages", ar);
					return ret;
				}
			}else if(level.equals("private")){
				String sql = sqls.privateMSG;
				Object[] obj = {level, mSender, mReciever, number};
				
				try {
					ISFSArray res = db.executeQuery(sql, obj);
					ISFSObject ret = new SFSObject();
					ret.putSFSArray("messages", res);
					if(user != null)
						send("Messages", ret, user);
					return ret;
					
				} catch (SQLException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					ISFSObject ret = new SFSObject();
					ISFSArray ar = new SFSArray();
					ISFSObject dif = new SFSObject();
					dif.putUtfString("message", e.getMessage());
					dif.putUtfString("sender", "fail");
					ar.addSFSObject(dif);
					ret.putSFSArray("messages", ar);
					return ret;
				}
			}
			
			
			
		}else if(mode.equals("send")){
			String sql = sqls.sendMSG;
			Object[] obj = {mSender, mReciever, new Date(System.currentTimeMillis()), message, level, levelname};
			
			
			try{
				db.executeInsert(sql, obj);
				ISFSObject ret = new SFSObject();
				ISFSArray ar = new SFSArray();
				ISFSObject dif = new SFSObject();
				dif.putUtfString("message", "success");
				dif.putUtfString("sender", mSender);
				ar.addSFSObject(dif);
				ret.putSFSArray("messages", ar);
				return ret;
				
			}catch(Exception e){
				ISFSObject ret = new SFSObject();
				ISFSArray ar = new SFSArray();
				ISFSObject dif = new SFSObject();
				dif.putUtfString("message", e.getMessage());
				dif.putUtfString("sender", "fail");
				ar.addSFSObject(dif);
				ret.putSFSArray("messages", ar);
				return ret;
			}
		}else{
			return null;
		}
		return null;
		
	}
	

}
