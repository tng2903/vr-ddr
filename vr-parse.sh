#!/bin/bash
# i'm sorry
# parse simfiles for vr

mv ratherbe.sm ratherbe.sm.original

# remove comments
sed '27,5000/\/\/ .*//g' ratherbe.sm.original >| ratherbe.sm.nocomments

# remove hold terminators
sed '27,5000s/3/0/g' ratherbe.sm.nocomments >| ratherbe.sm.nocomments.no3

# remove hold start indicators
sed '27,5000s/2/0/g' ratherbe.sm.nocomments.no3 >| ratherbe.sm.nocomments.no32

cp ratherbe.sm.nocomments.no32 ratherbe.sm
