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
-- Table structure for table `servants`
--

DROP TABLE IF EXISTS `servants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `servants` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AdmissionDate` datetime(6) NOT NULL,
  `InstituitionId` int NOT NULL,
  `DivisionId` int NOT NULL,
  `SectionId` int NOT NULL,
  `SectorId` int NOT NULL,
  `UserEvaluatorId1` int NOT NULL,
  `UserEvaluatorId2` int NOT NULL,
  `Masp` int NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Login` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Phone` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Position` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserType` int NOT NULL,
  `UserStats` int NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Servants_DivisionId` (`DivisionId`),
  KEY `IX_Servants_InstituitionId` (`InstituitionId`),
  KEY `IX_Servants_SectionId` (`SectionId`),
  KEY `IX_Servants_SectorId` (`SectorId`),
  KEY `IX_Servants_UserEvaluatorId1` (`UserEvaluatorId1`),
  KEY `IX_Servants_UserEvaluatorId2` (`UserEvaluatorId2`),
  CONSTRAINT `FK_Servants_Divisions_DivisionId` FOREIGN KEY (`DivisionId`) REFERENCES `divisions` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Servants_Evaluators_UserEvaluatorId1` FOREIGN KEY (`UserEvaluatorId1`) REFERENCES `evaluators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Servants_Evaluators_UserEvaluatorId2` FOREIGN KEY (`UserEvaluatorId2`) REFERENCES `evaluators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Servants_Instituitions_InstituitionId` FOREIGN KEY (`InstituitionId`) REFERENCES `instituitions` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Servants_Sections_SectionId` FOREIGN KEY (`SectionId`) REFERENCES `sections` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Servants_Sectors_SectorId` FOREIGN KEY (`SectorId`) REFERENCES `sectors` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servants`
--

LOCK TABLES `servants` WRITE;
/*!40000 ALTER TABLE `servants` DISABLE KEYS */;
INSERT INTO `servants` VALUES (1,'2024-02-10 00:00:00.000000',14,25,23,21,10,11,24681,'Fernanda Oliveira Cabral Neto','fernanda.oliveira','$2a$11$l/RxfrHc2x.bkEDwmD4RC.3s7.29ko6mZp0d9B1QOstWFel1Wa9LK','fernanda.oliveira@example.com','(38) 99512-0366','Técnico em Informática',3,2,'2024-03-19 13:25:36.342032','2024-03-19 14:43:47.499845','lucio.albuquerque'),(2,'2024-02-20 00:00:00.000000',14,25,23,21,10,10,24682,'Ricardo Martins','ricardo.martins','$2a$11$EWBuTBebtxz5zlVyybEjze2HvTN4W1/yzDGYmpdRrPWeS0EhVj86G','ricardo.martins@example.com','(38) 99516-2033','Técnico em Informática',3,1,'2024-03-19 13:26:36.017012',NULL,NULL),(3,'2024-03-12 00:00:00.000000',10,61,81,75,7,9,24683,'Aline Sousa','aline.sousa','$2a$11$wofs68hUakz6Jqd7iW.YWOdV4RsHrY4Mo3OsYGrlxOSKS8ud41GQu','aline.sousa@example.com','(38) 99203-1050','Assistente Social',3,1,'2024-03-19 13:27:48.536226',NULL,NULL),(4,'2024-03-10 00:00:00.000000',10,61,81,75,7,7,24685,'Camila Costa','camila.costa','$2a$11$fedoi10MbzgxSgqv47syu.rWuz31U77cQ/a3i0wvFFnqrTtAfccd.','camila.costa@example.com','(31) 91165-0244','Psicólogo',3,1,'2024-03-19 13:29:12.078060',NULL,NULL),(5,'2023-02-11 00:00:00.000000',11,45,39,36,2,5,24686,'Lucas Santos','lucas.santos','$2a$11$T7NsdSDHLDRpu3dbWSo6L.t5dHRG3fX4/SlVCndkF0F4a3Io4dGJW','lucas.santos@example.com','(38) 99554-1688','Servente Escolar',3,1,'2024-03-19 13:30:30.518230',NULL,NULL),(6,'2024-02-11 00:00:00.000000',11,2,43,40,2,2,24687,'Amanda Fernandes','amanda.fernandes','$2a$11$m01lX1bRBAEzDWGTyV.yhu.V4Ayqvda.LVgFhMkfJGeFlT5suieBG','amanda.fernandes@example.com','(38) 99120-2560','Servente de Limpeza',3,1,'2024-03-19 13:31:28.394902',NULL,NULL);
/*!40000 ALTER TABLE `servants` ENABLE KEYS */;
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
