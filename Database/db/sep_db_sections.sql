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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-19 17:57:00