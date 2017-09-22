; Simple Color File Setup Program

;--------------------------------------

!include MUI.nsh ;Include Modern UI
!include WordFunc.nsh
!insertmacro VersionCompare
!include LogicLib.nsh
;--------------------------------------

;General

  RequestExecutionLevel admin

  ;The name of the installer
  Name "Simple Color File"

  ;The setup executable file to create
  OutFile "output\simplecolorfile-install.exe"

  ;The default installation directory
  InstallDir "$PROGRAMFILES64\Appliberated\Simple Color File"

;--------------------------------------

;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------------

;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "Files\License.txt"
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES

  !define MUI_FINISHPAGE_RUN SimpleColorFile.exe
  !define MUI_FINISHPAGE_RUN_TEXT "Start Simple Color File"
  !insertmacro MUI_PAGE_FINISH

  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------------

;Languages

  !insertmacro MUI_LANGUAGE "English"

;--------------------------------------

;Version Information

  VIProductVersion "1.0.0.3"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "Simple Color File"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "https://appliberated.com/simplecolorfile/"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Appliberated"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks" ""
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright © 2017 Appliberated (https://appliberated.com)"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "Simple Color File Installer"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "1.0.0.3"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" "1.0.0.3"

;--------------------------------------

Section
  ; Copy files to installation directory
  SetOutPath $INSTDIR
  File Files\SimpleColorFile.exe
  File Files\SimpleColorFile.exe.config
  File Files\License.txt
  File "Files\SimpleColorFileLink.html"

  ; Create Start Menu shortcuts
  CreateShortCut "$SMPROGRAMS\Simple Color File.lnk" "$INSTDIR\SimpleColorFile.exe" "" "" "" SW_SHOWNORMAL "" ""

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\uninstall.exe"
  SetRegView 64
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "DisplayName" "Simple Color File"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S" 
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "DisplayVersion" "1.0.0.3"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "DisplayIcon" "$\"$INSTDIR\SimpleColorFile.exe$\""
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "EstimatedSize" "103"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "HelpLink" "https://appliberated.com/simplecolorfile/"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "InstallLocation" "$INSTDIR"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "NoModify" "1"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "NoRepair" "1"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "Publisher" "Appliberated"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "URLInfoAbout" "https://appliberated.com/simplecolorfile/"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "URLUpdateInfo" "https://appliberated.com/simplecolorfile/"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "VersionMajor" "1"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File" "VersionMinor" "0"
SectionEnd

;--------------------------------------

;Uninstaller Section

Section "Uninstall"

  ; Delete files from installation directory
  Delete "$INSTDIR\uninstall.exe"
  Delete "$INSTDIR\SimpleColorFile.exe"
  Delete "$INSTDIR\SimpleColorFile.exe.config"
  Delete "$INSTDIR\License.txt"
  Delete "$INSTDIR\SimpleColorFileLink.html"
  RMDir "$INSTDIR"
  RMDir "$PROGRAMFILES64\Appliberated"

  ; Delete Start Menu shortcut
  Delete "$SMPROGRAMS\Simple Color File.lnk"

  ; Delete Uninstall registry key
  SetRegView 64
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Simple Color File"

SectionEnd