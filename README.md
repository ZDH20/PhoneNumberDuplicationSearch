# PhoneNumberDuplicationSearch
Stores text from an external file into a list. The program will look for duplicates and create a new file without them.

Code is in the Program.cs file.

When the program runs, it will prompt the user to enter a path and the name of the file.
It will get the file and store every line of the file to a list (I made this specifically for phone numbers, but should work with anything).
It will then iterate through the list to find duplicates.
A prompt will show up asking the user if they would like to print a new file without duplicates.
Once the program is finished, it will print the results. Total amount of entries, number of duplicates, total amount of entries of the new file.

Not yet implemented correctly:
  After the new file has been printed, it will prompt the user if they would like to scan the new file (if for some reason duplicates are still there).
