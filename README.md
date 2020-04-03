# fileExample
Create a C# WPF program called fileExample.

In this program:
MainWindow - simply three buttons that allow you to open up the other windows

WindowText.xaml - contains a menu and a datagrid (spreadsheet).  The CS file contains code to read/write files in comma separated text files.
When you save a file it is in this format:
Id, Name, Birthday
1,John Doe,116935632000000000
When you open a file it uses that format to set up the spreadsheet.

The above is for ICS3U (and ICS4U).  Below is for ICS4U only (ICS3U, feel free to check it out)

WindowBinary.xaml is set up the same way as WindowText.xaml.  
The difference is it opens and saves files in a binary (i.e. machine readable but not human readable) form.
Once you've save a file in this form, open it up in Notepad to see what I mean.

Window1XML.xaml is set up the same way as WindowText.xaml.
The difference is it opens and saves files in XML form.
