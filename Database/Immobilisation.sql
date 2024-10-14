CREATE TABLE CategorieImmo(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(50),
	label varchar(50)
);
CREATE SEQUENCE categorieimmo_sequence START 1;

INSERT INTO CategorieImmo(id,nom,label) VALUES('CTI7001', 'Construction', 'Construction');
INSERT INTO CategorieImmo(id,nom,label) VALUES('CTI7002', 'MaterielTransport', 'Materiel de transport');
INSERT INTO CategorieImmo(id,nom,label) VALUES('CTI7003', 'MaterielInformatique', 'Materiel informatiques');
INSERT INTO CategorieImmo(id,nom,label) VALUES('CTI7004', 'MaterielBureautique', 'Materiel bureautiques');
INSERT INTO CategorieImmo(id,nom,label) VALUES('CTI7005', 'Autres', 'Autres immobilisations');

CREATE TABLE Immobilier(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(50),
	idCatImmo varchar(7),
	idEse varchar(7)
);
CREATE SEQUENCE immobilier_sequence START 1;


ALTER TABLE Immobilier ADD FOREIGN KEY(idCatImmo) REFERENCES CategorieImmo(id);
ALTER TABLE Immobilier ADD FOREIGN KEY(idEse) REFERENCES Entreprise(id);

ALTER TABLE UniteEquivalence ADD FOREIGN KEY(idImmobilier) REFERENCES Immobilier(id);
ALTER TABLE UniteEquivalence ADD FOREIGN KEY(idunite) REFERENCES Unite(id);

	INSERT INTO UniteEquivalence VALUES('UEQ7001','ART7001','UNI7002',1);
	INSERT INTO UniteEquivalence VALUES('UEQ7002','ART7001','UNI7003',1000);

	CREATE OR REPLACE VIEW UniteDetaillee AS
	SELECT ue.id,ue.idImmobilier,i.nom as immobilier,i.idCatImmo,ue.idunite,u.nom as unite,ue.valeur
	FROM UniteEquivalence as ue
	JOIN Immobilier as i ON ue.idImmobilier = i.id
	JOIN Unite as u ON ue.idunite = u.id;

CREATE TABLE Fournisseur(
	id varchar(7) PRIMARY KEY NOT NULL,
	nom varchar(30),
	mail varchar(40),
	tel varchar(15),
	responsable varchar(30)
);
CREATE SEQUENCE fournisseur_sequence START 1;

	INSERT INTO Fournisseur VALUES('FOU7000', '', '', '', '');
	INSERT INTO Fournisseur VALUES('FOU7001', 'Sanda Raz', '25sandanirina@gmail.com', '', 'Sanda Razanajatovo');
	INSERT INTO Fournisseur VALUES('FOU7002', 'Greg', 'robbygreg25@gmail.com', '', 'Greg');
	INSERT INTO Fournisseur VALUES('FOU9001', 'Jumbo & Score', 'jumboscore@gmail.com', '', '');
	INSERT INTO Fournisseur VALUES('FOU9002', 'Leader Price', 'leaderprice@gmail.com', '', '');


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

CREATE TABLE EtatImmobilier(
	id varchar(7) PRIMARY KEY NOT NULL,
	etat varchar(30),
	valeur int
);
CREATE SEQUENCE etatimmobilier_sequence START 1;
	INSERT INTO EtatImmobilier(id,etat,valeur) VALUES('ETA7001', 'Neuf', 10);
	INSERT INTO EtatImmobilier(id,etat,valeur) VALUES('ETA7002', 'Bonne', 8);
	INSERT INTO EtatImmobilier(id,etat,valeur) VALUES('ETA7003', 'Utilisable', 5);
	INSERT INTO EtatImmobilier(id,etat,valeur) VALUES('ETA7004', 'Mauvais', 3);

CREATE TABLE ProformaImmo(
	id varchar(7) PRIMARY KEY NOT NULL,
	idImmobilier varchar(7),
	quantite double precision,
	prix double precision,
	dateemission date,
	dateexpiration date,
	idEtat varchar(7),
	idFournisseur varchar(7),
	fournisseur varchar(50),
	reference varchar(50),
	statut int
);
CREATE SEQUENCE proformaimmo_sequence START 1;

ALTER TABLE ProformaImmo ADD FOREIGN KEY(idImmobilier) REFERENCES Immobilier(id);
ALTER TABLE ProformaImmo ADD FOREIGN KEY(idEtat) REFERENCES EtatImmobilier(id);
ALTER TABLE ProformaImmo ADD FOREIGN KEY(idFournisseur) REFERENCES Fournisseur(id);

	CREATE OR REPLACE VIEW v_ProformaImmo AS 
	SELECT 
		p.id,
		p.idImmobilier,
		i.idCatImmo,
		p.quantite,
		p.prix,
		p.dateemission,
		p.dateexpiration,
		p.idEtat,
		p.idFournisseur,
		p.fournisseur,
		p.reference,
		p.statut
	FROM Immobilier as i
	JOIN ProformaImmo as p
	ON p.idImmobilier = i.id;

CREATE TABLE BonDeCommande(
	id varchar(7) PRIMARY KEY NOT NULL,
	idProforma varchar(7),
	prixHt double precision,
	tva double precision,
	prixTtc double precision,
	dateCommande date,
	delaiLivraison date,
	statut int
);
CREATE SEQUENCE bondecommande_sequence START 1;

ALTER TABLE BonDeCommande ADD FOREIGN KEY(idProforma) REFERENCES ProformaImmo(id);

	CREATE OR REPLACE VIEW v_BonDeCommande AS
	SELECT
		b.id,
		vp.idImmobilier,
		vp.idCatImmo,
		b.idProforma,
		b.prixHt,
		b.tva,
		b.prixTtc,
		b.dateCommande,
		b.delaiLivraison,
		b.statut
	FROM v_ProformaImmo as vp
	JOIN BonDeCommande as b
	ON b.idProforma = vp.id;

CREATE TABLE BonDeLivraison(
	id varchar(7) PRIMARY KEY NOT NULL,
	idBonDeCommande varchar(7),
	quantiteLivree double precision,
	prix double precision,
	frais double precision,
	descriptions text,
	observations text,
	adresse varchar(40),
	livreur varchar(40),
	contactLivreur varchar(13),
	dateLivraison date,
	statut int
);
CREATE SEQUENCE bondelivraison_sequence START 1;

ALTER TABLE BonDeLivraison ADD FOREIGN KEY(idBonDeCommande) REFERENCES BonDeCommande(id);

	CREATE OR REPLACE VIEW v_BonDeLivraison AS
	SELECT 
		b.id,
		b.idBonDeCommande,
		bc.idImmobilier,
		bc.idCatImmo,
		b.quantiteLivree,
		b.prix,
		b.frais,
		b.descriptions,
		b.observations,
		b.adresse,
		b.livreur,
		b.contactLivreur,
		b.dateLivraison,
		b.statut
	FROM v_BonDeCommande as bc
	JOIN BonDeLivraison as b
	ON b.idBonDeCommande = bc.id;

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
	recepteur varchar(50),
	statut int
);
CREATE SEQUENCE pvreception_sequence START 1;

ALTER TABLE PvReception ADD FOREIGN KEY(idImmobilier) REFERENCES Immobilier(id);
ALTER TABLE PvReception ADD FOREIGN KEY(idBonDeLivraison) REFERENCES BonDeLivraison(id);
ALTER TABLE PvReception ADD FOREIGN KEY(idEtat) REFERENCES EtatImmobilier(id);

	CREATE OR REPLACE VIEW v_PvReception AS 
	SELECT 
		p.id,
		p.idImmobilier,
		i.idCatImmo,
		p.idBonDeLivraison,
		p.dateReception,
		p.numeroCompta,
		p.marque,
		p.modele,
		p.descriptions,
		p.idEtat,
		p.recepteur,
		p.statut
	FROM Immobilier as i
	JOIN PvReception as p
	ON p.idImmobilier = i.id;

CREATE TABLE PvUtilisation(
	id varchar(7) PRIMARY KEY NOT NULL,
	idPvReception varchar(7),
	dateDebutUtilisation date,
	utilisateurActuel varchar(50),
	idEtatActuel varchar(7)
);
CREATE SEQUENCE pvutilisation_sequence START 1;

ALTER TABLE PvUtilisation ADD FOREIGN KEY(idPvReception) REFERENCES PvReception(id);
ALTER TABLE PvUtilisation ADD FOREIGN KEY(idEtatActuel) REFERENCES EtatImmobilier(id);

	CREATE OR REPLACE VIEW v_PvUtilisation AS
	SELECT 
		pu.id,
		pu.idPvReception,
		pu.dateDebutUtilisation,
		pu.utilisateurActuel,
		pu.idEtatActuel,
		e.etat
	FROM EtatImmobilier as e
	JOIN PvUtilisation as pu
	ON pu.idEtatActuel = e.id;


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

	CREATE OR REPLACE VIEW DetailImmobilier AS
	SELECT 
		pr.id,
		pr.idImmobilier,
		i.nom as immobilier,
		i.idCatImmo,
		pi.reference,
		pi.fournisseur,
		bl.prix,
		pr.dateReception,
		pr.numeroCompta,
		pr.marque,
		pr.modele,
		pr.descriptions,
		pr.idEtat,
		e.etat,
		pr.recepteur,
		pr.statut
	FROM Immobilier AS i
	JOIN PvReception as pr
	ON pr.idImmobilier = i.id
	JOIN EtatImmobilier as e
	ON pr.idEtat = e.id
	JOIN BonDeLivraison as bl
	ON pr.idBonDeLivraison = bl.id
	JOIN BonDeCommande as bc
	ON bl.idBonDeCommande = bc.id
	JOIN ProformaImmo as pi
	ON bc.idProforma = pi.id;


SELECT dateDebutUtilisation FROM Pvutilisation WHERE idpvreception='PVR0005' ORDER BY dateDebutUtilisation DESC, id DESC LIMIT 1