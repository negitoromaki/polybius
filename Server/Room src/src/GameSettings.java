

public class GameSettings {
	private String gameName = "";
	private int maxPlayers = 4;
	private int numPlayers = 0;
	private int numSpectators = 0;
	private boolean gameInProgress = false;
	private Game currentGame;
	public String getGameName() {
		return gameName;
	}
	public void setGameName(String gameName) {
		this.gameName = gameName;
	}
	public int getMaxPlayers() {
		return maxPlayers;
	}
	public void setMaxPlayers(int maxPlayers) {
		this.maxPlayers = maxPlayers;
	}
	public int getNumPlayers() {
		return numPlayers;
	}
	public void setNumPlayers(int numPlayers) {
		this.numPlayers = numPlayers;
	}
	public int getNumSpectators() {
		return numSpectators;
	}
	public void setNumSpectators(int numSpectators) {
		this.numSpectators = numSpectators;
	}
	public boolean isGameInProgress() {
		return gameInProgress;
	}
	public void setGameInProgress(boolean gameInProgress) {
		this.gameInProgress = gameInProgress;
	}
	public Game getCurrentGame() {
		return currentGame;
	}
	public void setCurrentGame(Game currentGame) {
		this.currentGame = currentGame;
	}
	
	
	
	
	
	

}
