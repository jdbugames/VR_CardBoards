REM "C:\Program Files\Gource\gource" .git --hide filenames,usernames

"C:\Program Files\Gource\gource" .git -800x600 --user-scale 5 --seconds-per-day 0.5 --hide dirnames,filenames --auto-skip-seconds 0.1 

REM "C:\Program Files\Gource\gource" .git -1280x720 --user-scale 5 --seconds-per-day 1  --hide dirnames,filenames,usernames --disable-progress --highlight-all-users --auto-skip-seconds 0.1 -o - | "D:\Program Files\ffmpeg\bin\ffmpeg" -y -r 60 -f image2pipe -vcodec ppm -i - -vcodec libx264 -preset ultrafast -pix_fmt yuv420p -crf 1 -threads 0 -bf 0 gource.mp4


REM comandos extras
REM --start-date "2017-08-24 21:50"

REM Pagina Web
REM https://github.com/acaudwell/Gource/wiki/Controls
REM https://github.com/acaudwell/Gource