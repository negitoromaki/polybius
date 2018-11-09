-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Nov 09, 2018 at 03:59 PM
-- Server version: 5.7.23
-- PHP Version: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `messages`
--
CREATE DATABASE IF NOT EXISTS `messages` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `messages`;

-- --------------------------------------------------------

--
-- Table structure for table `msgs`
--

DROP TABLE IF EXISTS `msgs`;
CREATE TABLE IF NOT EXISTS `msgs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sender` text NOT NULL,
  `reciever` text NOT NULL,
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `message` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
--
-- Database: `reporting`
--
CREATE DATABASE IF NOT EXISTS `reporting` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `reporting`;

-- --------------------------------------------------------

--
-- Table structure for table `userfeedback`
--

DROP TABLE IF EXISTS `userfeedback`;
CREATE TABLE IF NOT EXISTS `userfeedback` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user` text NOT NULL,
  `feedback` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `userreport`
--

DROP TABLE IF EXISTS `userreport`;
CREATE TABLE IF NOT EXISTS `userreport` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `reporteduser` text NOT NULL,
  `reporter` text NOT NULL,
  `report` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
--
-- Database: `users`
--
CREATE DATABASE IF NOT EXISTS `users` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `users`;

-- --------------------------------------------------------

--
-- Table structure for table `msgs`
--

DROP TABLE IF EXISTS `msgs`;
CREATE TABLE IF NOT EXISTS `msgs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sender` text NOT NULL,
  `reciever` text NOT NULL,
  `time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `message` text NOT NULL,
  `level` text NOT NULL,
  `levelname` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=571 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `msgs`
--

INSERT INTO `msgs` (`id`, `sender`, `reciever`, `time`, `message`, `level`, `levelname`) VALUES
(558, 'testone', 'testtwo', '2018-11-02 00:00:00', 'Sup', 'private', 'none'),
(559, 'testone', 'testtwo', '2018-11-02 00:00:00', 'E', 'private', 'none'),
(569, 'testone', 'demotest', '2018-11-02 00:00:00', 'Hello', 'private', 'none'),
(570, 'testone', 'demotest', '2018-11-02 00:00:00', 'test', 'private', 'none'),
(557, 'testone', 'testtwo', '2018-11-02 00:00:00', 'oh hey', 'private', 'none'),
(553, 'testtwo', 'testone', '2018-11-02 00:00:00', 'how are ya', 'private', 'none'),
(552, 'testone', 'testtwo', '2018-11-02 00:00:00', 'hey', 'private', 'none'),
(551, 'testone', 'testtwo', '2018-11-01 00:00:00', 'hey', 'private', 'none'),
(548, 'testone', 'testtwo', '2018-11-01 00:00:00', 'dddd', 'private', 'none'),
(549, 'testone', 'testtwo', '2018-11-01 00:00:00', 'ddd', 'private', 'none'),
(550, 'testone', 'testtwo', '2018-11-01 00:00:00', 'Hello', 'private', 'none'),
(547, 'testone', 'testtwo', '2018-11-01 00:00:00', 'dddddddd', 'private', 'none'),
(546, 'testone', 'testtwo', '2018-11-01 00:00:00', 'ddddd', 'private', 'none'),
(540, 'testone', 'testtwo', '2018-11-01 00:00:00', 'eat', 'private', 'none'),
(541, 'testone', 'testtwo', '2018-11-01 00:00:00', 'eat', 'private', 'none'),
(542, 'testtwo', 'testone', '2018-11-01 00:00:00', 'eat', 'private', 'none'),
(543, 'testtwo', 'testone', '2018-11-01 00:00:00', 'pant', 'private', 'none'),
(544, 'testone', 'testtwo', '2018-11-01 00:00:00', 'EAEWAREFDW', 'private', 'none'),
(545, 'testone', 'testtwo', '2018-11-01 00:00:00', 'ddddddd', 'private', 'none'),
(539, 'asd', 'testone', '2018-11-01 00:00:00', 'asdf', 'private', 'none'),
(537, 'asd', 'testone', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(538, 'asd', 'testone', '2018-11-01 00:00:00', 'asd', 'private', 'none'),
(536, 'testone', 'testtwo', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(535, 'testone', 'testtwo', '2018-11-01 00:00:00', 'hello', 'private', 'none'),
(534, 'testone', 'testtwo', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(533, 'testone', 'testtwo', '2018-11-01 00:00:00', 'what u doin', 'private', 'none'),
(532, 'testone', 'testtwo', '2018-11-01 00:00:00', 'hey', 'private', 'none'),
(530, 'testone', 'testtwo', '2018-11-01 00:00:00', 'oh hello', 'private', 'none'),
(531, 'asd', 'asdfasdf', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(529, 'asd', 'asdfasdf', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(528, 'testone', 'testtwo', '2018-11-01 00:00:00', 'hello', 'private', 'none'),
(506, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(505, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(504, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(503, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(502, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(501, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(500, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(499, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(498, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(497, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(496, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(495, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(494, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(493, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(492, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(491, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(490, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(489, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(488, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(487, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(486, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(485, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(484, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(483, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(482, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(481, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(480, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(479, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(478, 'asd', 'asdfasdf', '2018-11-01 00:00:00', '', 'private', 'none'),
(477, 'asdfasdf', 'asd', '2018-11-01 00:00:00', 'sdfsasdfasdfsafadsfddsfadsf', 'private', 'none'),
(476, 'asdfasdf', 'asd', '2018-11-01 00:00:00', 'sdfsdff', 'private', 'none'),
(460, 'asd', 'asdfasdf', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(459, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(458, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(457, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(456, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(455, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(452, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(453, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(454, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(451, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(449, 'testone', 'testtwo', '2018-11-01 00:00:00', 'eeeeeeeeee', 'private', 'none'),
(448, 'testone', 'testtwo', '2018-11-01 00:00:00', 'each parnt', 'private', 'none'),
(446, 'testtwo', 'testone', '2018-11-01 00:00:00', 'yeet', 'private', 'none'),
(444, 'testtwo', 'testone', '2018-11-01 00:00:00', 'sup', 'private', 'none'),
(445, 'testone', 'testtwo', '2018-11-01 00:00:00', 'yeet', 'private', 'none'),
(443, 'testone', 'testtwo', '2018-11-01 00:00:00', 'wassup', 'private', 'none'),
(374, 'testone', 'asd', '2018-11-01 00:00:00', 'who are you', 'private', 'none'),
(447, 'asd', 'testone', '2018-11-01 00:00:00', 'test', 'private', 'none'),
(369, 'testone', 'asd', '2018-10-31 00:00:00', 'testing1', 'private', 'none'),
(450, 'testone', 'testtwo', '2018-11-01 00:00:00', '', 'private', 'none'),
(439, 'testtwo', 'testone', '2018-11-01 00:00:00', 'oh boy', 'private', 'none');

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

DROP TABLE IF EXISTS `rooms`;
CREATE TABLE IF NOT EXISTS `rooms` (
  `roomName` text NOT NULL,
  `roomType` int(11) NOT NULL DEFAULT '0',
  `roomID` int(11) NOT NULL AUTO_INCREMENT,
  `usersInRoom` text,
  `gameType` text NOT NULL,
  `latcord` float NOT NULL,
  `longcord` float NOT NULL,
  PRIMARY KEY (`roomID`)
) ENGINE=MyISAM AUTO_INCREMENT=52 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`roomName`, `roomType`, `roomID`, `usersInRoom`, `gameType`, `latcord`, `longcord`) VALUES
('3.540847E+28', 0, 20, NULL, '1661958801', 25, 25),
('4.781481E+28', 0, 19, NULL, '1059902339', 25, 25),
('3.548617E+28', 0, 21, NULL, '858644778', 25, 25),
('3.979649E+28', 0, 23, NULL, '775739495', 25, 25),
('3.707664E+28', 0, 25, NULL, '1798033181', 25, 25),
('2.749634E+28', 0, 26, NULL, '2133833123', 25, 25),
('5.048668E+28', 25, 27, NULL, 'Pong', -2051520000, 25),
('1.502013E+28', 25, 28, NULL, 'Pong', 634654000, 25),
('8.062861E+28', 25, 29, NULL, 'Pong', -1082600000, 25),
('7.117522E+28', 25, 30, NULL, 'Pong', 682123000, 25),
('8.84363E+28', 25, 31, NULL, 'Pong', -387906000, 25),
('2.411665E+28', 25, 32, NULL, 'Pong', -1414970000, 25),
('6.500861E+28', 25, 33, NULL, 'Pong', 740807000, 25),
('3.43648E+28', 25, 34, NULL, 'Pong', -486140000, 25),
('9.094546E+28', 25, 35, NULL, 'Pong', -1310440000, 25),
('6.159742E+28', 25, 36, NULL, 'Pong', -1358890000, 25),
('6.220226E+28', 25, 37, NULL, 'Pong', 870452000, 25),
('7.679264E+28', -87, 38, NULL, 'Pong', -1615760000, 40.427),
('4.02395E+28', -87, 39, NULL, 'Pong', 681780000, 40.427),
('1.636741E+28', -87, 40, NULL, 'Pong', -306350000, 40.427),
('8.712553E+28', -87, 41, NULL, 'Pong', -405181000, 40.427),
('5.211453E+28', -87, 42, NULL, 'Pong', 1854790000, 40.427),
('7.02727E+28', -87, 43, NULL, 'Pong', -1357620000, 40.427),
('1.174151E+28', -87, 44, NULL, 'Pong', 1844250000, 40.427),
('8.665316E+28', -87, 45, NULL, 'Pong', -1609290000, 40.427),
('5.812409E+28', -87, 46, NULL, 'Pong', -2014050000, 40.427),
('6.579603E+28', -87, 47, NULL, 'Pong', 1828980000, 40.427),
('1.810824E+28', -87, 48, NULL, 'Pong', 1881620000, 40.427),
('8.928561E+28', -87, 49, NULL, 'Pong', -711953000, 40.427),
('2.372428E+28', -87, 50, NULL, 'Pong', 677778000, 40.427),
('8.112925E+28', -87, 51, NULL, 'Pong', -55093100, 40.427);

-- --------------------------------------------------------

--
-- Table structure for table `userdata`
--

DROP TABLE IF EXISTS `userdata`;
CREATE TABLE IF NOT EXISTS `userdata` (
  `username` text NOT NULL,
  `password` text NOT NULL,
  `email` text NOT NULL,
  `dob` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `isonline` int(1) NOT NULL DEFAULT '0',
  `currentgame` int(11) DEFAULT NULL,
  `friends` text,
  `statistics` text,
  `notification` text,
  `private` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=239 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `userdata`
--

INSERT INTO `userdata` (`username`, `password`, `email`, `dob`, `id`, `isonline`, `currentgame`, `friends`, `statistics`, `notification`, `private`) VALUES
('testone', 'testonepass', 'testerone@test.com', '2018-10-28 15:49:53', 91, 1, NULL, '154S94S210S237S236S', NULL, 'welcome!', 0),
('testing111', 'testing111', 'testertester@test.com', '2018-10-28 16:03:32', 94, 0, NULL, '91S85S', NULL, 'welcome!', 0),
('lucash', 'lucashahn', 'lucas@lucas.com', '2018-10-28 15:41:10', 89, 0, NULL, NULL, NULL, 'welcome!', 0),
('testinguser', 'testeduser', 'testing@testing.com', '2018-10-28 15:20:11', 85, 0, NULL, NULL, NULL, 'welcome!', 0),
('eatmypants', 'nothanks', 'pant@pan.com', '2018-10-29 17:13:14', 105, 0, NULL, NULL, NULL, 'welcome!', 0),
('testing1', 'asdfasdf', 'asdf@asdf.com', '2018-10-26 20:40:10', 57, 0, NULL, NULL, NULL, 'welcome!', 0),
('ree', 'everyone', 'eee@gen.com', '2018-10-21 17:10:20', 52, 0, NULL, NULL, NULL, 'welcome!', 0),
('Keaton', 'everyone', 'G', '2018-10-21 17:04:30', 51, 0, NULL, NULL, NULL, 'welcome!', 0),
('Tested', 'everyone', 'tested@gen.com', '2018-10-21 17:04:30', 50, 0, NULL, NULL, NULL, 'welcome!', 0),
('Test', 'everyone', 'tester@gen.com', '2018-10-21 17:03:49', 49, 0, NULL, NULL, NULL, 'welcome!', 0),
('T', 'everyone', 'test@gen.com', '2018-10-21 17:00:28', 48, 0, NULL, NULL, NULL, 'welcome!', 0),
('gib', 'megamind', 'tim@gove.com', '2018-10-21 01:29:32', 45, 0, NULL, NULL, NULL, 'welcome!', 0),
('jeff', 'everyone', 'jhon@gen.com', '2018-10-21 16:55:30', 47, 0, NULL, NULL, NULL, 'welcome!', 0),
('tim', 'bob', 'bob@bob.com', '2018-10-20 16:58:12', 41, 0, NULL, NULL, NULL, NULL, 0),
('eats', 'eatspass', 'eat@pant.com', '2018-10-28 16:18:45', 96, 0, NULL, NULL, NULL, 'welcome!', 0),
('hello', 'testinghello', 'hello@hello.com', '2018-10-28 16:59:46', 99, 0, NULL, NULL, NULL, 'welcome!', 0),
('asd', 'asdfasdf', 'asdf@asdf.co', '2018-10-30 21:51:11', 154, 0, NULL, '91S219S', NULL, 'welcome!', 0),
('demotest', 'demotestpass', 'demo@demo.com', '2018-11-02 11:57:24', 236, 1, NULL, '', NULL, 'welcome!', 0),
('testtwo', 'testtwopass', 'testtwo@two.com', '2018-11-01 16:38:04', 210, 0, NULL, '91S', NULL, 'welcome!', 0),
('asdfasdf', 'asdfasdf', 'asdf@aasdf.co', '2018-11-01 18:57:07', 219, 0, NULL, '154S', NULL, 'welcome!', 0),
('Sjsisjdjdjdj', 'shdhsjsjsjsjs', 'dndndjd@gmail.com', '2018-11-02 11:56:49', 235, 1, NULL, NULL, NULL, 'welcome!', 0),
('tester', 'bigtestboy', 'asdf@gmail.com', '2018-11-02 11:58:00', 237, 1, NULL, '', NULL, 'welcome!', 0);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
