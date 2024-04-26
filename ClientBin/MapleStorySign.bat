set /p password="Password: "

signtool sign /f MapleStoryPFX.pfx /fd sha512 /p %password% "%1"

pause
