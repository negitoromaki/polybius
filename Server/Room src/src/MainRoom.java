

import com.smartfoxserver.v2.extensions.SFSExtension;

public class MainRoom extends SFSExtension{

	public static GameSettings gs = new GameSettings();
	public static String roomname;
	UTest t = new UTest(gs);
	@Override
	public void init() {
		
		roomname = getParentRoom().getName();
		trace("Room Extension loaded into: " + getParentRoom().getName());
		if(getParentRoom().getName().equals("Test")){
			
			trace("UTest exists: " + (t != null? "true": "false"));
			if(t != null)
				trace("\n=====Begin Test=====\n\n" + t.startTest());
			addRequestHandler("UnitTest", t);
		}else{
			
			
			
		}
		
		addRequestHandler("Game", GameManager.class);
		
	}
	
	
	

}
