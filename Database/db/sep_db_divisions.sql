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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-19 17:57:00
