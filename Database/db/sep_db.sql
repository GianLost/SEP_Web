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
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20240319151959_SEP_Migration-01','6.0.0');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `administrators`
--

DROP TABLE IF EXISTS `administrators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `administrators` (
  `Id` int NOT NULL AUTO_INCREMENT,
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
  UNIQUE KEY `IX_Administrators_Email` (`Email`),
  UNIQUE KEY `IX_Administrators_Login` (`Login`),
  UNIQUE KEY `IX_Administrators_Masp` (`Masp`),
  UNIQUE KEY `IX_Administrators_Name` (`Name`),
  UNIQUE KEY `IX_Administrators_Password` (`Password`),
  UNIQUE KEY `IX_Administrators_Phone` (`Phone`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `administrators`
--

LOCK TABLES `administrators` WRITE;
/*!40000 ALTER TABLE `administrators` DISABLE KEYS */;
INSERT INTO `administrators` VALUES (NULL, 14205,'Gianluca Vialli Ribeiro Vieira','gianluca.vialli','$2a$11$xuKD7jOEbn78QYKqIio4DuyzAOJid2MuO9x1Wt5DoExKjEDRqGa7C','gianluca19993m@gmail.com','(38) 98808-7655','Técnico em Informática',1,1,'2023-09-24 16:12:35.870940',NULL,NULL),
(NULL,28405,'Anna Caroline Ribeiro Vieira','anna.caroline','$2a$11$QBnq1.zqjNBe3t.BcKJA1erGrtIxnUnNB0d6aydmSUJwLJRvUswV6','annacaroline@gmail.com','(38) 98475-6542','Auxiliar de Serviços Gerais',1,1,'2023-11-20 15:06:38.335022',NULL,NULL),
(NULL,28407,'Amilton Vieira de Souza','amilton.vieira','$2a$11$7jofOhICcRI.HPRBrrdv7e5vTcxcjyOWtvQq/PkXkNFWCT61U5v/e','amiltonvieira@gmail.com','(38) 98475-1325','Servente Escolar',1,0,'2023-11-20 15:07:14.821344','2023-11-20 15:17:08.734814','gianluca.vialli'),
(NULL,12345,'João da Silva','joao.silva','$2a$11$AtnUmbHNPlUITDQdRPmSYONd3zCcVi3PtT.ImtsMlBHihWhWDRl/S','joaosilva@gmail.com','(38) 98888-8888','Analista de Sistemas',1,1,'2024-03-19 12:40:39.826395',NULL,NULL),
(NULL,54321,'Maria Oliveira','maria.oliveira','$2a$11$nfGim7JyW.0pQK/I2haALOgA/ZCnv/NmuNzgPQG/0GP./oxg0Fllm','mariaoliveira@gmail.com','(38) 99999-9999','Desenvolvedora Web',1,1,'2024-03-19 12:50:23.354754',NULL,NULL),
(NULL,98765,'Pedro Santos','pedro.santos','$2a$11$zpmZ4f1NFmztWrg/kVJ2lOnKsxU0SzxLAC0fyq4mmXfQVTG7hw882','pedrosantos@gmail.com','(38) 91111-1111','Engenheiro de Software',1,1,'2024-03-19 12:51:16.003742',NULL,NULL),
(NULL,67890,'Ana Pereira','ana.pereira','$2a$11$cdIy/jw2ktgooJxvOZJAI.yweApZBaabkgDb2T8001/bt4v4vswAO','anapereira@gmail.com','(38) 92222-2222','Administradora de Redes',1,1,'2024-03-19 12:52:30.208444',NULL,NULL),
(NULL,24680,'Carlos Mendes','carlos.mendes','$2a$11$zmvo9c1SD2AByD0fnWMBsOG3vglPMd/bTp2q1LBSRz6j4Xqxi5OQy','carlosmendes@gmail.com','(38) 93333-3333','Técnico de Suporte',1,1,'2024-03-19 12:53:14.014471',NULL,NULL),
(NULL,13579,'Paula Costa','paula.costa','$2a$11$xchZRMxH6Vs9BCg08CkGcuC6PeapBjRo9WDfPRhEa3fx9/v8bgduG','paulacosta@gmail.com','(38) 94444-4444','Analista de Banco de Dados',1,1,'2024-03-19 12:54:14.876347',NULL,NULL),
(NULL,80246,'Juliana Lima','juliana.lima','$2a$11$D8jZ6kYi572Wt9IxxlpY0.m30GorjEK3D6n5c07wfS8lV9JtfA4Ay','julianalima@gmail.com','(38) 95555-5555','Farmacêutico',1,1,'2024-03-19 12:55:28.190266',NULL,NULL);
/*!40000 ALTER TABLE `administrators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `assessments`
--

DROP TABLE IF EXISTS `assessments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `assessments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Stats` int NOT NULL,
  `Phase` int NOT NULL,
  `StartEvaluationPeriod` datetime(6) DEFAULT NULL,
  `EndEvaluationPeriod` datetime(6) DEFAULT NULL,
  `CivilServantId` int NOT NULL,
  `UserEvaluatorId1` int NOT NULL,
  `UserEvaluatorId2` int NOT NULL,
  `EvaluatedFor` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Crit1_Clau1` int NOT NULL,
  `Tot_Crit1` int NOT NULL,
  `Average_Crit1` double NOT NULL,
  `Crit2_Clau1` int NOT NULL,
  `Crit2_Clau2` int NOT NULL,
  `Crit2_Clau3` int NOT NULL,
  `Crit2_Clau4` int NOT NULL,
  `Crit2_Clau5` int NOT NULL,
  `Tot_Crit2` int NOT NULL,
  `Average_Crit2` double NOT NULL,
  `Crit3_Clau1` int NOT NULL,
  `Crit3_Clau2` int NOT NULL,
  `Crit3_Clau3` int NOT NULL,
  `Crit3_Clau4` int NOT NULL,
  `Crit3_Clau5` int NOT NULL,
  `Tot_Crit3` int NOT NULL,
  `Average_Crit3` double NOT NULL,
  `Crit4_Clau1` int NOT NULL,
  `Crit4_Clau2` int NOT NULL,
  `Crit4_Clau3` int NOT NULL,
  `Crit4_Clau4` int NOT NULL,
  `Crit4_Clau5` int NOT NULL,
  `Tot_Crit4` int NOT NULL,
  `Average_Crit4` double NOT NULL,
  `Crit5_Clau1` int NOT NULL,
  `Crit5_Clau2` int NOT NULL,
  `Crit5_Clau3` int NOT NULL,
  `Crit5_Clau4` int NOT NULL,
  `Crit5_Clau5` int NOT NULL,
  `Tot_Crit5` int NOT NULL,
  `Average_Crit5` double NOT NULL,
  `Crit6_Clau1` int NOT NULL,
  `Crit6_Clau2` int NOT NULL,
  `Crit6_Clau3` int NOT NULL,
  `Crit6_Clau4` int NOT NULL,
  `Crit6_Clau5` int NOT NULL,
  `Tot_Crit6` int NOT NULL,
  `Average_Crit6` double NOT NULL,
  `Crit7_Clau1` int NOT NULL,
  `Crit7_Clau2` int NOT NULL,
  `Crit7_Clau3` int NOT NULL,
  `Crit7_Clau4` int NOT NULL,
  `Crit7_Clau5` int NOT NULL,
  `Tot_Crit7` int NOT NULL,
  `Average_Crit7` double NOT NULL,
  `Crit8_Clau1` int NOT NULL,
  `Tot_Crit8` int NOT NULL,
  `Average_Crit8` double NOT NULL,
  `Crit9_Clau1` int NOT NULL,
  `Crit9_Clau2` int NOT NULL,
  `Crit9_Clau3` int NOT NULL,
  `Crit9_Clau4` int NOT NULL,
  `Crit9_Clau5` int NOT NULL,
  `Tot_Crit9` int NOT NULL,
  `Average_Crit9` double NOT NULL,
  `MedicalRestriction` int NOT NULL,
  `Crit10_Justification` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ForwardingDate` datetime(6) DEFAULT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Grand_Tot` double NOT NULL,
  `Overall_Average` double NOT NULL,
  `AssessmentResult` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Assessments_CivilServantId` (`CivilServantId`),
  KEY `IX_Assessments_UserEvaluatorId1` (`UserEvaluatorId1`),
  KEY `IX_Assessments_UserEvaluatorId2` (`UserEvaluatorId2`),
  CONSTRAINT `FK_Assessments_Evaluators_UserEvaluatorId1` FOREIGN KEY (`UserEvaluatorId1`) REFERENCES `evaluators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Assessments_Evaluators_UserEvaluatorId2` FOREIGN KEY (`UserEvaluatorId2`) REFERENCES `evaluators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Assessments_Servants_CivilServantId` FOREIGN KEY (`CivilServantId`) REFERENCES `servants` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `assessments`
--

LOCK TABLES `assessments` WRITE;
/*!40000 ALTER TABLE `assessments` DISABLE KEYS */;
INSERT INTO `assessments` VALUES (1,1,1,'2024-09-07 00:00:00.000000','2024-03-19 17:34:25.690197',1,10,11,'Gianluca Vialli Ribeiro Vieira',15,15,15,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,15,15,15,10,10,10,10,10,50,10,0,NULL,NULL,'0001-01-01 00:00:00.000000','2024-03-19 17:34:25.675419','gianluca.vialli',100,11.11111111111111,1),(2,0,2,'2025-04-05 00:00:00.000000',NULL,1,10,11,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(3,0,3,'2025-11-01 00:00:00.000000',NULL,1,10,11,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(4,0,4,'2026-05-30 00:00:00.000000',NULL,1,10,11,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(5,0,5,'2026-12-26 00:00:00.000000',NULL,1,10,11,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(6,1,1,'2024-09-17 00:00:00.000000','2024-03-19 13:45:23.318424',2,10,10,'Lúcio Albuquerque Andrade',15,15,15,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,10,10,10,10,10,50,10,15,15,15,10,10,10,10,10,50,10,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,100,11.11111111111111,1),(7,0,2,'2025-04-15 00:00:00.000000',NULL,2,10,10,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(8,0,3,'2025-11-11 00:00:00.000000',NULL,2,10,10,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(9,0,4,'2026-06-09 00:00:00.000000',NULL,2,10,10,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(10,0,5,'2027-01-05 00:00:00.000000',NULL,2,10,10,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(11,0,1,'2024-10-08 00:00:00.000000',NULL,3,7,9,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(12,0,2,'2025-05-06 00:00:00.000000',NULL,3,7,9,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(13,0,3,'2025-12-02 00:00:00.000000',NULL,3,7,9,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(14,0,4,'2026-06-30 00:00:00.000000',NULL,3,7,9,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(15,0,5,'2027-01-26 00:00:00.000000',NULL,3,7,9,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(16,0,1,'2024-10-06 00:00:00.000000',NULL,4,7,7,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(17,0,2,'2025-05-04 00:00:00.000000',NULL,4,7,7,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(18,0,3,'2025-11-30 00:00:00.000000',NULL,4,7,7,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(19,0,4,'2026-06-28 00:00:00.000000',NULL,4,7,7,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(20,0,5,'2027-01-24 00:00:00.000000',NULL,4,7,7,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(21,0,1,'2023-09-09 00:00:00.000000',NULL,5,2,5,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(22,0,2,'2024-04-06 00:00:00.000000',NULL,5,2,5,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(23,0,3,'2024-11-02 00:00:00.000000',NULL,5,2,5,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(24,0,4,'2025-05-31 00:00:00.000000',NULL,5,2,5,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(25,0,5,'2025-12-27 00:00:00.000000',NULL,5,2,5,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(26,0,1,'2024-09-08 00:00:00.000000',NULL,6,2,2,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(27,0,2,'2025-04-06 00:00:00.000000',NULL,6,2,2,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(28,0,3,'2025-11-02 00:00:00.000000',NULL,6,2,2,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(29,0,4,'2026-05-31 00:00:00.000000',NULL,6,2,2,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL),(30,0,5,'2026-12-27 00:00:00.000000',NULL,6,2,2,NULL,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,NULL,'0001-01-01 00:00:00.000000',NULL,NULL,0,0,NULL);
/*!40000 ALTER TABLE `assessments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `divisions`
--

DROP TABLE IF EXISTS `divisions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `divisions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `InstituitionId` int NOT NULL,
  `Name` varchar(70) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserAdministratorId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Divisions_InstituitionId` (`InstituitionId`),
  KEY `IX_Divisions_UserAdministratorId` (`UserAdministratorId`),
  CONSTRAINT `FK_Divisions_Administrators_UserAdministratorId` FOREIGN KEY (`UserAdministratorId`) REFERENCES `administrators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Divisions_Instituitions_InstituitionId` FOREIGN KEY (`InstituitionId`) REFERENCES `instituitions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=67 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `divisions`
--

LOCK TABLES `divisions` WRITE;
/*!40000 ALTER TABLE `divisions` DISABLE KEYS */;
INSERT INTO `divisions` VALUES (1,11,'Escola Municipal Antônio Fonseca Leal','2023-09-27 13:39:48.921364',NULL,NULL,3),(2,11,'Escola Municipal Memorial Zumbi','2023-09-27 13:52:41.191752',NULL,NULL,3),(3,10,'Sítio da Criança','2023-09-27 13:55:16.883640',NULL,NULL,3),(4,7,'Centro de Assistência Psico-Social','2023-09-27 13:56:16.607620',NULL,NULL,3),(5,11,'Centro de Educação Inf. Lucilênia A. O. Sil','2023-09-27 13:57:15.912343',NULL,NULL,3),(6,11,'Creche Cantinho da Criança','2023-09-27 13:58:06.465636',NULL,NULL,3),(7,11,'Creche Lar da Criança','2023-09-27 13:58:38.047980',NULL,NULL,3),(8,11,'Creche Pequeno Polegar','2023-09-27 13:59:02.961080',NULL,NULL,3),(9,7,'Divisão Ambulatorial','2023-09-27 13:59:22.751990',NULL,NULL,3),(10,7,'Divisão Coord. do P. Saúde da Família - PSF','2023-09-27 14:00:26.799950',NULL,NULL,3),(11,11,'Divisão de Administração','2023-09-27 14:01:10.369448',NULL,NULL,3),(12,6,'Divisão de Administração financeira','2023-09-27 14:03:58.597844',NULL,NULL,3),(13,8,'Divisão de Apoio Administrativo','2023-09-27 14:04:33.725006',NULL,NULL,3),(14,4,'Divisão de Apoio Jurídico','2023-09-27 14:05:35.479189',NULL,NULL,3),(15,6,'Divisão de Cadastro Imobiliário','2023-09-27 14:06:17.198766',NULL,NULL,3),(16,14,'Divisão de Comunicação Social','2023-09-27 14:07:16.495161',NULL,NULL,3),(17,6,'Divisão de Controle Interno','2023-09-27 14:07:39.552575',NULL,NULL,3),(18,11,'Divisão de Cultura','2023-09-27 14:08:29.881150',NULL,NULL,3),(19,10,'Divisão de Desenvolvimento Comunitário','2023-09-27 14:08:58.129685',NULL,NULL,3),(20,11,'Divisão de Ensino','2023-09-27 14:10:01.754540',NULL,NULL,3),(21,5,'Divisão de Esportes e Lazer','2023-09-27 14:18:24.539964',NULL,NULL,1),(22,6,'Divisão de Execução Orçamentária','2023-09-27 14:19:44.021563',NULL,NULL,1),(23,5,'Divisão de Fomento Comercial e Industrial','2023-09-27 14:20:19.862295',NULL,NULL,1),(24,8,'Divisão de Garagens e Oficina','2023-09-27 14:21:25.901370',NULL,NULL,1),(25,14,'Divisão de Informática','2023-09-27 14:22:44.957759',NULL,NULL,1),(26,8,'Divisão de Material e Patrimônio','2023-09-27 14:23:17.630001',NULL,NULL,1),(27,8,'Divisão de Modernização Administrativa','2023-09-27 14:23:51.015744',NULL,NULL,1),(28,7,'Divisão de Odontologia','2023-09-27 15:03:50.376335',NULL,NULL,1),(29,14,'Divisão de Orçamento','2023-09-27 15:04:20.958485',NULL,NULL,1),(30,6,'Divisão de Orçamento E Contabilidade','2023-09-27 15:04:57.950331',NULL,NULL,1),(31,14,'Divisão de Planejamento','2023-09-27 15:05:21.487159',NULL,NULL,1),(32,13,'Divisão de Projetos','2023-09-27 15:05:42.775588',NULL,NULL,1),(33,10,'Divisão de Promoção Humana e Social','2023-09-27 15:06:26.796580',NULL,NULL,1),(34,10,'Divisão de Reabilitação','2023-09-27 15:06:44.655919',NULL,NULL,1),(35,8,'Divisão de Recursos Humanos','2023-09-27 15:07:29.814849',NULL,NULL,1),(36,13,'Divisão de Serviços Urbanos','2023-09-27 15:07:55.616309',NULL,NULL,1),(37,6,'Divisão de Tributação e Arrecadação','2023-09-27 15:09:00.913708',NULL,NULL,1),(38,13,'Divisão de Vias Urbanas','2023-09-27 15:09:33.121986',NULL,NULL,1),(39,7,'Divisão de Vigilância Epidemiológica','2023-09-27 15:10:41.434923',NULL,NULL,1),(40,7,'Divisão de Vigilância Sanitária','2023-09-27 15:11:05.689127',NULL,NULL,1),(41,11,'Escola Municipal Geralda Márcia Pereira Gonçalves','2023-09-27 15:13:19.566111',NULL,NULL,2),(42,11,'Escola Municipal Professor Johnsen','2023-09-27 15:14:01.365257',NULL,NULL,2),(43,11,'Escola Municipal Rosa Pedroso de Almeida','2023-09-27 15:14:22.733663',NULL,NULL,2),(44,11,'Escola Municipal Umes/Telecurso','2023-09-27 15:14:53.181630',NULL,'11',2),(45,11,'Escola Municipal Carlindo Nascimento Gaia','2023-09-27 15:15:24.181736',NULL,NULL,2),(46,11,'Escola Municipal Clarinda Firmina A. Santos','2023-09-27 15:15:45.597039',NULL,NULL,2),(47,11,'Escola Municipal Irene Castelo Branco','2023-09-27 15:17:11.275038',NULL,NULL,2),(48,11,'Escola Municipal Policena Alves de Amorim','2023-09-27 15:17:40.135669',NULL,NULL,2),(49,11,'Escola Municipal Pref. Joaquim Cândido Gonçalves','2023-09-27 15:18:00.182792',NULL,NULL,2),(50,11,'Escolas Municipais Rurais','2023-09-27 15:18:25.871588',NULL,NULL,2),(51,2,'Gabinete do Prefeito','2023-09-27 15:18:50.023491',NULL,NULL,2),(52,3,'Instituto de Previdência Municipal de Três Marias','2023-09-27 15:19:09.718930',NULL,NULL,2),(53,11,'Núcleo Pedagógico do Ensino Supletivo','2023-09-27 15:19:57.446946',NULL,NULL,2),(54,11,'Secretaria Municipal de Educação e Cultura','2023-09-27 15:20:26.288586',NULL,NULL,2),(55,5,'Secretaria Municipal de Des., Esportes e Turismo','2023-09-27 15:20:59.992601',NULL,NULL,2),(56,6,'Secretaria Municipal da Fazenda','2023-09-27 15:23:16.283027',NULL,NULL,2),(57,8,'Secretaria Municipal de Administração','2023-09-27 15:23:46.393650',NULL,NULL,2),(58,9,'Secretaria Municipal de Agricultura','2023-09-27 15:24:06.064707',NULL,NULL,2),(59,3,'Superintendência Administrativa','2023-09-27 15:24:55.521047',NULL,NULL,2),(60,7,'Serviço de Fisioterapia','2023-09-27 15:25:13.761825',NULL,NULL,2),(61,10,'Secretaria Municipal de Assist. e Promoção Social','2023-09-27 15:25:51.897780',NULL,NULL,2),(62,12,'Secretaria Municipal de Meio Ambiente','2023-09-27 15:26:29.522880',NULL,NULL,2),(63,13,'Secretaria Municipal de Obras e Serviços Urbanos','2023-09-27 15:26:54.170229',NULL,NULL,2),(64,14,'Secretaria Municipal de Planejamento','2023-09-27 15:27:29.241515',NULL,NULL,2),(65,15,'Sub-Prefeitura de Andrequicé','2023-09-27 15:27:47.945982',NULL,NULL,2),(66,8,'Secretaria Municipal de Saúde','2023-09-29 13:53:00.025948',NULL,NULL,3);
/*!40000 ALTER TABLE `divisions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evaluators`
--

DROP TABLE IF EXISTS `evaluators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `evaluators` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `InstituitionId` int NOT NULL,
  `DivisionId` int NOT NULL,
  `SectionId` int NOT NULL,
  `SectorId` int NOT NULL,
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
  UNIQUE KEY `IX_Evaluators_Email` (`Email`),
  UNIQUE KEY `IX_Evaluators_Login` (`Login`),
  UNIQUE KEY `IX_Evaluators_Masp` (`Masp`),
  UNIQUE KEY `IX_Evaluators_Name` (`Name`),
  UNIQUE KEY `IX_Evaluators_Password` (`Password`),
  UNIQUE KEY `IX_Evaluators_Phone` (`Phone`),
  KEY `IX_Evaluators_DivisionId` (`DivisionId`),
  KEY `IX_Evaluators_InstituitionId` (`InstituitionId`),
  KEY `IX_Evaluators_SectionId` (`SectionId`),
  KEY `IX_Evaluators_SectorId` (`SectorId`),
  CONSTRAINT `FK_Evaluators_Divisions_DivisionId` FOREIGN KEY (`DivisionId`) REFERENCES `divisions` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluators_Instituitions_InstituitionId` FOREIGN KEY (`InstituitionId`) REFERENCES `instituitions` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluators_Sections_SectionId` FOREIGN KEY (`SectionId`) REFERENCES `sections` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Evaluators_Sectors_SectorId` FOREIGN KEY (`SectorId`) REFERENCES `sectors` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evaluators`
--

LOCK TABLES `evaluators` WRITE;
/*!40000 ALTER TABLE `evaluators` DISABLE KEYS */;
INSERT INTO `evaluators` VALUES (1,1,1,1,1,804,'Fenanda Montenegro de Carvalho','fernanda.carvalho','$2a$11$qVvbtUMNGYqoBS/byb1nbuzzPWIAcogAg6r7wLtVxDehD0ejoX3n6','fernanda@gmail.com','(31) 98741-2563','Técnico em Informática',2,0,'2023-11-14 22:51:56.721105',NULL,NULL),(2,11,6,7,5,21704,'Marcilene Correa Freitas','marcilene.correa','$2a$11$VeHFzkjKoRTrZrrnN6l0N.4KWA/UxoKAlzBMJaSihaXS89pR8EbHK','marcilene@gmail.com','(31) 98472-5546','Professora',2,1,'2023-12-12 12:44:58.549924','2024-01-08 01:01:02.895478','marcilene correa'),(3,12,62,72,66,10478,'Pablo Coelho Ferreira','pablo.coelho','$2a$11$dMAa6udQo50HybYeW.EkuuQ.XugU1rskvVEUXg9ivRDStIn71uy8.','pablo@gmail.com','(11) 98741-2533','Auxiliar de Serviços Gerais',2,1,'2023-12-12 12:46:58.609507','2023-12-29 19:24:45.739102','anna.caroline'),(4,7,9,8,8,21493,'Marcelo Ramos Garcia','marcelo.garcia','$2a$11$YBNawk7dyRI5LrF7GsE6c.AMQb0b14jvgOLrAvdfIBx6oJEaGxAOy','marcelo@gmail.com','(32) 95896-3214','Enfermeiro',2,1,'2023-12-12 12:48:34.654862',NULL,NULL),(5,11,54,82,76,840,'Lisandra Eduarda Faria','lisandra.faria','$2a$11$pArOkLjjlwlc3hIFNFPbY.aITQwm5/x1VbCB2mOolsUe/3xJZMUC2','lisandra@gmail.com','(34) 97412-5840','Secretaria de Educação',2,1,'2023-12-12 12:49:49.819340',NULL,NULL),(6,7,28,26,23,795,'Patrícia Faria Lima','patricia.lima','$2a$11$cp.PWUwDNlLKzSiR2RH3cuaYku5C/Z4Iusm47.BTqYZbUH/665k4q','patricia@outlook.com','(38) 97482-5655','Cirurgiã Dentista',2,1,'2023-12-28 22:58:42.915748',NULL,NULL),(7,10,61,81,75,17184,'Marcely Pereira de Fátima','marcely.pereira','$2a$11$4F6cpRE44IOXJUavYDdKF.ytoH0N.NJsHkVsditYxAXLeVWcW3dhi','marcely@yahoo.com','(34) 95416-2855','Assitente Social',2,1,'2023-12-28 23:00:40.029284',NULL,NULL),(9,10,3,91,95,1086,'Mirian Garcia Coutinho','mirian.garcia','$2a$11$OED4h3fjyEOwiy0.2m2sNuT3M2g8sCri5XtXaDgp9PE3z9IXzqaBy','mirian@yahoo.com','(21) 97423-3659','Chefe do Sítio da Criança',2,1,'2023-12-29 00:18:05.842746','2023-12-29 19:10:46.242538','mirian.garcia'),(10,14,25,23,21,1042,'Lúcio Albuquerque Andrade','lucio.albuquerque','$2a$11$jfrtc8eQqvRnGfJd2e2wTOI9e21d0EXUt3zZJAnlTv6PaLVqN8saS','lucio123@hotmail.com','(38) 99125-6306','Técnico em Informática',2,1,'2024-03-19 12:56:52.681279',NULL,NULL),(11,14,25,23,21,28975,'Marilia Garcia Freitas','marilia.freitas','$2a$11$Ly7JeVC5TCRncgzr7pmxWO8s86tsmUdCH0FNEhwiptQ3/fiiuuGEK','\'marilia123@yahoo.com','(38) 94102-5656','Assistente Técnico',2,1,'2024-03-19 13:22:13.767076',NULL,NULL);
/*!40000 ALTER TABLE `evaluators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instituitions`
--

DROP TABLE IF EXISTS `instituitions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `instituitions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(70) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserAdministratorId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Instituitions_UserAdministratorId` (`UserAdministratorId`),
  CONSTRAINT `FK_Instituitions_Administrators_UserAdministratorId` FOREIGN KEY (`UserAdministratorId`) REFERENCES `administrators` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instituitions`
--

LOCK TABLES `instituitions` WRITE;
/*!40000 ALTER TABLE `instituitions` DISABLE KEYS */;
INSERT INTO `instituitions` VALUES (1,'Fundação Municipal São Francisco','2023-11-20 17:05:45.064894',NULL,NULL,1),(2,'Gabinete do Prefeito','2023-11-20 17:11:30.367020',NULL,NULL,1),(3,'Instituto de Previdência Municipal de Três Marias','2023-11-20 17:11:41.912027',NULL,NULL,1),(4,'Procuradoria Geral do Município','2023-11-20 17:11:57.389073',NULL,NULL,1),(5,'Secretaria Municipal de Des., Esportes e Turismo','2023-11-20 17:12:22.990537',NULL,NULL,1),(6,'Secretaria Municipal da Fazenda','2023-11-20 17:12:32.790429',NULL,NULL,1),(7,'Secretaria Municipal da Saúde','2023-11-20 17:13:08.038478',NULL,NULL,1),(8,'Secretaria Municipal de Administração','2023-11-20 17:13:20.550034',NULL,NULL,1),(9,'Secretaria Municipal de Agricultura','2023-11-20 17:13:32.775860',NULL,NULL,1),(10,'Secretaria Municipal de Assist. e Promoção Social','2023-11-20 17:13:40.133362',NULL,NULL,1),(11,'Secretaria Municipal de Educação','2023-11-20 17:14:06.416127',NULL,NULL,1),(12,'Secretaria Municipal de Meio Ambiente','2023-11-20 17:14:16.426005',NULL,NULL,1),(13,'Secretaria Municipal de Obras e Serviços Urbanos','2023-11-20 17:14:29.967294',NULL,NULL,1),(14,'Secretaria Municipal de Planejamento','2023-11-20 17:14:58.447364',NULL,NULL,1),(15,'Sub-Prefeitura de Andrequicé','2023-11-20 17:15:06.456787',NULL,NULL,1);
/*!40000 ALTER TABLE `instituitions` ENABLE KEYS */;
UNLOCK TABLES;

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

--
-- Table structure for table `sections`
--

DROP TABLE IF EXISTS `sections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sections` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DivisionId` int NOT NULL,
  `Name` varchar(70) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserAdministratorId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Sections_DivisionId` (`DivisionId`),
  KEY `IX_Sections_UserAdministratorId` (`UserAdministratorId`),
  CONSTRAINT `FK_Sections_Administrators_UserAdministratorId` FOREIGN KEY (`UserAdministratorId`) REFERENCES `administrators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Sections_Divisions_DivisionId` FOREIGN KEY (`DivisionId`) REFERENCES `divisions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=93 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sections`
--

LOCK TABLES `sections` WRITE;
/*!40000 ALTER TABLE `sections` DISABLE KEYS */;
INSERT INTO `sections` VALUES (1,4,'Centro de Assistência Psico Social','2023-09-29 12:28:02.500711',NULL,NULL,1),(2,5,'Centro de Educação Inf. Luculênia A. O. Sil','2023-09-29 12:29:17.818665',NULL,NULL,1),(3,21,'Centro Público de Prom. Trabalho','2023-09-29 12:29:54.155940',NULL,NULL,1),(4,37,'Coordenadoria do SIAT','2023-09-29 12:30:26.683266',NULL,NULL,1),(5,7,'Creche Lar da Criança','2023-09-29 12:30:56.010718',NULL,NULL,1),(6,8,'Creche Pequeno Polegar','2023-09-29 12:50:31.263164',NULL,NULL,1),(7,6,'Creche Cantinho da Criança','2023-09-29 12:51:23.352579',NULL,NULL,1),(8,9,'Divisão Ambulatorial','2023-09-29 12:51:49.890135',NULL,NULL,1),(9,10,'Divisão Coord. do P.Saúde Da Familia - PSF','2023-09-29 12:52:58.515628',NULL,NULL,1),(10,11,'Divisão de Administração','2023-09-29 12:53:25.275656',NULL,NULL,1),(11,12,'Divisão de Administração Financeira','2023-09-29 12:54:42.992967',NULL,NULL,1),(12,13,'Divisão de Apoio Administrativo','2023-09-29 12:55:15.475195',NULL,NULL,1),(13,14,'Divisão de Apoio Jurídico','2023-09-29 12:55:38.970093',NULL,NULL,1),(14,15,'Divisão de Cadastro Imobiliário','2023-09-29 12:56:00.028518',NULL,NULL,1),(15,16,'Divisão de Comunicação Social','2023-09-29 12:56:22.458815',NULL,NULL,1),(16,17,'Divisão de Controle Interno','2023-09-29 12:56:56.611264',NULL,NULL,1),(17,18,'Divisão de Cultura','2023-09-29 12:57:18.267964',NULL,NULL,1),(18,19,'Divisão de Desenvolvimento Comunitário','2023-09-29 12:57:40.331803',NULL,NULL,1),(19,20,'Divisão de Ensino','2023-09-29 12:58:25.440049',NULL,NULL,1),(20,21,'Divisão de Esportes e Lazer','2023-09-29 12:59:04.299305',NULL,NULL,1),(21,22,'Divisão de Execução Orçamentária','2023-09-29 12:59:27.659227',NULL,NULL,1),(22,24,'Divisão de Garagens e Oficina','2023-09-29 12:59:53.717086',NULL,NULL,1),(23,25,'Divisão de Informática','2023-09-29 13:00:25.305998',NULL,NULL,1),(24,26,'Divisão de Material e Patrimônio','2023-09-29 13:00:51.212589',NULL,NULL,1),(25,27,'Divisão de Modernização Administrativa','2023-09-29 13:01:22.651781',NULL,NULL,1),(26,28,'Divisão de Odontologia','2023-09-29 13:01:45.914288',NULL,NULL,1),(27,29,'Divisão de Orçamento','2023-09-29 13:02:05.125851',NULL,NULL,1),(28,31,'Divisão de Planejamento','2023-09-29 13:02:28.819454',NULL,NULL,1),(29,32,'Divisão de Projetos','2023-09-29 13:02:41.068601',NULL,NULL,1),(30,33,'Divisão de Promoção Humana e Social','2023-09-29 13:03:02.853669',NULL,NULL,1),(31,34,'Divisão de Reabilitação','2023-09-29 13:03:22.044506',NULL,NULL,1),(32,35,'Divisão de Recursos Humanos','2023-09-29 13:03:41.037760',NULL,NULL,1),(33,36,'Divisão de Serviços Urbanos','2023-09-29 13:04:00.155388',NULL,NULL,1),(34,37,'Divisão de Tributação e Arrecadação','2023-09-29 13:04:35.148005',NULL,NULL,1),(35,38,'Divisão de Vias Urbanas','2023-09-29 13:04:53.427467',NULL,NULL,1),(36,39,'Divisão de Vigilância Epidemiológica','2023-09-29 13:05:30.823122',NULL,NULL,1),(37,40,'Divisão de Vigilância Sanitária','2023-09-29 13:05:55.591793',NULL,NULL,1),(38,1,'Escola Municipal Antônio Fonseca Leal','2023-09-29 13:06:17.414484',NULL,NULL,1),(39,45,'Escola Municipal Carlindo Nascimento Gaia','2023-09-29 13:08:08.210192',NULL,NULL,2),(40,46,'Escola Municipal Clarinda Firmina A. Santos','2023-09-29 13:09:08.226715',NULL,NULL,2),(41,41,'Escola Municipal Geralda Márica Pereira Ginçalves','2023-09-29 13:09:38.741186',NULL,NULL,2),(42,47,'Escola Municipal Irene Castelo Branco','2023-09-29 13:10:03.091967',NULL,NULL,2),(43,2,'Escola Municipal Memorial Zumbi','2023-09-29 13:10:47.485390',NULL,NULL,2),(44,48,'Escola Municipal Policena Alves de Amorim','2023-09-29 13:11:19.948919',NULL,NULL,2),(45,49,'Escola Municipal Pref. Joaquim Cândido Gonçalves','2023-09-29 13:11:49.659003',NULL,NULL,2),(46,42,'Escola Municipal Professor Johnsen','2023-09-29 13:12:12.006054',NULL,NULL,2),(47,43,'Escola Municipal Rosa Pedroso de Almeida','2023-09-29 13:12:52.074701',NULL,NULL,2),(48,44,'Escola Municipal Umes/Telecurso','2023-09-29 13:13:16.118953',NULL,NULL,2),(49,51,'Gabinete do Prefeito','2023-09-29 13:13:30.197779',NULL,NULL,2),(50,33,'Horta Comunitária','2023-09-29 13:14:09.877549',NULL,NULL,2),(51,52,'Instituto de Previdência Municipal de Três Marias','2023-09-29 13:14:45.469514',NULL,NULL,2),(52,33,'Núcleo de Apoio a Família 1','2023-09-29 13:15:14.374076',NULL,NULL,2),(53,33,'Núcleo de Apoio a Família 2','2023-09-29 13:15:39.413814',NULL,NULL,2),(54,33,'Núcleo de Apoio a Família 3','2023-09-29 13:15:58.077862',NULL,NULL,2),(55,33,'Núcleo de Apoio a Família 4','2023-09-29 13:16:14.086886',NULL,NULL,2),(56,36,'Núcleo de Fiscalização','2023-09-29 13:16:32.655751',NULL,NULL,2),(57,20,'Núcleo Pedagógico de Ensino Supletivo e Alf. J. Adult','2023-09-29 13:17:23.040314',NULL,NULL,2),(58,53,'Núcleo Pedagógico do Ensino Supletivo','2023-09-29 13:17:48.663153',NULL,NULL,2),(59,58,'Sessão de Agropecuária e Comercialização','2023-09-29 13:24:10.810136',NULL,NULL,2),(60,9,'Seção de Almoxarifado','2023-09-29 13:24:43.678868',NULL,NULL,2),(61,36,'Seção de Carpintaria e Estoques','2023-09-29 13:25:22.998168',NULL,NULL,2),(62,62,'Seção de Controle Ambiental','2023-09-29 13:26:03.882254',NULL,NULL,2),(63,58,'Seção de Controle e Fito Sanitário','2023-09-29 13:26:39.277917',NULL,NULL,2),(64,23,'Seção de Desenvolvimento, Emprego e Renda','2023-09-29 13:27:24.398470',NULL,NULL,2),(65,58,'Seção de Estradas Vic. E Mecanização Agrícola','2023-09-29 13:28:03.701161',NULL,NULL,2),(66,21,'Sessão de Lazer','2023-09-29 13:28:36.571932',NULL,NULL,2),(67,26,'Sessão de Licitação','2023-09-29 13:29:06.461877',NULL,NULL,2),(68,36,'Sessão de Limpeza Urbana','2023-09-29 13:29:44.175776',NULL,NULL,2),(69,36,'Seção de Manutenção das Torres de Ret de Sinal de Tv','2023-09-29 13:39:25.786712',NULL,NULL,2),(70,24,'Seção de Manauteção de Mecânica de Autos','2023-09-29 13:39:59.145529',NULL,NULL,2),(71,26,'Seção de Patrimônio','2023-09-29 13:40:20.906715',NULL,NULL,2),(72,62,'Seção de Praças e Jardins','2023-09-29 13:42:52.579024',NULL,NULL,3),(73,36,'Seção de Reformas e Manutenção','2023-09-29 13:43:23.444983',NULL,NULL,3),(74,36,'Sessão de Serviços Públicos','2023-09-29 13:43:44.300641',NULL,NULL,3),(75,26,'Sessão de Vigilância Patrimonial','2023-09-29 13:44:19.446612',NULL,NULL,3),(76,23,'Seção de Desenvolvimento da Pequena Empresa','2023-09-29 13:44:56.495959',NULL,NULL,3),(77,55,'Secretaria Municipal de Des., Esportes e Turismo','2023-09-29 13:46:01.332223',NULL,NULL,3),(78,56,'Secretaria Municipal da Fazenda','2023-09-29 13:46:57.198221',NULL,NULL,3),(79,57,'Secretaria Municipal de Administração','2023-09-29 13:47:17.671347',NULL,NULL,3),(80,58,'Secretaria Municipal de Agricultura','2023-09-29 13:47:46.918988',NULL,NULL,3),(81,61,'Secretaria Municipal de Assist. e Promoção Social','2023-09-29 13:48:25.209056',NULL,NULL,3),(82,54,'Secretaria Municipal de Educação e Cultura','2023-09-29 13:48:50.351967',NULL,NULL,3),(83,62,'Secretaria Municipal de Meio Ambiente','2023-09-29 13:49:23.855763',NULL,NULL,3),(84,63,'Secretaria Municipal de Obras e Serviços Urbanos','2023-09-29 13:50:04.233786',NULL,NULL,3),(85,64,'Secretaria Municipal de Planejamento','2023-09-29 13:50:31.506225',NULL,NULL,3),(86,66,'Secretaria Municipal de Saúde','2023-09-29 13:53:37.915049',NULL,NULL,3),(87,60,'Serviço de Fisioterapia','2023-09-29 13:53:58.111821',NULL,NULL,3),(88,36,'Setor de Limpeza Urbana 1','2023-09-29 13:54:36.625983',NULL,NULL,3),(89,36,'Setor de Limpeza Urbana 2','2023-09-29 13:54:50.386004',NULL,NULL,3),(90,36,'Setor de Limpeza Urbana 3','2023-09-29 13:55:07.234799',NULL,NULL,3),(91,3,'Sítio da Criança e do Adolescente','2023-09-29 13:55:35.299894',NULL,NULL,3),(92,65,'Sub-Prefeitura de Andrequicé','2023-09-29 13:55:55.498691',NULL,NULL,3);
/*!40000 ALTER TABLE `sections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sectors`
--

DROP TABLE IF EXISTS `sectors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sectors` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SectionId` int NOT NULL,
  `Name` varchar(70) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserAdministratorId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Sectors_SectionId` (`SectionId`),
  KEY `IX_Sectors_UserAdministratorId` (`UserAdministratorId`),
  CONSTRAINT `FK_Sectors_Administrators_UserAdministratorId` FOREIGN KEY (`UserAdministratorId`) REFERENCES `administrators` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Sectors_Sections_SectionId` FOREIGN KEY (`SectionId`) REFERENCES `sections` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sectors`
--

LOCK TABLES `sectors` WRITE;
/*!40000 ALTER TABLE `sectors` DISABLE KEYS */;
INSERT INTO `sectors` VALUES (1,44,'Escola Municipal Policena Alves de Amorim','2023-10-02 09:54:06.667739',NULL,NULL,1),(2,1,'Centro de Assistência Psico-Social','2023-10-02 09:54:32.571313',NULL,NULL,1),(3,2,'Centro de Educação Inf. Lucilênia A. O. Sil','2023-10-02 09:55:09.947182',NULL,NULL,1),(4,4,'Coordenadoria do SIAT','2023-10-02 09:55:32.543196',NULL,NULL,1),(5,7,'Creche Cantinho da Criança','2023-10-02 09:56:13.123118',NULL,NULL,1),(6,5,'Creche Lar da Criança','2023-10-02 09:56:36.345509',NULL,NULL,1),(7,6,'Creche Pequeno Polegar','2023-10-02 09:57:36.236552',NULL,NULL,1),(8,8,'Divisão Ambulatorial','2023-10-02 09:58:17.056375',NULL,NULL,1),(9,9,'Divisão Coord. do P.Saúde da Família - PSF','2023-10-02 09:59:13.036717',NULL,NULL,1),(10,10,'Divisão de Administração','2023-10-02 09:59:31.979794',NULL,NULL,1),(11,11,'Divisão de Administração Financeira','2023-10-02 10:00:31.666332',NULL,NULL,1),(12,13,'Divisão de Apoio Jurídico','2023-10-02 10:01:36.558261',NULL,NULL,1),(13,14,'Divisão de Cadastro Imobiliário','2023-10-02 10:02:08.768270',NULL,NULL,1),(14,16,'Divisão de Controle Interno','2023-10-02 10:03:28.117909',NULL,NULL,1),(15,17,'Divisão de Cultura','2023-10-02 10:03:46.413967',NULL,NULL,1),(16,18,'Divisão de Desenvolvimento Comunitário','2023-10-02 10:04:38.036447',NULL,NULL,1),(17,19,'Divisão de Ensino','2023-10-02 10:05:17.788683',NULL,NULL,1),(18,20,'Divisão de Esportes e Lazer','2023-10-02 10:05:38.853765',NULL,NULL,1),(19,21,'Divisão de Execução Orçamentária','2023-10-02 10:07:32.354614',NULL,NULL,1),(20,22,'Divisão de Garagens e Oficina','2023-10-02 10:07:54.399445',NULL,NULL,1),(21,23,'Divisão de Informática','2023-10-02 10:09:09.735688',NULL,NULL,1),(22,25,'Divisão de Modernização Administrativa','2023-10-02 10:09:35.761336',NULL,NULL,1),(23,26,'Divisão de Odontologia','2023-10-02 10:10:23.952875',NULL,NULL,1),(24,28,'Divisão de Planejamento','2023-10-02 10:11:03.328265',NULL,NULL,1),(25,27,'Divisão de Orçamento','2023-10-02 10:11:36.239351',NULL,NULL,1),(26,29,'Divisão de Projetos','2023-10-02 10:11:58.616641',NULL,NULL,1),(27,30,'Divisão de Promoção Humana Social','2023-10-02 10:12:33.480718',NULL,NULL,1),(28,31,'Divisão de Reabilitação','2023-10-02 10:13:14.681466',NULL,NULL,1),(29,32,'Divisão de Recursos Humanos','2023-10-02 10:13:58.265111',NULL,NULL,1),(30,33,'Divisão de Serviços Urbanos','2023-10-02 10:14:41.135417',NULL,NULL,1),(31,34,'Divisão de Tributação e Arrecadação','2023-10-02 10:15:03.528881',NULL,NULL,1),(32,35,'Divisão de Vias Urbanas','2023-10-02 10:15:20.095974',NULL,NULL,1),(33,36,'Divisão de Vigilância Epidemiológica','2023-10-02 10:15:51.067447',NULL,NULL,1),(34,37,'Divisão de Vigilância Sanitária','2023-10-02 10:16:13.755785',NULL,NULL,1),(35,38,'Escola Municipal Antônio Fonseca Leal','2023-10-02 10:16:28.130206',NULL,NULL,1),(36,39,'Escola Municipal Carlindo Nascimento Gaia','2023-10-02 10:16:57.815328',NULL,NULL,1),(37,40,'Escola Municipal Clarinda Firmina de A. Santos','2023-10-02 10:17:33.382686',NULL,NULL,1),(38,41,'Escola Municipal Geralda Márcia Pereira Gonçalves','2023-10-02 10:20:50.804449',NULL,NULL,1),(39,42,'Escola Municipal Irene Castelo Branco','2023-10-02 18:58:31.702795',NULL,NULL,2),(40,43,'Escola Municipal Memorial Zumbi','2023-10-02 18:58:55.465788',NULL,NULL,2),(41,44,'Escola Municipal Policena Alves de Amorim','2023-10-02 19:00:16.716993',NULL,NULL,2),(42,45,'Escola Municipal Pref. Joaquim Cândido Gonçalves','2023-10-02 19:00:47.138205',NULL,NULL,2),(43,46,'Escola Municipal Professor Johnsen','2023-10-02 19:01:48.205940',NULL,NULL,2),(44,47,'Escola Municipal Rosa Pedroso de Almeida','2023-10-02 19:02:22.130390',NULL,NULL,2),(45,48,'Escola Municipal Umes/Telecurso','2023-10-02 19:02:44.409677',NULL,NULL,2),(46,49,'Gabinete do Prefeito','2023-10-02 19:03:03.161758',NULL,NULL,2),(47,50,'Horta Comunitária','2023-10-02 19:03:25.844588',NULL,NULL,2),(48,51,'Instituto de Previdência Municipal de Três Marias','2023-10-02 19:03:50.353460',NULL,NULL,2),(49,52,'Núcleo de Apoio a Família 1','2023-10-02 19:04:19.195129',NULL,NULL,2),(50,53,'Núcleo de Apoio a Família 2','2023-10-02 19:04:32.954606',NULL,NULL,2),(51,54,'Núcleo de Apoio a Família 3','2023-10-02 19:05:14.459695',NULL,NULL,2),(52,55,'Núcleo de Apoio a Família 4','2023-10-02 19:05:25.130619',NULL,NULL,2),(53,24,'Núcleo de Compras','2023-10-02 19:13:24.082779',NULL,NULL,2),(54,56,'Núcleo de Fiscalização','2023-10-02 19:13:55.645760',NULL,NULL,2),(55,10,'Núcleo de Merenda Escolar','2023-10-02 19:14:35.307718',NULL,NULL,2),(56,58,'Núcleo Pedagógico do Ensino Supletivo','2023-10-02 19:15:09.719313',NULL,NULL,2),(57,59,'Seção de Agropecuária e Comercialização','2023-10-02 19:15:36.601639',NULL,NULL,2),(58,61,'Seção de Carpintaria e Estoques','2023-10-02 19:16:01.679365',NULL,NULL,2),(59,62,'Seção de Controle Ambiental','2023-10-02 19:17:25.431057',NULL,NULL,2),(60,63,'Seção de Controle Fito Sanitário','2023-10-02 19:48:02.600434',NULL,NULL,2),(61,65,'Seção de Estradas Vic. e Mecanização Agrícola','2023-10-02 19:48:30.057875',NULL,NULL,2),(62,66,'Seção de Lazer','2023-10-02 19:48:40.666072',NULL,NULL,2),(63,68,'Seção de Limpeza Urbana','2023-10-02 19:48:59.163714',NULL,NULL,2),(64,70,'Seção de Manutenção de Mecânica de Autos','2023-10-02 19:50:10.196626',NULL,NULL,2),(65,71,'Seção de Patrimônio','2023-10-02 19:51:00.086214',NULL,NULL,2),(66,72,'Seção de Praças e Jardins','2023-10-02 19:51:49.820429',NULL,NULL,2),(67,73,'Seção de Reformas e Manutenção','2023-10-02 19:53:22.420460',NULL,NULL,2),(68,74,'Seção de Serviços Públicos','2023-10-02 19:53:59.174499',NULL,NULL,2),(69,75,'Seção de Vigilância Patrimonial','2023-10-02 19:54:29.369828',NULL,NULL,2),(70,86,'Secretaria Municipal de Saúde','2023-10-02 19:55:03.142213',NULL,NULL,2),(71,77,'Secretaria Municipal de Des., Esportes e Turismo','2023-10-02 19:55:38.874934',NULL,NULL,2),(72,78,'Secretaria Municipal da Fazenda','2023-10-02 19:56:04.918432',NULL,NULL,2),(73,79,'Secretaria Municipal de Administração','2023-10-02 19:56:29.594805',NULL,NULL,2),(74,80,'Secretaria Municipal de Agricultura','2023-10-02 19:56:55.948673',NULL,NULL,2),(75,81,'Secretaria Municipal de Assist. e Promoção Social','2023-10-02 20:00:20.536098',NULL,NULL,3),(76,82,'Secretaria Municipal de Educação e Cultura','2023-10-02 20:04:23.272865',NULL,NULL,3),(77,83,'Secretaria Municipal de Meio Ambiente','2023-10-02 20:14:42.569589',NULL,NULL,3),(78,84,'Secretaria Municipal de Obras e Serviços Urbanos','2023-10-02 20:15:10.510797',NULL,NULL,3),(79,85,'Secretaria Municipal de Planejamento','2023-10-02 20:15:45.674271',NULL,NULL,3),(80,87,'Serviço de Fisioterpia','2023-10-02 20:17:14.407219',NULL,NULL,3),(81,19,'Setor de Formação Musical','2023-10-02 20:17:52.124383',NULL,NULL,3),(82,74,'Setor de Limpeza Urbana 1','2023-10-02 20:18:16.651265',NULL,NULL,3),(83,74,'Setor de Limpeza Urbana 1','2023-10-02 20:18:16.665616',NULL,NULL,3),(84,89,'Setor de Limpeza Urbana 2','2023-10-02 20:18:55.698952',NULL,NULL,3),(85,74,'Setor de Limpeza Urbana 2','2023-10-02 20:19:23.803194',NULL,NULL,3),(86,90,'Setor de Limpeza Urbana 3','2023-10-02 20:19:43.657879',NULL,NULL,3),(87,74,'Setor de Limpeza Urbana 3','2023-10-02 20:20:01.642605',NULL,NULL,3),(88,88,'Setor de Limpeza Urbana 1','2023-10-02 20:20:24.019388',NULL,NULL,3),(89,74,'Setor de Manutenção Elétrica','2023-10-02 20:21:22.388837',NULL,NULL,3),(90,74,'Setor de Manutenção Hidráulica','2023-10-02 20:21:55.255547',NULL,NULL,3),(91,19,'Setor de Produção Artística','2023-10-02 20:23:17.551224',NULL,NULL,3),(92,17,'Setor de Produção Artística','2023-10-02 20:24:20.527998',NULL,NULL,3),(93,66,'Setor de Promoção de Eventos','2023-10-02 20:24:58.765638',NULL,NULL,3),(94,74,'Setor de Reformas e Manutenção de Prédios','2023-10-02 20:25:35.449736',NULL,NULL,3),(95,91,'Sítio da Criança e do Adolescente','2023-10-02 20:26:11.637592',NULL,NULL,3),(96,92,'Sub-Prefeitura de Andrequicé','2023-10-02 20:26:28.358315',NULL,NULL,3);
/*!40000 ALTER TABLE `sectors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `servantlicense`
--

DROP TABLE IF EXISTS `servantlicense`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `servantlicense` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CivilServantId` int NOT NULL,
  `LicensesId` int DEFAULT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) NOT NULL,
  `RegisterDate` datetime(6) NOT NULL,
  `ModifyDate` datetime(6) DEFAULT NULL,
  `LastModifiedBy` varchar(35) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ServantLicense_CivilServantId` (`CivilServantId`),
  KEY `IX_ServantLicense_LicensesId` (`LicensesId`),
  CONSTRAINT `FK_ServantLicense_Licenses_LicensesId` FOREIGN KEY (`LicensesId`) REFERENCES `licenses` (`Id`),
  CONSTRAINT `FK_ServantLicense_Servants_CivilServantId` FOREIGN KEY (`CivilServantId`) REFERENCES `servants` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servantlicense`
--

LOCK TABLES `servantlicense` WRITE;
/*!40000 ALTER TABLE `servantlicense` DISABLE KEYS */;
INSERT INTO `servantlicense` VALUES (5,1,1,'2024-03-19 00:00:00.000000','2024-04-19 00:00:00.000000','2024-03-19 17:36:46.399789',NULL,NULL);
/*!40000 ALTER TABLE `servantlicense` ENABLE KEYS */;
UNLOCK TABLES;

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

-- Dump completed on 2024-03-19 17:55:28
