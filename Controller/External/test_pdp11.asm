    .ORG 0
        MOV     #0, R0
LOOP:   MOV     R0, 0x0000(R0)
        ADD     #2, R0
        CMP     R0, #4096
        BNE     LOOP
        HALT