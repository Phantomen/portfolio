	#
	# Maskinnara programmering, program template

	.data
prompt: .asciiz "Enter number: "
newLine: .asciiz "\n"
#multNumber: .word 0

	.text

main:
	addi $t0, $zero, 0

	#Enter number message
	li $v0, 4
	la $a0, prompt
	syscall

	#User input
	li $v0, 5
	syscall
	
	#Store result in $t0
	move $t1, $v0
	
	jal write_loop




write_loop:
	#if t0 is above 12, exit
	bgt $t0, 12, exit
	li $v0, 1
	mult $t0, $t1
	#Gets mult and moves it to s0
	mflo $s0
	#moves the multiplied number to a0
	move $a0, $s0
	#Writes it out
	syscall
	
	#t0 increases by one
	addi $t0, $t0, 1
	
	#new line
	la $a0, newLine
	li $v0, 4
	syscall
	
	#Start loop again
	j write_loop
	
exit: 
	ori	$v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!