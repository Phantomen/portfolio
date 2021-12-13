
	.data	#Insert datalen and data under
	


newLine: .asciiz "\n"

	.text
	
# t0    - iterator
# t1    - datalen - 1
# t2    - data address
# t3    - address pointer	(end)
# t4 	- temp address pointer
# t5	- gap
# t6	- gap * 4
# t7    - value A
# t8    - value B
# t9    - temporary storage
# s0	- mult/div



main:
	li $t0, -1           	# Iterator
	lw $t1, datalen     	# Data length
	addi $t1, $t1, -1	#Data length -1
	la $t2, data        	# Data address
	li $s0, 0
	li $s0, 0
	
	lw $t5, datalen

gap:
	#gap = (gap / 2)
	li $t9, 2
	div $t5, $t9
	mflo $s1
	move $t5, $s1
	li   $s1, 0
	
	#gap * 4
	li $t6, 4
	mult $t6, $t5
	mflo $s1
	move $t6, $s1
	li   $s1, 0
	
continue:
	addi $t0, $t0, 1 	#Increase iteraot by one

loop:
	sub $t9, $t1, $t5
	bgt $t0, $t9, end	#If iterator is bigger than the length - gap, end
	nop
	
	
	sll $t3, $t0, 2          # Multiply iterator by 4
	add $t3, $t3, $t2        # Add iterator to address
	
	move $t4, $t3		#copies so that the t4 (temp) is t3
	
	
	lw $t7, 0($t3)           # Fetch value at address
				 
	add $t4, $t3, $t6	 # increase address by gap * 4
	lw $t8, 0($t4)           # Fetch value at address
	
	ble  $t7, $t8, continue	#if t7 is less than t8, start loop again and increase iterator by one
	nop

switch:
	sw $t7, 0($t4) 			#Store bigger value in RAM
				 
	sub $t4, $t4, $t6		#Degrease t4 to get the place that  
	
	sw $t8, 0($t4) 			#Store switched values in RAM
	
	sub $t4, $t4, $t6	 	# decrease address by gap * 4 #go back x spaces since t4 was old t4 + 4*gap
	bge  $t4, $t2, checkBehind	#If the adress behind is not lower than the data address (same as if(t4 -t2 > 0)) check the one behind
	nop 
	j continue
	nop
	
checkBehind:
	
	lw $t7, 0($t4)           # Fetch value at address
	add $t4, $t4, $t6	 # go one up since in switch, t4 is the place of t8
	bgt $t7, $t8, switch
	nop
	j continue
	nop


end:
	bgt $t5, 0, restartloop	#If gap is bigger than 0, restart loop
	nop
	li $t0, 0	#Sets iterator to 0
	
write:
	bgt  $t0, $t1, exit	#If iterator is bigger than (list length -1), exit
	nop
	
	#Write out number in list
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
	
	
	addi $t0, $t0, 1	#increase iterator by one
	
	j write
	nop

	
exit:
	ori $v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!	# Bye!
	
restartloop:
	li $t0, -1
	j gap
	nop
	
	
