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

	.text
	
# t0    - iterator
# t1    - datalen - 1
# t2    - data address
# t3    - address pointer
# t4	- gap
# t5	- gap * 4
# t6    - value A
# t7    - value B
# t8    - temporary storage
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
	
	jal gap
	
	j loop

gap:
	li $t8, 2
	div $t1, $t8
	mflo $s1
	move $t4, $s1
	li   $s1, 0
	
	#gap * 4
	addi $t5, $zero, 4
	mult $t5, $t4
	
	mflo $s1
	move $t5, $s1
	li   $s1, 0
	
	
	
	jr $ra
	

reset_iterator:
	li $t0, 0            # Iterator
	jr $ra
	
	
loop:
	
	beq $t0, $t1, end	#if bigger or equal, end
	nop
	
	#sllv $t3, $t0 $t5
	
	sll $t3, $t0, 2          # Multiply iterator by 4
	add $t3, $t3, $t2        # Add iterator to address
	

	
	
	
	lw $t6, 0($t3)           # Fetch value at address
				 
	add $t3, $t3, $t6	 # increase address by gap * 4
	lw $t7, 0($t3)           # Fetch value at address
	
	bgt $t5, $t6, switch
continue:
	# Increment and loop
	addi $t0, $t0, 1
	j loop
	

switch:
	move $t8, $t6
	move $t6, $t7
	move $t7, $t8
	
	sw $t7, 0($t3) #Store switched values in RAM
	
				 
	sub $t3, $t3, $t5	 # decrease address by gap * 4 #go back x spaces since t3 is the adress to value b
	
	sw $t6, 0($t3) 
	
	li $s0, 1
	
	sub $t9, $t3, $t5
	
	#bgez
	nop 
	j continue


checkBehind:
	##gap * 4
	#addi $t6, $zero, 4
	#mult $t6, $t4
	
	#mflo $s1
	#move $t6, $s1
	#li   $s1, 0
	
				 
	sub $t3, $t3, $t6	 # decrease address by gap * 4 #go back x spaces
	lw $t6, 0($t3)           # Fetch value at address
	
	bgt $t5, $t6, switch



write:
	
	jr $ra

end:
	beq $s0, 1, restartloop
	nop
	jal write
	ori $v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!	# Bye!
	
restartloop:
	li $t0, 0
	li $s0, 0
	
	jal gap
	
	j loop
	
	
