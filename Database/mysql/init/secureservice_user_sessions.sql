-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: secureservice
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `user_sessions`
--

DROP TABLE IF EXISTS `user_sessions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_sessions` (
  `SessionID` varchar(255) NOT NULL,
  `UserId` varchar(10) NOT NULL,
  `IsActiveSessionFlag` tinyint(1) NOT NULL DEFAULT '0',
  `SessionStartTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SessionEndTime` datetime DEFAULT NULL,
  PRIMARY KEY (`SessionID`),
  KEY `fk-usersession_idx` (`UserId`),
  CONSTRAINT `fk-usersession` FOREIGN KEY (`UserId`) REFERENCES `users_details` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_sessions`
--

LOCK TABLES `user_sessions` WRITE;
/*!40000 ALTER TABLE `user_sessions` DISABLE KEYS */;
INSERT INTO `user_sessions` VALUES ('0421a884-e73e-4bb6-9587-e333e9844afa','sourav',0,'2025-04-20 02:16:35','2025-04-20 02:31:35'),('64b05e18-51ce-4033-8d0f-91f6f9a126aa','samith',0,'2025-04-06 23:08:04','2025-04-06 23:23:04'),('77359b8f-ee88-4891-b6f3-de03aafc8d42','sourav',0,'2025-04-20 02:18:10','2025-04-20 02:33:10'),('822029df-e31e-42fe-a3ae-64488b23d689','samith',0,'2025-03-17 20:49:29','2025-03-17 21:49:29'),('8494ccbd-b90d-4513-b41e-45e2ee8d4db2','sourav',0,'2025-04-20 02:17:38','2025-04-20 02:32:38'),('856f4048-63a0-4226-b5f5-be258c40cc58','samith',1,'2025-04-20 03:18:46','2025-04-20 03:33:46'),('874729b3-e24f-4937-9c65-d5a26ab697a8','sourav',0,'2025-04-20 02:11:26','2025-04-20 02:26:26'),('8cd49f90-708b-46f2-9211-e35dc83225e8','sourav',0,'2025-04-20 02:17:34','2025-04-20 02:32:34'),('920b205d-08d7-4c29-9c81-2d8b844ac1ff','samith',0,'2025-04-06 23:25:17','2025-04-06 23:40:17'),('ad1a0e2c-fd98-448c-9eec-64d3382b1bce','samith',0,'2025-04-06 23:10:37','2025-04-06 23:25:37'),('c52d7c90-c4b4-41e7-9ba5-83dc2e7f5882','samith',0,'2025-04-11 10:40:42','2025-04-11 10:55:42'),('ef4c6b1c-92f1-4f81-b492-6672c072b598','sourav',0,'2025-04-06 23:17:24','2025-04-06 23:32:24'),('f12de16c-65e5-412e-9da4-3fa5721c0d4f','sourav',1,'2025-04-20 03:11:37','2025-04-20 03:26:37'),('f8e1ba70-72a1-4fc5-9a1f-8cf0a95847ba','samith',0,'2025-04-11 10:39:52','2025-04-11 10:54:52');
/*!40000 ALTER TABLE `user_sessions` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-20 20:30:58
