import java.sql.SQLException;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class CleanUp extends BaseServerEventHandler{

	@Override
	public void handleServerEvent(ISFSEvent arg0) throws SFSException {
		// TODO Auto-generated method stub
		Room r = (Room)arg0.getParameter(SFSEventParam.ROOM);
		IDBManager db = getParentExtension().getParentZone().getDBManager();
		cleaner(r,db);
		
	}
	
	public boolean cleaner(Room r, IDBManager db){
		
		String roomName = r.getName();
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.removeRoom;
		trace("removing room: " + roomName);
		try {
			db.executeUpdate(sql, new Object[] {roomName});
			return true;
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return false;
		}
	}
	
public boolean cleanerS(String r, IDBManager db){
		
		String roomName = r;
		SQLStrings sqls = new SQLStrings();
		String sql = sqls.removeRoom;
		
		try {
			db.executeUpdate(sql, new Object[] {roomName});
			return true;
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return false;
		}
	}

}
