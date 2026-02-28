    .ORG 0x0000
        .word   0               ; 000000
        .word   0               ; 000002

        ; 000004  Bus Error
        .word   trap_handler
        .word   0xFFFF

        ; 000010  Illegal Instruction
        .word   trap_handler
        .word   0xFFFF

        ; 000014  BPT
        .word   trap_handler
        .word   0xFFFF

        ; 000020  IOT
        .word   trap_handler
        .word   0xFFFF

        ; 000024  Power Fail
        .word   trap_handler
        .word   0xFFFF

        ; 000030  EMT
        .word   trap_handler
        .word   0xFFFF

        ; 000034  TRAP
        .word   trap_handler
        .word   0xFFFF    
        
        .org 0x1000
    emt 5
    
    .org 0x2000
      trap_handler:
              RTT