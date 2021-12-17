# Introduction 
EasySave est un logiciel simple d'utililisation qui vous permet de compl�tement controller et configurer vos sauvegardes.

# Bien d�marrer
1.	Installation
	- Ouvrez la solution Visual Studio
	- G�n�rez l'application pour votre OS
	- Maintenant vous pouvez lancer l'executable via le dossier associ�.
2.	Versions les plus r�centes
	- 1.0 : Version uniquement bas�e sur l'invite de commande
	- 2.0 : Version avec interface
	- 3.0 : Version avec interface, multi-tasking, et acc�s � distance
3.	R�f�rences
	- Json.NET (NewtonSoft) [NuGet]
	- CryptoSoft� [Built-in]

# Manuel d'utilisation
1. Processus de copie
	- Double-cliquer sur le preset
	- Choisir votre mode de copie
	- D�marrer la copie

	Notes :
		- Vous pouvez lancer plusieurs jobs de copie � la fois
		- Vous pouvez annuler un job en cours
		- Vous pouvez mettre en pause et reprendre votre job
		- D'autres param�tres sont disponibles, voir 2.

2. Param�tres disponibles
	- Langue : 
		Vous pouvez changer la langue de l'application en cliquant sur le bouton associ�.
		Vous pouvez m�me ajouter votre propre fichier de langues dans le dossier data/lang.
	- Presets :
		Vous pouvez ajouter, modifier et supprimer vos pr�r�glages en cliquant sur le bouton associ�, puis en suivant les �tapes.
		Chaque pr�r�glage a besoin d'un nom, d'un chemin source (ce que vous voulez copier) et d'un chemin de destination (o� vous voulez avoir la copie).
	- Extensions :
		Vous permet de choisir les fichiers d'extension que vous souhaitez chiffrer � l'aide de CryptoSoft�.
	- Application M�tier :
		Vous permet de bloquer la copie si le logiciel/processus s�lectionn� est d�marr�.
	- Stockage : 
		Permet de choisir le format de stockage des logs entre JSON et XML.
	- Priorit� :
		Permet de d�finir un ordre de priorit� pour la copie des fichiers.
	- Taille :
		Vous permet de choisir la taille maximale des fichiers sauvegard�s.

3. Logs
L'application logue les actions du logiciel�:
	- Logs Journaliers : Enregistre toutes les actions effectu�es dans le processus de copie. Vous pouvez retrouver ce genre d'informations dans l'onglet Logs. Les donn�es enregistr�es sont :
		"FileSize"
		"TransferTime"
		"EncryptTime"
		"Name"
		"SourceDir"
		"TargetDir"
	- Logs d'�tat : Enregistre les pr�r�glages et leur �tat pendant le lancement du programme. Celui-ci est utile pour le fonctionnement du logiciel.