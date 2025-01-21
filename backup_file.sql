-- MariaDB dump 10.19  Distrib 10.4.32-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: casestudy
-- ------------------------------------------------------
-- Server version	10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `admins`
--

DROP TABLE IF EXISTS `admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admins` (
  `admin_id` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `username` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`admin_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admins`
--

LOCK TABLES `admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
INSERT INTO `admins` VALUES (2,'Echo','username','password');
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classes`
--

DROP TABLE IF EXISTS `classes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `classes` (
  `class_id` int(11) NOT NULL,
  `professor_id` int(11) NOT NULL,
  `subject_code` int(11) NOT NULL,
  PRIMARY KEY (`class_id`),
  UNIQUE KEY `professor_id` (`professor_id`,`subject_code`),
  KEY `subject_code` (`subject_code`),
  CONSTRAINT `classes_ibfk_1` FOREIGN KEY (`professor_id`) REFERENCES `professors` (`faculty_id`) ON DELETE CASCADE,
  CONSTRAINT `classes_ibfk_2` FOREIGN KEY (`subject_code`) REFERENCES `subjects` (`subject_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classes`
--

LOCK TABLES `classes` WRITE;
/*!40000 ALTER TABLE `classes` DISABLE KEYS */;
INSERT INTO `classes` VALUES (2,1,101),(1,1,105);
/*!40000 ALTER TABLE `classes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `course_year_section`
--

DROP TABLE IF EXISTS `course_year_section`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `course_year_section` (
  `course_year_sectionId` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`course_year_sectionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `course_year_section`
--

LOCK TABLES `course_year_section` WRITE;
/*!40000 ALTER TABLE `course_year_section` DISABLE KEYS */;
INSERT INTO `course_year_section` VALUES (1,'BSIT2A');
/*!40000 ALTER TABLE `course_year_section` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grades`
--

DROP TABLE IF EXISTS `grades`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `grades` (
  `grade_id` int(11) NOT NULL AUTO_INCREMENT,
  `student_id` varchar(50) DEFAULT NULL,
  `class_id` int(11) DEFAULT NULL,
  `term_name` varchar(50) DEFAULT NULL,
  `attendance` int(11) DEFAULT NULL,
  `cw` int(11) DEFAULT NULL,
  `quiz1` int(11) DEFAULT NULL,
  `quiz2` int(11) DEFAULT NULL,
  `project` int(11) DEFAULT NULL,
  `exam` int(11) DEFAULT NULL,
  `grade` int(11) DEFAULT NULL,
  `remarks` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`grade_id`),
  UNIQUE KEY `unique_student_class_term` (`student_id`,`class_id`,`term_name`),
  KEY `class_id` (`class_id`),
  CONSTRAINT `fk_student_id` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`) ON DELETE CASCADE,
  CONSTRAINT `grades_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`),
  CONSTRAINT `grades_ibfk_2` FOREIGN KEY (`class_id`) REFERENCES `classes` (`class_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grades`
--

LOCK TABLES `grades` WRITE;
/*!40000 ALTER TABLE `grades` DISABLE KEYS */;
INSERT INTO `grades` VALUES (30,'23-11111',1,'midterm',1,1,1,1,1,1,100,'Passed'),(31,'23-11111',1,'final',1,1,1,1,1,1,73,'Failed'),(32,'23-22222',1,'midterm',1,1,1,1,1,1,87,'Passed'),(33,'23-22222',1,'final',1,1,1,1,1,1,73,'Failed'),(36,'23-22222',2,'midterm',1,1,1,1,1,1,73,'Failed'),(37,'23-22222',2,'final',1,1,1,1,1,1,73,'Failed');
/*!40000 ALTER TABLE `grades` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gradingweight`
--

DROP TABLE IF EXISTS `gradingweight`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gradingweight` (
  `id` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gradingweight`
--

LOCK TABLES `gradingweight` WRITE;
/*!40000 ALTER TABLE `gradingweight` DISABLE KEYS */;
INSERT INTO `gradingweight` VALUES (1),(2);
/*!40000 ALTER TABLE `gradingweight` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pins`
--

DROP TABLE IF EXISTS `pins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pins` (
  `pin` varchar(50) NOT NULL,
  PRIMARY KEY (`pin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pins`
--

LOCK TABLES `pins` WRITE;
/*!40000 ALTER TABLE `pins` DISABLE KEYS */;
INSERT INTO `pins` VALUES ('11111'),('12211'),('22222'),('33333');
/*!40000 ALTER TABLE `pins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `professors`
--

DROP TABLE IF EXISTS `professors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `professors` (
  `faculty_id` int(11) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `department` varchar(50) DEFAULT NULL,
  `username` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `pin` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`faculty_id`),
  UNIQUE KEY `unique_username_password` (`username`,`password`),
  KEY `pin` (`pin`),
  CONSTRAINT `professors_ibfk_1` FOREIGN KEY (`pin`) REFERENCES `pins` (`pin`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `professors`
--

LOCK TABLES `professors` WRITE;
/*!40000 ALTER TABLE `professors` DISABLE KEYS */;
INSERT INTO `professors` VALUES (1,'Sir Red','CCS','red123','red123','12211');
/*!40000 ALTER TABLE `professors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sections`
--

DROP TABLE IF EXISTS `sections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sections` (
  `section_id` int(11) NOT NULL,
  `section_name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`section_id`),
  UNIQUE KEY `section_name` (`section_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sections`
--

LOCK TABLES `sections` WRITE;
/*!40000 ALTER TABLE `sections` DISABLE KEYS */;
INSERT INTO `sections` VALUES (4,'BSIT2B'),(2,'BSIT2C'),(3,'BSIT3A');
/*!40000 ALTER TABLE `sections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary table structure for view `student_grades`
--

DROP TABLE IF EXISTS `student_grades`;
/*!50001 DROP VIEW IF EXISTS `student_grades`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `student_grades` AS SELECT
 1 AS `student_id`,
  1 AS `student_name`,
  1 AS `class_id`,
  1 AS `term_name`,
  1 AS `attendance`,
  1 AS `cw`,
  1 AS `quiz1`,
  1 AS `quiz2`,
  1 AS `project`,
  1 AS `exam`,
  1 AS `grade`,
  1 AS `remarks` */;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `students` (
  `student_id` varchar(50) NOT NULL,
  `student_name` varchar(50) DEFAULT NULL,
  `course_year_section` varchar(50) DEFAULT NULL,
  `department` varchar(50) DEFAULT NULL,
  `username` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `pin` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`student_id`),
  UNIQUE KEY `unique_username_password` (`username`,`password`),
  KEY `pin` (`pin`),
  CONSTRAINT `students_ibfk_1` FOREIGN KEY (`pin`) REFERENCES `pins` (`pin`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES ('23-11111','Pares Diwata','BSIT2B','CCS','pares123','pares123','11111'),('23-22222','Kiel the great','BSIT2C','CCS','kiel123','kiel123','22222');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subjects`
--

DROP TABLE IF EXISTS `subjects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `subjects` (
  `subject_code` int(11) NOT NULL,
  `subject_title` varchar(100) DEFAULT NULL,
  `curriculum_year` year(4) DEFAULT NULL,
  PRIMARY KEY (`subject_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subjects`
--

LOCK TABLES `subjects` WRITE;
/*!40000 ALTER TABLE `subjects` DISABLE KEYS */;
INSERT INTO `subjects` VALUES (101,'Java Programming',2025),(102,'Python Programming',2025),(103,'Information Management',2024),(104,'Data Structures and Algorithms',2024),(105,'OOP',2023);
/*!40000 ALTER TABLE `subjects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `weightdetails`
--

DROP TABLE IF EXISTS `weightdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weightdetails` (
  `weight_id` int(11) NOT NULL AUTO_INCREMENT,
  `grading_id` int(11) DEFAULT NULL,
  `component_name` varchar(50) NOT NULL,
  `weight` decimal(5,2) NOT NULL,
  PRIMARY KEY (`weight_id`),
  KEY `grading_id` (`grading_id`),
  CONSTRAINT `weightdetails_ibfk_1` FOREIGN KEY (`grading_id`) REFERENCES `gradingweight` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `weightdetails`
--

LOCK TABLES `weightdetails` WRITE;
/*!40000 ALTER TABLE `weightdetails` DISABLE KEYS */;
INSERT INTO `weightdetails` VALUES (1,1,'attendance',10.00),(2,1,'cw',15.00),(3,1,'q1',20.00),(4,1,'q2',15.00),(5,1,'project',25.00),(6,1,'exam',30.00),(7,2,'Attendance',5.00),(8,2,'Classwork',20.00),(9,2,'Quiz1',12.50),(10,2,'Quiz2',12.50),(11,2,'Project',20.00),(12,2,'Exam',30.00);
/*!40000 ALTER TABLE `weightdetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Final view structure for view `student_grades`
--

/*!50001 DROP VIEW IF EXISTS `student_grades`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `student_grades` AS select `grades`.`student_id` AS `student_id`,`students`.`student_name` AS `student_name`,`grades`.`class_id` AS `class_id`,`grades`.`term_name` AS `term_name`,`grades`.`attendance` AS `attendance`,`grades`.`cw` AS `cw`,`grades`.`quiz1` AS `quiz1`,`grades`.`quiz2` AS `quiz2`,`grades`.`project` AS `project`,`grades`.`exam` AS `exam`,`grades`.`grade` AS `grade`,`grades`.`remarks` AS `remarks` from (`grades` join `students` on(`grades`.`student_id` = `students`.`student_id`)) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-13 10:04:01
