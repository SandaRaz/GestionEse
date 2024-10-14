CREATE DATABASE Entreprise;
\c Entreprise

CREATE TABLE Entreprise(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30) NOT NULL,
	email varchar(30),
	objet varchar(50),
	siege varchar(30),
	nomChef varchar(50),
	nif varchar(20),
	numStat varchar(20),
	status varchar(20),
	dateCreation date,
	logoFormat varchar(10),
	logoName varchar(100)
);

INSERT INTO Entreprise VALUES('INDEF01','Non existant',null,null,null,null,null,null,null,null,null);
INSERT INTO Entreprise VALUES('ESE7001','Radisson', 'Radisson@gmail.com','Restauration','Antananarivo','','','','','2022-02-14','','');

CREATE TABLE Authentification(
	id serial PRIMARY KEY NOT NULL,
	idEse varchar(7),
	mdp varchar(200)
);
ALTER TABLE Authentification ADD FOREIGN KEY(idEse) REFERENCES Entreprise(id);

INSERT INTO Authentification VALUES(default,'ESE0001','abc');

SELECT * FROM entreprise WHERE id=(SELECT idEse FROM Authentification WHERE idEse='ESE0001' AND mdp='abc');

CREATE TABLE Compte(
	id serial PRIMARY KEY NOT NULL,
	username varchar(40),
	email varchar(30),
	tel varchar(13),
	mdp varchar(200),
	status int
);

CREATE TABLE Dept(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	code varchar(15),
	dateCreation date,
	idEse varchar(7),
	idResponsable varchar(7)
);
ALTER TABLE Dept ADD FOREIGN KEY(idEse) REFERENCES Entreprise(id);
ALTER TABLE Dept DROP CONSTRAINT dept_idresponsable_fkey;

INSERT INTO Dept(id,nom,code,dateCreation,idEse,idResponsable) VALUES('DEP7001','Finance','DFB','2023-11-29','ENT0014',null);
INSERT INTO Dept(id,nom,code,dateCreation,idEse,idResponsable) VALUES('INDEF04','Vide',null,'INDEF01',null);

CREATE TABLE Poste(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	idDept varchar(7),
	hierarchie int
);
ALTER TABLE Poste ADD FOREIGN KEY(idDept) REFERENCES Dept(id);

INSERT INTO Poste(id,nom,iddept,hierarchie) VALUES('POS7001','Directeur Finance','DEP7001',1);
INSERT INTO Poste(id,nom,iddept,hierarchie) VALUES('POS7002','Chef comptable','DEP7001',2);

INSERT INTO Poste(id,nom,iddept,hierarchie) VALUES('INDEF05','Vide','INDEF04',0);

CREATE TABLE Emp(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	prenoms varchar(30),
	dateNaissance date,
	genre varchar(1),
	adresse varchar(50),
	email varchar(30),
	contact varchar(13),
	photoFormat varchar(10),
	photoName varchar(100),
	idPoste varchar(7)
);
ALTER TABLE Emp ADD FOREIGN KEY(idPoste) REFERENCES Poste(id);
ALTER TABLE Dept ADD FOREIGN KEY(idResponsable) REFERENCES Emp(id);

CREATE TABLE Embauche(
	id serial PRIMARY KEY NOT NULL,
	idEmp varchar(7),
	dateEmbauche date,
	salaire decimal(15,2)
);

CREATE TABLE Test(
	id SERIAL PRIMARY KEY NOT NULL,
	numero int,
	poids double precision,
	salaire decimal(15,2),
	nom varchar(20),
	genre char(1),
	naissance date,
	dateheure timestamp,
	verite bool
);

CREATE TABLE Article(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	typeAchat int,
	idese varchar(7)
);
CREATE SEQUENCE article_sequence START 1;

	INSERT INTO Article VALUES('ART7001','Riz',5,'ENT0014');

CREATE TABLE Unite(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(20)
);
CREATE SEQUENCE unite_sequence START 1;
	
	INSERT INTO Unite VALUES('UNI7001','Unite');
	INSERT INTO Unite VALUES('UNI7002','Kilogramme');
	INSERT INTO Unite VALUES('UNI7003','Tonne');

CREATE TABLE UniteEquivalence(
	id varchar(7) PRIMARY KEY NOT NULL,
	idarticle varchar(7),
	idunite varchar(7),
	valeur double precision
);
CREATE SEQUENCE uniteequivalence_sequence START 1;
ALTER TABLE UniteEquivalence ADD FOREIGN KEY(idarticle) REFERENCES article(id);
ALTER TABLE UniteEquivalence ADD FOREIGN KEY(idunite) REFERENCES Unite(id);

	INSERT INTO UniteEquivalence VALUES('UEQ7001','ART7001','UNI7002',1);
	INSERT INTO UniteEquivalence VALUES('UEQ7002','ART7001','UNI7003',1000);

	CREATE OR REPLACE VIEW UniteDetaillee AS
	SELECT ue.id,ue.idarticle,a.nom as article,a.typeAchat,ue.idunite,u.nom as unite,ue.valeur
	FROM UniteEquivalence as ue
	JOIN Article as a ON ue.idarticle = a.id
	JOIN Unite as u ON ue.idunite = u.id;

CREATE TABLE Fournisseur(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	mail varchar(40),
	tel varchar(15),
	responsable varchar(30)
);
CREATE SEQUENCE fournisseur_sequence START 1;

	INSERT INTO Fournisseur VALUES('FOU7001', 'Sanda Raz', '25sandanirina@gmail.com', '', 'Sanda Razanajatovo');
	INSERT INTO Fournisseur VALUES('FOU7002', 'Greg', 'robbygreg25@gmail.com', '', 'Greg');
	INSERT INTO Fournisseur VALUES('FOU9001', 'Jumbo & Score', 'jumboscore@gmail.com', '', '');
	INSERT INTO Fournisseur VALUES('FOU9002', 'Leader Price', 'leaderprice@gmail.com', '', '');
	INSERT INTO Fournisseur VALUES('FOU9003', 'U', 'superu@gmail.com', '', '');


CREATE TABLE Client(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	mail varchar(40),
	tel varchar(15)
);
CREATE SEQUENCE client_sequence START 1;
	
	INSERT INTO Client VALUES('CLI7001', 'Radisson', 'test@gmail.com', '');
	INSERT INTO Client VALUES('CLI7002', 'Sanda Razanajatovo', '25sandanirina@gmail.com', '');
	INSERT INTO Client VALUES('CLI7003', 'Robby Greg', 'robbygreg25@gmail.com', '');

CREATE TABLE Proforma(
	id varchar(7) PRIMARY KEY NOT NULL,
	idarticle varchar(7),
	quantite double precision,
	prixunitaire double precision,
	iduniteequivalence varchar(7),
	dateemission date,
	dateexpiration date,
	etat int,
	idFournisseur varchar(7),
	idClient varchar(7)
);
CREATE SEQUENCE proforma_sequence START 1;
ALTER TABLE Proforma ADD FOREIGN KEY(iduniteequivalence) REFERENCES UniteEquivalence(id);
ALTER TABLE Proforma ADD FOREIGN KEY(idFournisseur) REFERENCES Fournisseur(id);
ALTER TABLE Proforma ADD FOREIGN KEY(idClient) REFERENCES Client(id);

CREATE TABLE BonDeCommande(
	id varchar(7) PRIMARY KEY NOT NULL,
	idProforma varchar(7),
	prixHt double precision,
	tva double precision,
	prixTtc double precision,
	dateCommande date,
	delaiLivraison date,
	etat int
);
CREATE SEQUENCE bondecommande_sequence START 1;

ALTER TABLE BonDeCommande ADD FOREIGN KEY(idProformat) REFERENCES Proforma(id);

CREATE TABLE BonDeLivraison(
	id varchar(7) PRIMARY KEY NOT NULL,
	idBonDeCommande varchar(7),
	quantiteLivree double precision,
	prixTotale double precision,
	descriptions text,
	observations text,
	adresse varchar(40),
	dateLivraison date,
	etat int
);

CREATE TABLE Mouvement(
	id varchar(7) PRIMARY KEY NOT NULL,
	idarticle varchar(7),
	entrer double precision,
	sortie double precision,
	prixunitaire decimal(15,2),
	dates date,
	etatMvt int default 1
);
CREATE SEQUENCE mouvement_sequence START 1;
ALTER TABLE Mouvement ADD FOREIGN KEY(idarticle) REFERENCES Article(id);

CREATE OR REPLACE VIEW mvtdetaillee AS
SELECT m.id,m.idarticle,a.nom as nomarticle,a.typeachat,a.idese,m.entrer,m.sortie,m.prixunitaire,m.dates,m.etatmvt
FROM Mouvement as m
JOIN Article as a
ON m.idarticle = a.id;

CREATE TABLE ProformaMvt(
	id serial PRIMARY KEY NOT NULL,
	idmvt varchar(7),
	idproforma varchar(7)
);
ALTER TABLE ProformaMvt ADD FOREIGN KEY(idmvt) REFERENCES Mouvement(id);
ALTER TABLE ProformaMvt ADD FOREIGN KEY(idproforma) REFERENCES Proforma(id);

CREATE TABLE Mail(
	messageid varchar(512),
	sender varchar(50),
	senderMail varchar(50),
	receiver varchar(50),
	receiverMail varchar(50),
	subject varchar(100),
	content varchar(1000),
	dates date,
	state int
);

SELECT SUM(entrer) AS quantite FROM mvtdetaillee WHERE idese='ENT0014' AND idarticle='ART7001' AND dates>='2023-11-28' AND dates<='2023-11-30' AND entrer>0 AND etatmvt=15;

CREATE TABLE EtatArticle(
	id varchar(7) PRIMARY KEY NOT NULL,
	etat varchar(30),
	valeur int
);
CREATE SEQUENCE etatarticle_sequence START 1;
	INSERT INTO EtatArticle(id,etat,valeur) VALUES('ETA7001', 'Neuf', 10);
	INSERT INTO EtatArticle(id,etat,valeur) VALUES('ETA7002', 'Bonne', 8);
	INSERT INTO EtatArticle(id,etat,valeur) VALUES('ETA7003', 'Utilisable', 5);


CREATE TABLE PvReception(
	id varchar(7) PRIMARY KEY NOT NULL,
	idImmobilier varchar(7),
	idBonDeLivraison varchar(7),
	dateReception date,
	numeroCompta varchar(15),
	marque varchar(30),
	modele varchar(30),
	descriptions text,
	idEtat varchar(7),
	recepteur varchar(50)
);
CREATE SEQUENCE pvreception_sequence START 1;

ALTER TABLE PvReception ADD FOREIGN KEY(idImmobilier) REFERENCES Article(id);
ALTER TABLE PvReception ADD FOREIGN KEY(idBonDeLivraison) REFERENCES BonDeLivraison(id);
ALTER TABLE PvReception ADD FOREIGN KEY(idEtat) REFERENCES EtatArticle(id);

CREATE TABLE PvUtilisation(
	id varchar(7) PRIMARY KEY NOT NULL,
	idPvReception varchar(7),
	dateDebutUtilisation date,
	utilisateurActuel varchar(50),
	idEtatActuel varchar(7)
);
CREATE SEQUENCE pvutilisation_sequence START 1;

ALTER TABLE PvUtilisation ADD FOREIGN KEY(idPvReception) REFERENCES PvReception(id);
ALTER TABLE PvUtilisation ADD FOREIGN KEY(idEtatActuel) REFERENCES EtatArticle(id);

CREATE TABLE InfoAmortissement(
	id varchar(7) PRIMARY KEY NOT NULL,
	idPvReception varchar(7),
	valeurDuBien double precision,
	jourAnnee double precision,
	tauxAmortissementAnnuel double precision,
	tauxDegressif double precision
);
CREATE SEQUENCE infoamortissement_sequence START 1;

ALTER TABLE InfoAmortissement ADD FOREIGN KEY(idPvReception) REFERENCES PvReception(id);

----------- VIEW -------------
CREATE OR REPLACE VIEW ProformaEse AS
SELECT p.*,a.idese FROM Proforma as p JOIN Article as a ON p.idarticle = a.id;

CREATE OR REPLACE VIEW BonDeCommandeEse AS
SELECT b.*,p.idese FROM BonDeCommande as b JOIN Proformaese as p ON b.idproforma = p.id;

CREATE OR REPLACE VIEW BonDeLivraisonEse AS
SELECT b.*,bc.idese FROM BonDeLivraison as b JOIN BonDeCommande as bc ON b.idbondecommande = bc.id;