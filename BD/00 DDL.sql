-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema agbd5to_DragonBallZ
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `agbd5to_DragonBallZ` ;

-- -----------------------------------------------------
-- Schema agbd5to_DragonBallZ
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `agbd5to_DragonBallZ` DEFAULT CHARACTER SET utf8 ;
USE `agbd5to_DragonBallZ` ;

-- -----------------------------------------------------
-- Table `agbd5to_DragonBallZ`.`Luchador`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `agbd5to_DragonBallZ`.`Luchador` (
  `idLuchador` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45) NOT NULL,
  `poderDePelea` MEDIUMINT UNSIGNED NOT NULL,
  PRIMARY KEY (`idLuchador`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `agbd5to_DragonBallZ`.`Tecnica`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `agbd5to_DragonBallZ`.`Tecnica` (
  `idTecnica` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` VARCHAR(45) NOT NULL,
  `poder` SMALLINT UNSIGNED NOT NULL,
  PRIMARY KEY (`idTecnica`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `agbd5to_DragonBallZ`.`LuchadorTecnica`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `agbd5to_DragonBallZ`.`LuchadorTecnica` (
  `idLuchador` INT UNSIGNED NOT NULL,
  `idTecnica` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`idLuchador`, `idTecnica`),
  INDEX `fk_LuchadorTecnica_Tecnica1_idx` (`idTecnica` ASC),
  CONSTRAINT `fk_LuchadorTecnica_Luchador`
    FOREIGN KEY (`idLuchador`)
    REFERENCES `agbd5to_DragonBallZ`.`Luchador` (`idLuchador`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_LuchadorTecnica_Tecnica1`
    FOREIGN KEY (`idTecnica`)
    REFERENCES `agbd5to_DragonBallZ`.`Tecnica` (`idTecnica`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `agbd5to_DragonBallZ` ;

-- -----------------------------------------------------
-- procedure altaTecnica
-- -----------------------------------------------------

DELIMITER $$
USE `agbd5to_DragonBallZ`$$
CREATE PROCEDURE `altaTecnica` (unNombre VARCHAR(45), unPoder SMALLINT UNSIGNED, OUT idGenerado INT UNSIGNED)
BEGIN
	INSERT INTO Tecnica (`nombre`, `poder`) VALUES (unNombre, unPoder);
    
    SELECT last_insert_id() INTO idGenerado;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure altaLuchador
-- -----------------------------------------------------

DELIMITER $$
USE `agbd5to_DragonBallZ`$$
CREATE PROCEDURE `altaLuchador` (unNombre VARCHAR(45), poderDePelea mediumint UNSIGNED, OUT idGenerado INT UNSIGNED)
BEGIN
	INSERT INTO `Luchador` (`nombre`, `poderDePelea`) VALUES (unNombre, poderDePelea);
    
    SELECT last_insert_id() INTO idGenerado;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- function poderTotal
-- -----------------------------------------------------

DELIMITER $$
USE `agbd5to_DragonBallZ`$$
CREATE FUNCTION `poderTotal` (unIdLuchador int unsigned) RETURNS INT UNSIGNED
BEGIN
	DECLARE retorno INT UNSIGNED;
    
	SELECT	L.poderDePelea + SUM(coalesce(T.poder,0)) INTO retorno
    FROM	Luchador L
    LEFT JOIN LuchadorTecnica LT
		ON L.idLuchador = LT.idLuchador
	LEFT JOIN Tecnica T
		ON LT.idTecnica = T.idTecnica
	WHERE	L.idLuchador = unIdLuchador;
    
    RETURN retorno;
END$$

DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;