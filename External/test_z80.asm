        org 0000h
 LD A,00h
 OR A          ; Z=1
 
 LD HL,0001h
 LD DE,0001h
 ADD HL,DE
 halt