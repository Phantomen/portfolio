	#
	# Maskinnara programmering, program template

	.data

	.text
main:
	
	#Old and with errors
	#lw   $3,0($4)
   	#addi $2,$2,1
   	#sw   $3,0($5)
   	#addi $4,$4,1
   	#addi $5,$5,1
   	#bne  $3,0,L
   	#nop
   	
   	
   	#Correct one
   	
   	

	ori	$v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!
