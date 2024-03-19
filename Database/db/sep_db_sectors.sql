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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-19 17:57:01
