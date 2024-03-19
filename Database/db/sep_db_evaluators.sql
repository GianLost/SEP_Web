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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-19 17:57:00
