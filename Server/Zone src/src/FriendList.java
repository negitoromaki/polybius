import java.sql.SQLException;
import java.time.LocalTime;
import java.util.ArrayList;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSZone;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;


public class FriendList extends BaseClientRequestHandler{

	private String splitter = "[[S]]";
	
	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		String cmd = arg1.getUtfString("cmd"); //addFriend, getFriends, removeFriend
		String username = arg1.getUtfString("username");
		int id = arg1.getInt("id");
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		SFSObject ret = new SFSObject();
		if(cmd.equals("addFriend")){
				if(addFriend(username,id,db)){
					
				ret.putUtfString("result", "success");
				ret.putUtfString("message", "added a friend");
				send("FriendList", ret,arg0);
			}else{
				ret.putUtfString("result", "error");
				ret.putUtfString("message", "user exists not");
				send("FriendList", ret,arg0);
				
			}
		}else if(cmd.equals("getFriends")){
			ret.putSFSArray("friends", getFriends(username,db));
			send("FriendList", ret,arg0);
			
		}else if(cmd.equals("removeFriend")){
			if(removeFriend(username,id,db)){
				
				ret.putUtfString("result", "success");
				ret.putUtfString("message", "ditched a friend");
				send("FriendList", ret,arg0);
			}else{
				ret.putUtfString("result", "error");
				ret.putUtfString("message", "no ID found");
				send("FriendList", ret,arg0);
				
			}
		}
		
		else{
			ret.putUtfString("result", "error");
			ret.putUtfString("message", "not a valid request");
			send("FriendList", ret,arg0);
			
		}
		
	}
	
	public Integer[] convertIDs(String ids){
		String[] ida = ids.split(splitter);
		ArrayList<Integer> converted = new ArrayList<Integer>();
		for(String s : ida){
			int t = Integer.parseInt(s);
			converted.add(t);
		}
		
		
		return (Integer[]) converted.toArray();
		
	}
	
	
	
	public boolean removeFriend(String username, int ID, IDBManager db){
		
			//get current friend list
			SQLStrings sqls = new SQLStrings();
			String sql = sqls.getFriends;
			ISFSArray res;
			try {
				res = db.executeQuery(sql, new Object[] {username});
				ISFSObject o = res.getSFSObject(0);
				
				String idList = o.getUtfString("friends");
				
				Integer[] ids = convertIDs(idList);
				idList = "";
				for(Integer i : ids){
					if(i != new Integer(ID)){
						idList = idList+ splitter + i;
					}
					
				}
				
				
				
				sql = sqls.updateFriend;
				db.executeUpdate(sql, new Object[] {idList,username});
				
				
				return true;
				
				
			} catch (SQLException e1) {
				// TODO Auto-generated catch block
				return false;
				
			}
		
		
		
	}
	
	public boolean addFriend(String username, int ID, IDBManager db){
		if(ID != -1){
			//get current friend list
			SQLStrings sqls = new SQLStrings();
			String sql = sqls.getFriends;
			ISFSArray res;
			try {
				res = db.executeQuery(sql, new Object[] {username});
				ISFSObject o = res.getSFSObject(0);
				
				String idList = o.getUtfString("friends");
				idList = idList + splitter + ID;
				sql = sqls.updateFriend;
				db.executeUpdate(sql, new Object[] {idList,username});
				return true;
				
				
			} catch (SQLException e1) {
				// TODO Auto-generated catch block
				return false;
				
			}
			
		}else{
			return false;
			
		}
		
		
	}
	
	public SFSArray getFriends(String username, IDBManager db){
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.getFriends;
		ISFSArray res;
		try {
			res = db.executeQuery(sql, new Object[] {username});
			ISFSObject o = res.getSFSObject(0);
			
			String idList = o.getUtfString("friends");
			Integer[] ids = convertIDs(idList);
			
			SFSArray friends = new SFSArray();
			sql = sqls.getUserFromID;
			for(Integer i : ids){
				//get usernames and add to final array
				res = db.executeQuery(sql, new Object[] {i});
				o = res.getSFSObject(0);
				SFSObject f = new SFSObject();
				f.putUtfString("username", o.getUtfString("username"));
				f.putInt("id", i);
				friends.addSFSObject(f);
			}
			
			return friends;
			
		} catch (SQLException e1) {
			// TODO Auto-generated catch block
			SFSObject err = new SFSObject();
			err.putUtfString("result", "fail");
			err.putUtfString("message", e1.getMessage()!=null?e1.getMessage():"eror");
			SFSArray ret = new SFSArray();
			ret.addSFSObject(err);
			return ret;
			
		}
	}

}
