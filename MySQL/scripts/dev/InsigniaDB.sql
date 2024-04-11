-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema APIServerDev
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema APIServerDev
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `APIServerDev` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `APIServerDev` ;

-- -----------------------------------------------------
-- Table `APIServerDev`.`AnswerBlobs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`AnswerBlobs` (
  `AnswerBlobID` INT(11) NOT NULL,
  `AnswerBlob` BLOB NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AnswerBlobID`),
  UNIQUE INDEX `AnswerBlobID_UNIQUE` (`AnswerBlobID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Answers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Answers` (
  `AnswerID` INT(11) NOT NULL AUTO_INCREMENT,
  `AnswerText` VARCHAR(200) NOT NULL,
  `AnswerValue` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AnswerID`),
  UNIQUE INDEX `AnswerID_UNIQUE` (`AnswerID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`AnswersAnswerBlobs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`AnswersAnswerBlobs` (
  `AnswerID` INT(11) NOT NULL,
  `AnswerBlobID` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AnswerID`, `AnswerBlobID`),
  INDEX `AnswersAnswerBlobs.AnswerBlobID_idx` (`AnswerBlobID` ASC) VISIBLE,
  CONSTRAINT `AnswersAnswerBlobs.AnswerBlobID`
    FOREIGN KEY (`AnswerBlobID`)
    REFERENCES `APIServerDev`.`AnswerBlobs` (`AnswerBlobID`),
  CONSTRAINT `AnswersAnswerBlobs.AnswerID`
    FOREIGN KEY (`AnswerID`)
    REFERENCES `APIServerDev`.`Answers` (`AnswerID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Subjects`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Subjects` (
  `SubjectID` INT NOT NULL AUTO_INCREMENT,
  `SubjectName` VARCHAR(45) NOT NULL,
  `SubjectDescription` VARCHAR(120) NULL DEFAULT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`SubjectID`),
  UNIQUE INDEX `SubjectID_UNIQUE` (`SubjectID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Assignments`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Assignments` (
  `AssignmentID` INT NOT NULL AUTO_INCREMENT,
  `SubjectID` INT NOT NULL,
  `AssignmentName` VARCHAR(45) NOT NULL,
  `AssignmentDescription` VARCHAR(120) NULL DEFAULT NULL,
  `IsPublished` TINYINT(4) NOT NULL DEFAULT '0',
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AssignmentID`),
  UNIQUE INDEX `AssignmentID_UNIQUE` (`AssignmentID` ASC) VISIBLE,
  INDEX `SubjectID_idx` (`SubjectID` ASC) VISIBLE,
  CONSTRAINT `Assignments.SubjectID`
    FOREIGN KEY (`SubjectID`)
    REFERENCES `APIServerDev`.`Subjects` (`SubjectID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionPacks`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionPacks` (
  `QuestionPackID` INT(11) NOT NULL AUTO_INCREMENT,
  `QuestionPackName` VARCHAR(45) NOT NULL,
  `QuestionPackDescription` VARCHAR(120) NULL DEFAULT NULL,
  `IsPublished` TINYINT(4) NOT NULL DEFAULT '0',
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionPackID`),
  UNIQUE INDEX `QuestionPackID_UNIQUE` (`QuestionPackID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`AssignmentQuestionPacks`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`AssignmentQuestionPacks` (
  `AssignmentID` INT(11) NOT NULL,
  `QuestionPackID` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AssignmentID`, `QuestionPackID`),
  INDEX `AssignmentQuestionPacks.QuestionPackID_idx` (`QuestionPackID` ASC) VISIBLE,
  CONSTRAINT `AssignmentQuestionPacks.AssignmentID`
    FOREIGN KEY (`AssignmentID`)
    REFERENCES `APIServerDev`.`Assignments` (`AssignmentID`),
  CONSTRAINT `AssignmentQuestionPacks.QuestionPackID`
    FOREIGN KEY (`QuestionPackID`)
    REFERENCES `APIServerDev`.`QuestionPacks` (`QuestionPackID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`UserType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`UserType` (
  `UserTypeID` INT NOT NULL AUTO_INCREMENT,
  `UserTypeName` VARCHAR(20) NOT NULL,
  `UserTypeDesc` VARCHAR(120) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserTypeID`),
  UNIQUE INDEX `UserTypeID_UNIQUE` (`UserTypeID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Users` (
  `UserID` INT NOT NULL AUTO_INCREMENT,
  `UserTypeID` INT NOT NULL,
  `UserName` VARCHAR(30) NOT NULL,
  `FirstName` VARCHAR(45) NULL DEFAULT NULL,
  `FamilyName` VARCHAR(45) NULL DEFAULT NULL,
  `DateOfBirth` DATE NULL DEFAULT NULL,
  `Email` VARCHAR(320) NULL DEFAULT NULL,
  `PhoneCountryCode` INT(11) NULL DEFAULT NULL,
  `PhoneNumber` INT(11) NULL DEFAULT NULL,
  `RegistrationDate` DATE NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`),
  UNIQUE INDEX `UserID_UNIQUE` (`UserID` ASC) VISIBLE,
  UNIQUE INDEX `UserName_UNIQUE` (`UserName` ASC) VISIBLE,
  INDEX `UserTypeID_idx` (`UserTypeID` ASC) VISIBLE,
  INDEX `UserName_idx` (`UserName` ASC) VISIBLE,
  CONSTRAINT `Users.UserTypeID`
    FOREIGN KEY (`UserTypeID`)
    REFERENCES `APIServerDev`.`UserType` (`UserTypeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`AssignmentRegistrations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`AssignmentRegistrations` (
  `AssignmentRegistrationID` INT(11) NOT NULL AUTO_INCREMENT,
  `UserID` INT(11) NOT NULL,
  `AssignmentID` INT(11) NOT NULL,
  `IsSubmitted` TINYINT(4) NOT NULL DEFAULT '0',
  `RegistrationDateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SubmissionDateTime` DATETIME NULL DEFAULT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AssignmentRegistrationID`),
  UNIQUE INDEX `ResponseID_UNIQUE` (`AssignmentRegistrationID` ASC) VISIBLE,
  INDEX `AssignmentRegistrations.UserID_idx` (`UserID` ASC) VISIBLE,
  INDEX `AssignmentRegistrations.AssignmentID_idx` (`AssignmentID` ASC) VISIBLE,
  CONSTRAINT `AssignmentRegistrations.AssignmentID`
    FOREIGN KEY (`AssignmentID`)
    REFERENCES `APIServerDev`.`Assignments` (`AssignmentID`),
  CONSTRAINT `AssignmentRegistrations.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`AssignmentScores`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`AssignmentScores` (
  `ScoreID` INT NOT NULL AUTO_INCREMENT,
  `UserID` INT NOT NULL,
  `AssignmentID` INT NOT NULL,
  `ScoreDateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Score` INT NOT NULL DEFAULT '0',
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ScoreID`),
  UNIQUE INDEX `ScoreID_UNIQUE` (`ScoreID` ASC) VISIBLE,
  INDEX `AssignmentID_idx` (`AssignmentID` ASC) VISIBLE,
  INDEX `UserID_idx` (`UserID` ASC) VISIBLE,
  CONSTRAINT `AssignmentScores.AssignmentID`
    FOREIGN KEY (`AssignmentID`)
    REFERENCES `APIServerDev`.`Assignments` (`AssignmentID`),
  CONSTRAINT `AssignmentScores.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`AuditRecords`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`AuditRecords` (
  `UserID` INT(11) NOT NULL,
  `ActivityTime` DATETIME NOT NULL,
  `ActivityType` VARCHAR(30) NOT NULL,
  `ActivityDetails` VARCHAR(120) NULL DEFAULT NULL,
  `UpdateDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`, `ActivityTime`),
  CONSTRAINT `AuditRecords.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Classrooms`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Classrooms` (
  `ClassroomID` INT NOT NULL AUTO_INCREMENT,
  `ClassroomName` VARCHAR(45) NOT NULL,
  `ClassroomDescription` VARCHAR(120) NULL DEFAULT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ClassroomID`),
  UNIQUE INDEX `ClassroomID_UNIQUE` (`ClassroomID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`ClassroomSubject`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`ClassroomSubject` (
  `ClassroomID` INT(11) NOT NULL,
  `SubjectID` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ClassroomID`, `SubjectID`),
  INDEX `ClassroomSubject.SubjectID_idx` (`SubjectID` ASC) VISIBLE,
  CONSTRAINT `ClassroomSubject.ClassroomID`
    FOREIGN KEY (`ClassroomID`)
    REFERENCES `APIServerDev`.`Classrooms` (`ClassroomID`),
  CONSTRAINT `ClassroomSubject.SubjectID`
    FOREIGN KEY (`SubjectID`)
    REFERENCES `APIServerDev`.`Subjects` (`SubjectID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Passwords`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Passwords` (
  `UserID` INT(11) NOT NULL,
  `PasswordHash` VARCHAR(100) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`),
  UNIQUE INDEX `UserID_UNIQUE` (`UserID` ASC) VISIBLE,
  CONSTRAINT `Passwords.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Preferences`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Preferences` (
  `UserID` INT NOT NULL,
  `Locale` VARCHAR(15) NULL DEFAULT NULL,
  `UTCOffset` INT(11) NULL DEFAULT NULL,
  `ObserveDaylightSaving` TINYINT(4) NULL DEFAULT '1',
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`),
  UNIQUE INDEX `UserID_UNIQUE` (`UserID` ASC) VISIBLE,
  CONSTRAINT `Preferences.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionType`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionType` (
  `QuestionTypeID` INT(11) NOT NULL AUTO_INCREMENT,
  `QuestionType` VARCHAR(10) NOT NULL,
  `QuestionTypeDescription` VARCHAR(120) NULL DEFAULT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionTypeID`),
  UNIQUE INDEX `QuestionTypeID_UNIQUE` (`QuestionTypeID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`Questions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`Questions` (
  `QuestionID` INT(11) NOT NULL AUTO_INCREMENT,
  `QuestionTitle` VARCHAR(45) NOT NULL,
  `QuestionText` VARCHAR(500) NOT NULL,
  `QuestionTypeID` INT(11) NOT NULL,
  `QuestionValue` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionID`),
  UNIQUE INDEX `QuestionID_UNIQUE` (`QuestionID` ASC) VISIBLE,
  INDEX `Questions.QuestionType_idx` (`QuestionTypeID` ASC) VISIBLE,
  CONSTRAINT `Questions.QuestionTypeID`
    FOREIGN KEY (`QuestionTypeID`)
    REFERENCES `APIServerDev`.`QuestionType` (`QuestionTypeID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionAnswers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionAnswers` (
  `QuestionAnswerID` INT(11) NOT NULL AUTO_INCREMENT,
  `QuestionID` INT(11) NOT NULL,
  `AnswerID` INT(11) NOT NULL,
  `IsCorrect` TINYINT(1) NOT NULL DEFAULT 0,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionAnswerID`),
  UNIQUE INDEX `QuestionAnswerID_UNIQUE` (`QuestionAnswerID` ASC) VISIBLE,
  INDEX `QuestionAnswers.QuestionID_idx` (`QuestionID` ASC) VISIBLE,
  INDEX `QuestionAnswers.AnswerID_idx` (`AnswerID` ASC) VISIBLE,
  CONSTRAINT `QuestionAnswers.AnswerID`
    FOREIGN KEY (`AnswerID`)
    REFERENCES `APIServerDev`.`Answers` (`AnswerID`),
  CONSTRAINT `QuestionAnswers.QuestionID`
    FOREIGN KEY (`QuestionID`)
    REFERENCES `APIServerDev`.`Questions` (`QuestionID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionBlobs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionBlobs` (
  `QuestionBlobID` INT(11) NOT NULL AUTO_INCREMENT,
  `QuestionBlob` BLOB NOT NULL,
  `Tmestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionBlobID`),
  UNIQUE INDEX `QuestionBlobID_UNIQUE` (`QuestionBlobID` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionPackQuestions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionPackQuestions` (
  `QuestionPackID` INT(11) NOT NULL,
  `QuestionID` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionPackID`, `QuestionID`),
  INDEX `QuestionPackQuestions.QuestionID_idx` (`QuestionID` ASC) VISIBLE,
  CONSTRAINT `QuestionPackQuestions.QuestionID`
    FOREIGN KEY (`QuestionID`)
    REFERENCES `APIServerDev`.`Questions` (`QuestionID`),
  CONSTRAINT `QuestionPackQuestions.QuestionPackID`
    FOREIGN KEY (`QuestionPackID`)
    REFERENCES `APIServerDev`.`QuestionPacks` (`QuestionPackID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionPackRegistrations`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionPackRegistrations` (
  `QuestionPackRegistrationID` INT(11) NOT NULL AUTO_INCREMENT,
  `AssignmentRegistrationID` INT(11) NOT NULL,
  `QuestionPackID` INT(11) NOT NULL,
  `IsSubmitted` TINYINT(4) NOT NULL DEFAULT '0',
  `RegistrationDateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `SubmissionDateTime` DATETIME NULL DEFAULT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionPackRegistrationID`),
  UNIQUE INDEX `QuestionPackRegistrationID_UNIQUE` (`QuestionPackRegistrationID` ASC) VISIBLE,
  INDEX `QuestionPackRegistrations.QuestionPackID_idx` (`QuestionPackID` ASC) VISIBLE,
  INDEX `QuestionPackRegistrations.AssignmentRegistrationID_idx` (`AssignmentRegistrationID` ASC) VISIBLE,
  CONSTRAINT `QuestionPackRegistrations.AssignmentRegistrationID`
    FOREIGN KEY (`AssignmentRegistrationID`)
    REFERENCES `APIServerDev`.`AssignmentRegistrations` (`AssignmentRegistrationID`),
  CONSTRAINT `QuestionPackRegistrations.QuestionPackID`
    FOREIGN KEY (`QuestionPackID`)
    REFERENCES `APIServerDev`.`QuestionPacks` (`QuestionPackID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionPackResponses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionPackResponses` (
  `QuestionPackRegistrationID` VARCHAR(45) NOT NULL,
  `QuestionPackID` INT(11) NOT NULL,
  `QuestionID` INT(11) NOT NULL,
  `AnswerID` INT(11) NOT NULL,
  `ResponseDateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionPackRegistrationID`, `QuestionPackID`, `QuestionID`, `AnswerID`),
  INDEX `QuestionPackResponses.AnswerID_idx` (`AnswerID` ASC) VISIBLE,
  INDEX `QuestionPackResponses.QuestionID_idx` (`QuestionID` ASC) VISIBLE,
  INDEX `QuestionPackResponses.QuestionPackID_idx` (`QuestionPackID` ASC) VISIBLE,
  CONSTRAINT `QuestionPackResponses.AnswerID`
    FOREIGN KEY (`AnswerID`)
    REFERENCES `APIServerDev`.`Answers` (`AnswerID`),
  CONSTRAINT `QuestionPackResponses.QuestionID`
    FOREIGN KEY (`QuestionID`)
    REFERENCES `APIServerDev`.`Questions` (`QuestionID`),
  CONSTRAINT `QuestionPackResponses.QuestionPackID`
    FOREIGN KEY (`QuestionPackID`)
    REFERENCES `APIServerDev`.`QuestionPacks` (`QuestionPackID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionPackScores`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionPackScores` (
  `ScoreID` INT(11) NOT NULL AUTO_INCREMENT,
  `UserID` INT(11) NOT NULL,
  `QuestionPackID` INT(11) NOT NULL,
  `ScoreDateTime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Score` INT(11) NOT NULL DEFAULT '0',
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ScoreID`),
  UNIQUE INDEX `ScoreID_UNIQUE` (`ScoreID` ASC) VISIBLE,
  INDEX `QuestionPackScores.UserID_idx` (`UserID` ASC) VISIBLE,
  INDEX `QuestionPackScores.QuestionPackID_idx` (`QuestionPackID` ASC) VISIBLE,
  CONSTRAINT `QuestionPackScores.QuestionPackID`
    FOREIGN KEY (`QuestionPackID`)
    REFERENCES `APIServerDev`.`QuestionPacks` (`QuestionPackID`),
  CONSTRAINT `QuestionPackScores.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`QuestionsQuestionBlobs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`QuestionsQuestionBlobs` (
  `QuestionID` INT(11) NOT NULL,
  `QuestionBlobID` INT(11) NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionID`, `QuestionBlobID`),
  INDEX `QuestionsQuestionBlobs.QuestionBlobID_idx` (`QuestionBlobID` ASC) VISIBLE,
  CONSTRAINT `QuestionsQuestionBlobs.QuestionBlobID`
    FOREIGN KEY (`QuestionBlobID`)
    REFERENCES `APIServerDev`.`QuestionBlobs` (`QuestionBlobID`),
  CONSTRAINT `QuestionsQuestionBlobs.QuestionID`
    FOREIGN KEY (`QuestionID`)
    REFERENCES `APIServerDev`.`Questions` (`QuestionID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`SubjectTeacher`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`SubjectTeacher` (
  `UserID` INT NOT NULL,
  `SubjectID` INT NOT NULL,
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`, `SubjectID`),
  INDEX `SubjectID_idx` (`SubjectID` ASC) VISIBLE,
  CONSTRAINT `SubjectTeacher.SubjectID`
    FOREIGN KEY (`SubjectID`)
    REFERENCES `APIServerDev`.`Subjects` (`SubjectID`),
  CONSTRAINT `SubjectTeacher.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `APIServerDev`.`UserLogon`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `APIServerDev`.`UserLogon` (
  `UserID` INT(11) NOT NULL,
  `LastLogonDate` DATETIME NOT NULL,
  `LogonAttempts` INT(11) NOT NULL,
  `IsLocked` TINYINT(4) NOT NULL DEFAULT '0',
  `Timestamp` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`),
  UNIQUE INDEX `UserID_UNIQUE` (`UserID` ASC) VISIBLE,
  CONSTRAINT `UserLogon.UserID`
    FOREIGN KEY (`UserID`)
    REFERENCES `APIServerDev`.`Users` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
