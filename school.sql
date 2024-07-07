-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 07, 2024 at 10:51 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `school`
--

-- --------------------------------------------------------

--
-- Table structure for table `students`
--

CREATE TABLE `students` (
  `StudentId` char(36) NOT NULL,
  `StudentName` varchar(255) DEFAULT NULL,
  `Class` varchar(255) DEFAULT NULL,
  `RollNumber` int(11) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL,
  `Marks` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `students`
--

INSERT INTO `students` (`StudentId`, `StudentName`, `Class`, `RollNumber`, `Age`, `Marks`) VALUES
('246b8b44-c815-4efe-aaeb-7c17a8a4521d', 'John Dose bruh', '1A', 29, 28, 450),
('e7cbdd2c-3c97-11ef-b7ce-50ebf6b6af3a', 'John Doe', '1C', 1, 20, 450),
('e7cbde9a-3c97-11ef-b7ce-50ebf6b6af3a', 'Jane Smith', '2B', 2, 19, 475),
('e7cbdec6-3c97-11ef-b7ce-50ebf6b6af3a', 'Bob Johnson', '3C', 3, 20, 425),
('e7cbdede-3c97-11ef-b7ce-50ebf6b6af3a', 'Alice Brown', '4D', 4, 21, 480),
('e7cbdef2-3c97-11ef-b7ce-50ebf6b6af3a', 'Charlie Green', '5C', 5, 22, 460),
('e7cbdf09-3c97-11ef-b7ce-50ebf6b6af3a', 'Emily White', '6A', 6, 18, 440),
('e7cbdf1b-3c97-11ef-b7ce-50ebf6b6af3a', 'David Lee', '7G', 7, 19, 470),
('e7cbdf2f-3c97-11ef-b7ce-50ebf6b6af3a', 'Sophia Taylor', '8H', 8, 20, 430),
('e7cbdf3e-3c97-11ef-b7ce-50ebf6b6af3a', 'James Wilson', '2C', 9, 21, 485),
('e7cbdf4e-3c97-11ef-b7ce-50ebf6b6af3a', 'Olivia Martin', '1B', 10, 22, 455),
('e7cbdf60-3c97-11ef-b7ce-50ebf6b6af3a', 'William Anderson', '4A', 11, 18, 465),
('e7cbdf71-3c97-11ef-b7ce-50ebf6b6af3a', 'Ava Thompson', '2D', 12, 19, 445),
('e7cbdf82-3c97-11ef-b7ce-50ebf6b6af3a', 'Joseph Garcia', '1A', 13, 20, 475),
('e7cbdf92-3c97-11ef-b7ce-50ebf6b6af3a', 'Mia Harris', '2B', 14, 21, 435),
('e7cbdfa1-3c97-11ef-b7ce-50ebf6b6af3a', 'Daniel Martin', '3C', 15, 18, 450),
('e7cbdfb5-3c97-11ef-b7ce-50ebf6b6af3a', 'Emma Robinson', '4D', 16, 19, 460),
('e7cbdfc7-3c97-11ef-b7ce-50ebf6b6af3a', 'Matthew Clark', '5B', 17, 20, 480),
('e7cbdfd7-3c97-11ef-b7ce-50ebf6b6af3a', 'Sophia Lewis', '6B', 18, 21, 440),
('e7cbdfe7-3c97-11ef-b7ce-50ebf6b6af3a', 'Joshua Walker', '7A', 19, 18, 455),
('e7cbdff6-3c97-11ef-b7ce-50ebf6b6af3a', 'Isabella Hall', '8A', 20, 19, 470);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `students`
--
ALTER TABLE `students`
  ADD PRIMARY KEY (`StudentId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
