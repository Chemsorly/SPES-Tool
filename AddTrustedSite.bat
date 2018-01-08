REM See http://technet.microsoft.com/en-us/library/cc732643.aspx for Reg Commands/Switches
REG ADD "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains\uni-due.de\paluno\spes-tool" /v "https" /t REG_DWORD /d 00000002
pause