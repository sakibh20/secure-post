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
-- Table structure for table `user_match`
--

DROP TABLE IF EXISTS `user_match`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_match` (
  `match_id` varchar(500) NOT NULL,
  `player1` varchar(100) NOT NULL,
  `player2` varchar(100) NOT NULL,
  `start_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `end_time` datetime DEFAULT NULL,
  `player1_moves` int DEFAULT NULL,
  `player2_moves` int DEFAULT NULL,
  `winner` varchar(100) DEFAULT NULL,
  `Status` varchar(50) DEFAULT 'PENDING',
  PRIMARY KEY (`match_id`),
  KEY `PK_user_match` (`match_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_match`
--

LOCK TABLES `user_match` WRITE;
/*!40000 ALTER TABLE `user_match` DISABLE KEYS */;
INSERT INTO `user_match` VALUES ('0f915ef7-8feb-4ba4-95de-976b41e67b6f','sourav','samith','2025-04-20 03:33:16','2025-04-20 03:34:14',5,6,'samith','FINNISHED'),('2a1e2e25-32cf-49cd-a681-6c7c592d0bd5','samith','sourav','2025-04-06 23:23:58','2025-04-06 23:27:40',5,6,'sourav','FINNISHED'),('c5a8fce3-7f19-4a17-9881-1b8c9b765d30','sourav','samith','2025-04-20 03:12:08',NULL,0,0,NULL,'DECLINED');
/*!40000 ALTER TABLE `user_match` ENABLE KEYS */;
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
