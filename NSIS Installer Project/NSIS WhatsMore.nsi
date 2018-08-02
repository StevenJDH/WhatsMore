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
  ;Multilingual (Multi User) Script
  !define INSTALLER_VERSION 1.2

  !pragma warning error all
  SetCompressor lzma
  Unicode true
  
  ;The following defines have to appear before including "MultiUser.nsh" to work.
  !define PRODUCT_NAME "WhatsMore"
  !define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  ;MULTIUSER_INSTALLMODE_DEFAULT_CURRENTUSER ;Sets default to a per-user installation, even if per-machine rights are available.
  !define MULTIUSER_INSTALLMODE_INSTDIR "${PRODUCT_NAME}"
  !define MULTIUSER_INSTALLMODE_INSTDIR_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
  !define MULTIUSER_INSTALLMODE_INSTDIR_REGISTRY_VALUENAME "InstallLocation"
  ;!define MULTIUSER_USE_PROGRAMFILES64
  !define MULTIUSER_EXECUTIONLEVEL Highest
  !define MULTIUSER_MUI
  !define MULTIUSER_INSTALLMODE_COMMANDLINE
  
;--------------------------------
;Includes 

  !include "MultiUser.nsh"
  !include "MUI2.nsh"
  !include "FileFunc.nsh"

;--------------------------------
;General

  !define PRODUCT_VERSION "1.0.0"
  !define MIN_WIN_VER "7"
  !define COMPANY_NAME "Steven Jenkins De Haro"
  !define COPYRIGHT_TEXT "Copyright © 2018 ${COMPANY_NAME}"
  !define PRODUCT_WEB_SITE "https://github.com/StevenJDH/WhatsMore"
  !define PRODUCT_UNINST_ROOT_KEY SHCTX

  !define CONFIG_DIRECTORY "$APPDATA\${PRODUCT_NAME}"
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

  ;Name, title bar caption, file, and branding
  Name "${PRODUCT_NAME}"
  Caption "${PRODUCT_NAME} ${PRODUCT_VERSION}" ;Default is used if left empty or removed.
  OutFile "${PRODUCT_NAME}_Setup.exe"
  BrandingText "${COPYRIGHT_TEXT}"
  
  ;Installer properties
  VIFileVersion "${INSTALLER_VERSION}.0.0" ;Will use VIProductVersion if not defined. Requires x.x.x.x format.
  VIProductVersion "${PRODUCT_VERSION}.0" ;Requires x.x.x.x format.
  VIAddVersionKey ProductName "${PRODUCT_NAME}"
  VIAddVersionKey Comments "This program is being distributed under the terms of the GNU General Public License (GPL)."
  VIAddVersionKey CompanyName "${COMPANY_NAME}"
  VIAddVersionKey LegalCopyright "${COPYRIGHT_TEXT}"
  VIAddVersionKey FileDescription "Batch send messages to multiple WhatsApp users from your computer."
  VIAddVersionKey FileVersion "${INSTALLER_VERSION}"
  VIAddVersionKey ProductVersion ${PRODUCT_VERSION}
  VIAddVersionKey InternalName "${PRODUCT_NAME}"
  VIAddVersionKey LegalTrademarks "${PRODUCT_NAME} and all logos are trademarks of ${COMPANY_NAME}."
  VIAddVersionKey OriginalFilename "${PRODUCT_NAME}.exe"

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
  !insertmacro MULTIUSER_PAGE_INSTALLMODE
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_COMPONENTS
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English" ; The first language is the default language
  !insertmacro MUI_LANGUAGE "Spanish"

  LangString MODE_CurrentUser ${LANG_ENGLISH} "(Current User)"
  LangString MODE_CurrentUser ${LANG_SPANISH} "(Usuario actual)"

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
  LangString COMP_SMShortcut ${LANG_SPANISH} "Acceso directo del menú de inicio"
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
  ${If} $MultiUser.InstallMode == "CurrentUser"
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME} $(MODE_CurrentUser).lnk" "$INSTDIR\WhatsMore.exe"
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall $(MODE_CurrentUser).lnk" "$INSTDIR\Uninstall.exe" "/CurrentUser"
  ${Else}
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\WhatsMore.exe"
    CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "/AllUsers"
  ${EndIf}
  
SectionEnd

Section $(COMP_DShortcut) SectionDesktop
SectionIn 1

  ${If} $MultiUser.InstallMode == "CurrentUser"
    CreateShortCut "$DESKTOP\${PRODUCT_NAME} $(MODE_CurrentUser).lnk" "$INSTDIR\WhatsMore.exe"
  ${Else}
    CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\WhatsMore.exe"
  ${EndIf}
  
SectionEnd

Section -Post

  ;Creates uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe" 

  ${If} $MultiUser.InstallMode == "CurrentUser"
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name) $(MODE_CurrentUser)"
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe /CurrentUser"
  ${Else}
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
    WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe /AllUsers"
  ${EndIf}
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\WhatsMore.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLUpdateInfo" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${COMPANY_NAME}"
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "EstimatedSize" "$0"
  ;The following two reg entries are for the Windows' uninstall button so it displays as Uninstall only.
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" 1
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRepair" 1
  
SectionEnd

;--------------------------------
;Descriptions for Installer

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
;Installer Functions

  LangString MSG_NotCompatible ${LANG_ENGLISH} "This program requires at least Windows ${MIN_WIN_VER}."
  LangString MSG_NotCompatible ${LANG_SPANISH} "Este programa requiere al menos Windows ${MIN_WIN_VER}."
  
Function .onInit

  ${ifnot} ${AtLeastWin${MIN_WIN_VER}}
    MessageBox MB_ICONSTOP $(MSG_NotCompatible) /SD IDOK
    Quit ; will SetErrorLevel 2 - Installation aborted by script
  ${endif}

  !insertmacro MULTIUSER_INIT
  !insertmacro MUI_LANGDLL_DISPLAY ;This has to come after the language macros

FunctionEnd

;--------------------------------
;Uninstaller Section

  LangString UNCOMP_Core ${LANG_ENGLISH} "Uninstall Core Files (required)"
  LangString UNCOMP_RemoveConfig ${LANG_ENGLISH} "Remove Configuration"
  LangString UNCOMP_Core ${LANG_SPANISH} "Desinstalar archivos principales (requerido)"
  LangString UNCOMP_RemoveConfig ${LANG_SPANISH} "Eliminar configuración"

Section "un.$(UNCOMP_Core)" SectionCoreUninstall
  SectionIn RO
  
  ;Stuff to uninstall/remove...
  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\WhatsMore.exe"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\es\WhatsMore.resources.dll"

  ${If} $MultiUser.InstallMode == "CurrentUser"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall $(MODE_CurrentUser).lnk"
    Delete "$DESKTOP\${PRODUCT_NAME} $(MODE_CurrentUser).lnk"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME} $(MODE_CurrentUser).lnk"
  ${Else}
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk"
    Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
    Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk"
  ${EndIf}

  RMDir "$SMPROGRAMS\${PRODUCT_NAME}"
  RMDir "$INSTDIR\es"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "Software\${PRODUCT_NAME}" ;Removes language choice.

SectionEnd

Section /o "un.$(UNCOMP_RemoveConfig)" SectionRemoveConfig

  ;This will remove the configuration created by the application.
  IfFileExists "${CONFIG_FILE}" 0
    Delete "${CONFIG_FILE}"
  IfFileExists "${CONFIG_DIRECTORY}\*.*" 0
    RMDir "${CONFIG_DIRECTORY}"

SectionEnd

;--------------------------------
;Descriptions for Uninstaller

  LangString DESC_SecCoreUninstall ${LANG_ENGLISH} "The core files required by ${PRODUCT_NAME}."
  LangString DESC_SecRemoveConfig ${LANG_ENGLISH} "Leave this unchecked if you plan to use ${PRODUCT_NAME} again."
  LangString DESC_SecCoreUninstall ${LANG_SPANISH} "Los archivos principales requeridos por ${PRODUCT_NAME}."
  LangString DESC_SecRemoveConfig ${LANG_SPANISH} "Deje esto sin marcar si planea usar ${PRODUCT_NAME} de nuevo."

  ;Assign descriptions to sections
  !insertmacro MUI_UNFUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionCoreUninstall} $(DESC_SecCoreUninstall)
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionRemoveConfig} $(DESC_SecRemoveConfig)
  !insertmacro MUI_UNFUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Functions

Function un.onInit

  !insertmacro MULTIUSER_UNINIT
  !insertmacro MUI_UNGETLANGUAGE
  
FunctionEnd