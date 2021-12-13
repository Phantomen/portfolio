	#
	# Maskinnara programmering, program template
	# https://www.toptal.com/developers/sorting-algorithms/shell-sort

	.data
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


newLine: .asciiz "\n"

	.text
	
# t0    - iterator
# t1    - datalen - 1
# t2    - data address
# t3    - address pointer
# t4 	- temp address pointer
# t5	- gap
# t6	- gap * 4
# t7    - value A
# t8    - value B
# t9    - temporary storage
# s0    - done flag
# s1	- mult/div



main:
	li $t0, 0           # Iterator
	lw $t1, datalen     # Data length
	addi $t1, $t1, -1
	la $t2, data        # Data address
	li $s0, 0
	li $s0, 0
	
	lw $t5, datalen
	jal gap
	
	j loop

gap:
	li $t9, 2
	div $t5, $t9
	mflo $s1
	move $t5, $s1
	li   $s1, 0
	
	#gap * 4
	addi $t6, $zero, 4
	mult $t6, $t5
	
	mflo $s1
	move $t6, $s1
	li   $s1, 0
	
	jr $ra
	
	
loop:
	
	beq $t0, $t1, end	#if bigger or equal, end
	nop
	
	#sllv $t3, $t0 $t6
	
	sll $t3, $t0, 2          # Multiply iterator by 4
	add $t3, $t3, $t2        # Add iterator to address
	
	move $t4, $t3		#copies so taht the t4 (temp) is t3
	

	
	
	
	lw $t7, 0($t3)           # Fetch value at address
				 
	add $t4, $t3, $t6	 # increase address by gap * 4
	lw $t8, 0($t4)           # Fetch value at address
	
	bgt  $t7, $t8, switch
	nop
continue:
	# Increment and loop
	addi $t0, $t0, 1
	j loop
	

switch:
	#move $t9, $t7
	#move $t7, $t8
	#move $t8, $t9
	
	sw $t7, 0($t4) #Store switched values in RAM
	
				 
	sub $t4, $t4, $t6	 # decrease address by gap * 4 #go back x spaces since t4 was old t4 + 4*gap
	
	sw $t8, 0($t4) 
	
	li $s0, 1
	
	sub $t4, $t4, $t6
	
	bgez $t4, checkBehind
	nop 
	j continue
	
	#bltz $t4, continue
	#nop
checkBehind:		 
	lw $t7, 0($t4)           # Fetch value at address
	bgt $t7, $t8, switch
	nop
	j continue



write:
	beq  $t0, $t1, exit
	nop
	
	li, $v0, 1
	
	sll $t3, $t0, 2          # Multiply iterator by 4
	add $t3, $t3, $t2        # Add iterator to address
	
	lw $t9, 0($t3)           # Fetch value at address
	move $a0, $t9
	syscall
	
	#newline
	la $a0, newLine
	li $v0, 4
	syscall
	
	
	addi $t0, $t0, 1
	
	j write

end:
	bgt $t5, 1, restartloop
	nop
	li $t0, 0
	lw $t1, datalen     # Data length
	j write

	
exit:
	ori $v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!	# Bye!
	
restartloop:
	li $t0, 0
	li $s0, 0
	
	jal gap
	
	j loop
	
	
