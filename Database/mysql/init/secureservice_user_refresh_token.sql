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
-- Table structure for table `user_refresh_token`
--

DROP TABLE IF EXISTS `user_refresh_token`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_refresh_token` (
  `user_id` varchar(100) NOT NULL,
  `token_id` varchar(255) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `expiry_date` datetime NOT NULL,
  `is_active` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`token_id`),
  KEY `pk_user_refresh_token` (`token_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_refresh_token`
--

LOCK TABLES `user_refresh_token` WRITE;
/*!40000 ALTER TABLE `user_refresh_token` DISABLE KEYS */;
INSERT INTO `user_refresh_token` VALUES ('samith','64b05e18-51ce-4033-8d0f-91f6f9a126aa#2df509cb-3d98-4c64-a324-9549fb53ac2f','2025-04-06 23:08:04','2025-04-13 23:08:04',1),('samith','856f4048-63a0-4226-b5f5-be258c40cc58#6bd775ae-e425-4023-a518-9be767344343','2025-04-20 03:18:46','2025-04-27 03:18:46',1),('sourav','874729b3-e24f-4937-9c65-d5a26ab697a8#ae05eaea-b63d-4213-9244-669b81c8ea64','2025-04-20 02:11:27','2025-04-27 02:11:27',0),('sourav','ef4c6b1c-92f1-4f81-b492-6672c072b598#f04e66e1-d9fb-44b5-bd2a-30914f05f139','2025-04-06 23:17:24','2025-04-13 23:17:24',0),('sourav','f12de16c-65e5-412e-9da4-3fa5721c0d4f#e9b6fb73-ba9e-44cc-bbdb-768e69a2970e','2025-04-20 03:11:37','2025-04-27 03:11:37',1),('samith','f8e1ba70-72a1-4fc5-9a1f-8cf0a95847ba#a256ff17-ba20-4b9c-b5cc-05862dce1364','2025-04-11 10:39:52','2025-04-18 10:39:52',1);
/*!40000 ALTER TABLE `user_refresh_token` ENABLE KEYS */;
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
