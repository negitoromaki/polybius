#!/usr/bin/env python
import sqlite3
from datetime import date, datetime
from flask import Flask
from flask import request
from flask import jsonify
from functools import wraps
import json

# Connect database
db = sqlite3.connect('polybius.db')
c = db.cursor()

# Make sure user database exists
c.execute("""
	CREATE TABLE IF NOT EXISTS users (
		userID 			integer primary key autoincrement unique,
		username 	text unique not null,
		password 	text not null,
		email 		text unique not null,
		dob 		text not null,
		isOnline 	bit not null,
		privacy		bit not null,
		currLobbyID integer,
		reports		integer
	);
""")
db.commit()

# Make sure messages db exists
c.execute("""
	CREATE TABLE IF NOT EXISTS messages (
		messageID 	integer primary key autoincrement unique,
		senderID 	integer not null,
		receiverID 	integer not null,
		time 		datetime unique not null,
		message 	text not null
	);
""")
db.commit()

# Make sure room database exists
c.execute("""
	CREATE TABLE IF NOT EXISTS lobbies (
		lobbyID 	integer primary key autoincrement unique,
		name 		text unique not null,
		gameType 	text not null,
		latCoord 	float unique not null,
		longCoord	float unique not null
	);
""")
db.commit()

# Make sure friends database exists
c.execute("""
	CREATE TABLE IF NOT EXISTS friends (
		id	integer primary key autoincrement unique,
		user1ID	integer not null,
		user2ID	integer not null
	);
""")
db.commit()

# Make sure block database exists
c.execute("""
	CREATE TABLE IF NOT EXISTS blocked (
		id          integer primary key autoincrement unique,
		blockerID   text not null,
		blockedID   text not null
	);
""")
db.commit()

# Make sure statistics database exists
c.execute("""
	CREATE TABLE IF NOT EXISTS stats (
		id		integer primary key autoincrement unique,
		userID		integer not null,
		pongWins	integer not null
	);
""")
db.commit()

# Close cursor and db
c.close()
db.close()

# Start flask server
# Authentication from http://blog.luisrei.com/articles/flaskrest.html
app = Flask(__name__)

# Auth checking, basic http authentication
def check_auth(username, password):
    return username == 'asdf' and password == 'asdf'

def authenticate():
    message = {'message': "Authenticate."}
    resp = jsonify(message)
    resp.status_code = 401
    resp.headers['WWW-Authenticate'] = 'Basic realm="Example"'
    return resp

def requires_auth(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        auth = request.authorization
        if not auth:
            return authenticate()
        elif not check_auth(auth.username, auth.password):
            return authenticate()
        return f(*args, **kwargs)
    return decorated

# Routes
@app.route('/users', methods = ['GET', 'POST', 'PUT', 'DELETE'])
@app.route('/users/<user_id>', methods = ['GET', 'POST', 'PUT', 'DELETE'])
#@requires_auth
def api_users(user_id = -1):
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Search Users
	if request.method == 'GET':

		# Get vars
		userID 		= request.args.get('userID')
		username 	= request.args.get('username')
		search 		= request.args.get('search')
		lobby		= request.args.get('lobby')

		# Search by id
		if userID:
			c.execute('SELECT * FROM users WHERE userID = ?', (userID,))

		# Search by username
		elif username:
			c.execute('SELECT * FROM users WHERE username = ?', (username,))

		# String search
		elif search:
			c.execute('SELECT * FROM users WHERE username LIKE ?', ("%" + search + "%",))

		# Search for user in lobby
		elif lobby:
			c.execute('SELECT * FROM users WHERE currLobbyID = ?', (lobby,))

		# Get all users
		else:
			c.execute('SELECT * FROM users')

		# Get column names and desc and turn into json
		columns = c.description
		resp = jsonify([{columns[index][0]:column for index, column in enumerate(value)} for value in c.fetchall()])

	# Add new user by posting
	elif request.method == 'POST':

		# Get vars
		username 	= json.get('username')
		password 	= json.get('password')
		email 		= json.get('email')
		dob			= json.get('dob')
		privacy 	= json.get('privacy')

		# Error checking
		if not username or not password or not email or not dob:
			resp = jsonify(dict(success=False, message="Username/Password/Email/Dob not specified"))
		else:
			# Insert into server or ignore if duplicate
			c.execute('INSERT or IGNORE INTO users (username, password, email, dob, privacy, isOnline, loggedIn, reports) VALUES (?, ?, ?, ?, ?, 1, 1, 0)', (username, password, email, dob, privacy))
			db.commit()

			if c.rowcount > 0:
				resp = jsonify(dict(success=True, message="Added new user"))
				c.execute('SELECT userID FROM users WHERE username = ?',(username,))
				r= int(c.fetchone()[0])
				print(r)
				c.execute('INSERT or IGNORE INTO stats (userID,pongWins) VALUES (?,0)',(r,))
				db.commit()
			else:
				resp = jsonify(dict(success=False, message="User fields not unique"))

	# Update user information
	elif request.method == "PUT":

		# Get vars
		if json:
			userID 			= json.get('userID')
			username 		= json.get('username')
			password 		= json.get('password')
			email 			= json.get('email')
			dob				= json.get('dob')
			isOnline		= json.get('isOnline')
			privacy 		= json.get('privacy')
			currLobbyID 	= json.get('currLobbyID')

		# ID is required to update user
		if userID:

			# Update username
			if username:
				c.execute('UPDATE users SET username = ? WHERE userID = ?', (username, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated username"))
				else:
					resp = jsonify(dict(success=False, message="Could not update username"))

			# Update password
			elif password:
				c.execute('UPDATE users SET password = ? WHERE userID = ?',
				          (password, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated password"))
				else:
					resp = jsonify(dict(success=False, message="Could not update password"))

			# Update email
			elif email:
				c.execute('UPDATE users SET email = ? WHERE userID = ?', (email, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated email"))
				else:
					resp = jsonify(dict(success=False, message="Could not update email"))

			# Update dob
			elif dob:
				c.execute('UPDATE users SET dob = ? WHERE userID = ?', (dob, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated dob"))
				else:
					resp = jsonify(dict(success=False, message="Could not update dob"))

			# Update isOnline
			elif isOnline:
				c.execute('UPDATE users SET isOnline = ? WHERE userID = ?',
				          (isOnline, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated isOnline"))
				else:
					resp = jsonify(dict(success=False, message="Could not update isOnline"))

			# Update privacy
			elif privacy:
				c.execute('UPDATE users SET privacy = ? WHERE userID = ?', (privacy, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated privacy"))
				else:
					resp = jsonify(dict(success=False, message="Could not update privacy"))

			# Update currLobbyID
			elif currLobbyID:
				c.execute('UPDATE users SET currLobbyID = ? WHERE userID = ?',
				          (currLobbyID, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated currLobbyID"))
				else:
					resp = jsonify(dict(success=False, message="Could not update currLobbyID"))

			# Error
			else:
				resp = jsonify(dict(success=False, message = "No user fields found"))

		# No id
		else:
			resp = jsonify(dict(success=False, message = "userID not specified"))

	# Delete user
	elif request.method == 'DELETE':

		# Get vars
		if user_id is not -1:
			userID = user_id

		if userID:
			c.execute('DELETE FROM users WHERE userID = ?', (userID,))
			db.commit()
			if c.rowcount > 0:
				resp = jsonify(dict(success=True, message="Deleted user"))
			else:
				resp = jsonify(dict(success=False, message="Could not delete user"))
		else:
			resp = jsonify(dict(success=False, message = "userID not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

@app.route('/messages', methods = ['GET', 'POST'])
#@requires_auth
def api_count():
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Get messages
	if request.method == "GET":

		# Get vars
		receiverID  = request.args.get('receiverID')
		senderID 	= request.args.get('senderID')

		if receiverID and senderID:
			c.execute('SELECT * FROM messages WHERE receiverID = ? AND senderID = ?', (receiverID, senderID))

			# Get column names and desc and turn into json
			columns = c.description
			resp = jsonify([{columns[index][0]:column for index, column in enumerate(value)} for value in c.fetchall()])

			# Delete messages once read
			c.execute('DELETE FROM messages WHERE receiverID = ? AND senderID = ?', (receiverID, senderID))
			db.commit()
		else:
			resp = jsonify(dict(success=False, message = "receiverID/senderID not specified"))

	# Add messages
	elif request.method == "POST":

		# Get variables
		senderID 	= json.get('senderID')
		receiverID 	= json.get('receiverID')
		message 	= json.get('message')

		# Error checking
		if senderID and receiverID and message:
			# Insert into messages db or ignore if duplicate
			c.execute('INSERT or IGNORE INTO messages (senderID, receiverID, time, message) VALUES (?, ?, ?, ?)',
			          (senderID, receiverID, datetime.now(), message))
			db.commit()
			if c.rowcount > 0:
				resp = jsonify(dict(success=True, message="Sent message"))
			else:
				resp = jsonify(dict(success=False, message="Could not send message"))
		else:
			resp = jsonify(dict(success=False, message = "senderID/receiverID/message not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

@app.route('/lobbies', methods=['GET', 'POST', 'PUT', 'DELETE'])
@app.route('/lobbies/<lobby_id>', methods=['GET', 'POST', 'PUT', 'DELETE'])
#@requires_auth
def api_lobbies(lobby_id = -1):
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Get lobbies
	if request.method == 'GET':

		# Get vars
		gameType = request.args.get('gameType')

		if gameType:
			c.execute('SELECT * FROM lobbies WHERE gameType = ?', (gameType,))

			# Get column names and desc and turn into json
			columns = c.description
			resp = jsonify([{columns[index][0]:column for index,
                            column in enumerate(value)} for value in c.fetchall()])
		else:
			resp = jsonify(dict(success=False, message="gameType not specified"))

	# Add lobby
	elif request.method == 'POST':

		# Get vars
		name = json.get('name')
		gameType = json.get('gameType')
		latCoord = json.get('latCoord')
		longCoord = json.get('longCoord')

		# Error checking
		if name and gameType and latCoord and longCoord:
			# Insert into messages db or ignore if duplicate
			c.execute('INSERT or IGNORE INTO lobbies (name, gameType, latCoord, longCoord) VALUES (?, ?, ?, ?)',
			          (name, gameType, latCoord, longCoord))
			db.commit()
			if c.rowcount > 0:
				resp = jsonify(dict(success=True, message="Added lobby"))
			else:
				resp = jsonify(dict(success=False, message="Could not add lobby"))
		else:
			resp = jsonify(
			    dict(success=False, message="name/gameType/latCoord/longCoord not specified"))

	# Delete lobby
	elif request.method == 'DELETE':

		# Get vars
		if lobby_id is not -1:
			lobbyID = lobby_id

		# Error checking
		if lobbyID:
			# Insert into messages db or ignore if duplicate
			c.execute('DELETE from lobbies WHERE lobbyID = ?', (lobbyID,))
			db.commit()
			if c.rowcount > 0:
				resp = jsonify(dict(success=True, message="Deleted lobby"))
			else:
				resp = jsonify(dict(success=False, message="Could not delete lobby"))
		else:
			resp = jsonify(
			    dict(success=False, message="lobbyID not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

@app.route('/report', methods=['GET', 'POST'])
#@requires_auth
def api_report():
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Get messages
	if request.method == 'GET':

		# Get vars
		userID = request.args.get('userID')

		if userID:
			c.execute('SELECT reports FROM users WHERE userID = ?', (userID,))
			numReports = (c.fetchone())
			resp = jsonify(dict(reports=numReports))
		else:
			resp = jsonify(dict(success=False, message="userID not specified"))

	# Add messages
	elif request.method == 'POST':

		# Get vars
		userID = json.get('userID')

		if userID:
			c.execute('SELECT reports FROM users WHERE userID = ?', (userID,))
			numReports = int(c.fetchone()[0]) + 1

			# Deleting user if above 5
			if numReports > 5:
				c.execute('DELETE FROM users WHERE userID= ?', (numReports, userID))
			else:
				c.execute('UPDATE users SET reports = ? WHERE userID = ?',
				          (numReports, userID))
			db.commit()
			resp = jsonify(dict(success=True, message="User has been reported"))
		else:
			resp = jsonify(dict(success=False, message="userID not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

@app.route('/block', methods=['GET', 'POST', 'PUT'])
#@requires_auth
def api_block():
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Get blocking relationship
	if request.method == 'GET':

		# Get vars
		blockerID = request.args.get('blockerID')
		blockedID = request.args.get('blockedID')

		if blockerID and blockedID:
			c.execute('SELECT COUNT(*) FROM blocked WHERE blockerID = ? AND blockedID = ?', (blockerID, blockedID))
			resp = jsonify(dict(result=int(c.fetchone()[0])))
		else:
			resp = jsonify(dict(success=False, message="blockerID/blockedID not specified"))

	# Add blocking relationship
	elif request.method == 'POST':

		# Get vars
		blockerID = json.get('blockerID')
		blockedID = json.get('blockedID')

		if blockerID and blockedID:
			c.execute('INSERT or IGNORE INTO blocked (blockerID, blockedID) VALUES (?, ?)',
			          (blockerID, blockedID))
			db.commit()
			resp = jsonify(dict(success=True, message="User successfully blocked"))
		else:
			resp = jsonify(dict(success=False, message="blockerID/blockedID not specified"))

	# Remove blocking relationship
	elif request.method == 'PUT':

		# Get vars
		blockerID = json.get('blockerID')
		blockedID = json.get('blockedID')

		if blockerID and blockedID:
			c.execute('DELETE FROM blocked WHERE blockerID = ? AND blockedID = ?', (blockerID, blockedID,))
			db.commit()
			resp = jsonify(dict(success=True, message="Block successfully deleted"))
		else:
			resp = jsonify(dict(success=False, message="blockerID/blockedID not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

@app.route('/stats', methods=['GET', 'PUT'])
#@requires_auth
def api_stats():
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Get STATs
	if request.method == 'GET':

		# Get vars
		userID = request.args.get('userID')

		if userID:
			c.execute('SELECT * FROM stats WHERE userID = ?', (userID,))

			# Get column names and desc and turn into json
			columns = c.description
			resp = jsonify([{columns[index][0]:column for index,
                            column in enumerate(value)} for value in c.fetchall()])
		else:
			resp = jsonify(dict(success=False, message="userID not specified"))

	# Modify stats
	elif request.method == 'PUT':

		# Get vars
		userID 		= json.get('userID')
		pongWins 	= json.get('pongWins')

		if userID:
			# Update username
			if pongWins:
				c.execute('UPDATE stats SET pongWins = ? WHERE userID = ?', (pongWins, userID))
				db.commit()
				if c.rowcount > 0:
					resp = jsonify(dict(success=True, message="Updated pongWins"))
				else:
					resp = jsonify(dict(success=False, message="Could not update pongWins"))
		else:
			resp = jsonify(dict(success=False, message="userID not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

@app.route('/friends', methods=['GET', 'POST', 'PUT'])
#@requires_auth
def api_friends():
	db = sqlite3.connect('polybius.db')
	c = db.cursor()
	json = request.get_json()

	# Get friends
	if request.method == 'GET':

		# Get vars
		user1ID = request.args.get('user1ID')
		user2ID = request.args.get('user2ID')

		if user1ID:
			# Check if 2 userIDs, return friend status
			if user2ID:
				# Eliminate double entries
				if (user2ID < user1ID):
					temp = user2ID
					user2ID = user1ID
					user1ID = temp

				c.execute('SELECT COUNT(*) FROM friends WHERE user1ID = ? AND user2ID = ?', (user1ID, user2ID))
				resp = jsonify(dict(result=(c.fetchone())))

			# Only 1 userID, return all friends
			else:
				c.execute('SELECT * from friends WHERE user1ID = ? OR user2ID = ?', (user1ID, user1ID));

				# Get column names and desc and turn into json
				columns = c.description
				resp = jsonify([{columns[index][0]:column for index, column in enumerate(value)} for value in c.fetchall()])

		# No user1ID
		else:
			resp = jsonify(dict(success=False, message="user1ID not specified"))

	# Add friends
	elif request.method == 'POST':

		# Get vars
		user1ID = json.get('user1ID')
		user2ID = json.get('user2ID')

		# Eliminate double entries
		if (user2ID < user1ID):
			temp = user2ID
			user2ID = user1ID
			user1ID = temp

		if user1ID and user2ID:
			c.execute('INSERT or IGNORE INTO friends (user1ID, user2ID) VALUES (?, ?)', (user1ID, user2ID,))
			db.commit()
			resp = jsonify(dict(success=True, message="Friend successfully added"))
		else:
			resp = jsonify(dict(success=False, message="user1ID/user2ID not specified"))

	# Remove friends
	elif request.method == 'PUT':

		# Get vars
		user1ID = json.get('user1ID')
		user2ID = json.get('user2ID')

		# Eliminate double entries
		if (user2ID < user1ID):
			temp = user2ID
			user2ID = user1ID
			user1ID = temp

		if user1ID and user2ID:
			c.execute('DELETE FROM friends WHERE user1ID = ? AND user2ID = ?', (user1ID, user2ID,))
			db.commit()
			resp = jsonify(dict(success=True, message="Friend successfully deleted"))
		else:
			resp = jsonify(dict(success=False, message="user1ID/user2ID not specified"))

	# Close db connection and return data
	c.close()
	db.close()
	return resp

# Run server
if __name__ == '__main__':
    app.run(host="0.0.0.0", port="5000")