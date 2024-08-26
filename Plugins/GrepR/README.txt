GrepR - Find string/regexp in project folder recursively
===
Created by 2nek

Please read Official Site's ReadMe(Manual) - https://github.com/gadget114514/UnityGrepRPlugin

UniRx is available on the Unity Asset Store

Support thread on the Unity Forums: Ask me any question - http://forum.unity3d.com/threads/248535-UniRx-Reactive-Extensions-for-Unity
Release Notes, see [UniRx/releases](https://github.com/neuecc/UniRx/releases)

Author Info
---
2nek: Individual Game developper
X: @2nekcom

License
---
This tool is under MIT license.



Description
---
This tool searches string/regexp in a project folder recursively.

This package is simple tool to find string pattern from text/source files in project folder.
Install
This package is installed into "Plugin" folder
Getting started
After installed, you can use this tool from [Tool/GrepR] menu.
Click the menu, then GrepR window will open. You can drag the window into where you like.

Input string pattern
Input string pattern which you want to find, then click find button.
The tool searches the string in the specified folder recursively.

How to use the result
If the tool finds the pattern in files, the result is shown in the window.
The result line consists like "file":line number:the string of the line.
You can click the result line, if you use Super editor or Script Inspector 3,  the file will open and cursor goes to the line.

Use case 
- Annotation comment
You might remain "TODO" string as comment. Such annotation comments are useful.
Some plugins of VSCode treat well.
This tool can be used as such annotation comment tool.  Search "TODO" by this tool,
TODO items are listed in the window. So by clicking each line, you can open the file and 
go to the line. 

- Fixing names globally in broken code
Type based search and Caller/Callee based seach are very powerful. But in some
cases, they do not work well. This tool search a string from sources to fix it. 




Future
- Adding replace (query and non query)
- Multi folders



Visit github
https://github.com/gadget114514/UnityGrepRPlugin


≈
