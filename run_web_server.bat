SET "PORT=8000"
SET "EDITOR_VER=2021.1.18f1"

CD /d "C:\Program Files\Unity\Hub\Editor\%EDITOR_VER%\Editor\Data\PlaybackEngines\WebGLSupport\BuildTools"
START SimpleWebServer.EXE D:\UnityProjects\SilkMelody\Build_Web %port%