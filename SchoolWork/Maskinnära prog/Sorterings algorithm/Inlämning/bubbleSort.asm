.data
newLine: .asciiz "\n"
datalen: 
    .word 16
data:
    .word    0xffff7e81
    .word    0x00000001
    .word    0x00000002
    .word    0xffff0001
    .word    0x00000000
    .word    0x00000001
    .word    0xffffffff
    .word    0x00000000
    .word    0xe3456687
    .word    0xa001aa88
    .word    0xf0e159ea
    .word    0x9152137b
    .word    0xaab385a1
    .word    0x31093c54
    .word    0x42102f37
    .word    0x00ee655b
    
    .text

# t0    - iterator
# t1    - datalen - 1
# t2    - data address
# t3    - address pointer
# t4    - value A
# t5    - value B
# t6    - temporary storage
# t7    - temporary storage
# s0    - done flag

main:
    li         $t0, 0            # Iterator
    lw         $t1, datalen     # Data length
    addi     $t1, $t1, -1
    la         $t2, data        # Data address
    li         $s0, 0
    
loop:
    beq $t0, $t1, end
    
    # Get value at iterator pos
    sll $t3, $t0, 2            # Multiply iterator by 4
    add $t3, $t3, $t2        # Add iterator to address
    
    lw $t4, 0($t3)            # Fetch value at address
    lw $t5, 4($t3)            # -||-

    bgt $t4, $t5, switch     # Switch values if wrong order
cont1:
    # Increment and loop
    addi $t0, $t0, 1
    j loop

write:
	beq  $t0, $t1, exit
	nop
	
	li, $v0, 1
	
	sll $t3, $t0, 2          # Multiply iterator by 4
	add $t3, $t3, $t2        # Add iterator to address
	
	lw $t9, 0($t3)           # Fetch value at address
	move $a0, $t9
	syscall
	
	
	la $a0, newLine
	li $v0, 4
	syscall
	
	
	addi $t0, $t0, 1
	
	j write    
    
end:
    beq $s0, 1, restartLoop # Restart iteration if list might still be unsorted
    nop
    li $t0, 0
    
    lw $t1, datalen     # Data length
    
    j write

exit:
	ori $v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall		

restartLoop:
    li $t0, 0 # Reset iterator
    li $s0, 0 # Reset flag
    addi     $t1, $t1, -1 # Limit top
    j loop
    
switch:
    move $t6, $t4
    move $t4, $t5
    move $t5, $t6
    
    sw $t4, 0($t3) # Store switched values in RAM
    sw $t5, 4($t3)
    
    li $s0, 1
    
    j cont1
