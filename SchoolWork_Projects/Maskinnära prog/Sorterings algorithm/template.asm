	#
	# Maskinnara programmering, program template

	.data

	.text
main:

	ori	$v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!
