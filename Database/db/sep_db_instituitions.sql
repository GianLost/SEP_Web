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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-19 17:57:00
