-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 15, 2025 at 11:32 PM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 7.4.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bvs_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `customers`
--

CREATE TABLE `customers` (
  `customer_id` int(11) NOT NULL,
  `customer_name` varchar(100) NOT NULL,
  `phone` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customers`
--

INSERT INTO `customers` (`customer_id`, `customer_name`, `phone`) VALUES
(2, 'Ben', '09952209078'),
(3, 'Ken', '12345678'),
(6, 'Monica', '09952209078'),
(7, 'Tofee Labrador', '09967345323'),
(14, 'benny', '123456732'),
(18, 'Maria Joana Kawasaki', '09783452512'),
(20, 'Jes', '091235723821'),
(22, 'Kenny', '123456789'),
(23, 'Ken Llanera', '09973452631'),
(26, 'Jane Enderes', '09424523213');

-- --------------------------------------------------------

--
-- Table structure for table `rentals`
--

CREATE TABLE `rentals` (
  `rental_id` int(11) NOT NULL,
  `customer_id` int(11) DEFAULT NULL,
  `video_id` int(11) DEFAULT NULL,
  `rent_date` date NOT NULL,
  `due_date` date NOT NULL,
  `return_date` date DEFAULT NULL,
  `total_price` decimal(10,2) DEFAULT NULL,
  `overdue_price` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `rentals`
--

INSERT INTO `rentals` (`rental_id`, `customer_id`, `video_id`, `rent_date`, `due_date`, `return_date`, `total_price`, `overdue_price`) VALUES
(6, 2, 4, '2025-05-12', '2025-05-14', '2025-05-12', 10.00, 0.00),
(7, 6, 1, '2025-05-12', '2025-05-14', '2025-05-12', 50.00, 0.00),
(8, 7, 1, '2025-05-13', '2025-05-14', '2025-05-16', 50.00, 10.00),
(9, 3, 5, '2025-05-13', '2025-05-15', NULL, 50.00, 0.00),
(10, 3, 2, '2025-05-13', '2025-05-15', '2025-05-15', 25.00, 0.00),
(11, 2, 5, '2025-05-16', '2025-05-17', NULL, 50.00, 0.00),
(15, 6, 4, '2025-05-14', '2025-05-16', NULL, 50.00, 0.00),
(16, 7, 4, '2025-05-16', '2025-05-17', '2025-05-16', 50.00, 0.00),
(17, 18, 4, '2025-05-14', '2025-05-16', NULL, 50.00, 0.00),
(18, 20, 4, '2025-05-14', '2025-05-16', NULL, 50.00, 0.00),
(19, 2, 12, '2025-05-14', '2025-05-17', NULL, 50.00, 0.00),
(20, 20, 15, '2025-05-15', '2025-05-16', NULL, 50.00, 0.00),
(21, 26, 13, '2025-05-15', '2025-05-18', NULL, 50.00, 0.00),
(22, 6, 4, '2025-05-16', '2025-05-18', NULL, 50.00, 0.00),
(23, 2, 2, '2025-05-16', '2025-05-19', NULL, 25.00, 0.00),
(24, 2, 4, '2025-05-16', '2025-05-18', NULL, 50.00, 0.00),
(25, 3, 8, '2025-05-16', '2025-05-18', NULL, 25.00, 0.00),
(26, 3, 17, '2025-05-16', '2025-05-18', NULL, 50.00, 0.00);

-- --------------------------------------------------------

--
-- Table structure for table `videos`
--

CREATE TABLE `videos` (
  `video_id` int(11) NOT NULL,
  `title` varchar(100) NOT NULL,
  `category` enum('VCD','DVD') NOT NULL,
  `quantity_in` int(11) DEFAULT 0,
  `quantity_out` int(11) DEFAULT 0,
  `rental_days_allowed` int(11) DEFAULT NULL CHECK (`rental_days_allowed` between 1 and 3)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `videos`
--

INSERT INTO `videos` (`video_id`, `title`, `category`, `quantity_in`, `quantity_out`, `rental_days_allowed`) VALUES
(1, 'Avengers', 'DVD', 17, 9, 2),
(2, 'Superman', 'VCD', 5, 2, 3),
(4, 'Thor', 'DVD', 0, 5, 2),
(5, 'Above All', 'DVD', 4, 0, 2),
(6, 'Saving Private Ryan', 'VCD', 13, 2, 3),
(7, 'Batman', 'DVD', 8, 0, 3),
(8, 'Gone with the Wind', 'VCD', 9, 1, 2),
(12, 'Psyco', 'DVD', 2, 1, 3),
(13, 'One Piece', 'DVD', 9, 1, 3),
(15, 'Loki', 'DVD', 8, 1, 3),
(16, 'One Piece', 'VCD', 10, 0, 3),
(17, 'Superman', 'DVD', 4, 1, 3);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`customer_id`);

--
-- Indexes for table `rentals`
--
ALTER TABLE `rentals`
  ADD PRIMARY KEY (`rental_id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `video_id` (`video_id`);

--
-- Indexes for table `videos`
--
ALTER TABLE `videos`
  ADD PRIMARY KEY (`video_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `customers`
--
ALTER TABLE `customers`
  MODIFY `customer_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT for table `rentals`
--
ALTER TABLE `rentals`
  MODIFY `rental_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `videos`
--
ALTER TABLE `videos`
  MODIFY `video_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `rentals`
--
ALTER TABLE `rentals`
  ADD CONSTRAINT `rentals_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  ADD CONSTRAINT `rentals_ibfk_2` FOREIGN KEY (`video_id`) REFERENCES `videos` (`video_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
