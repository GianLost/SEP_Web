CREATE DATABASE  IF NOT EXISTS `sep_db` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `sep_db`;
-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: sep_db
-- ------------------------------------------------------
-- Server version	8.0.35

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
-- Table structure for table `licenses`
--

DROP TABLE IF EXISTS `licenses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `licenses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Time` int NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserAdministratorId` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Licenses_Name` (`Name`),
  KEY `IX_Licenses_UserAdministratorId` (`UserAdministratorId`),
  CONSTRAINT `FK_Licenses_Administrators_UserAdministratorId` FOREIGN KEY (`UserAdministratorId`) REFERENCES `administrators` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `licenses`
--

LOCK TABLES `licenses` WRITE;
/*!40000 ALTER TABLE `licenses` DISABLE KEYS */;
INSERT INTO `licenses` VALUES (1,'Licença Afastamento',30,'2024-03-13 13:46:22.137315',NULL,NULL,1),(2,'Licença Cargo Comissionado',40,'2024-03-13 13:46:43.281478',NULL,NULL,1),(3,'Licença Acidente',25,'2024-03-13 13:47:06.705659',NULL,NULL,1),(4,'Licença Afastamento p/ Estudos',28,'2024-03-13 13:47:34.451075',NULL,NULL,1),(5,'Licença Casamento',43,'2024-03-13 13:47:47.491334',NULL,NULL,1),(6,'Licença Interesse Particular',27,'2024-03-13 13:48:30.509475',NULL,NULL,1),(7,'Licença Maternidade',38,'2024-03-13 13:48:42.485592',NULL,NULL,1),(8,'Licença Óbito',32,'2024-03-13 13:48:55.126268',NULL,NULL,1),(9,'Licença p/ Servir em Outro Órgão',41,'2024-03-13 13:49:16.942946',NULL,NULL,1),(10,'Licença Paternidade',25,'2024-03-13 13:49:47.260657',NULL,NULL,1),(11,'Licença Prêmio',20,'2024-03-13 13:50:02.745007',NULL,NULL,1),(12,'Licença Saúde',30,'2024-03-13 13:50:15.160263',NULL,NULL,1);
/*!40000 ALTER TABLE `licenses` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-19 17:57:01
