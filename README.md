								
Welcome to VanillaLauncher !


The aim of this application is to login automatically on Vanilla and Burning Crusade private servers, and is also compatible with Turtle WoW.
It is helpful if you play on several realms, if you have many accounts, and overall if you are a multiboxer. 


INSTALLATION
------------

There's nothing special to do for the installation. Just unzip all the files of the zip file anywhere on your HDD, and start the application by executing the file VanillaLauncher.exe
All your configuration is stored in a SQLITE database called "VanillaLauncher.db" which should be kept in the application's folder. At first use, if the database doesn't exist, the application will create a blank one.
If a new version of the the application is released, you can backup your database file somewhere and re-use it with the new install, just copy/paste it in application folder. The application will detect that you have an older database file and will upgrade it, all your data will be kept.
You can also copy your database over multiple PC installations.



CONFIGURATION
-------------

The very first thing to do is to locate your game directory (or directories if you both play Vanilla and Burning Crusade). 
Go in the SETTINGS tab and do that now. 

For everything you can configure in the SERVERS, REALMS, ACCOUNTS, CHARACTERS, LINKS and MACROS tabs, the principle stay the same:
On the left side you have a datagrid displaying the related data in your database.
On the right side you can add, modify or delete stuff.
Click on an row in the datagrid to select it, and its contain will appear on the right pane, so you can modify, delete, or duplicate it.
Across all the config you will use the 3 same buttons : ADD, SAVE, DELETE. It's important to understand how they really work:
 * ADD will add a new record in the database. You can also easily duplicate existing stuff with it. Just select a row, change its parameters, and hit ADD.
 * SAVE will update/modify the current selected row.
 * DELETE is meaningfull enough. Note that if you delete something that is linked to it elsewhere, it will also delete that. For instance if you delete a REALM, it will also delete all the ACCOUNTS and CHARACTERS linked to it. There's no way to cancel your deletion so think twice prior to hit a DELETE button.

In order to customize the application properly, you should keep the following logical order in mind :

	VERSION (Vanilla/BC/Turtle) --> SERVER --> REALM --> ACCOUNT --> CHARACTER



SERVERS & REALMS
----------------

The application comes with several popular private servers embedded. A private server serves most often several Realms. 
If the Realm you play on is not listed, you should add it using the SERVERS and REALMS tabs (in that order).
 * For SERVERS, the most important is to properly define the authentication server address (what you normally put in your "realmlist.wtf" file).
 * For REALMS, their names should match exactly those presented to you in game the first time you login into a server.


ACCOUNTS
--------

In the ACCOUNTS tab you add the account(s) name and password for each of the Realms on which you want to Auto-Login.
I strongly advise you to encrypt your passwords to keep them save. It is even more important if you are on a shared computer. To do that, activate this option in the SETTINGS prior to enter your accounts. You will have to choose a KEY that will be your master password for the application. The key will be asked once each time you launch the application and securely stored in memory to temporarily decrypt the passwords when you launch the game or select your accounts in the config. 


CHARACTERS
----------

Last but not least, in order to Auto-Login directly on specific characters, you should add them to your account(s) from the CHARACTERS tab. 
Note that this is not mandatory.
The most important for character Auto-Login to work is to specify the INDEX correctly, ie the position in WoW's character selection screen. The top character in the selection screen list has index 1, the second has index 2, and so on...



LINKS
-----

From the LINKS tab , you have the possibility to create a custom Links menu to quickly open the website(s) of your choice in your default browser.
For links related to your GUILD, check the appropriate checkbox and the those links will appear under the guild menu.


MACROS (Advanced)
-----------------

The macros basically allow you to chain-login with one click, on several accounts and/or characters, and to resize and reposition each WoW window as you want.
Multiboxers will appreciate it.


FAVORITES
---------

On ACCOUNTS, CHARACTERS and MACROS, you have the possibility to tick the "Favorite" checkbox.
All favorite items are duplicated on top root of the Tray menu, and you can quickly display them in the TreeView.



MINIMIZED MODE
--------------

When the application is minimized, you can then interact with it using the Ragnaros icon appearing in the notification area.

* Double-click on Ragnaros will bring back the application in Normal mode. 
* Right-click on Ragnaros shows the Tray Menu which give you a fast access to everything in your configuration. 

The tray menu is highly customizable from the SETTINGS.

When the application is minimized, the monitoring stops. 



PASSWORD SECURITY 
-----------------

I strongly advice you to activate Password encryption in the SETTINGS.

If you don't, your passwords will be saved in clear text in your database and everyone who can access your database can also access your passwords.
I am not responsible if such a situation happens.

The only cons of password encryption are: 
1. You have to choose and memorize a KEY. It can be anything... as usual, the longer, the better.
2. When the application is launched, you'll be asked the KEY . If you don't provide it you won't be able to decrypt your passwords and auto-login.

That's the litlle price to pay to ensure the maximum security for your accounts.

If password encryption is active:
* When you modify an ACCOUNT, passwords are immediately encrypted in the database using Triple DES algorithm (provided by Microsoft)
* The passwords are never being kept decrypted in memory, they are only decrypted when you start WoW or when you select your account in the config.
* Your key is stored in a sanitized area of the memory (SecureString), and only safely accessed when a decryption occurs. 



HOW DOES IT WORKS ?
-----------------

Here is how the autologin works in case you really not understand it !
When you click on your account or character, behind the scenes, the application modifies your WoW config files on-the-fly to :
 1. adapt the authentication server address in the realmlist.wtf and WTF/config.wtf files.
 2. adapt the Realm name in WTF/config.wtf
 3. adapt the windowed mode setting in WTF/config.wtf
 4. resets your last character index in WTF/config.wtf
You don't see this, it takes a nanosecond.
Then it starts the WoW.exe, wait a little bit, and send the following key sequence in the WoW window: 
YOUR_ACCOUNT | TAB | YOUR_PASSWORD | ENTER. 
If you clicked on a character, it waits a little bit again, and then sends a sequence of DOWN keys corresponding to your character index, and ENTER.

You will probably have to play a little bit with the Wait Time parameters in the SETTINGS to find what’s best for your computer, mostly depending on your processor speed and bandwidth.



You now have more time to farm, enjoy :-)


