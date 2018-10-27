import java.util.Random;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.api.response.*;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;

public class Pong extends Game{
	
	int[] score = {0,0};
	Random random = new Random();
	Room r;

	public Pong(){
		setGameName("Pong");
		Zone z = SmartFoxServer.getInstance().getZoneManager().getZoneByName("Polybius");
		r = z.getRoomByName(MainRoom.roomname);
	}
	
	@Override
	public void start(){
		ISFSObject starter = new SFSObject();
		starter.putInt("StartSide", random.nextInt(1)+1); //return 1 or 2 for player 1 or player 2
		score[0] = 0;
		score[1] = 0;
		starter.putInt("p1Score", score[0]);
		starter.putInt("p2Score", score[1]);
		MainRoom.gs.setGameInProgress(true);
		api.sendExtensionResponse("GameStart", starter, r.getUserList(), r, false);
	}
	
	public void updateScore(int pID, int amount){ //pID represents player 1, player 2, etc
		score[pID-1] += amount;
		
	}
	
	public int[] getScores(){
		return score;
	}

	@Override
	public void end(int endCode) {
		// TODO Auto-generated method stub
		ISFSObject ed = new SFSObject();
		ed.putInt("winner", (score[0] > score[1]? 1:2));
		ed.putInt("p1Score", score[0]);
		ed.putInt("p2Score", score[1]);
		MainRoom.gs.setGameInProgress(false);
		api.sendExtensionResponse("GameEnd", ed, r.getUserList(), r, false);
		
	}

	@Override
	public void input(ISFSObject o) {
		api.sendExtensionResponse("Input", o, r.getUserList(),r, false);
		
	}

}
