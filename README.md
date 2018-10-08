# **CodinGame example solutions**

These are quick examples of ways to solve the codinGame challenges. They are mostly naive implementations and can definitely be improved.

## _Getting Started_

The solution has individual files for each challenge. They are each in their own namespaces but Main has been renamed to avoid a conflict

## _Prerequisites_

No IDE or extra software is required to code at www.codinGame.com


## _Installing_

Extract the file and launch the solution inside CodinGameSolutions

## **Enjoy!**

twitch.tv/engihero for CodinGame livestreams




# **Sharp Namespace Blobber! [FOR C#]**

CodinGame only allows you to submit 1 file and it can be hard to manage all that code in the same file. The blobber is meant to take all the files containing certain namespaces and merging it into 1.


## _How it works_

The blobber will parse all ".cs" files in subdirectories as long as the directory has not been flagged as ignored. If a file contains a namespace that has not been flagged for blob, the entire file will be ignored. The blobber only pastes entire code pages. A file named blob.cs will be generated next to your solution.

## _Setup_

Extract SharpNamespaceBlobber.exe from SharpNamespaceBlobber.rar and put it in the same directory as your solution (.sln).
Add launch arguments to specify namespaces to include and folders to ignore.
Launch the executable.

## _Arguments_

### _Adding Namespaces_

Namespaces are added as launch options. **Example:**
    SharpNamespaceBlobber.exe CodinGame Utility
*This will only blob files containing the CodinGame and/or Utility namespaces. If no namespaces are given the program will terminate and no blob.cs file will be generated.*

### _Adding additional Ignore directories_

Additional ignore directories are also specified as launch options but the Directory must use the xignore prefix. **Example:**
    SharpNamespaceBlobber.exe xignoreMyDirectory xignoreMyOtherDirectory
*This will ignore MyDirectory and MyOtherDirectory when looking for .cs files*

### _Combining arguments_

Namespaces and ignore files can be added together as launch options. **Example:**

SharpNamespaceBlobber.exe CodinGame xignoreMyDirectory Utility xignoreMyOtherDirectory
*This will add CodinGame and Utility as valid namespaces to parse, and will exclude MyDirectory and MyOtherDirectory from the .cs file search*

### _Defaults_

By default, the SharpNamespaceBlobber will ignore **bin**, **obj** and **Properties**. These **do not** need to be specified in the launch options.



