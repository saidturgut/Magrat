; =========================================
; Minimal 6502 Monitor OS
; Flat binary version
; =========================================

; -----------------------------------------
; ZEROPAGE
; -----------------------------------------
        org $0000
tick:   db 0
ptrlo:  db 0
ptrhi:  db 0
tmp:    db 0

; -----------------------------------------
; CODE START
; -----------------------------------------
        org $8000

reset:
        sei
        cld
        ldx #$ff
        txs

        jsr timer_init
        cli

main:
        jsr print_prompt
        jsr read_char
        jsr handle_command
        jmp main

; -----------------------------------------
; TIMER INIT
; -----------------------------------------
timer_init:
        lda #$00
        sta $FF09

        lda #$00
        sta $FF07
        lda #$20
        sta $FF08

        lda #$01
        sta $FF09
        rts

; -----------------------------------------
; IRQ
; -----------------------------------------
irq:
        inc tick
        lda $FF03
        rti

; -----------------------------------------
; TERMINAL
; -----------------------------------------

; A = char
print_char:
        sta $FF02
        rts

read_char:
wait_rx:
        lda $FF00
        beq wait_rx
        lda $FF01
        rts
        
print_string:
        ldy #0
ps_loop:
        lda (ptrlo),y
        beq ps_done
        jsr print_char
        iny
        bne ps_loop
ps_done:
        rts

print_prompt:
        lda #<prompt
        sta ptrlo
        lda #>prompt
        sta ptrhi
        jmp print_string

prompt:
        db 13,10
        db "MOS> ",0

; -----------------------------------------
; COMMAND HANDLER
; -----------------------------------------
handle_command:
        cmp #'h'
        beq cmd_help
        cmp #'r'
        beq cmd_read
        cmp #'m'
        beq cmd_mem
        rts

cmd_help:
        lda #<help
        sta ptrlo
        lda #>help
        sta ptrhi
        jmp print_string

help:
        db 13,10
        db "h - help",13,10
        db "r - read block 0",13,10
        db "m - dump $0200",13,10
        db 0

; -----------------------------------------
; MEMORY DUMP
; -----------------------------------------
cmd_mem:
        lda #$00
        sta ptrlo
        lda #$02
        sta ptrhi

        ldy #0
dump_loop:
        lda (ptrlo),y
        jsr print_hex
        lda #' '
        jsr print_char
        iny
        cpy #$10
        bne dump_loop

        lda #13
        jsr print_char
        lda #10
        jsr print_char
        rts

; -----------------------------------------
; DISK READ BLOCK 0 -> $0200
; -----------------------------------------
cmd_read:
        lda #$00
        sta $FF0A
        sta $FF0B

        lda #$00
        sta $FF0C
        lda #$02
        sta $FF0D

        lda #$01
        sta $FF0E

wait_disk:
        lda $FF0F
        and #$01
        beq wait_disk

        rts

; -----------------------------------------
; HEX PRINT
; -----------------------------------------
print_hex:
        pha
        lsr
        lsr
        lsr
        lsr
        jsr hex_digit
        pla
        and #$0F
hex_digit:
        cmp #10
        bcc hex_num
        adc #6
hex_num:
        adc #'0'
        jsr print_char
        rts

; -----------------------------------------
; VECTORS
; -----------------------------------------
        org $FFFA
        dw 0
        dw reset
        dw irq