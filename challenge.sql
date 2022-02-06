/*
 Navicat Premium Data Transfer

 Source Server         : Servidor Local
 Source Server Type    : MySQL
 Source Server Version : 80027
 Source Host           : localhost:3306
 Source Schema         : challenge

 Target Server Type    : MySQL
 Target Server Version : 80027
 File Encoding         : 65001

 Date: 06/02/2022 04:00:50
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters`  (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Image` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Name` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Age` int NULL DEFAULT NULL,
  `Weight` decimal(10, 2) NULL DEFAULT NULL,
  `History` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters
-- ----------------------------

-- ----------------------------
-- Table structure for productions
-- ----------------------------
DROP TABLE IF EXISTS `productions`;
CREATE TABLE `productions`  (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Image` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Title` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Launch` date NULL DEFAULT NULL,
  `Rating` decimal(10, 2) NULL DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of productions
-- ----------------------------

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Email` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Password` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES (1, 'user@example.com', 'password');

-- ----------------------------
-- Procedure structure for CreateCharacter
-- ----------------------------
DROP PROCEDURE IF EXISTS `CreateCharacter`;
delimiter ;;
CREATE PROCEDURE `CreateCharacter`(IN `cImage` text,IN `cName` text,IN `cAge` int,IN `cWeight` decimal,IN `cHistory` text)
BEGIN
	INSERT INTO characters (Image, Name, Age, Weight, History) VALUES (cImage, cName, cAge, cWeight, cHistory);
	SELECT LAST_INSERT_ID();
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for CreateProduction
-- ----------------------------
DROP PROCEDURE IF EXISTS `CreateProduction`;
delimiter ;;
CREATE PROCEDURE `CreateProduction`(IN `pImage` text,IN `pTitle` text,IN `pLaunch` date,IN `pRating` decimal)
BEGIN
	INSERT INTO productions (Image, Title, Launch, Rating) VALUES (pImage, pTitle, pLaunch, pRating);
	SELECT LAST_INSERT_ID();
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for DeleteCharacter
-- ----------------------------
DROP PROCEDURE IF EXISTS `DeleteCharacter`;
delimiter ;;
CREATE PROCEDURE `DeleteCharacter`(IN `cID` int)
BEGIN
	DELETE FROM characters WHERE ID = cID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for DeleteProduction
-- ----------------------------
DROP PROCEDURE IF EXISTS `DeleteProduction`;
delimiter ;;
CREATE PROCEDURE `DeleteProduction`(IN `pID` int)
BEGIN
	DELETE FROM productions WHERE ID = pID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetCharacter
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetCharacter`;
delimiter ;;
CREATE PROCEDURE `GetCharacter`(IN `cID` int)
BEGIN
	SELECT * FROM characters WHERE ID = cID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetProduction
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetProduction`;
delimiter ;;
CREATE PROCEDURE `GetProduction`(IN `pID` int)
BEGIN
	SELECT * FROM productions WHERE ID = pID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for GetUser
-- ----------------------------
DROP PROCEDURE IF EXISTS `GetUser`;
delimiter ;;
CREATE PROCEDURE `GetUser`(IN `uID` int)
BEGIN
	SELECT * FROM users WHERE ID = uID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for Login
-- ----------------------------
DROP PROCEDURE IF EXISTS `Login`;
delimiter ;;
CREATE PROCEDURE `Login`(IN `uEmail` text,IN `uPassword` text)
BEGIN
	SELECT ID FROM users WHERE Email = uEmail AND Password = uPassword LIMIT 1;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for Register
-- ----------------------------
DROP PROCEDURE IF EXISTS `Register`;
delimiter ;;
CREATE PROCEDURE `Register`(IN `uEmail` text,IN `uPassword` text)
BEGIN
	INSERT INTO users (Email, Password) VALUES (uEmail, uPassword);
	SELECT LAST_INSERT_ID();
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for UpdateCharacter
-- ----------------------------
DROP PROCEDURE IF EXISTS `UpdateCharacter`;
delimiter ;;
CREATE PROCEDURE `UpdateCharacter`(IN `cID` int,IN `cImage` text,IN `cName` text,IN `cAge` int,IN `cWeight` decimal,IN `cHistory` text)
BEGIN
	UPDATE characters SET Image = cImage, Name = cName, Age = cAge, Weight = cWeight, History = cHistory WHERE ID = cID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for UpdateProduction
-- ----------------------------
DROP PROCEDURE IF EXISTS `UpdateProduction`;
delimiter ;;
CREATE PROCEDURE `UpdateProduction`(IN `pID` int,IN `pImage` text,IN `pTitle` text,IN `pLaunch` date,IN `pRating` decimal)
BEGIN
	UPDATE productions SET Image = pImage, Title = pTitle, Launch = pLaunch, Rating = pRating WHERE ID = pID;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for UserExists
-- ----------------------------
DROP PROCEDURE IF EXISTS `UserExists`;
delimiter ;;
CREATE PROCEDURE `UserExists`(IN `uEmail` text,IN `uPassword` text)
BEGIN
	SELECT EXISTS(SELECT * FROM users WHERE Email = uEmail AND Password = uPassword);
END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
