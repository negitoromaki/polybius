

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.SFSUser;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class UTest2 extends BaseClientRequestHandler {
	
	
	
	public String startTest(IDBManager db){
		
		
		String testUser = "qmwne1029";
		String testPass = "poiuytrewq";
		String testEmail = "test@test.com";
		String testPrivateMessage = "This is a private message";
		String testZoneMessage = "This is a zone message";
		String testRoomMessage = "This is a room message";
		String testZone = "Polybius";
		String testRoom = "TestRoom";
		int amount = 1;
		
		String URes = "";
		URes = URes + "testAddRoom: " + testAddRoom(db) + "\n";
		URes = URes + "testRemoveRoom: " + testRemoveRoom(db) + "\n";
		URes = URes + "testCreateUser: " + testCreate(testUser,testPass,testEmail,db) + "\n";
		URes = URes + "testLogin: " + testLogin(testUser,testPass,db) + "\n";
		URes = URes + "testSendMessageZone: " + testSendZoneMessage(testUser,testZoneMessage,testZone,amount,db) + "\n";
		URes = URes + "testSendMessageRoom: " + testSendRoomMessage(testUser,testRoomMessage,testRoom,amount,db) + "\n";
		URes = URes + "testSendMessagePrivate: " + testSendPrivateMessage(testUser,testPrivateMessage,amount,db) + "\n";
		URes = URes + "testGetMessageZone: " + testGetZoneMessage(testUser, testZone, amount, testZoneMessage,db) + "\n";
		URes = URes + "testGetMessageRoom: " + testGetRoomMessage(testUser, testRoom, amount, testRoomMessage,db) + "\n";
		URes = URes + "testAddFriends: " + testAddFriends(testUser,db) + "\n";
		URes = URes + "testGetFriends: " + testGetFriends(testUser,db) + "\n";
		URes = URes + "testRemove1Friends: " + testRemoveFriends(testUser,db) + "\n";
		URes = URes + "testGet1Friends: " + testGetFriends(testUser,db) + "\n";
		URes = URes + "testGetMessagePrivate: " + testGetPrivateMessage(testUser, testRoom, amount, testPrivateMessage,db) + "\n";
		URes = URes + "testLogoutGame: " + testLogout(testUser,db) + "\n";
		URes = URes + "testGetUsers: " + testGetUsers(testUser,db) + "\n";
		
		URes += "\n\n=====End Tests=====\n";
		URes += "\n=====Start Clean=====\n\n";
		
		//clean up
		String sql = "DELETE FROM users.msgs WHERE sender=?";
		String sql2 = "DELETE FROM users.userdata WHERE username=?";
		try{
			db.executeUpdate(sql, new Object[] {testUser});
			db.executeUpdate(sql2, new Object[] {testUser});
			URes += ">> Removed Test User <<\n";
		}catch(Exception e){
			URes += "=====Failed to Remove Test User=====\n";
			URes += "Reason: " + (e.getMessage() != null? e.getMessage() : "unknown") + "\n";
		}
		
		URes += "\n=====End Clean=====\n";
		
		
		return URes;
	}
	
	
	
	private String testAddRoom(IDBManager db){
		Lobby l = new Lobby();
		
		return l.hostLobby(null, "tester", 0, 0, 0, null, db) == true? "pass" : "fail";
		
		
	}
	
	private String testRemoveRoom(IDBManager db){
		CleanUp c = new CleanUp();
		
		return c.cleanerS("tester", db)== true? "pass" : "fail";
		
		
	}
	
	
	
	
	private String testAddFriends(String testUser, IDBManager db) {
		
		
		
		FriendList f = new FriendList();
		boolean ar = f.addFriend(testUser, 94, db);
		f.addFriend(testUser, 91, db);
		f.addFriend(testUser, 94, db);
		
		return ar == true? "pass" : "fail";
		
		
	}
	private String testRemoveFriends(String testUser, IDBManager db) {
		
		
		
		FriendList f = new FriendList();
		boolean ar = f.removeFriend(testUser, 94, db);
		
		
		return ar == true? "pass" : "fail";
		
		
	}
	private String testGetFriends(String testUser, IDBManager db) {
		
		
		
		FriendList f = new FriendList();
		ISFSArray ar = f.getFriends(testUser, db);
		
		if (ar == null)
			return "no friend";
		return ar.size()>0? ar.getSFSObject(0).getUtfString("username") : "fail";
		
		
	}
	
	private String testGetUsers(String testUser, IDBManager db) {
		
		
		
		Users2 u2 = new Users2();
		ISFSArray ar = u2.getUsers(db);
		
		if (ar == null)
			return "fail";
		return ar.size()>0? "pass" : "fail";
		
		
	}
	
	private String testCreate(String testUser, String testPass, String testEmail, IDBManager db) {
		
		
		
		CreateUser cu = new CreateUser();
		String t = cu.create(null, testUser, testPass, testEmail, db);
		return t.equals("success")? "pass" : "fail: " + t;
		
		
	}
	private String testLogin(String testUser, String testPass, IDBManager db) {
		
		
		
		UserLogin ul = new UserLogin();
		String t = ul.login(null, testUser, testPass, db);
		return t.equals("success")? "pass" : "fail: " + t;
		
		
	}
	private String testLogout(String testUser, IDBManager db) {
		
		
		
		UserLogout ul = new UserLogout();
		String t = ul.logOut(null, testUser,db);
		
		return t.equals("success")? "pass" : "fail: " + t;
		
		
	}
	private String testSendZoneMessage(String sender, String message, String levelName, int amount,IDBManager db){
		Messaging mg = new Messaging();
		ISFSObject ret = mg.messanger(null, "zone", levelName, "send", "", sender, amount, message,db);
		if(ret == null)
			return "fail";
		ISFSArray ar = ret.getSFSArray("messages");
		String msg = ret.getUtfString("result");
		if(msg.equals("success"))
			return "pass";
		
		
		return msg;
	}
	private String testSendRoomMessage(String sender, String message, String levelName, int amount,IDBManager db){
		Messaging mg = new Messaging();
		ISFSObject ret = mg.messanger(null, "room", levelName, "send", "", sender, amount, message,db);
		if(ret == null)
			return "fail";
		ISFSArray ar = ret.getSFSArray("messages");
		String msg = ret.getUtfString("result");;
		if(msg.equals("success"))
			return "pass";
		
		
		return msg;
	}
	private String testSendPrivateMessage(String sender, String message, int amount,IDBManager db){
		Messaging mg = new Messaging();
		ISFSObject ret = mg.messanger(null, "private", "", "send", sender, sender, amount, message,db);
		if(ret == null)
			return "fail";
		ISFSArray ar = ret.getSFSArray("messages");
		String msg = ret.getUtfString("result");;
		if(msg.equals("success"))
			return "pass";
		
		
		return msg;
	}

	
	private String testGetZoneMessage(String sender, String zoneName, int amount, String message,IDBManager db){
		Messaging mg = new Messaging();
		ISFSObject ret = mg.messanger(null, "zone", zoneName, "get", sender, sender, amount, null,db);
		if(ret == null)
			return "fail";
		ISFSArray ar = ret.getSFSArray("messages");
		String msg = ar.getSFSObject(0).getUtfString("message");
		String msgSender = ar.getSFSObject(0).getUtfString("sender");
		if(msg.equals(message) && msgSender.equals(sender))
			return "pass";
		
		
		return msg;
	}
	private String testGetRoomMessage(String sender, String roomName, int amount, String message,IDBManager db){
		Messaging mg = new Messaging();
		ISFSObject ret = mg.messanger(null, "room", roomName, "get", sender, sender, amount, null,db);
		if(ret == null)
			return "fail";
		ISFSArray ar = ret.getSFSArray("messages");
		String msg = ar.getSFSObject(0).getUtfString("message");
		String msgSender = ar.getSFSObject(0).getUtfString("sender");
		if(msg.equals(message) && msgSender.equals(sender))
			return "pass";
		
		
		return msg;
	}
	private String testGetPrivateMessage(String sender, String zoneName, int amount, String message,IDBManager db){
		Messaging mg = new Messaging();
		ISFSObject ret = mg.messanger(null, "private", null, "get", sender, sender, amount, null,db);
		if(ret == null)
			return "fail";
		ISFSArray ar = ret.getSFSArray("messages");
		String msg = ar.getSFSObject(0).getUtfString("message");
		String msgSender = ar.getSFSObject(0).getUtfString("sender");
		if(msg.equals(message) && msgSender.equals(sender))
			return "pass";
		
		
		return msg;
	}
	
	
	
	
	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		
		
	}

}
