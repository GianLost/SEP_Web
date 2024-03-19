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
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `administrators`
--

LOCK TABLES `administrators` WRITE;
/*!40000 ALTER TABLE `administrators` DISABLE KEYS */;
INSERT INTO `administrators` VALUES (1,14205,'Gianluca Vialli Ribeiro Vieira','gianluca.vialli','$2a$11$xuKD7jOEbn78QYKqIio4DuyzAOJid2MuO9x1Wt5DoExKjEDRqGa7C','gianluca19993m@gmail.com','(38) 98808-7655','Técnico em Informática',1,1,'2023-09-24 16:12:35.870940',NULL,NULL),(2,28405,'Anna Caroline Ribeiro Vieira','anna.caroline','$2a$11$QBnq1.zqjNBe3t.BcKJA1erGrtIxnUnNB0d6aydmSUJwLJRvUswV6','annacaroline@gmail.com','(38) 98475-6542','Auxiliar de Serviços Gerais',1,1,'2023-11-20 15:06:38.335022',NULL,NULL),(3,28407,'Amilton Vieira de Souza','amilton.vieira','$2a$11$7jofOhICcRI.HPRBrrdv7e5vTcxcjyOWtvQq/PkXkNFWCT61U5v/e','amiltonvieira@gmail.com','(38) 98475-1325','Servente Escolar',1,0,'2023-11-20 15:07:14.821344','2023-11-20 15:17:08.734814','gianluca.vialli'),(67,12345,'João da Silva','joao.silva','$2a$11$AtnUmbHNPlUITDQdRPmSYONd3zCcVi3PtT.ImtsMlBHihWhWDRl/S','joaosilva@gmail.com','(38) 98888-8888','Analista de Sistemas',1,1,'2024-03-19 12:40:39.826395',NULL,NULL),(68,54321,'Maria Oliveira','maria.oliveira','$2a$11$nfGim7JyW.0pQK/I2haALOgA/ZCnv/NmuNzgPQG/0GP./oxg0Fllm','mariaoliveira@gmail.com','(38) 99999-9999','Desenvolvedora Web',1,1,'2024-03-19 12:50:23.354754',NULL,NULL),(69,98765,'Pedro Santos','pedro.santos','$2a$11$zpmZ4f1NFmztWrg/kVJ2lOnKsxU0SzxLAC0fyq4mmXfQVTG7hw882','pedrosantos@gmail.com','(38) 91111-1111','Engenheiro de Software',1,1,'2024-03-19 12:51:16.003742',NULL,NULL),(70,67890,'Ana Pereira','ana.pereira','$2a$11$cdIy/jw2ktgooJxvOZJAI.yweApZBaabkgDb2T8001/bt4v4vswAO','anapereira@gmail.com','(38) 92222-2222','Administradora de Redes',1,1,'2024-03-19 12:52:30.208444',NULL,NULL),(71,24680,'Carlos Mendes','carlos.mendes','$2a$11$zmvo9c1SD2AByD0fnWMBsOG3vglPMd/bTp2q1LBSRz6j4Xqxi5OQy','carlosmendes@gmail.com','(38) 93333-3333','Técnico de Suporte',1,1,'2024-03-19 12:53:14.014471',NULL,NULL),(72,13579,'Paula Costa','paula.costa','$2a$11$xchZRMxH6Vs9BCg08CkGcuC6PeapBjRo9WDfPRhEa3fx9/v8bgduG','paulacosta@gmail.com','(38) 94444-4444','Analista de Banco de Dados',1,1,'2024-03-19 12:54:14.876347',NULL,NULL),(73,80246,'Juliana Lima','juliana.lima','$2a$11$D8jZ6kYi572Wt9IxxlpY0.m30GorjEK3D6n5c07wfS8lV9JtfA4Ay','julianalima@gmail.com','(38) 95555-5555','Farmacêutico',1,1,'2024-03-19 12:55:28.190266',NULL,NULL);
/*!40000 ALTER TABLE `administrators` ENABLE KEYS */;
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
