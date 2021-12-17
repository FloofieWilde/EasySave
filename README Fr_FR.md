# Introduction 
EasySave est un logiciel simple d'utililisation qui vous permet de complètement controller et configurer vos sauvegardes.

# Bien démarrer
1.	Installation
	- Ouvrez la solution Visual Studio
	- Générez l'application pour votre OS
	- Maintenant vous pouvez lancer l'executable via le dossier associé.
2.	Versions les plus récentes
	- 1.0 : Version uniquement basée sur l'invite de commande
	- 2.0 : Version avec interface
	- 3.0 : Version avec interface, multi-tasking, et accès à distance
3.	Références
	- Json.NET (NewtonSoft) [NuGet]
	- CryptoSoft™ [Built-in]

# Manuel d'utilisation
1. Processus de copie
	- Double-cliquer sur le preset
	- Choisir votre mode de copie
	- Démarrer la copie

	Notes :
		- Vous pouvez lancer plusieurs jobs de copie à la fois
		- Vous pouvez annuler un job en cours
		- Vous pouvez mettre en pause et reprendre votre job
		- D'autres paramètres sont disponibles, voir 2.

2. Paramètres disponibles
	- Langue : 
		Vous pouvez changer la langue de l'application en cliquant sur le bouton associé.
		Vous pouvez même ajouter votre propre fichier de langues dans le dossier data/lang.
	- Presets :
		Vous pouvez ajouter, modifier et supprimer vos préréglages en cliquant sur le bouton associé, puis en suivant les étapes.
		Chaque préréglage a besoin d'un nom, d'un chemin source (ce que vous voulez copier) et d'un chemin de destination (où vous voulez avoir la copie).
	- Extensions :
		Vous permet de choisir les fichiers d'extension que vous souhaitez chiffrer à l'aide de CryptoSoft™.
	- Application Métier :
		Vous permet de bloquer la copie si le logiciel/processus sélectionné est démarré.
	- Stockage : 
		Permet de choisir le format de stockage des logs entre JSON et XML.
	- Priorité :
		Permet de définir un ordre de priorité pour la copie des fichiers.
	- Taille :
		Vous permet de choisir la taille maximale des fichiers sauvegardés.

3. Logs
L'application logue les actions du logiciel :
	- Logs Journaliers : Enregistre toutes les actions effectuées dans le processus de copie. Vous pouvez retrouver ce genre d'informations dans l'onglet Logs. Les données enregistrées sont :
		"FileSize"
		"TransferTime"
		"EncryptTime"
		"Name"
		"SourceDir"
		"TargetDir"
	- Logs d'état : Enregistre les préréglages et leur état pendant le lancement du programme. Celui-ci est utile pour le fonctionnement du logiciel.