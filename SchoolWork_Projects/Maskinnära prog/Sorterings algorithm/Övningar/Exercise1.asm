	#
	# Maskinnara programmering, program template

	.data
intA: .word 5
intB: .word 10
#intC: .byte 
	.text
main:
	sb $t1, 0($t0)

	lw $t0, intA
	lw $t1, intB
	
	add $t2, $t0, $t1
	
	li $v0, 1
	add $a0, $t2, $zero
	syscall
	
	

	ori	$v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!
