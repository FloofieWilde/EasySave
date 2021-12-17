# Introduction 
EasySave is an easy-to-use backup manager on which you can totally controll and configure your saves.

# Getting Started
1.	Installation process
	- Open the Visual Studio solution
	- Build the application for you OS
	- Now you can start the excutable file and starting using the named software
2.	Latest releases
	- 1.0 : Only Command line based version
	- 2.0 : Interface based version
	- 3.0 : Interface, multi-tasking and remote version
3.	References
	- Json.NET (NewtonSoft) [NuGet]
	- CryptoSoft™ [Built-in]

# How to use ?
1. Copy process
	- Double-click on the preset
	- Choose your copy mode
	- Start the copy

	Notes :
		- You can launch different works simultaneously
		- You can cancel jobs
		- You can pause and resume jobs
		- Other parameters are available, but will be explained in 2.

2. Available parameters
	- Language : 
		You can change the app's language by clicking one of the buttons.
		You can even add your own language by adding a json file containing all the text values in the folder data/lang.
	- Presets :
		You can add edit and delete your presets by clicking the needed button, then following the steps.
		Each preset needs a name, a source path (what you want to copy) and a destination path (wher you want to have the copy).
	- Extensions :
		Allows you to choose which extension files you wants to encrypt using CryptoSoft™
	- Master Application :
		Allows you to block the copy if the selected software/process is started.
	- Storage : 
		Allows you to choose the storage format of the logs betweet JSON and XML.
	- Priority :
		Allows you to set an order of priority among (us) the copy of files.
	- Size :
		Lets you choose the maximum size of the backed files.

3. Logs
The application is writting files that are logging action from the software :
	- "Daily" Logs : Logs all the actions did in the copying process. You can find this kind of informations in the Logs tab. The logged data is :
		"FileSize"
		"TransferTime"
		"EncryptTime"
		"Name"
		"SourceDir"
		"TargetDir"
	- "State" Logs : Logs the presets and them status while the program is launched. This one is useful for the software funtionnal.