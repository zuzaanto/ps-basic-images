# ps-basic-images
Basic operations on bitmaps: adding, subtracting, averaging etc. in a Powershell script + c# module

To run, first run the psModuleImageEdition.csproj or create a C# class library project with the cs files ("cmdlet.cs" and "ImageFunctions.cs") as source files. Then compile it, and edit the Script.ps1 file to import the compiled dll (you'll find it in Release/bin or Debug/bin).
You can then provide right paths to your bitmaps in the Script, and then execute it.
