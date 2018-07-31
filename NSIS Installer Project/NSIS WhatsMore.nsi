/**
 * This file is part of WhatsMore <https://github.com/StevenJDH/WhatsMore>.
 * Copyright (C) 2018 Steven Jenkins De Haro.
 *
 * WhatsMore is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * WhatsMore is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with WhatsMore.  If not, see <http://www.gnu.org/licenses/>.
 */

  ;NSIS Modern User Interface
  ;Multilingual Script
  
  !pragma warning error all
  SetCompressor lzma
  Unicode true ;Properly display all languages (Installer will not work on Windows 95, 98 or ME!)
;--------------------------------
;Includes 

  !include "MUI2.nsh" ;Modern UI 2
  !include "FileFunc.nsh"

;--------------------------------
;General

  !define PRODUCT_NAME "WhatsMore"
  !define PRODUCT_VERSION "1.0.0"
  !define PRODUCT_PUBLISHER "Steven Jenkins De Haro"
  !define PRODUCT_WEB_SITE "https://github.com/StevenJDH/WhatsMore"
  !define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  !define PRODUCT_UNINST_ROOT_KEY "HKLM"

  !define CONFIG_DIRECTORY "$APPDATA\WhatsMore"
  !define CONFIG_FILE "${CONFIG_DIRECTORY}\WhatsMoreConfig.json"
  
  !define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-install.ico" ;Installer icon
  !define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-uninstall.ico" ;Uninstaller icon
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "Custom Graphics\Header\150_X_57_computer.bmp" ;recommended size: 150x57 pixels
  !define MUI_HEADERIMAGE_UNBITMAP "Custom Graphics\Header\150_X_57_computer.bmp"
  !define MUI_HEADERIMAGE_RIGHT
  !define MUI_WELCOMEFINISHPAGE_BITMAP "Custom Graphics\Wizard\164_X_314_computer.bmp" ;recommended size: 164x314 pixels
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP "Custom Graphics\Wizard\164_X_314_computer.bmp"
  !define MUI_COMPONENTSPAGE_SMALLDESC ;Puts component description on the bottom.

  ;Name, title bar caption, and file
  Name "${PRODUCT_NAME}"
  Caption "${PRODUCT_NAME} ${PRODUCT_VERSION}" ;Default is used if left empty or removed.
  OutFile "${PRODUCT_NAME} Setup.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\${PRODUCT_NAME}"
  
  ;Get installation folder from registry if available
  InstallDirRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation"

  ;Request application privileges for Windows Vista+
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  
  ;Show all languages, despite user's codepage
  !define MUI_LANGDLL_ALLLANGUAGES

;--------------------------------
;Language Selection Dialog Settings

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
  !define MUI_LANGDLL_REGISTRY_KEY "Software\${PRODUCT_NAME}" 
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "..\LICENSE"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English" ; The first language is the default language
  !insertmacro MUI_LANGUAGE "Spanish"

;--------------------------------
;Reserve Files
  
  ;If you are using solid compression, files that are required before
  ;the actual installation should be stored first in the data block,
  ;because this will make your installer start faster.
  
  !insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections

  LangString COMP_Full ${LANG_ENGLISH} "Full"
  LangString COMP_Minimal ${LANG_ENGLISH} "Minimal"
  LangString COMP_Full ${LANG_SPANISH} "Completo"
  LangString COMP_Minimal ${LANG_SPANISH} "Mínimo"

  LangString COMP_Core ${LANG_ENGLISH} "${PRODUCT_NAME} Core Files (required)"
  LangString COMP_SMShortcut ${LANG_ENGLISH} "Start Menu Shortcut"
  LangString COMP_DShortcut ${LANG_ENGLISH} "Desktop Shortcut"
  LangString COMP_Core ${LANG_SPANISH} "${PRODUCT_NAME} archivos principales (requerido)"
  LangString COMP_SMShortcut ${LANG_SPANISH} "acceso directo del menú de inicio"
  LangString COMP_DShortcut ${LANG_SPANISH} "Acceso directo de escritorio"

  InstType $(COMP_Full)
  InstType $(COMP_Minimal)

Section $(COMP_Core) SectionCore
  SectionIn RO ;Makes the install option required/read-only.
  
  ;Files to install...
  SetOutPath "$INSTDIR\es"
  SetOverwrite try
  File "..\WhatsMore\bin\Release\es\WhatsMore.resources.dll"
  SetOutPath "$INSTDIR"
  File "..\WhatsMore\bin\Release\Newtonsoft.Json.dll"
  File "..\WhatsMore\bin\Release\WhatsMore.exe"
  
SectionEnd

Section $(COMP_SMShortcut) SectionStartMenu
SectionIn 1 2

  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\WhatsMore.exe"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  
SectionEnd

Section $(COMP_DShortcut) SectionDesktop
SectionIn 1

  CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\WhatsMore.exe"
  
SectionEnd

Section -Post

  ;Creates uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe" 

  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\WhatsMore.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "EstimatedSize" "$0"
  ;The following two reg entries are for the Windows' uninstall button so it displays as Uninstall only.
  IntFmt $0 "0x%08X" 1
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" "$0"
  IntFmt $0 "0x%08X" 1
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRepair" "$0"
  
SectionEnd

;--------------------------------
;Installer Functions

Function .onInit

  !insertmacro MUI_LANGDLL_DISPLAY ;This has to come after the language macros
  
FunctionEnd

;--------------------------------
;Descriptions

  LangString DESC_SecCore ${LANG_ENGLISH} "The core files required to use ${PRODUCT_NAME}."
  LangString DESC_SecStartMenu ${LANG_ENGLISH} "Adds an icon to your start menu for easy access."
  LangString DESC_SecDesktop ${LANG_ENGLISH} "Adds an icon to your desktop for easy access."
  LangString DESC_SecCore ${LANG_SPANISH} "Los archivos principales necesarios para usar ${PRODUCT_NAME}."
  LangString DESC_SecStartMenu ${LANG_SPANISH} "Agrega un icono a su menú de inicio para un fácil acceso."
  LangString DESC_SecDesktop ${LANG_SPANISH} "Agrega un icono a su escritorio para un facil acceso."

  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionCore} $(DESC_SecCore)
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionStartMenu} $(DESC_SecStartMenu)
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionDesktop} $(DESC_SecDesktop)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

  LangString MSGQ_DeleteConfig ${LANG_ENGLISH} "Do you want to delete your configuration? Click 'No' if you plan to use this program again."
  LangString MSGQ_DeleteConfig ${LANG_SPANISH} "¿Quieres eliminar tu configuración? Haga clic en 'No' si planea usar este programa nuevamente."
  
Section "Uninstall"

  ;Stuff to uninstall/remove...
  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\WhatsMore.exe"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\es\WhatsMore.resources.dll"

  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk"
  Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk"

  RMDir "$SMPROGRAMS\${PRODUCT_NAME}"
  RMDir "$INSTDIR\es"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "Software\${PRODUCT_NAME}" ;Removes language choice.

  ;This will remove the configuration created by the application.
  MessageBox MB_ICONQUESTION|MB_YESNO $(MSGQ_DeleteConfig) IDYES true IDNO false
  true:
    IfFileExists "${CONFIG_FILE}" 0
      Delete "${CONFIG_FILE}"
    IfFileExists "${CONFIG_DIRECTORY}\*.*" 0
      RMDir "${CONFIG_DIRECTORY}"
  false:

SectionEnd

;--------------------------------
;Uninstaller Functions

Function un.onInit

  !insertmacro MUI_UNGETLANGUAGE
  
FunctionEnd