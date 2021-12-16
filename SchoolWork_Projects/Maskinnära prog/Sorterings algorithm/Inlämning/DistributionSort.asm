	#
	# Maskinnara programmering, program template

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
	
## t0    - iterator
## t1    - datalen - 1
## t2    - data address
## t3    - address pointer
## t4    - value A
## t5    - value B
## t6    - temporary storage
## t7    - temporary storage
## s0    - done flag

# https://www.youtube.com/watch?v=8uyB78HNR4M
# https://stackoverflow.com/questions/13092548/which-sorting-algorithm-uses-the-fewest-comparisons
# https://www.geeksforgeeks.org/counting-sort/
	
	
main:
	li $t0, 0            # Iterator
	lw $t1, datalen     # Data length
	addi $t1, $t1, -1
	la $t2, data        # Data address
	li $s0, 0

reset_iterator:
	li $t0, 0            # Iterator
	jr $ra


find_biggest_smallest_loop:
	beq $t0,$t1, loop
	
	j find_biggest_smallest_loop


loop:
	bge $t0, $t1, exit
	nop
	
	
	
	
	
	
	
	j loop


exit:
	ori $v0,$zero,10	# Prepare syscall to exit program cleanly
	syscall			# Bye!
