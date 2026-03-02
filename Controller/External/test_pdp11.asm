    .ORG 0x0000
           mov     #3,r4
   loop:   nop
           nop
           nop
           .word   077403      ; offset = 3
           halt