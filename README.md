## Text Based Adventure Engine

A Simple text based adventure system that parses json files.
Written in under an hour. Code may be questionable.

#### Design:

Each file is primarily composed of **sections**.

Each section is essentially a node in the adventure.

an example section:
```
{
      "sectionID":"_start",
      "titletext":"Your stand in an open field, with the ability to walk freely in all directions.",
      "options":["Go North", "Go South", "Go East", "Go West"],
      "optionSectionLinks":["_start-n","_start-s", "_start-e", "_start-w"]
}
```  

the sectionID is the internal indentifier of your section.

the titleText is what is displayed when the section is loaded.

the options list is the available navagation routes from the section.

the optionSectionLinks list is the corresponding ID for each option.

Some section IDs are reserved.

- `_start` is used to mark the entry point of the file

- `_exit` causes the program to exit when loaded.

You can see an example adventure in `TextBasedAdventureEngine/testAdventure.json`.
