function Invoke-displayImage
{
	[cmdletbinding()]
	Param(
	[Parameter(Position=0,Mandatory=$True,HelpMessage="Enter your menu text")]
	[ValidateNotNullOrEmpty()]
	$img
	)
  [void][reflection.assembly]::LoadWithPartialName("System.Windows.Forms")
  
  #$file = (get-item 'C:\Users\szafi\Pictures\22712475_1398848403560677_2400551809602002580_o.jpg')
  ##$file = (get-item "c:\image.jpg")

  #$img = (get-imagesSum -Image1 $file -Image2 $file -operation add)
  #         sleep -seconds 2
  [System.Windows.Forms.Application]::EnableVisualStyles();
  $form = new-object Windows.Forms.Form
  $form.Text = "Result"
  $form.Width = $img.Size.Width;
  $form.Height =  $img.Size.Height;
  $pictureBox = new-object Windows.Forms.PictureBox
  $pictureBox.Width =  $img.Size.Width;
  $pictureBox.Height =  $img.Size.Height;
  
  $pictureBox.Image = $img;
  $form.controls.add($pictureBox)
  $form.Add_Shown( { $form.Activate() } )
  $form.ShowDialog()
}
Function Invoke-Menu {
	[cmdletbinding()]
	Param(
	[Parameter(Position=0,Mandatory=$True,HelpMessage="Enter your menu text")]
	[ValidateNotNullOrEmpty()]
	[string]$Menu,
	[Parameter(Position=1)]
	[ValidateNotNullOrEmpty()]
	[string]$Title = "Menu glowne",
	[Alias("cls")]
	[switch]$ClearScreen
	)
 
	#clear the screen if requested
	if ($ClearScreen) { 
	 Clear-Host 
	}
 
	#build the menu prompt
	$menuprompt = $Title
	#add a return
	$menuprompt+="`n"
	#add an underline
	$menuprompt+="-"*$Title.Length
	#add another return
	$menuprompt+="`n"
	#add the menu
	$menuprompt+=$Menu
 
	Read-Host -Prompt $menuprompt
} 
Import-Module "C:\Users\szafi\Documents\Visual Studio 2015\Projects\psModuleImageEdition\psModuleImageEdition\bin\Debug\psModuleImageEdition.dll"
Clear-Variable file* -Scope Global
$file1 = (get-item 'C:\Users\szafi\Pictures\mononoke1.jpg')
$file2 = (get-item 'C:\Users\szafi\Pictures\mononoke2.jpg')
$menu=@"
1 Dodaj obrazy
2 Odejmij obrazy
3 Pokaz srednia z obrazow
Q Wyjscie
 
Wybierz numer czynnosci lub wpisz Q by wyjsc
"@

Do {
    Switch (Invoke-Menu -menu $Menu -title "Wybierz co chcesz zrobic" -clear) {
     "1" {
		 
		  $img1 = (get-imagesSum -Image1 $file1 -Image2 $file2 -operation add)
				   sleep -seconds 2
		 Invoke-displayImage -img $img1
         sleep -seconds 1
         } 
     "2" {
		  $img1 = (get-imagesSum -Image1 $file1 -Image2 $file2 -operation subtract)
				   sleep -seconds 2
		 Invoke-displayImage -img $img1
         sleep -seconds 1
		 
          }
     "3" {
		$img1 = (get-imagesSum -Image1 $file1 -Image2 $file2 -operation average)
		sleep -seconds 2
		 Invoke-displayImage -img $img1
         sleep -seconds 1
         }
     "Q" {Write-Host "Goodbye" -ForegroundColor Cyan
         Return
         }
     Default {Write-Warning "Invalid Choice. Try again."
              sleep -milliseconds 750}
    } #switch
} While ($True)


