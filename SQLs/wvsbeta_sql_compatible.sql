CREATE DATABASE  IF NOT EXISTS `wvsbeta` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `wvsbeta`;
-- MySQL dump 10.13  Distrib 5.7.9, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: wvsbeta
-- ------------------------------------------------------
-- Server version	5.5.5-10.2.9-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `beta_keys`
--

DROP TABLE IF EXISTS `beta_keys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `beta_keys` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `used` int(20) DEFAULT 0,
  `betakey` varchar(255) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT current_timestamp(),
  `updated_at` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `value_UNIQUE` (`betakey`),
  KEY `username_idx` (`used`),
  KEY `redeemed_by_username_idx` (`used`),
  KEY `user_idx` (`used`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `beta_keys`
--

LOCK TABLES `beta_keys` WRITE;
/*!40000 ALTER TABLE `beta_keys` DISABLE KEYS */;
/*!40000 ALTER TABLE `beta_keys` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `buddylist`
--

DROP TABLE IF EXISTS `buddylist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `buddylist` (
  `charid` int(11) NOT NULL,
  `buddy_charid` int(11) NOT NULL,
  `buddy_charname` varchar(12) NOT NULL,
  UNIQUE KEY `charid` (`charid`,`buddy_charid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `buddylist`
--

LOCK TABLES `buddylist` WRITE;
/*!40000 ALTER TABLE `buddylist` DISABLE KEYS */;
/*!40000 ALTER TABLE `buddylist` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `buddylist_pending`
--

DROP TABLE IF EXISTS `buddylist_pending`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `buddylist_pending` (
  `charid` int(11) NOT NULL,
  `inviter_charid` int(11) NOT NULL,
  `inviter_charname` varchar(13) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `buddylist_pending`
--

LOCK TABLES `buddylist_pending` WRITE;
/*!40000 ALTER TABLE `buddylist_pending` DISABLE KEYS */;
/*!40000 ALTER TABLE `buddylist_pending` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashitem_bundle`
--

DROP TABLE IF EXISTS `cashitem_bundle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashitem_bundle` (
  `userid` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` smallint(6) NOT NULL DEFAULT 1,
  `cashid` bigint(20) NOT NULL,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  PRIMARY KEY (`cashid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashitem_bundle`
--

LOCK TABLES `cashitem_bundle` WRITE;
/*!40000 ALTER TABLE `cashitem_bundle` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashitem_bundle` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashitem_eqp`
--

DROP TABLE IF EXISTS `cashitem_eqp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashitem_eqp` (
  `userid` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `slots` tinyint(4) NOT NULL DEFAULT 7,
  `scrolls` tinyint(4) NOT NULL DEFAULT 0,
  `istr` smallint(6) NOT NULL DEFAULT 0,
  `idex` smallint(6) NOT NULL DEFAULT 0,
  `iint` smallint(6) NOT NULL DEFAULT 0,
  `iluk` smallint(6) NOT NULL DEFAULT 0,
  `ihp` smallint(6) NOT NULL DEFAULT 0,
  `imp` smallint(6) NOT NULL DEFAULT 0,
  `iwatk` smallint(6) NOT NULL DEFAULT 0,
  `imatk` smallint(6) NOT NULL DEFAULT 0,
  `iwdef` smallint(6) NOT NULL DEFAULT 0,
  `imdef` smallint(6) NOT NULL DEFAULT 0,
  `iacc` smallint(6) NOT NULL DEFAULT 0,
  `iavo` smallint(6) NOT NULL DEFAULT 0,
  `ihand` smallint(6) NOT NULL DEFAULT 0,
  `ispeed` smallint(6) NOT NULL DEFAULT 0,
  `ijump` smallint(6) NOT NULL DEFAULT 0,
  `cashid` bigint(20) NOT NULL,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  PRIMARY KEY (`cashid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashitem_eqp`
--

LOCK TABLES `cashitem_eqp` WRITE;
/*!40000 ALTER TABLE `cashitem_eqp` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashitem_eqp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashitem_pet`
--

DROP TABLE IF EXISTS `cashitem_pet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashitem_pet` (
  `userid` int(11) NOT NULL,
  `cashid` bigint(20) NOT NULL,
  `itemid` int(11) NOT NULL,
  `name` varchar(12) COLLATE latin1_general_ci NOT NULL,
  `level` tinyint(3) NOT NULL,
  `closeness` smallint(6) NOT NULL,
  `fullness` tinyint(3) NOT NULL,
  `expiration` bigint(20) NOT NULL,
  `deaddate` bigint(20) NOT NULL,
  PRIMARY KEY (`cashid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashitem_pet`
--

LOCK TABLES `cashitem_pet` WRITE;
/*!40000 ALTER TABLE `cashitem_pet` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashitem_pet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashshop_coupon_codes`
--

DROP TABLE IF EXISTS `cashshop_coupon_codes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashshop_coupon_codes` (
  `serial` varchar(22) NOT NULL,
  `maplepoints` int(11) NOT NULL DEFAULT 0,
  `nxcredit` int(11) NOT NULL DEFAULT 0,
  `nxprepaid` int(11) NOT NULL DEFAULT 0,
  `mesos` int(11) NOT NULL DEFAULT 0,
  `used` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`serial`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashshop_coupon_codes`
--

LOCK TABLES `cashshop_coupon_codes` WRITE;
/*!40000 ALTER TABLE `cashshop_coupon_codes` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashshop_coupon_codes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashshop_coupon_item_rewards`
--

DROP TABLE IF EXISTS `cashshop_coupon_item_rewards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashshop_coupon_item_rewards` (
  `serial` varchar(22) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(11) NOT NULL DEFAULT 1,
  `days_usable` int(5) NOT NULL DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashshop_coupon_item_rewards`
--

LOCK TABLES `cashshop_coupon_item_rewards` WRITE;
/*!40000 ALTER TABLE `cashshop_coupon_item_rewards` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashshop_coupon_item_rewards` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashshop_limit_sell`
--

DROP TABLE IF EXISTS `cashshop_limit_sell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashshop_limit_sell` (
  `serial` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashshop_limit_sell`
--

LOCK TABLES `cashshop_limit_sell` WRITE;
/*!40000 ALTER TABLE `cashshop_limit_sell` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashshop_limit_sell` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashshop_modified_items`
--

DROP TABLE IF EXISTS `cashshop_modified_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashshop_modified_items` (
  `serial` int(11) NOT NULL,
  `discount_price` int(11) DEFAULT NULL,
  `mark` enum('sale','hot','event','new') DEFAULT NULL,
  `showup` tinyint(1) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashshop_modified_items`
--

LOCK TABLES `cashshop_modified_items` WRITE;
/*!40000 ALTER TABLE `cashshop_modified_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashshop_modified_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cashshop_sell_log`
--

DROP TABLE IF EXISTS `cashshop_sell_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cashshop_sell_log` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `charid` int(11) NOT NULL,
  `packageid` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cashshop_sell_log`
--

LOCK TABLES `cashshop_sell_log` WRITE;
/*!40000 ALTER TABLE `cashshop_sell_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `cashshop_sell_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_quests`
--

DROP TABLE IF EXISTS `character_quests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_quests` (
  `id` INT(10) NOT NULL AUTO_INCREMENT,
  `charid` INT(10) NOT NULL,
  `questid` SMALLINT(5) NOT NULL,
  `data` VARCHAR(40) NULL DEFAULT NULL COLLATE 'latin1_swedish_ci',
  `state` TINYINT(3) NOT NULL DEFAULT '-1',
  `endtime` BIGINT(19) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) COLLATE='latin1_swedish_ci' ENGINE=InnoDB;

/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_quests`
--

LOCK TABLES `character_quests` WRITE;
/*!40000 ALTER TABLE `character_quests` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_quests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_variables`
--

DROP TABLE IF EXISTS `character_variables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_variables` (
  `charid` int(11) NOT NULL,
  `key` varchar(255) NOT NULL,
  `value` varchar(255) NOT NULL,
  UNIQUE KEY `charid_2` (`charid`,`key`),
  KEY `charid` (`charid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_variables`
--

LOCK TABLES `character_variables` WRITE;
/*!40000 ALTER TABLE `character_variables` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_variables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `character_wishlist`
--

DROP TABLE IF EXISTS `character_wishlist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_wishlist` (
  `charid` int(11) NOT NULL,
  `serial` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `character_wishlist`
--

LOCK TABLES `character_wishlist` WRITE;
/*!40000 ALTER TABLE `character_wishlist` DISABLE KEYS */;
/*!40000 ALTER TABLE `character_wishlist` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(12) COLLATE latin1_general_ci NOT NULL,
  `userid` int(11) NOT NULL,
  `world_id` tinyint(1) unsigned NOT NULL,
  `level` tinyint(3) unsigned NOT NULL DEFAULT 1,
  `job` smallint(6) NOT NULL DEFAULT 0,
  `str` smallint(6) NOT NULL DEFAULT 4,
  `dex` smallint(6) NOT NULL DEFAULT 4,
  `int` smallint(6) NOT NULL DEFAULT 4,
  `luk` smallint(6) NOT NULL DEFAULT 4,
  `chp` smallint(6) NOT NULL DEFAULT 50,
  `mhp` smallint(6) NOT NULL DEFAULT 50,
  `cmp` smallint(6) NOT NULL DEFAULT 50,
  `mmp` smallint(6) NOT NULL DEFAULT 50,
  `hpmp_ap` int(11) NOT NULL DEFAULT 0,
  `ap` smallint(6) NOT NULL DEFAULT 0,
  `sp` smallint(6) NOT NULL DEFAULT 0,
  `exp` int(11) NOT NULL DEFAULT 0,
  `fame` smallint(6) NOT NULL DEFAULT 0,
  `map` int(11) NOT NULL DEFAULT 0,
  `pos` smallint(6) NOT NULL DEFAULT 0,
  `gender` tinyint(1) NOT NULL,
  `skin` tinyint(4) NOT NULL,
  `eyes` int(11) NOT NULL,
  `hair` int(11) NOT NULL,
  `mesos` int(11) NOT NULL DEFAULT 0,
  `equip_slots` int(11) NOT NULL DEFAULT 24,
  `use_slots` int(11) NOT NULL DEFAULT 24,
  `setup_slots` int(11) NOT NULL DEFAULT 24,
  `etc_slots` int(11) NOT NULL DEFAULT 24,
  `cash_slots` int(11) NOT NULL DEFAULT 48,
  `buddylist_size` int(3) unsigned NOT NULL DEFAULT 20,
  `online` tinyint(1) NOT NULL DEFAULT 0,
  `time_level` datetime NOT NULL DEFAULT '2012-01-09 12:37:00',
  `overall_cpos` int(11) NOT NULL DEFAULT 0,
  `overall_opos` int(11) NOT NULL DEFAULT 0,
  `world_cpos` int(11) NOT NULL DEFAULT 0,
  `world_opos` int(11) NOT NULL DEFAULT 0,
  `job_cpos` int(11) NOT NULL DEFAULT 0,
  `job_opos` int(11) NOT NULL DEFAULT 0,
  `fame_cpos` int(11) NOT NULL DEFAULT 0,
  `fame_opos` int(11) NOT NULL DEFAULT 0,
  `party` int(11) NOT NULL DEFAULT -1,
  `last_savepoint` datetime DEFAULT '2012-01-09 12:37:00',
  `hash` varchar(50) COLLATE latin1_general_ci DEFAULT NULL,
  `rankbanned` tinyint(1) NOT NULL DEFAULT 0 COMMENT '0 false 1 true',
  `pet_cash_id` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`ID`),
  KEY `userid` (`userid`),
  KEY `world_id` (`world_id`),
  KEY `name` (`name`),
  KEY `job` (`job`),
  KEY `level` (`level`),
  KEY `fame` (`fame`),
  KEY `online` (`online`),
  KEY `userid_worldid` (`userid`,`world_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characters`
--

LOCK TABLES `characters` WRITE;
/*!40000 ALTER TABLE `characters` DISABLE KEYS */;
/*!40000 ALTER TABLE `characters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `connections`
--

DROP TABLE IF EXISTS `connections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `connections` (
  `when` datetime NOT NULL,
  `howmany` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `connections`
--

LOCK TABLES `connections` WRITE;
/*!40000 ALTER TABLE `connections` DISABLE KEYS */;
/*!40000 ALTER TABLE `connections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cooldowns`
--

DROP TABLE IF EXISTS `cooldowns`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cooldowns` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `charid` int(11) DEFAULT NULL,
  `skillid` int(11) DEFAULT 0,
  `timeleft` smallint(6) DEFAULT 0,
  PRIMARY KEY (`id`),
  KEY `charid` (`charid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cooldowns`
--

LOCK TABLES `cooldowns` WRITE;
/*!40000 ALTER TABLE `cooldowns` DISABLE KEYS */;
/*!40000 ALTER TABLE `cooldowns` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `data_ids`
--

DROP TABLE IF EXISTS `data_ids`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `data_ids` (
  `objectid` int(10) NOT NULL,
  `objectname` varchar(150) NOT NULL,
  `objecttype` enum('item','map','mob','script','npc','skill') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `data_ids`
--

LOCK TABLES `data_ids` WRITE;
/*!40000 ALTER TABLE `data_ids` DISABLE KEYS */;
/*!40000 ALTER TABLE `data_ids` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fame_log`
--

DROP TABLE IF EXISTS `fame_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fame_log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `from` int(11) NOT NULL,
  `to` int(11) NOT NULL,
  `time` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `from` (`from`,`to`,`time`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fame_log`
--

LOCK TABLES `fame_log` WRITE;
/*!40000 ALTER TABLE `fame_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `fame_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gamestats`
--

DROP TABLE IF EXISTS `gamestats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `gamestats` (
  `charid` INT(10) NOT NULL DEFAULT '0',
	`type` INT(10) NOT NULL DEFAULT '0',
	`wins` INT(10) NOT NULL DEFAULT '0',
	`losses` INT(10) NOT NULL DEFAULT '0',
	`ties` INT(10) NOT NULL DEFAULT '0',
	`points` INT(10) NOT NULL DEFAULT '0',
  UNIQUE INDEX `charid_type` (`charid`, `type`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gamestats`
--

LOCK TABLES `gamestats` WRITE;
/*!40000 ALTER TABLE `gamestats` DISABLE KEYS */;
/*!40000 ALTER TABLE `gamestats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guilds`
--

DROP TABLE IF EXISTS `guilds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guilds` (
	`id` INT(10) NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(13) NOT NULL,
	`capacity` INT(10) NOT NULL,
	`emblem_bg` SMALLINT(5) NOT NULL DEFAULT '0',
	`emblem_bgc` TINYINT(3) NOT NULL DEFAULT '0',
	`emblem_logo` SMALLINT(5) NOT NULL DEFAULT '0',
	`emblem_logoc` TINYINT(3) NOT NULL DEFAULT '0',
	`rank1` VARCHAR(50) NOT NULL DEFAULT '',
	`rank2` VARCHAR(50) NOT NULL DEFAULT '',
	`rank3` VARCHAR(50) NOT NULL DEFAULT '',
	`rank4` VARCHAR(50) NOT NULL DEFAULT '',
	`rank5` VARCHAR(50) NOT NULL DEFAULT '',
	PRIMARY KEY (`id`) USING BTREE,
	UNIQUE INDEX `id` (`id`) USING BTREE,
	UNIQUE INDEX `name` (`name`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guilds`
--

LOCK TABLES `guilds` WRITE;
/*!40000 ALTER TABLE `guilds` DISABLE KEYS */;
/*!40000 ALTER TABLE `guilds` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guild_members`
--

DROP TABLE IF EXISTS `guild_members`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guild_members` (
	`id` INT(10) NOT NULL AUTO_INCREMENT,
	`guild_id` INT(10) NOT NULL,
	`character_id` INT(10) NOT NULL,
	`rank` INT(10) NOT NULL DEFAULT '0',
	PRIMARY KEY (`id`) USING BTREE,
	UNIQUE INDEX `id` (`id`) USING BTREE,
	INDEX `FK__guilds` (`guild_id`) USING BTREE,
	INDEX `FK__characters` (`character_id`) USING BTREE,
	CONSTRAINT `FK__characters` FOREIGN KEY (`character_id`) REFERENCES `wvsbeta`.`characters` (`ID`) ON UPDATE NO ACTION ON DELETE CASCADE,
	CONSTRAINT `FK__guilds` FOREIGN KEY (`guild_id`) REFERENCES `wvsbeta`.`guilds` (`id`) ON UPDATE NO ACTION ON DELETE CASCADE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guild_members`
--

LOCK TABLES `guild_members` WRITE;
/*!40000 ALTER TABLE `guild_members` DISABLE KEYS */;
/*!40000 ALTER TABLE `guild_members` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inc_table`
--

DROP TABLE IF EXISTS `inc_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inc_table` (
  `last_cash_serial` bigint(20) unsigned NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inc_table`
--

LOCK TABLES `inc_table` WRITE;
/*!40000 ALTER TABLE `inc_table` DISABLE KEYS */;
/*!40000 ALTER TABLE `inc_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory_bundle`
--

DROP TABLE IF EXISTS `inventory_bundle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inventory_bundle` (
  `charid` int(11) NOT NULL,
  `inv` tinyint(4) NOT NULL,
  `slot` smallint(6) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(11) NOT NULL DEFAULT 1,
  `cashid` bigint(20) NOT NULL DEFAULT 0,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  PRIMARY KEY (`charid`,`inv`,`slot`),
  KEY `charid` (`charid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory_bundle`
--

LOCK TABLES `inventory_bundle` WRITE;
/*!40000 ALTER TABLE `inventory_bundle` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventory_bundle` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory_eqp`
--

DROP TABLE IF EXISTS `inventory_eqp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inventory_eqp` (
  `charid` int(11) NOT NULL,
  `slot` smallint(6) NOT NULL,
  `itemid` int(11) NOT NULL,
  `slots` tinyint(4) NOT NULL DEFAULT 7,
  `scrolls` tinyint(4) NOT NULL DEFAULT 0,
  `istr` smallint(6) NOT NULL DEFAULT 0,
  `idex` smallint(6) NOT NULL DEFAULT 0,
  `iint` smallint(6) NOT NULL DEFAULT 0,
  `iluk` smallint(6) NOT NULL DEFAULT 0,
  `ihp` smallint(6) NOT NULL DEFAULT 0,
  `imp` smallint(6) NOT NULL DEFAULT 0,
  `iwatk` smallint(6) NOT NULL DEFAULT 0,
  `imatk` smallint(6) NOT NULL DEFAULT 0,
  `iwdef` smallint(6) NOT NULL DEFAULT 0,
  `imdef` smallint(6) NOT NULL DEFAULT 0,
  `iacc` smallint(6) NOT NULL DEFAULT 0,
  `iavo` smallint(6) NOT NULL DEFAULT 0,
  `ihand` smallint(6) NOT NULL DEFAULT 0,
  `ispeed` smallint(6) NOT NULL DEFAULT 0,
  `ijump` smallint(6) NOT NULL DEFAULT 0,
  `cashid` bigint(20) NOT NULL DEFAULT 0,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  PRIMARY KEY (`charid`,`slot`),
  KEY `charid` (`charid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory_eqp`
--

LOCK TABLES `inventory_eqp` WRITE;
/*!40000 ALTER TABLE `inventory_eqp` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventory_eqp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ipbans`
--

DROP TABLE IF EXISTS `ipbans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ipbans` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ip` varchar(15) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ipbans`
--

LOCK TABLES `ipbans` WRITE;
/*!40000 ALTER TABLE `ipbans` DISABLE KEYS */;
/*!40000 ALTER TABLE `ipbans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itemlocker`
--

DROP TABLE IF EXISTS `itemlocker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `itemlocker` (
  `cashid` bigint(20) NOT NULL,
  `slot` smallint(6) NOT NULL,
  `userid` int(11) NOT NULL,
  `charid` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `commodity_id` int(11) NOT NULL,
  `amount` smallint(6) NOT NULL,
  `expiration` bigint(20) NOT NULL,
  `gift_unread` tinyint(4) NOT NULL DEFAULT 0,
  `worldid` tinyint(4) NOT NULL,
  `couplecashid` BIGINT(19) NOT NULL DEFAULT 0,
  `couple_charid` INT(10) NOT NULL DEFAULT 0,
  PRIMARY KEY (`cashid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itemlocker`
--

LOCK TABLES `itemlocker` WRITE;
/*!40000 ALTER TABLE `itemlocker` DISABLE KEYS */;
/*!40000 ALTER TABLE `itemlocker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `jobs`
--

DROP TABLE IF EXISTS `jobs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `jobs` (
  `id` varchar(5) NOT NULL,
  `name` varchar(255) NOT NULL,
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `jobs`
--

LOCK TABLES `jobs` WRITE;
/*!40000 ALTER TABLE `jobs` DISABLE KEYS */;
/*!40000 ALTER TABLE `jobs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `machine_ban`
--

DROP TABLE IF EXISTS `machine_ban`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `machine_ban` (
  `machineid` varchar(32) NOT NULL,
  `last_username` varchar(13) NOT NULL,
  `last_ip` varchar(15) NOT NULL,
  `last_try` datetime NOT NULL,
  `reason` text DEFAULT NULL,
  `last_unique_id` varchar(26) DEFAULT '',
  UNIQUE KEY `machineid` (`machineid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `machine_ban`
--

LOCK TABLES `machine_ban` WRITE;
/*!40000 ALTER TABLE `machine_ban` DISABLE KEYS */;
/*!40000 ALTER TABLE `machine_ban` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `memos`
--

DROP TABLE IF EXISTS `memos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `memos` (
	`id` INT(10) NOT NULL AUTO_INCREMENT,
	`from` INT(10) NOT NULL,
	`to` INT(10) NOT NULL,
	`message` VARCHAR(255) NOT NULL COLLATE 'latin1_swedish_ci',
	`sent` BIGINT(19) NOT NULL DEFAULT '0',
	`read` BIGINT(19) NOT NULL DEFAULT '0',
  `fromname` VARCHAR(13) NOT NULL COLLATE 'latin1_general_ci',
  `gift_cashid` BIGINT(19) NULL DEFAULT NULL,
	PRIMARY KEY (`id`) USING BTREE,
	INDEX `fromcid_fk` (`from`) USING BTREE,
	INDEX `tocid_fk` (`to`) USING BTREE,
	CONSTRAINT `fromcid_fk` FOREIGN KEY (`from`) REFERENCES `wvsbeta`.`characters` (`ID`) ON UPDATE NO ACTION ON DELETE CASCADE,
	CONSTRAINT `tocid_fk` FOREIGN KEY (`to`) REFERENCES `wvsbeta`.`characters` (`ID`) ON UPDATE NO ACTION ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `memos`
--

LOCK TABLES `memos` WRITE;
/*!40000 ALTER TABLE `memos` DISABLE KEYS */;
/*!40000 ALTER TABLE `memos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `password_resets`
--

DROP TABLE IF EXISTS `password_resets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `password_resets` (
  `email` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `token` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `password_resets`
--

LOCK TABLES `password_resets` WRITE;
/*!40000 ALTER TABLE `password_resets` DISABLE KEYS */;
/*!40000 ALTER TABLE `password_resets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pets`
--

DROP TABLE IF EXISTS `pets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pets` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `index` tinyint(3) NOT NULL DEFAULT -1,
  `name` varchar(12) NOT NULL,
  `level` tinyint(3) NOT NULL DEFAULT 1,
  `closeness` smallint(6) NOT NULL DEFAULT 0,
  `fullness` tinyint(3) NOT NULL DEFAULT 100,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  UNIQUE KEY `id` (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pets`
--

LOCK TABLES `pets` WRITE;
/*!40000 ALTER TABLE `pets` DISABLE KEYS */;
/*!40000 ALTER TABLE `pets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `servers`
--

DROP TABLE IF EXISTS `servers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `servers` (
   `configname` varchar(45) NOT NULL,
   `world_id` tinyint(4) NOT NULL,
   `private_ip` varchar(45) DEFAULT NULL,
   PRIMARY KEY (`world_id`,`configname`)
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servers`
--

LOCK TABLES `servers` WRITE;
/*!40000 ALTER TABLE `servers` DISABLE KEYS */;
/*!40000 ALTER TABLE `servers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skills`
--

DROP TABLE IF EXISTS `skills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `skills` (
  `charid` int(11) NOT NULL,
  `skillid` int(11) NOT NULL,
  `points` smallint(6) NOT NULL DEFAULT 1,
  UNIQUE KEY `charid_2` (`charid`,`skillid`),
  KEY `charid` (`charid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skills`
--

LOCK TABLES `skills` WRITE;
/*!40000 ALTER TABLE `skills` DISABLE KEYS */;
/*!40000 ALTER TABLE `skills` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage`
--

DROP TABLE IF EXISTS `storage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage` (
  `userid` int(11) NOT NULL,
  `world_id` int(11) NOT NULL,
  `slots` smallint(6) NOT NULL DEFAULT 4,
  `mesos` int(11) NOT NULL DEFAULT 0,
  `char_slots` int(11) NOT NULL DEFAULT 3,
  `credit_nx` int(11) NOT NULL DEFAULT 0,
  `maplepoints` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`userid`,`world_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage`
--

LOCK TABLES `storage` WRITE;
/*!40000 ALTER TABLE `storage` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_bundle`
--

DROP TABLE IF EXISTS `storage_bundle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_bundle` (
  `userid` int(11) NOT NULL,
  `world_id` int(11) NOT NULL,
  `inv` tinyint(4) NOT NULL,
  `slot` smallint(6) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` smallint(11) NOT NULL DEFAULT 1,
  `cashid` bigint(20) NOT NULL,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  KEY `userid_worldid` (`userid`,`world_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_bundle`
--

LOCK TABLES `storage_bundle` WRITE;
/*!40000 ALTER TABLE `storage_bundle` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage_bundle` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_cash`
--

DROP TABLE IF EXISTS `storage_cash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_cash` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `world_id` int(11) NOT NULL,
  `bought_userid` int(11) NOT NULL,
  `sn` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(3) NOT NULL DEFAULT 1,
  `from` varchar(13) NOT NULL DEFAULT '',
  `expires` bigint(20) NOT NULL DEFAULT 150842304000000000,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_cash`
--

LOCK TABLES `storage_cash` WRITE;
/*!40000 ALTER TABLE `storage_cash` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage_cash` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_cashshop`
--

DROP TABLE IF EXISTS `storage_cashshop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_cashshop` (
  `userid` int(11) NOT NULL,
  `world_id` int(2) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(11) NOT NULL DEFAULT 1,
  `slots` tinyint(4) DEFAULT 7,
  `scrolls` smallint(6) DEFAULT 0,
  `istr` smallint(6) DEFAULT 0,
  `idex` smallint(6) DEFAULT 0,
  `iint` smallint(6) DEFAULT 0,
  `iluk` smallint(6) DEFAULT 0,
  `ihp` smallint(6) DEFAULT 0,
  `imp` smallint(6) DEFAULT 0,
  `iwatk` smallint(6) DEFAULT 0,
  `imatk` smallint(6) DEFAULT 0,
  `iwdef` smallint(6) DEFAULT 0,
  `imdef` smallint(6) DEFAULT 0,
  `iacc` smallint(6) DEFAULT 0,
  `iavo` smallint(6) DEFAULT 0,
  `ihand` smallint(6) DEFAULT 0,
  `ispeed` smallint(6) DEFAULT 0,
  `ijump` smallint(6) DEFAULT 0,
  `name` varchar(12) COLLATE latin1_general_ci NOT NULL,
  `cashid` int(11) NOT NULL DEFAULT 0,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_cashshop`
--

LOCK TABLES `storage_cashshop` WRITE;
/*!40000 ALTER TABLE `storage_cashshop` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage_cashshop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_eqp`
--

DROP TABLE IF EXISTS `storage_eqp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_eqp` (
  `userid` int(11) NOT NULL,
  `world_id` int(11) NOT NULL,
  `slot` smallint(6) NOT NULL,
  `itemid` int(11) NOT NULL,
  `slots` tinyint(4) NOT NULL DEFAULT 7,
  `scrolls` tinyint(4) NOT NULL DEFAULT 0,
  `istr` smallint(6) NOT NULL DEFAULT 0,
  `idex` smallint(6) NOT NULL DEFAULT 0,
  `iint` smallint(6) NOT NULL DEFAULT 0,
  `iluk` smallint(6) NOT NULL DEFAULT 0,
  `ihp` smallint(6) NOT NULL DEFAULT 0,
  `imp` smallint(6) NOT NULL DEFAULT 0,
  `iwatk` smallint(6) NOT NULL DEFAULT 0,
  `imatk` smallint(6) NOT NULL DEFAULT 0,
  `iwdef` smallint(6) NOT NULL DEFAULT 0,
  `imdef` smallint(6) NOT NULL DEFAULT 0,
  `iacc` smallint(6) NOT NULL DEFAULT 0,
  `iavo` smallint(6) NOT NULL DEFAULT 0,
  `ihand` smallint(6) NOT NULL DEFAULT 0,
  `ispeed` smallint(6) NOT NULL DEFAULT 0,
  `ijump` smallint(6) NOT NULL DEFAULT 0,
  `cashid` bigint(20) NOT NULL,
  `expiration` bigint(20) NOT NULL DEFAULT 150842304000000000,
  KEY `userid_worldid` (`userid`,`world_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_eqp`
--

LOCK TABLES `storage_eqp` WRITE;
/*!40000 ALTER TABLE `storage_eqp` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage_eqp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storage_items`
--

DROP TABLE IF EXISTS `storage_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `storage_items` (
  `userid` int(11) NOT NULL,
  `world_id` int(11) NOT NULL,
  `slot` smallint(6) NOT NULL,
  `itemid` int(11) NOT NULL,
  `amount` int(11) NOT NULL DEFAULT 1,
  `slots` tinyint(4) NOT NULL DEFAULT 7,
  `scrolls` tinyint(4) NOT NULL DEFAULT 0,
  `istr` smallint(6) NOT NULL DEFAULT 0,
  `idex` smallint(6) NOT NULL DEFAULT 0,
  `iint` smallint(6) NOT NULL DEFAULT 0,
  `iluk` smallint(6) NOT NULL DEFAULT 0,
  `ihp` smallint(6) NOT NULL DEFAULT 0,
  `imp` smallint(6) NOT NULL DEFAULT 0,
  `iwatk` smallint(6) NOT NULL DEFAULT 0,
  `imatk` smallint(6) NOT NULL DEFAULT 0,
  `iwdef` smallint(6) NOT NULL DEFAULT 0,
  `imdef` smallint(6) NOT NULL DEFAULT 0,
  `iacc` smallint(6) NOT NULL DEFAULT 0,
  `iavo` smallint(6) NOT NULL DEFAULT 0,
  `ihand` smallint(6) NOT NULL DEFAULT 0,
  `ispeed` smallint(6) NOT NULL DEFAULT 0,
  `ijump` smallint(6) NOT NULL DEFAULT 0,
  `name` varchar(12) NOT NULL,
  `expiration` bigint(18) NOT NULL DEFAULT 150842304000000000,
  PRIMARY KEY (`userid`,`world_id`,`slot`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storage_items`
--

LOCK TABLES `storage_items` WRITE;
/*!40000 ALTER TABLE `storage_items` DISABLE KEYS */;
/*!40000 ALTER TABLE `storage_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teleport_rock_locations`
--

DROP TABLE IF EXISTS `teleport_rock_locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `teleport_rock_locations` (
  `charid` int(11) NOT NULL,
  `mapindex` tinyint(3) NOT NULL,
  `mapid` int(11) NOT NULL DEFAULT 999999999,
  PRIMARY KEY (`charid`,`mapindex`),
  KEY `charid` (`charid`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teleport_rock_locations`
--

LOCK TABLES `teleport_rock_locations` WRITE;
/*!40000 ALTER TABLE `teleport_rock_locations` DISABLE KEYS */;
/*!40000 ALTER TABLE `teleport_rock_locations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_point_transactions`
--

DROP TABLE IF EXISTS `user_point_transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_point_transactions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `amount` mediumint(9) NOT NULL,
  `date` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `sn` INT(10) NOT NULL,
  `note` text NOT NULL,
  `pointtype` enum('maplepoints','nx') NOT NULL,
  PRIMARY KEY (`id`),
  KEY `uid_pointtype` (`userid`,`pointtype`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_point_transactions`
--

LOCK TABLES `user_point_transactions` WRITE;
/*!40000 ALTER TABLE `user_point_transactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_point_transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) COLLATE latin1_general_ci NOT NULL,
  `password` char(130) COLLATE latin1_general_ci NOT NULL,
  `salt` char(10) COLLATE latin1_general_ci DEFAULT NULL,
  `email` varchar(255) COLLATE latin1_general_ci NOT NULL,
  `pin` varchar(4) COLLATE latin1_general_ci DEFAULT NULL,
  `gender` tinyint(1) unsigned DEFAULT NULL,
  `gm` tinyint(4) NOT NULL DEFAULT 0,
  `admin` tinyint(1) NOT NULL DEFAULT 0,
  `char_delete_password` int(8) unsigned NOT NULL DEFAULT 11111111,
  `online` int(5) NOT NULL DEFAULT 0,
  `ban_expire` datetime NOT NULL DEFAULT '2012-01-09 12:37:00',
  `ban_reason` tinyint(2) unsigned NOT NULL DEFAULT 0,
  `ban_reason_message` varchar(255) COLLATE latin1_general_ci NOT NULL DEFAULT '',
  `banned_by` varchar(13) COLLATE latin1_general_ci DEFAULT NULL,
  `banned_at` datetime DEFAULT NULL,
  `last_ip` varchar(45) COLLATE latin1_general_ci DEFAULT NULL,
  `last_machine_id` varchar(32) COLLATE latin1_general_ci DEFAULT NULL,
  `last_login` datetime NOT NULL DEFAULT '2012-01-09 12:37:00',
  `quiet_ban_expire` datetime NOT NULL DEFAULT '2012-01-09 12:37:00',
  `quiet_ban_reason` tinyint(3) NOT NULL DEFAULT 0,
  `creation_date` datetime NOT NULL DEFAULT '2012-01-09 12:37:00',
  `donator` tinyint(1) DEFAULT 0,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `remember_token` varchar(255) COLLATE latin1_general_ci NOT NULL DEFAULT '',
  `superadmin` tinyint(1) NOT NULL DEFAULT 0,
  `beta` tinyint(1) NOT NULL DEFAULT 0,
  `confirmed_eula` tinyint(1) NOT NULL DEFAULT 0,
  `web_admin` tinyint(4) NOT NULL DEFAULT 0,
  `last_unique_id` varchar(26) COLLATE latin1_general_ci DEFAULT NULL,
  `max_unique_id_ban_count` tinyint(1) DEFAULT 5,
  `max_ip_ban_count` tinyint(1) DEFAULT 3,
  PRIMARY KEY (`ID`),
  KEY `username` (`username`),
  KEY `online` (`online`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'wvsbeta'
--

--
-- Dumping routines for database 'wvsbeta'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-10-06 15:49:38
